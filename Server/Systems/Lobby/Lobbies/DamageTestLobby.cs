using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.DamageSystem;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.BansHandlers;
using TDS.Server.LobbySystem.Deathmatch;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.Lobbies.Abstracts;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.Weapons;
using TDS.Shared.Data.Enums;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies
{
    public class DamageTestLobby : FightLobby, IDamageTestLobby
    {
        public new IDamageTestLobbyDeathmatch Deathmatch => (IDamageTestLobbyDeathmatch)base.Deathmatch;

        public DamageTestLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper,
            EventsHandler eventsHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider,
            ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
        }

        public DamageTestLobby(ITDSPlayer owner, IDatabaseHandler databaseHandler, LangHelper langHelper, LobbiesHandler lobbiesHandler,
            EventsHandler eventsHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider,
            ILoggingHandler loggingHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(CreateEntity(owner, lobbiesHandler.DamageTestLobbyDummy.Entity), databaseHandler, langHelper, eventsHandler, serviceProvider,
                  teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            lobbyDependencies ??= new DamageTestLobbyDependencies();

            lobbyDependencies.Bans ??= new DamageTestLobbyBansHandler(this, LangHelper);
            lobbyDependencies.Events ??= new FightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            ((DamageTestLobbyDependencies)lobbyDependencies).DamageHandler ??= ServiceProvider.GetRequiredService<IDamageHandler>();
            lobbyDependencies.Deathmatch ??= new DamageTestLobbyDeathmatch(this, (FightLobbyEventsHandler)lobbyDependencies.Events,
                ((DamageTestLobbyDependencies)lobbyDependencies).DamageHandler!);
            lobbyDependencies.Players ??= new DamageTestLobbyPlayers(this, (FightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.MapHandler ??= new DamageTestLobbyMapHandler(this, (FightLobbyEventsHandler)lobbyDependencies.Events);
            ((DamageTestLobbyDependencies)lobbyDependencies).Weapons ??= new DamageTestLobbyWeapons(this, (FightLobbyEventsHandler)lobbyDependencies.Events, ServiceProvider);

            base.InitDependencies(lobbyDependencies);
        }

        private static LobbyDb CreateEntity(ITDSPlayer player, LobbyDb dummy)
        {
            var entity = new LobbyDb
            {
                Name = "DamageTest-" + player.Name,
                Teams = new List<Teams>
                {
                    new Teams { Index = 0, Name = "None", ColorR = 222, ColorB = 222, ColorG = 222 },
                    new Teams { Index = 1, Name = player.Name, ColorR = 222, ColorB = 222, ColorG = 222 }
                },
                Type = LobbyType.DamageTestLobby,
                OwnerId = player.Entity?.Id ?? -1,
                IsTemporary = true,
                DefaultSpawnX = dummy.DefaultSpawnX,
                DefaultSpawnY = dummy.DefaultSpawnY,
                DefaultSpawnZ = dummy.DefaultSpawnZ,
                DefaultSpawnRotation = dummy.DefaultSpawnRotation,
                FightSettings = new LobbyFightSettings()
            };

            return entity;
        }
    }
}