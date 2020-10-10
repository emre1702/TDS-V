using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.TeamsSystem;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class TeamCreator
    {
        public static IServiceCollection WithTeam(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ITeamsProvider, TeamsProvider>();
    }
}
