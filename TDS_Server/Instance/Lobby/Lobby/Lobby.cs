using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Instance.Lobby
{
    public partial class Lobby : EntityWrapperClass
    {
        public static readonly Dictionary<int, Lobby> LobbiesByIndex = new Dictionary<int, Lobby>();
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint> { 0 };

        private readonly TDSDbContext _dbContext;
        public readonly Lobbies LobbyEntity;

        public int Id => LobbyEntity.Id;
        public ELobbyType Type => LobbyEntity.Type;
        public string Name => LobbyEntity.Name;
        public bool IsOfficial => LobbyEntity.IsOfficial;
        public string CreatorName => LobbyEntity.Owner?.Name ?? "?";
        public string OwnerName => CreatorName;
        public int StartTotalHP => LobbyEntity.StartArmor + LobbyEntity.StartHealth;

        public readonly uint Dimension;
        protected readonly Vector3 SpawnPoint;

        private readonly SyncedLobbySettingsDto _syncedLobbySettings;

        public Lobby(Lobbies entity)
        {
            _dbContext = new TDSDbContext();
            LobbyEntity = entity;

            _dbContext.Attach(entity);

            Dimension = GetFreeDimension();
            SpawnPoint = new Vector3(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );

            _dimensionsUsed.Add(Dimension);

            Teams = new Team[entity.Teams.Count];
            foreach (Teams teamEntity in entity.Teams)
            {
                Team team = new Team(teamEntity);
                Teams[team.Entity.Index] = team;
            }

            _syncedLobbySettings = new SyncedLobbySettingsDto
            (
                Id: entity.Id,
                Name: entity.Name,
                Type: entity.Type,
                IsOfficial: entity.IsOfficial,
                BombDefuseTimeMs: entity.LobbyRoundSettings?.BombDefuseTimeMs,
                BombPlantTimeMs: entity.LobbyRoundSettings?.BombPlantTimeMs,
                SpawnAgainAfterDeathMs: entity.SpawnAgainAfterDeathMs,
                CountdownTime: entity.LobbyRoundSettings?.CountdownTime,
                RoundTime: entity.LobbyRoundSettings?.RoundTime,
                BombDetonateTimeMs: entity.LobbyRoundSettings?.BombDetonateTimeMs,
                InLobbyWithMaps: this is Arena,
                MapLimitTime: entity.LobbyMapSettings?.MapLimitTime,
                MapLimitType: entity.LobbyMapSettings?.MapLimitType
            );
        }

        public virtual void Start()
        {
        }

        protected async virtual void Remove()
        {
            if (IsOfficial)
                return;
            LobbiesByIndex.Remove(LobbyEntity.Id);
            LobbyManager.RemoveLobby(this);
            _dimensionsUsed.Remove(Dimension);

            foreach (TDSPlayer character in Players.ToArray())
            {
                RemovePlayer(character);
            }

            await ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Remove(LobbyEntity);
                await dbContext.SaveChangesAsync();
                dbContext.Dispose();
            });

           
        }

        private static uint GetFreeDimension()
        {
            uint tryid = 0;
            while (_dimensionsUsed.Contains(tryid))
                ++tryid;
            return tryid;
        }

        protected bool IsEmpty()
        {
            return Players.Count == 0;
        }

      
        /// <summary>
        /// Call this on lobby create.
        /// </summary>
        public Task AddToDB()
        {
            return ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Entry(LobbyEntity).State = EntityState.Added;
                await dbContext.SaveChangesAsync();

                await dbContext.Entry(LobbyEntity).Reference(e => e.Owner).LoadAsync();
            });

        }
    }
}