using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Handler.Entities.Gamemodes;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class LobbyCreator
    {
        public static IServiceCollection WithLobby(this IServiceCollection serviceCollection)
            => serviceCollection
                .WithGamemodes();

        private static IServiceCollection WithGamemodes(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddTransient<IArmsRace, ArmsRace>()
               .AddTransient<IBomb, Bomb>()
               .AddTransient<IDeathmatch, Deathmatch>()
               .AddTransient<IGangwar, Gangwar>()
               .AddTransient<ISniper, Sniper>();
        }
    }
}
