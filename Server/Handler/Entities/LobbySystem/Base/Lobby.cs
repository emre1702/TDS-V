﻿using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class Lobby : DatabaseEntityWrapper, ILobby
    {
        protected readonly BansHandler BansHandler;
        protected readonly BonusBotConnectorClient BonusBotConnectorClient;
        protected readonly DataSyncHandler DataSyncHandler;
        protected readonly EventsHandler EventsHandler;
        protected readonly LangHelper LangHelper;
        protected readonly LobbiesHandler LobbiesHandler;
        protected readonly Serializer Serializer;
        protected readonly ISettingsHandler SettingsHandler;
        protected SyncedLobbySettings SyncedLobbySettings;

        public Lobby(
            Lobbies entity,
            bool isGangActionLobby,

            TDSDbContext dbContext,
            ILoggingHandler loggingHandler,
            Serializer serializer,
            LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler,
            LangHelper langHelper,
            DataSyncHandler dataSyncHandler,
            EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient,
            BansHandler bansHandler) : base(dbContext, loggingHandler)
        {
            Serializer = serializer;
            LobbiesHandler = lobbiesHandler;
            SettingsHandler = settingsHandler;
            LangHelper = langHelper;
            DataSyncHandler = dataSyncHandler;
            EventsHandler = eventsHandler;
            BonusBotConnectorClient = bonusBotConnectorClient;
            BansHandler = bansHandler;

            Entity = entity;

            dbContext.Attach(entity);

            Dimension = lobbiesHandler.GetFreeDimension();
            SpawnPoint = new Vector3(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );

            Teams = new List<ITeam>(entity.Teams.Count);
            foreach (Teams teamEntity in entity.Teams.OrderBy(t => t.Index))
            {
                var team = new Team(serializer, teamEntity);
                Teams.Add(team);
            }

            SyncedLobbySettings = new SyncedLobbySettings
            (
                Id: entity.Id,
                Name: entity.Name,
                Type: entity.Type,
                IsOfficial: entity.IsOfficial,
                BombDefuseTimeMs: entity.LobbyRoundSettings?.BombDefuseTimeMs,
                BombPlantTimeMs: entity.LobbyRoundSettings?.BombPlantTimeMs,
                SpawnAgainAfterDeathMs: entity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                CountdownTime: isGangActionLobby ? 0 : entity.LobbyRoundSettings?.CountdownTime,
                RoundTime: entity.LobbyRoundSettings?.RoundTime,
                BombDetonateTimeMs: entity.LobbyRoundSettings?.BombDetonateTimeMs,
                InLobbyWithMaps: this is Arena,
                MapLimitTime: entity.LobbyMapSettings?.MapLimitTime,
                MapLimitType: entity.LobbyMapSettings?.MapLimitType,
                StartHealth: entity.FightSettings?.StartHealth ?? 100,
                StartArmor: entity.FightSettings?.StartArmor ?? 100,
                IsGangActionLobby: IsGangActionLobby
            );

            eventsHandler.PlayerLoggedOutBefore += OnPlayerLoggedOut;
        }

        public string CreatorName => Entity.Owner?.Name ?? "?";
        public uint Dimension { get; }
        public Lobbies Entity { get; }

        public bool IsGangActionLobby { get; set; }
        public string OwnerName => CreatorName;
        public int StartTotalHP => (Entity.FightSettings?.StartArmor ?? 100) + (Entity.FightSettings?.StartHealth ?? 100);
        public int Id => Entity.Id;
        public bool IsOfficial => Entity.IsOfficial;
        public string Name => Entity.Name;
        public LobbyType Type => Entity.Type;

        protected Vector3 SpawnPoint { get; }

        /// <summary>
        /// Call this on lobby create.
        /// </summary>
        public async Task AddToDB()
        {
            await ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Add(Entity);
                await dbContext.SaveChangesAsync();

                await dbContext.Entry(Entity)
                    .Reference(e => e.Owner)
                    .LoadAsync();

                await dbContext.Entry(Entity)
                    .Collection(e => e.LobbyMaps)
                    .Query()
                    .Include(e => e.Map)
                    .LoadAsync();

                // Reload again because Entity could have changed (default values in DB)
                SyncedLobbySettings = new SyncedLobbySettings
                (
                    Id: Entity.Id,
                    Name: Entity.Name,
                    Type: Entity.Type,
                    IsOfficial: Entity.IsOfficial,
                    BombDefuseTimeMs: Entity.LobbyRoundSettings?.BombDefuseTimeMs,
                    BombPlantTimeMs: Entity.LobbyRoundSettings?.BombPlantTimeMs,
                    SpawnAgainAfterDeathMs: Entity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                    CountdownTime: IsGangActionLobby ? 0 : Entity.LobbyRoundSettings?.CountdownTime,
                    RoundTime: Entity.LobbyRoundSettings?.RoundTime,
                    BombDetonateTimeMs: Entity.LobbyRoundSettings?.BombDetonateTimeMs,
                    InLobbyWithMaps: this is Arena,
                    MapLimitTime: Entity.LobbyMapSettings?.MapLimitTime,
                    MapLimitType: Entity.LobbyMapSettings?.MapLimitType,
                    StartHealth: Entity.FightSettings?.StartHealth ?? 100,
                    StartArmor: Entity.FightSettings?.StartArmor ?? 100,
                    IsGangActionLobby: IsGangActionLobby
                );
            });
        }

        public virtual void Start()
        {
        }

        protected bool IsEmpty()
        {
            return Players.Count == 0;
        }

        protected async virtual Task Remove()
        {
            LobbiesHandler.RemoveLobby(this);

            foreach (var player in Players.Values.ToList())
            {
                await RemovePlayer(player);
            }

            await ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Remove(Entity);
                await dbContext.SaveChangesAsync();
            });
        }
    }
}
