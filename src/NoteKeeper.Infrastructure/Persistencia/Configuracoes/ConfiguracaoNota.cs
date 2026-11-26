using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteKeeper.Domain.Categorias;
using NoteKeeper.Domain.Notas;

namespace NoteKeeper.Infrastructure.Persistencia.Configuracoes;

public sealed class ConfiguracaoNota : IEntityTypeConfiguration<Nota>
{
    public void Configure(EntityTypeBuilder<Nota> builder)
    {
        builder.ToTable("Notas");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Titulo)
            .IsRequired()
            .HasMaxLength(Nota.TamanhoMaximoTitulo);

        builder.Property(n => n.Descricao)
            .HasMaxLength(Nota.TamanhoMaximoDescricao);

        // DTA-01: enums persistidos como texto legível.
        builder.Property(n => n.Prioridade)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(n => n.Cor)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(n => n.Fixada).IsRequired();
        builder.Property(n => n.Arquivada).IsRequired();
        builder.Property(n => n.Excluida).IsRequired();
        builder.Property(n => n.CriadaEm).IsRequired();

        builder.HasIndex(n => new { n.Excluida, n.Arquivada })
            .HasDatabaseName("IX_Notas_Excluida_Arquivada");

        builder.HasIndex(n => n.LembreteEm)
            .HasDatabaseName("IX_Notas_LembreteEm");

        // RN-05/RN-06: notas excluídas só aparecem com IgnoreQueryFilters explícito.
        builder.HasQueryFilter(n => !n.Excluida);

        builder.HasMany(n => n.Categorias)
            .WithMany(c => c.Notas)
            .UsingEntity<Dictionary<string, object>>(
                "NotasCategorias",
                juncao => juncao
                    .HasOne<Categoria>()
                    .WithMany()
                    .HasForeignKey("CategoriaId")
                    .OnDelete(DeleteBehavior.Cascade),
                juncao => juncao
                    .HasOne<Nota>()
                    .WithMany()
                    .HasForeignKey("NotaId")
                    .OnDelete(DeleteBehavior.Cascade),
                juncao =>
                {
                    juncao.ToTable("NotasCategorias");
                    juncao.HasKey("NotaId", "CategoriaId");
                });
    }
}