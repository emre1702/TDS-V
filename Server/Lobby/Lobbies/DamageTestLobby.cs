using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Deathmatch;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Weapons;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class DamageTestLobby : FightLobby, IDamageTestLobby
    {
        public DamageTestLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper,
            EventsHandler eventsHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider)
        {
        }

        public DamageTestLobby(ITDSPlayer owner, IDatabaseHandler databaseHandler, LangHelper langHelper, LobbiesHandler lobbiesHandler,
            EventsHandler eventsHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider)
            : base(CreateEntity(owner, lobbiesHandler.DamageTestLobbyDummy.Entity), databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var damageSys = ServiceProvider.GetRequiredService<IDamagesys>();

            lobbyDependencies ??= new DamageTestLobbyDependencies();

            lobbyDependencies.Bans ??= new DamageTestLobbyBansHandler(this, LangHelper);
            lobbyDependencies.Events ??= new FightLobbyEventsHandler(this, GlobalEventsHandler);
            lobbyDependencies.Deathmatch ??= new DamageTestLobbyDeathmatch(this, (FightLobbyEventsHandler)lobbyDependencies.Events, damageSys, LangHelper);
            lobbyDependencies.Players ??= new DamageTestLobbyPlayers(this, (FightLobbyEventsHandler)lobbyDependencies.Events);
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
                DefaultSpawnRotation = dummy.DefaultSpawnRotation
            };

            return entity;
        }
    }
}
