using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.PlayersSystem;

namespace TDS.Server.Core.Init.Services.Creators
{
    public static class PlayerCreator
    {
        public static IServiceCollection WithPlayerSystem(this IServiceCollection serviceCollection)
            => serviceCollection
                .WithProvider()
                .WithDependencies();

        private static IServiceCollection WithProvider(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddTransient<IPlayerProvider, Provider>();

        private static IServiceCollection WithDependencies(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddTransient<IPlayerAdmin, Admin>()
                .AddTransient<IPlayerChallengesHandler, ChallengesHandler>()
                .AddTransient<IPlayerChat, Chat>()
                .AddTransient<IPlayerDatabaseHandler, DatabaseHandler>()
                .AddTransient<IPlayerDeathmatch, Deathmatch>()
                .AddTransient<IPlayerEvents, PlayersSystem.Events>()
                .AddTransient<IPlayerGangHandler, GangHandler>()
                .AddTransient<IPlayerHealthAndArmor, HealthAndArmor>()
                .AddTransient<IPlayerLanguageHandler, LanguageHandler>()
                .AddTransient<IPlayerLobbyHandler, LobbyHandler>()
                .AddTransient<IPlayerMapsVoting, MapsVoting>()
                .AddTransient<IPlayerMoneyHandler, MoneyHandler>()
                .AddTransient<IPlayerMuteHandler, MuteHandler>()
                .AddTransient<IPlayerPlayTime, PlayTime>()
                .AddTransient<IPlayerRelations, Relations>()
                .AddTransient<IPlayerSpectateHandler, SpectateHandler>()
                .AddTransient<IPlayerSync, Sync>()
                .AddTransient<IPlayerTeamHandler, TeamHandler>()
                .AddTransient<IPlayerTimezone, Timezone>()
                .AddTransient<IPlayerVoice, Voice>()
                .AddTransient<IPlayerWeaponStats, WeaponStats>();
    }
}
