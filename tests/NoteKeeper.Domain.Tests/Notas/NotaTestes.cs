using NoteKeeper.Domain.Categorias;
using NoteKeeper.Domain.Comum;
using NoteKeeper.Domain.Notas;

namespace NoteKeeper.Domain.Tests.Notas;

[TestClass]
public sealed class NotaTestes
{
    [TestMethod]
    public void Criar_ComTitulo_DeveUsarValoresPadrao()
    {
        Nota nota = Nota.Criar("  Minha nota  ");

        Assert.AreNotEqual(Guid.Empty, nota.Id);
        Assert.AreEqual("Minha nota", nota.Titulo);
        Assert.IsNull(nota.Descricao);
        Assert.AreEqual(Prioridade.Media, nota.Prioridade);
        Assert.AreEqual(CorNota.Padrao, nota.Cor);
        Assert.IsFalse(nota.Fixada);
        Assert.IsFalse(nota.Arquivada);
        Assert.IsFalse(nota.Excluida);
        Assert.IsNull(nota.ExcluidaEm);
        Assert.IsNull(nota.LembreteEm);
        Assert.AreEqual(0, nota.Categorias.Count);
    }

    [TestMethod]
    public void Criar_ComTituloNulo_DeveLancarExcecao()
    {
        Assert.ThrowsExactly<ExcecaoDeDominio>(() => Nota.Criar(null!));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Criar_ComTituloVazio_DeveLancarExcecao(string titulo)
    {
        Assert.ThrowsExactly<ExcecaoDeDominio>(() => Nota.Criar(titulo));
    }

    [TestMethod]
    public void Criar_ComTituloDe120Caracteres_DeveAceitar()
    {
        string titulo = new('a', Nota.TamanhoMaximoTitulo);

        Nota nota = Nota.Criar(titulo);

        Assert.AreEqual(titulo, nota.Titulo);
    }

    [TestMethod]
    public void Criar_ComTituloMaiorQue120Caracteres_DeveLancarExcecao()
    {
        string titulo = new('a', Nota.TamanhoMaximoTitulo + 1);

        Assert.ThrowsExactly<ExcecaoDeDominio>(() => Nota.Criar(titulo));
    }

    [TestMethod]
    public void Criar_ComDescricaoVazia_DeveNormalizarParaNulo()
    {
        Nota nota = Nota.Criar("Nota", "   ");

        Assert.IsNull(nota.Descricao);
    }

    [TestMethod]
    public void Criar_ComDescricao_DeveRemoverEspacos()
    {
        Nota nota = Nota.Criar("Nota", "  descrição  ");

        Assert.AreEqual("descrição", nota.Descricao);
    }

    [TestMethod]
    public void Criar_ComDescricaoDe4000Caracteres_DeveAceitar()
    {
        string descricao = new('a', Nota.TamanhoMaximoDescricao);

        Nota nota = Nota.Criar("Nota", descricao);

        Assert.AreEqual(descricao, nota.Descricao);
    }

    [TestMethod]
    public void Criar_ComDescricaoMaiorQue4000Caracteres_DeveLancarExcecao()
    {
        string descricao = new('a', Nota.TamanhoMaximoDescricao + 1);

        Assert.ThrowsExactly<ExcecaoDeDominio>(() => Nota.Criar("Nota", descricao));
    }

    [TestMethod]
    public void Criar_ComPrioridadeInvalida_DeveLancarExcecao()
    {
        Assert.ThrowsExactly<ExcecaoDeDominio>(
            () => Nota.Criar("Nota", prioridade: (Prioridade)0));
    }

    [TestMethod]
    public void Criar_ComCorInvalida_DeveLancarExcecao()
    {
        Assert.ThrowsExactly<ExcecaoDeDominio>(
            () => Nota.Criar("Nota", cor: (CorNota)10));
    }

    [TestMethod]
    public void Criar_ComLembreteLocal_DeveConverterParaUtc()
    {
        DateTime lembrete = new(2026, 11, 26, 10, 0, 0, DateTimeKind.Local);

        Nota nota = Nota.Criar("Nota", lembreteEm: lembrete);

        Assert.IsNotNull(nota.LembreteEm);
        Assert.AreEqual(DateTimeKind.Utc, nota.LembreteEm.Value.Kind);
        Assert.AreEqual(lembrete.ToUniversalTime(), nota.LembreteEm.Value);
    }

    [TestMethod]
    public void Criar_ComLembreteSemFuso_DeveAssumirUtc()
    {
        DateTime lembrete = new(2026, 11, 26, 10, 0, 0, DateTimeKind.Unspecified);

        Nota nota = Nota.Criar("Nota", lembreteEm: lembrete);

        Assert.IsNotNull(nota.LembreteEm);
        Assert.AreEqual(DateTimeKind.Utc, nota.LembreteEm.Value.Kind);
        Assert.AreEqual(lembrete, nota.LembreteEm.Value);
    }

    [TestMethod]
    public void AdicionarCategoria_DeveVincularCategoria()
    {
        Nota nota = Nota.Criar("Nota");
        Categoria categoria = Categoria.Criar("Trabalho");

        nota.AdicionarCategoria(categoria);

        Assert.AreEqual(1, nota.Categorias.Count);
        Assert.AreSame(categoria, nota.Categorias.Single());
    }

    [TestMethod]
    public void AdicionarCategoria_Duplicada_NaoDeveDuplicar()
    {
        Nota nota = Nota.Criar("Nota");
        Categoria categoria = Categoria.Criar("Trabalho");

        nota.AdicionarCategoria(categoria);
        nota.AdicionarCategoria(categoria);

        Assert.AreEqual(1, nota.Categorias.Count);
    }

    [TestMethod]
    public void RemoverCategoria_DeveDesvincular()
    {
        Nota nota = Nota.Criar("Nota");
        Categoria categoria = Categoria.Criar("Trabalho");
        nota.AdicionarCategoria(categoria);

        nota.RemoverCategoria(categoria.Id);

        Assert.AreEqual(0, nota.Categorias.Count);
    }

    [TestMethod]
    public void Atualizar_ComDadosValidos_DeveAlterarCampos()
    {
        Nota nota = Nota.Criar("Antigo", "Descrição antiga");

        nota.Atualizar("Novo", "Descrição nova", Prioridade.Alta, CorNota.Azul, null);

        Assert.AreEqual("Novo", nota.Titulo);
        Assert.AreEqual("Descrição nova", nota.Descricao);
        Assert.AreEqual(Prioridade.Alta, nota.Prioridade);
        Assert.AreEqual(CorNota.Azul, nota.Cor);
    }

    [TestMethod]
    public void Atualizar_ComTituloVazio_DeveLancarExcecao()
    {
        Nota nota = Nota.Criar("Nota");

        Assert.ThrowsExactly<ExcecaoDeDominio>(
            () => nota.Atualizar(string.Empty, null, Prioridade.Media, CorNota.Padrao, null));
    }
}