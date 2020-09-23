using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.Models;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class MainMenu : BaseLobby
    {
        private readonly BansHandler _globalBansHandler;

        public MainMenu(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler, BansHandler globalBansHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
            _globalBansHandler = globalBansHandler;
        }

        protected override void InitDependencies(LobbyDependencies? lobbyDependencies = null)
        {
            void doForPlayersInMainThreadFuncProvider(Action<ITDSPlayer> action) => Players.DoInMain(action);

            lobbyDependencies ??= new LobbyDependencies();
            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(GlobalEventsHandler, this);
            lobbyDependencies.Teams ??= new BaseLobbyTeamsHandler(Entity);
            lobbyDependencies.Chat ??= new BaseLobbyChat(doForPlayersInMainThreadFuncProvider, LangHelper);

            lobbyDependencies.Database ??= new BaseLobbyDatabase(this, GlobalDatabaseHandler, lobbyDependencies.Events);
            lobbyDependencies.Bans ??= new MainMenuBansHandler(lobbyDependencies.Database, lobbyDependencies.Events, LangHelper, _globalBansHandler, lobbyDependencies.Chat, Entity);
            lobbyDependencies.Players ??= new BaseLobbyPlayers(Entity, lobbyDependencies.Events, lobbyDependencies.Teams, lobbyDependencies.Bans);

            base.InitDependencies(lobbyDependencies);
        }
    }
}
