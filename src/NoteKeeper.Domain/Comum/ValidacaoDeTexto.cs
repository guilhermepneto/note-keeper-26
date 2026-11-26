namespace NoteKeeper.Domain.Comum;

internal static class ValidacaoDeTexto
{
    public static string NormalizarObrigatorio(string? valor, string nomeCampo, int tamanhoMaximo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcecaoDeDominio($"O campo {nomeCampo} é obrigatório.");
        }

        string normalizado = valor.Trim();

        if (normalizado.Length > tamanhoMaximo)
        {
            throw new ExcecaoDeDominio(
                $"O campo {nomeCampo} deve ter no máximo {tamanhoMaximo} caracteres.");
        }

        return normalizado;
    }

    public static string? NormalizarOpcional(string? valor, string nomeCampo, int tamanhoMaximo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        string normalizado = valor.Trim();

        if (normalizado.Length > tamanhoMaximo)
        {
            throw new ExcecaoDeDominio(
                $"O campo {nomeCampo} deve ter no máximo {tamanhoMaximo} caracteres.");
        }

        return normalizado;
    }
}