using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Models;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class MainMenu : BaseLobby
    {
        public MainMenu(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }

        protected override void InitDependencies(
            DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            LobbyDependencies? lobbyDependencies = null)
        {
            base.InitDependencies(databaseHandler, langHelper, eventsHandler, lobbyDependencies);
        }
    }
}
