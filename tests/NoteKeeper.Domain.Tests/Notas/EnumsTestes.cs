using NoteKeeper.Domain.Notas;

namespace NoteKeeper.Domain.Tests.Notas;

[TestClass]
public sealed class EnumsTestes
{
    [TestMethod]
    public void Prioridade_DeveTerValoresEsperados()
    {
        Assert.AreEqual(1, (int)Prioridade.Baixa);
        Assert.AreEqual(2, (int)Prioridade.Media);
        Assert.AreEqual(3, (int)Prioridade.Alta);
        Assert.AreEqual(4, (int)Prioridade.Urgente);
    }

    [TestMethod]
    public void CorNota_DeveTerValoresEsperados()
    {
        Assert.AreEqual(0, (int)CorNota.Padrao);
        Assert.AreEqual(1, (int)CorNota.Vermelho);
        Assert.AreEqual(2, (int)CorNota.Laranja);
        Assert.AreEqual(3, (int)CorNota.Amarelo);
        Assert.AreEqual(4, (int)CorNota.Verde);
        Assert.AreEqual(5, (int)CorNota.VerdeAgua);
        Assert.AreEqual(6, (int)CorNota.Azul);
        Assert.AreEqual(7, (int)CorNota.Roxo);
        Assert.AreEqual(8, (int)CorNota.Rosa);
        Assert.AreEqual(9, (int)CorNota.Cinza);
    }
}