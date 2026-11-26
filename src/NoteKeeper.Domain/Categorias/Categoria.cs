using NoteKeeper.Domain.Comum;
using NoteKeeper.Domain.Notas;

namespace NoteKeeper.Domain.Categorias;

public sealed class Categoria
{
    public const int TamanhoMaximoTitulo = 60;

    private Categoria()
    {
    }

    private Categoria(string titulo)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
    }

    public Guid Id { get; private set; }

    public string Titulo { get; private set; } = string.Empty;

    public DateTime CriadaEm { get; private set; }

    public ICollection<Nota> Notas { get; private set; } = new List<Nota>();

    public static Categoria Criar(string titulo)
    {
        return new Categoria(
            ValidacaoDeTexto.NormalizarObrigatorio(titulo, "título da categoria", TamanhoMaximoTitulo));
    }

    public void Renomear(string titulo)
    {
        Titulo = ValidacaoDeTexto.NormalizarObrigatorio(
            titulo, "título da categoria", TamanhoMaximoTitulo);
    }
}