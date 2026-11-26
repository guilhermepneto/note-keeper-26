using NoteKeeper.Domain.Categorias;
using NoteKeeper.Domain.Comum;

namespace NoteKeeper.Domain.Notas;

public sealed class Nota
{
    public const int TamanhoMaximoTitulo = 120;
    public const int TamanhoMaximoDescricao = 4000;

    private Nota()
    {
    }

    private Nota(
        string titulo,
        string? descricao,
        Prioridade prioridade,
        CorNota cor,
        DateTime? lembreteEm)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        Prioridade = prioridade;
        Cor = cor;
        LembreteEm = lembreteEm;
    }

    public Guid Id { get; private set; }

    public string Titulo { get; private set; } = string.Empty;

    public string? Descricao { get; private set; }

    public Prioridade Prioridade { get; private set; }

    public CorNota Cor { get; private set; }

    public bool Fixada { get; private set; }

    public bool Arquivada { get; private set; }

    public bool Excluida { get; private set; }

    public DateTime? ExcluidaEm { get; private set; }

    public DateTime? LembreteEm { get; private set; }

    public DateTime CriadaEm { get; private set; }

    public DateTime? AtualizadaEm { get; private set; }

    public ICollection<Categoria> Categorias { get; private set; } = new List<Categoria>();

    public static Nota Criar(
        string titulo,
        string? descricao = null,
        Prioridade prioridade = Prioridade.Media,
        CorNota cor = CorNota.Padrao,
        DateTime? lembreteEm = null)
    {
        string tituloNormalizado = ValidacaoDeTexto.NormalizarObrigatorio(
            titulo, "título da nota", TamanhoMaximoTitulo);

        string? descricaoNormalizada = ValidacaoDeTexto.NormalizarOpcional(
            descricao, "descrição da nota", TamanhoMaximoDescricao);

        ValidarPrioridade(prioridade);
        ValidarCor(cor);

        return new Nota(
            tituloNormalizado,
            descricaoNormalizada,
            prioridade,
            cor,
            NormalizarLembrete(lembreteEm));
    }

    public void Atualizar(
        string titulo,
        string? descricao,
        Prioridade prioridade,
        CorNota cor,
        DateTime? lembreteEm)
    {
        string tituloNormalizado = ValidacaoDeTexto.NormalizarObrigatorio(
            titulo, "título da nota", TamanhoMaximoTitulo);

        string? descricaoNormalizada = ValidacaoDeTexto.NormalizarOpcional(
            descricao, "descrição da nota", TamanhoMaximoDescricao);

        ValidarPrioridade(prioridade);
        ValidarCor(cor);

        Titulo = tituloNormalizado;
        Descricao = descricaoNormalizada;
        Prioridade = prioridade;
        Cor = cor;
        LembreteEm = NormalizarLembrete(lembreteEm);
    }

    public void AdicionarCategoria(Categoria categoria)
    {
        ArgumentNullException.ThrowIfNull(categoria);

        if (Categorias.Any(c => c.Id == categoria.Id))
        {
            return;
        }

        Categorias.Add(categoria);
    }

    public void RemoverCategoria(Guid categoriaId)
    {
        Categoria? categoria = Categorias.FirstOrDefault(c => c.Id == categoriaId);

        if (categoria is not null)
        {
            Categorias.Remove(categoria);
        }
    }

    private static void ValidarPrioridade(Prioridade prioridade)
    {
        if (!Enum.IsDefined(prioridade))
        {
            throw new ExcecaoDeDominio("A prioridade informada é inválida.");
        }
    }

    private static void ValidarCor(CorNota cor)
    {
        if (!Enum.IsDefined(cor))
        {
            throw new ExcecaoDeDominio("A cor informada é inválida.");
        }
    }

    private static DateTime? NormalizarLembrete(DateTime? lembreteEm)
    {
        if (lembreteEm is null)
        {
            return null;
        }

        return lembreteEm.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(lembreteEm.Value, DateTimeKind.Utc)
            : lembreteEm.Value.ToUniversalTime();
    }
}