using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NoteKeeper.Domain.Categorias;
using NoteKeeper.Domain.Notas;

namespace NoteKeeper.Infrastructure.Persistencia;

public sealed class ContextoNoteKeeper : DbContext
{
    public ContextoNoteKeeper(DbContextOptions<ContextoNoteKeeper> opcoes)
        : base(opcoes)
    {
    }

    public DbSet<Nota> Notas => Set<Nota>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoNoteKeeper).Assembly);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AplicarAuditoria();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        AplicarAuditoria();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void AplicarAuditoria()
    {
        DateTime agora = DateTime.UtcNow;

        foreach (EntityEntry<Nota> entrada in ChangeTracker.Entries<Nota>())
        {
            if (entrada.State == EntityState.Added)
            {
                entrada.Property(n => n.CriadaEm).CurrentValue = agora;
                entrada.Property(n => n.AtualizadaEm).CurrentValue = null;
            }
            else if (entrada.State == EntityState.Modified)
            {
                entrada.Property(n => n.AtualizadaEm).CurrentValue = agora;
            }
        }

        foreach (EntityEntry<Categoria> entrada in ChangeTracker.Entries<Categoria>())
        {
            if (entrada.State == EntityState.Added)
            {
                entrada.Property(c => c.CriadaEm).CurrentValue = agora;
            }
        }
    }
}