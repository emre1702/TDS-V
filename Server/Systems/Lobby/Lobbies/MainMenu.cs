using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Account;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.BansHandlers;
using TDS.Server.LobbySystem.Chats;
using TDS.Server.LobbySystem.Database;
using TDS.Server.LobbySystem.Deathmatch;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.Lobbies.Abstracts;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies
{
    public class MainMenu : BaseLobby, IMainMenu
    {
        public MainMenu(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler)
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
