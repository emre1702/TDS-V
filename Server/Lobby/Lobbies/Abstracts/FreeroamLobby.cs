using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Freeroam;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.Freeroam;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class FreeroamLobby : BaseLobby, IFreeroamLobby
    {
        public IFreeroamLobbyFreeroam Freeroam { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected FreeroamLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var freeroamDataHandler = ServiceProvider.GetRequiredService<FreeroamDataHandler>();

            lobbyDependencies ??= new FreeroamLobbyDependencies();

            ((FreeroamLobbyDependencies)lobbyDependencies).Freeroam ??= new FreeroamLobbyFreeroam(this, freeroamDataHandler);

            Freeroam = ((FreeroamLobbyDependencies)lobbyDependencies).Freeroam!;

            base.InitDependencies(lobbyDependencies);
        }
    }
}
