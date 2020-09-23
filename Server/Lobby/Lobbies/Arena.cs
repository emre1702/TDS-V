using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
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
    public class Arena : RoundFightLobby
    {
        protected new ArenaDatabase Database => (ArenaDatabase)base.Database;
        protected new ArenaLobbySync Sync => (ArenaLobbySync)base.Sync;

        public Arena(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }

        protected override void InitDependencies(LobbyDependencies? lobbyDependencies = null)
        {
            void doForPlayersInMainThreadFuncProvider(Action<ITDSPlayer> action) => Players.DoInMain(action);

            lobbyDependencies ??= new LobbyDependencies();

            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(GlobalEventsHandler, this);
            lobbyDependencies.MapHandler ??= new BaseLobbyMapHandler(Entity);
            lobbyDependencies.Sync ??= new ArenaLobbySync(Entity, lobbyDependencies.Events, () => lobbyDependencies.MapHandler.Dimension);
            lobbyDependencies.Teams ??= new BaseLobbyTeamsHandler(Entity);
            lobbyDependencies.Database ??= new BaseLobbyDatabase(this, GlobalDatabaseHandler, lobbyDependencies.Events);
            lobbyDependencies.Chat ??= new BaseLobbyChat(doForPlayersInMainThreadFuncProvider, LangHelper);
            lobbyDependencies.Bans ??= new BaseLobbyBansHandler(lobbyDependencies.Database, lobbyDependencies.Events, LangHelper, lobbyDependencies.Chat, Entity);
            lobbyDependencies.Players ??= new ArenaPlayers(Entity, lobbyDependencies.Events, lobbyDependencies.Teams, lobbyDependencies.Bans);

            base.InitDependencies(lobbyDependencies);
        }
    }
}
