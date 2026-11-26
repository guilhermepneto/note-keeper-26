# NoteKeeper

Aplicação web de anotações inspirada no Google Keep, de uso individual e sem autenticação. O usuário cria notas com título, descrição e prioridade, organiza tudo em categorias e filtra a listagem por categoria.

## Status do projeto

| Fase | Nome | Status |
|---|---|---|
| 0 | Fundação | Concluída |
| 1 | Domínio e persistência | Concluída |
| 2 | Notas e lixeira | Planejada |
| 3 | Categorias e vínculo N:N | Planejada |
| 4 | Filtro por categoria | Planejada |
| 5 | Busca, fixação e arquivamento | Planejada |
| 6 | Cores e lembretes | Planejada |
| 7 | Consolidação | Planejada |

A Fase 1 entrega o modelo de domínio, o `DbContext`, as configurações do EF Core, a migration inicial e os testes de domínio. Ainda não há telas de negócio, controllers de notas/categorias, repositórios nem serviços de aplicação.

## Stack

- .NET 10 (LTS) + ASP.NET Core MVC
- EF Core 10 + SQL Server (LocalDB no desenvolvimento)
- MSTest nos projetos de domínio e aplicação
- Bootstrap do template MVC

## Arquitetura

Clean Architecture com dependências apontando para dentro:

```text
Web ──> Application ──> Domain
 │            ▲
 └──> Infrastructure ──┘   (apenas composição/DI)
```

```text
src/
  NoteKeeper.Domain/          Entidades, enums e invariantes
    Comum/                    ExcecaoDeDominio e validações de texto
    Categorias/               Categoria
    Notas/                    Nota, Prioridade e CorNota
  NoteKeeper.Application/     Casos de uso, DTOs e interfaces
  NoteKeeper.Infrastructure/  EF Core, configurações, migrations e repositórios
    Persistencia/             ContextoNoteKeeper e configurações
  NoteKeeper.Web/             Controllers, ViewModels e Views Razor
tests/
  NoteKeeper.Domain.Tests/
  NoteKeeper.Application.Tests/
```

## O que já está implementado

### Domínio

- `Nota` com título, descrição, prioridade, cor, flags de fixação/arquivamento/exclusão, lembrete e categorias.
- `Categoria` com título e relacionamento N:N com notas.
- Enums `Prioridade` (Baixa, Media, Alta, Urgente) e `CorNota` (Padrao, Vermelho, ..., Cinza).
- Invariantes de criação e atualização:
  - Título da nota obrigatório, com `Trim`, entre 1 e 120 caracteres.
  - Descrição opcional, com `Trim`, até 4.000 caracteres.
  - Prioridade e cor obrigatórias, com valores padrão `Media` e `Padrao`.
  - Título da categoria obrigatório, com `Trim`, entre 1 e 60 caracteres.
  - Lembrete normalizado para UTC.
- Vínculo e desvínculo de categorias em uma nota, sem duplicidade.

### Persistência

- `ContextoNoteKeeper` com `Notas`, `Categorias` e a tabela de junção `NotasCategorias`.
- Configurações por entidade em `Persistencia/Configuracoes`.
- Enums persistidos como texto legível (decisão DTA-01).
- Índices:
  - `IX_Notas_Excluida_Arquivada`
  - `IX_Notas_LembreteEm`
  - `IX_Categorias_Titulo` (não único; a unicidade será validada na Application na Fase 3, conforme DTA-04)
- Filtro global `Excluida == false` para notas.
- `CriadaEm`/`AtualizadaEm` preenchidas em UTC no `SaveChanges`.
- Migration inicial versionada em `src/NoteKeeper.Infrastructure/Persistencia/Migrations`.

### Testes

- 29 testes de domínio cobrindo RN-01, RN-02, RN-03 e o formato do título de categoria (RN-04).
- Teste de fundação da Application mantido até a Fase 2.

## Pré-requisitos

- .NET SDK 10.0 ou superior
- SQL Server Express LocalDB (desenvolvimento)
- `dotnet-ef` (restaurado pelo manifesto `dotnet-tools.json`)

## Configuração

A connection string de desenvolvimento fica fora do controle de versão. Use uma das opções:

1. Copiar `src/NoteKeeper.Web/appsettings.Development.example.json` para `appsettings.Development.json` e ajustar a conexão.
2. Ou usar user secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:NoteKeeper" "Server=(localdb)\MSSQLLocalDB;Database=NoteKeeper;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True" --project src/NoteKeeper.Web
```

## Como executar

```powershell
dotnet restore
dotnet build
dotnet run --project src/NoteKeeper.Web
```

Perfis disponíveis:

- HTTP: `http://localhost:5251`
- HTTPS: `https://localhost:7132`

## Como testar

```powershell
dotnet test
```

## Banco de dados

O banco é criado e atualizado com migrations versionadas:

```powershell
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = "Development"

# Criar uma nova migration (quando o modelo mudar)
dotnet ef migrations add NomeDaMigration `
  --project src/NoteKeeper.Infrastructure `
  --startup-project src/NoteKeeper.Web `
  --output-dir Persistencia/Migrations

# Aplicar as migrations no banco
dotnet ef database update `
  --project src/NoteKeeper.Infrastructure `
  --startup-project src/NoteKeeper.Web
```

Tabelas criadas pela migration inicial: `Notas`, `Categorias`, `NotasCategorias` e `__EFMigrationsHistory`.

A aplicação não aplica migrations automaticamente no start. A estratégia de migrations em produção continua em aberto (DTA-06).

## Convenções

- Código, nomes de domínio e mensagens em pt-BR.
- Identificadores sem acentos (`Titulo`, `Descricao`, `Media`, `Excluida`).
- PascalCase para tipos/membros, camelCase para parâmetros e `_camelCase` para campos privados.
- Interfaces com prefixo `I`; métodos assíncronos com sufixo `Async`.
- Rotas em português (`/notas`, `/categorias`).

## Decisões técnicas fechadas

| ID | Decisão |
|---|---|
| DTA-01 | `Prioridade` e `CorNota` persistidos como texto legível. |
| DTA-02 | Limites mantidos: 120 (título da nota), 4.000 (descrição) e 60 (título da categoria). |
| DTA-04 | Unicidade do título de categoria validada apenas na Application; sem índice único no banco. |

## Documentação

- [Planejamento de produto](docs/prd.md)
- [Decisões técnicas](docs/tdd.md)

O projeto não possui autenticação por decisão de produto: é individual e deve rodar localmente ou em ambiente privado.

## Roadmap

O roadmap completo, com critérios de saída e dependências, está na seção 13 de [docs/tdd.md](docs/tdd.md).
