using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.GamemodesSystem;
using TDS_Server.GamemodesSystem.Gamemodes;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class GamemodeCreator
    {
        public static IServiceCollection WithGamemode(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IGamemodesProvider, GamemodesProvider>()
                .WithGamemodeTypes();

        internal static IServiceCollection WithGamemodeTypes(this IServiceCollection serviceCollection)
            => serviceCollection
                //.AddTransient<IArmsRace, ArmsRace>()
                .AddTransient<IBombGamemode, BombGamemode>()
                .AddTransient<IDeathmatchGamemode, DeathmatchGamemode>()
                //.AddTransient<IGangwar, Gangwar>()
                .AddTransient<ISniperGamemode, SniperGamemode>();
    }
}
