using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Actions;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.GangSystem;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.Actions;
using TDS.Server.LobbySystem.Chats;
using TDS.Server.LobbySystem.Deathmatch;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.Lobbies.Abstracts;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Notifications;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.TeamHandlers;
using TDS.Server.LobbySystem.Vehicles;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies
{
    public class GangLobby : FreeroamLobby, IGangLobby
    {
        private readonly List<IGangActionLobby> _gangActionLobbies = new List<IGangActionLobby>();

        public IGangLobbyActions Actions { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public GangLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var gangHousesHandler = ServiceProvider.GetRequiredService<GangHousesHandler>();
            var gangsHandler = ServiceProvider.GetRequiredService<GangsHandler>();

            lobbyDependencies ??= new GangLobbyDependencies();

            ((GangLobbyDependencies)lobbyDependencies).Actions ??= new GangLobbyActions();
            lobbyDependencies.Chat ??= new GangLobbyChat(this, LangHelper);
            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.Deathmatch ??= new GangLobbyDeathmatch(this, lobbyDependencies.Events);
            lobbyDependencies.MapHandler ??= new GangLobbyMapHandler(this, lobbyDependencies.Events, GlobalEventsHandler, gangHousesHandler);
            lobbyDependencies.Notifications ??= new GangLobbyNotifications(this, LangHelper);
            lobbyDependencies.Players ??= new GangLobbyPlayers(this, lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new GangLobbyTeamsHandler(this, lobbyDependencies.Events, gangsHandler, TeamsProvider);
            ((GangLobbyDependencies)lobbyDependencies).Vehicles ??= new GangLobbyVehicles(this, lobbyDependencies.Events);

            Actions = ((GangLobbyDependencies)lobbyDependencies).Actions!;

            base.InitDependencies(lobbyDependencies);
        }

        public void AddGangActionLobby(IGangActionLobby lobby)
        {
            lobby.Events.RemoveAfter += GangActionLobbyRemoved;
            lock (_gangActionLobbies)
            {
                _gangActionLobbies.Add(lobby);
            }
        }

        private void GangActionLobbyRemoved(IBaseLobby lobby)
        {
            if (!(lobby is IGangActionLobby gangActionLobby))
                return;
            lock (_gangActionLobbies)
            {
                _gangActionLobbies.Remove(gangActionLobby);
            }
        }

        public void DoForGangActionLobbies(Action<IGangActionLobby> action)
        {
            lock (_gangActionLobbies)
            {
                foreach (var lobby in _gangActionLobbies)
                    action(lobby);
            }
        }
    }
}
