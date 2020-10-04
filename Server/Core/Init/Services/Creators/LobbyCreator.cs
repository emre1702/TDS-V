using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.LobbySystem;
using TDS_Server.LobbySystem;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class LobbyCreator
    {
        public static IServiceCollection WithLobby(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ILobbiesProvider, LobbiesProvider>();
    }
}
