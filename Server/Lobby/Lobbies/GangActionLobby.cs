using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GangSystem.GangGamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Models;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class GangActionLobby : RoundFightLobby, IGangActionLobby
    {
        public IGangArea GangArea { get; }

        public GangActionLobby(LobbyDb entity, IGangArea gangArea, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
            GangArea = gangArea;
            gangArea.InLobby = this;
        }

        protected override void InitDependencies(LobbyDependencies? lobbyDependencies = null)
        {
            void doForPlayersInMainThreadFuncProvider(Action<ITDSPlayer> action) => Players.DoInMain(action);

            lobbyDependencies ??= new LobbyDependencies();

            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(GlobalEventsHandler, this);
            lobbyDependencies.MapHandler ??= new BaseLobbyMapHandler(Entity);
            lobbyDependencies.Sync ??= new GangActionLobbySync(Entity, lobbyDependencies.Events, () => lobbyDependencies.MapHandler.Dimension);
            lobbyDependencies.Database ??= new BaseLobbyDatabase(this, GlobalDatabaseHandler, lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new BaseLobbyTeamsHandler(Entity);

            lobbyDependencies.Chat ??= new BaseLobbyChat(doForPlayersInMainThreadFuncProvider, LangHelper);
            lobbyDependencies.Bans ??= new GangActionLobbyBansHandler(lobbyDependencies.Database, lobbyDependencies.Events, LangHelper, lobbyDependencies.Chat, Entity);
            lobbyDependencies.Players ??= new BaseLobbyPlayers(Entity, lobbyDependencies.Events, lobbyDependencies.Teams, lobbyDependencies.Bans);

            base.InitDependencies(lobbyDependencies);
        }

        public override async Task Remove()
        {
            await base.Remove();

            if (GangArea is { })
                GangArea.InLobby = null;
        }
    }
}
