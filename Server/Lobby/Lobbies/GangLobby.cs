using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class GangLobby : FreeroamLobby, IGangLobby
    {
        public GangLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }
    }
}
