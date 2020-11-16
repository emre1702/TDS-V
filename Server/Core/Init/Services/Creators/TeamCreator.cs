using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.TeamsSystem;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class TeamCreator
    {
        public static IServiceCollection WithTeam(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddTransient<ITeam, Team>()
                .WithProvider()
                .WithTeamDependencies();

        private static IServiceCollection WithProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<ITeamsProvider, TeamsProvider>();
        }

        private static IServiceCollection WithTeamDependencies(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddTransient<ITeamChat, Chat>()
               .AddTransient<ITeamPlayers, Players>()
               .AddTransient<ITeamSync, Sync>();
        }
    }
}
