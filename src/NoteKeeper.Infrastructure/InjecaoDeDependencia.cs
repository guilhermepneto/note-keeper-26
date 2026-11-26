using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoteKeeper.Infrastructure.Persistencia;

namespace NoteKeeper.Infrastructure;

public static class InjecaoDeDependencia
{
    public static IServiceCollection AdicionarInfraestrutura(
        this IServiceCollection servicos,
        IConfiguration configuracao)
    {
        string? conexao = configuracao.GetConnectionString("NoteKeeper");

        if (string.IsNullOrWhiteSpace(conexao))
        {
            throw new InvalidOperationException(
                "A connection string 'NoteKeeper' não foi configurada. " +
                "Defina-a em appsettings.Development.json ou em user secrets.");
        }

        servicos.AddDbContext<ContextoNoteKeeper>(opcoes => opcoes.UseSqlServer(conexao));

        return servicos;
    }
}