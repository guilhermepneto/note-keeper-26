using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NoteKeeper.Infrastructure;

public static class InjecaoDeDependencia
{
    public static IServiceCollection AdicionarInfraestrutura(
        this IServiceCollection servicos,
        IConfiguration configuracao)
    {
        return servicos;
    }
}
