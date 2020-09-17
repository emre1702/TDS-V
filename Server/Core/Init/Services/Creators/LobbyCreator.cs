using Microsoft.Extensions.DependencyInjection;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.EventsHandlers;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class LobbyCreator
    {
        public static IServiceCollection WithLobby(this IServiceCollection serviceCollection)
            => serviceCollection
                .WithLobbyDatabase()
                .WithEventsHandler();

        private static IServiceCollection WithLobbyDatabase(this IServiceCollection serviceCollection)
            => serviceCollection.AddTransient<BaseLobbyDatabase>();

        private static IServiceCollection WithEventsHandler(this IServiceCollection serviceCollection)
            => serviceCollection.AddTransient<BaseLobbyEventsHandler>();
    }
}
