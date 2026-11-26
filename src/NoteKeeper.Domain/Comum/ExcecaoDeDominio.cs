namespace NoteKeeper.Domain.Comum;

public sealed class ExcecaoDeDominio : Exception
{
    public ExcecaoDeDominio(string mensagem)
        : base(mensagem)
    {
    }
}