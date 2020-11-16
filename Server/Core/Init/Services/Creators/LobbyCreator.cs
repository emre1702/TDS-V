using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Interfaces.LobbySystem;
using TDS.Server.LobbySystem;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class LobbyCreator
    {
        public static IServiceCollection WithLobby(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ILobbiesProvider, LobbiesProvider>();
    }
}
