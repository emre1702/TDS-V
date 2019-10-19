using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TDS_Common.Dto;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public static readonly Dictionary<int, Lobby> LobbiesByIndex = new Dictionary<int, Lobby>();
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint> { 0 };

        private readonly TDSNewContext _dbContext;
        public readonly Lobbies LobbyEntity;

        public int Id => LobbyEntity.Id;
        public string Name => LobbyEntity.Name;
        public bool IsOfficial => LobbyEntity.IsOfficial;
        public string CreatorName => LobbyEntity.Owner?.Name ?? "?";
        public string OwnerName => CreatorName;
        public int StartTotalHP => LobbyEntity.StartArmor + LobbyEntity.StartHealth;

        public readonly uint Dimension;
        protected readonly Vector3 SpawnPoint;

        private readonly SyncedLobbySettingsDto _syncedLobbySettings;
        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1);
        private bool _usingDBContext;

        public Lobby(Lobbies entity)
        {
            _dbContext = new TDSNewContext();
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

        public async Task ExecuteForDBAsync(Func<TDSNewContext, Task> action)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                await action(_dbContext);
            }
            finally
            {
                if (!wasInDBContextBefore)
                {
                    _dbContextSemaphore.Release();
                    _usingDBContext = false;
                }
            }
        }

        public async Task<T> ExecuteForDBAsync<T>(Func<TDSNewContext, Task<T>> action)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                return await action(_dbContext);
            }
            finally
            {
                if (!wasInDBContextBefore)
                {
                    _dbContextSemaphore.Release();
                    _usingDBContext = false;
                }
            }
        }

        public async Task ExecuteForDB(Action<TDSNewContext> action)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                action(_dbContext);
            }
            finally
            {
                if (!wasInDBContextBefore)
                {
                    _dbContextSemaphore.Release();
                    _usingDBContext = false;
                }
            }
        }
    }
}