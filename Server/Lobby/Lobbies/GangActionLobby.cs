using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Models;
using TDS_Server.LobbySystem.Sync;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class GangActionLobby : RoundFightLobby
    {
        public GangActionLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }

        protected override void InitDependencies(DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler, LobbyDependencies? lobbyDependencies = null)
        {
            lobbyDependencies ??= new LobbyDependencies();

            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(eventsHandler, this);
            lobbyDependencies.MapHandler ??= new BaseLobbyMapHandler(Entity);
            lobbyDependencies.Sync ??= new GangActionLobbySync(Entity, lobbyDependencies.Events, () => lobbyDependencies.MapHandler.Dimension);

            base.InitDependencies(databaseHandler, langHelper, eventsHandler, lobbyDependencies);
        }
    }
}
