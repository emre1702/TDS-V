using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.Deathmatch;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class MainMenu : BaseLobby, IMainMenu
    {
        public MainMenu(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler, ILoggingHandler loggingHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider)
            : base(entity, databaseHandler, langHelper, eventsHandler, loggingHandler, serviceProvider, teamsProvider)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var bansHandler = ServiceProvider.GetRequiredService<BansHandler>();

            lobbyDependencies ??= new MainMenuDependencies();

            lobbyDependencies.Bans ??= new MainMenuBansHandler(this, LangHelper, bansHandler);
            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.Deathmatch ??= new MainMenuDeathmatch(this, lobbyDependencies.Events);
            lobbyDependencies.MapHandler ??= new MainMenuMapHandler(this, lobbyDependencies.Events);
            lobbyDependencies.Players ??= new MainMenuPlayers(this, lobbyDependencies.Events);

            base.InitDependencies(lobbyDependencies);
        }
    }
}
