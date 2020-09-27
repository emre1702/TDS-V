using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class RoundFightLobby : FightLobby, IRoundFightLobby
    {
        public new IFightLobbyDeathmatch Deathmatch => (IFightLobbyDeathmatch)base.Deathmatch;
        public new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;
        public IRoundFightLobbyGamemodesHandler Gamemodes { get; set; }
        public new IRoundFightLobbyMapHandler MapHandler => (IRoundFightLobbyMapHandler)base.MapHandler;
        public IRoundFightLobbyRoundsHandler Rounds { get; set; }
        public new IRoundFightLobbyWeapons Weapons => (IRoundFightLobbyWeapons)base.Weapons;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected RoundFightLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }
    }
}
