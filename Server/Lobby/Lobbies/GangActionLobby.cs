using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gangs;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Maps;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.RoundsHandlers;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class GangActionLobby : RoundFightLobby, IGangActionLobby
    {
        public IGangwarArea GangArea { get; }

        public new IGangActionLobbyTeamsHandler Teams => (IGangActionLobbyTeamsHandler)base.Teams;

        public GangActionLobby(LobbyDb entity, IGangwarArea gangArea, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider)
            : base(entity, databaseHandler, langHelper, eventsHandler, loggingHandler, serviceProvider, teamsProvider)
        {
            GangArea = gangArea;
            gangArea.InLobby = this;
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var settingsHandler = ServiceProvider.GetRequiredService<ISettingsHandler>();
            var mapsLoadingHandler = ServiceProvider.GetRequiredService<MapsLoadingHandler>();
            var gamemodesProvider = ServiceProvider.GetRequiredService<IGamemodesProvider>();

            lobbyDependencies ??= new GangActionLobbyDependencies();

            lobbyDependencies.Bans ??= new GangActionLobbyBansHandler(this, LangHelper);
            lobbyDependencies.Events ??= new RoundFightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.MapHandler ??=
                new GangActionLobbyMapHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, settingsHandler, mapsLoadingHandler);
            lobbyDependencies.Players ??= new GangActionLobbyPlayers(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((GangActionLobbyDependencies)lobbyDependencies).Rounds ??=
                new GangActionLobbyRoundsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, gamemodesProvider);
            lobbyDependencies.Sync ??= new GangActionLobbySync(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new GangActionLobbyTeamsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events,
                LangHelper, settingsHandler, TeamsProvider);

            base.InitDependencies(lobbyDependencies);
        }

        public override async Task Remove()
        {
            await base.Remove().ConfigureAwait(false);

            if (GangArea is { })
                GangArea.InLobby = null;
        }
    }
}
