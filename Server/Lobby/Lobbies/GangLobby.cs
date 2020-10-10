using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Actions;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Actions;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Deathmatch;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Notifications;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Server.LobbySystem.Vehicles;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class GangLobby : FreeroamLobby, IGangLobby
    {
        private readonly List<IGangActionLobby> _gangActionLobbies = new List<IGangActionLobby>();

        public IGangLobbyActions Actions { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public GangLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider)
            : base(entity, databaseHandler, langHelper, eventsHandler, loggingHandler, serviceProvider)
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
            lobbyDependencies.Teams ??= new GangLobbyTeamsHandler(this, lobbyDependencies.Events, gangsHandler);
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
