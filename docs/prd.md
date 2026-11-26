# NoteKeeper — Planejamento de Produto

> Documento de produto: visão, escopo, fluxos, regras de negócio, critérios de aceite e pontos em aberto. As decisões técnicas estão centralizadas em [tdd.md](tdd.md).
>
> Última atualização: 26/11/2026

## 1. Visão

NoteKeeper é uma aplicação web de anotações inspirada no Google Keep, de **uso individual**. O usuário cria notas com **título**, **descrição** e **prioridade**, organiza-as em **categorias** (que também possuem apenas título) e usa essas categorias para filtrar a listagem.

Uma nota pode **não ter categoria nenhuma ou ter várias**. Categorias são o eixo de organização e filtro do produto.

**Premissa de uso:** uma única pessoa, sem login, cadastro, perfis ou multiusuário. A ausência de autenticação é uma decisão de produto — não um adiamento — e o app roda na própria máquina ou em ambiente privado. Se um dia for publicado na internet, essa decisão precisa ser reavaliada (ver R5 em [tdd.md](tdd.md)).

### Proposta de valor

- Captura rápida de ideias com o mínimo de campos obrigatórios.
- Organização leve por categorias, sem hierarquia.
- Prioridade explícita para separar o urgente do "algum dia".
- Sensação de "mesa de trabalho" do Keep: fixar, arquivar, lixeira, cores e lembretes.

## 2. Escopo do MVP

### Dentro do escopo

| # | Funcionalidade | Resumo |
|---|---|---|
| F1 | CRUD de notas | Criar, listar, editar e excluir notas com título, descrição e prioridade |
| F2 | CRUD de categorias | Criar, listar, editar e excluir categorias (apenas título) |
| F3 | Vínculo N:N | Uma nota pode ter 0..N categorias |
| F4 | Filtro por categoria | Filtrar a listagem por **uma** categoria por vez |
| F5 | Fixar nota (pin) | Notas fixadas aparecem no topo da listagem |
| F6 | Busca textual | Buscar por título e/ou descrição |
| F7 | Arquivar | Tirar a nota da listagem principal sem excluí-la |
| F8 | Lixeira (exclusão lógica) | Exclusão reversível, com restauração e exclusão definitiva |
| F9 | Cores | Paleta fixa de cores aplicada à nota |
| F10 | Lembretes | Data/hora na nota, com destaque visual dos vencidos |

### Fora do escopo

- Login, cadastro, autenticação, perfis e multiusuário — decisão definitiva para o escopo atual.
- Compartilhamento e colaboração em tempo real.
- Anexos, imagens e checklist dentro da nota.
- Notificações no navegador ou e-mail para lembretes.
- API pública, app mobile e editor rich text.
- Multidioma, temas e acessibilidade avançada.

## 3. Fluxos principais

1. **Criar nota** — formulário com título, descrição, prioridade, cor, lembrete e seleção múltipla de categorias.
2. **Listar/filtrar** — grid de notas; sidebar com categorias; busca no topo; alternância entre ativas, arquivadas e lixeira.
3. **Fixar/arquivar** — ações rápidas no card, sem abrir a nota.
4. **Lixeira** — lista de notas excluídas com "Restaurar" e "Excluir definitivamente".
5. **Excluir categoria** — confirmação explícita informando que as notas irão para a lixeira.

## 4. Regras de negócio

> A modelagem técnica (campos, tipos, índices e persistência) está em [tdd.md](tdd.md).

- **RN-01** — Título da nota é obrigatório e limitado a 120 caracteres; descrição é opcional e limitada a 4.000.
- **RN-02** — Prioridade é obrigatória e assume um dos 4 valores do enum; padrão `Media`.
- **RN-03** — Nota pode ter zero, uma ou várias categorias. Categoria pode existir sem nenhuma nota.
- **RN-04** — Título de categoria é obrigatório, único (ignorando maiúsculas/minúsculas) e limitado a 60 caracteres.
- **RN-05** — Listagem principal mostra apenas notas com `Excluida = false` e `Arquivada = false`.
- **RN-06** — Excluir nota faz exclusão lógica (`Excluida = true`, `ExcluidaEm = agora`). Restaurar limpa os dois campos. Excluir definitivamente remove o registro e os vínculos da junção.
- **RN-07** — Excluir categoria remove a categoria e **envia em cascata as notas dela para a lixeira** (exclusão lógica). Notas restauradas depois ficam sem categoria.
- **RN-08** — Filtro por categoria considera apenas uma categoria e ignora notas na lixeira/arquivadas.
- **RN-09** — Busca é textual, em título e descrição, case-insensitive, e combina com o filtro de categoria e com a busca ativa.
- **RN-10** — Ordenação padrão: fixadas primeiro, depois lembretes mais próximos/vencidos, depois `AtualizadaEm` decrescente.
- **RN-11** — Arquivar e fixar são flags independentes; notas arquivadas não aparecem na listagem principal mesmo se fixadas.
- **RN-12** — Lembrete vencido (`LembreteEm < agora`) recebe destaque visual; não dispara notificação.

## 5. Critérios de aceite (resumo)

- Consigo criar uma nota apenas com título e prioridade; descrição e categorias são opcionais.
- Consigo criar uma nota com várias categorias e vê-la em cada filtro correspondente.
- Ao filtrar por uma categoria, só aparecem notas ativas daquela categoria.
- Notas fixadas aparecem antes das demais na listagem.
- Busca por termo no título ou na descrição retorna as notas certas, respeitando o filtro ativo.
- Nota arquivada some da listagem principal e reaparece em "Arquivadas".
- Nota excluída vai para a lixeira, pode ser restaurada e pode ser excluída definitivamente.
- Excluir categoria com notas manda essas notas para a lixeira, sem quebrar a aplicação.
- Nota com lembrete vencido é destacada visualmente.
- Regras de RN-01 a RN-12 cobertas por testes.

## 6. Pontos de produto em aberto

1. **Notas sem categoria** — haverá um filtro "Sem categoria" na sidebar?
2. **Layout** — sidebar fixa estilo Keep com cards em grid, ou lista vertical simples?
3. **Arquivadas/fixadas** — nota fixada e arquivada deve continuar fixada ao ser restaurada?
4. **Ordenação** — permitir que o usuário escolha (data, prioridade, título) ou manter a ordenação fixa da RN-10?
5. **Paleta de cores** — as 10 cores propostas são suficientes para o uso real?
6. **Idioma da interface** — pt-BR apenas, com textos hardcoded, ou recursos de localização desde já?

> Decisões técnicas pendentes (persistência, paginação, seed, deploy etc.) estão na seção 12 de [tdd.md](tdd.md).

## 7. Próximos passos de produto

1. Validar os pontos de produto em aberto com o usuário.
2. Detalhar jornadas e wireframes das telas principais.
3. Escrever histórias de usuário e montar o backlog do MVP.
4. Só então iniciar a implementação, seguindo as decisões e o roadmap de [tdd.md](tdd.md).

## 8. Referências

- [Decisões técnicas](tdd.md)
- Google Keep — referência funcional e de experiência
