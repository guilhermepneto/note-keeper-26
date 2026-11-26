using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteKeeper.Domain.Categorias;

namespace NoteKeeper.Infrastructure.Persistencia.Configuracoes;

public sealed class ConfiguracaoCategoria : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(Categoria.TamanhoMaximoTitulo)
            .UseCollation("Latin1_General_CI_AS");

        builder.Property(c => c.CriadaEm).IsRequired();

        // DTA-04: a unicidade é validada na Application (Fase 3); o índice é apenas de consulta.
        builder.HasIndex(c => c.Titulo)
            .HasDatabaseName("IX_Categorias_Titulo");
    }
}