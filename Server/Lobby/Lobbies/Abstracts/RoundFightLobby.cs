using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.GamemodesHandlers;
using TDS_Server.LobbySystem.RoundsHandlers;
using TDS_Server.LobbySystem.Weapons;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class RoundFightLobby : FightLobby
    {
        public RoundFightLobbyGamemodesHandler Gamemodes { get; set; }
        public RoundFightLobbyRoundsHandler Rounds { get; set; }
        public new RoundFightLobbyWeapons Weapons => (RoundFightLobbyWeapons)base.Weapons;

        protected RoundFightLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }
    }
}
