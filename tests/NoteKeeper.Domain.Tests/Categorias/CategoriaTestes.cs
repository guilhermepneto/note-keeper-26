using NoteKeeper.Domain.Categorias;
using NoteKeeper.Domain.Comum;

namespace NoteKeeper.Domain.Tests.Categorias;

[TestClass]
public sealed class CategoriaTestes
{
    [TestMethod]
    public void Criar_ComTituloValido_DeveCriarCategoria()
    {
        Categoria categoria = Categoria.Criar("  Trabalho  ");

        Assert.AreNotEqual(Guid.Empty, categoria.Id);
        Assert.AreEqual("Trabalho", categoria.Titulo);
        Assert.AreEqual(0, categoria.Notas.Count);
    }

    [TestMethod]
    public void Criar_ComTituloNulo_DeveLancarExcecao()
    {
        Assert.ThrowsExactly<ExcecaoDeDominio>(() => Categoria.Criar(null!));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Criar_ComTituloVazio_DeveLancarExcecao(string titulo)
    {
        Assert.ThrowsExactly<ExcecaoDeDominio>(() => Categoria.Criar(titulo));
    }

    [TestMethod]
    public void Criar_ComTituloDe60Caracteres_DeveAceitar()
    {
        string titulo = new('a', Categoria.TamanhoMaximoTitulo);

        Categoria categoria = Categoria.Criar(titulo);

        Assert.AreEqual(titulo, categoria.Titulo);
    }

    [TestMethod]
    public void Criar_ComTituloMaiorQue60Caracteres_DeveLancarExcecao()
    {
        string titulo = new('a', Categoria.TamanhoMaximoTitulo + 1);

        Assert.ThrowsExactly<ExcecaoDeDominio>(() => Categoria.Criar(titulo));
    }

    [TestMethod]
    public void Renomear_ComTituloValido_DeveAlterarTitulo()
    {
        Categoria categoria = Categoria.Criar("Trabalho");

        categoria.Renomear("  Pessoal  ");

        Assert.AreEqual("Pessoal", categoria.Titulo);
    }

    [TestMethod]
    public void Renomear_ComTituloVazio_DeveLancarExcecao()
    {
        Categoria categoria = Categoria.Criar("Trabalho");

        Assert.ThrowsExactly<ExcecaoDeDominio>(() => categoria.Renomear("   "));
    }
}