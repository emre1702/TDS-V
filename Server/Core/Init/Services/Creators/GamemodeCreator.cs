using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Interfaces.GamemodesSystem;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.GamemodesSystem;
using TDS.Server.GamemodesSystem.Gamemodes;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class GamemodeCreator
    {
        public static IServiceCollection WithGamemode(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IGamemodesProvider, GamemodesProvider>()
                .WithGamemodeTypes();

        internal static IServiceCollection WithGamemodeTypes(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddTransient<IArmsRaceGamemode, ArmsRaceGamemode>()
                .AddTransient<IBombGamemode, BombGamemode>()
                .AddTransient<IDeathmatchGamemode, DeathmatchGamemode>()
                .AddTransient<IGangwarGamemode, GangwarGamemode>()
                .AddTransient<ISniperGamemode, SniperGamemode>();
    }
}
