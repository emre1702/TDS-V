using Microsoft.Extensions.DependencyInjection;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class LobbyCreator
    {
        public static IServiceCollection WithLobby(this IServiceCollection serviceCollection)
            => serviceCollection;
    }
}
