using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Dto;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public static readonly Dictionary<int, Lobby> LobbiesByIndex = new Dictionary<int, Lobby>();
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint> { 0 };

        public TDSNewContext DbContext { get; set; }
        public readonly Lobbies LobbyEntity;

        public int Id => LobbyEntity.Id;
        public string Name => LobbyEntity.Name;
        public bool IsOfficial => LobbyEntity.IsOfficial;
        public string CreatorName => LobbyEntity.Owner.Name;
        public string OwnerName => CreatorName;
        public int StartTotalHP => LobbyEntity.StartArmor + LobbyEntity.StartHealth;

        public readonly uint Dimension;
        protected readonly Vector3 SpawnPoint;

        private readonly SyncedLobbySettingsDto _syncedLobbySettings;

        public Lobby(Lobbies entity)
        {
            DbContext = new TDSNewContext();
            LobbyEntity = entity;

            DbContext.Attach(entity);

            Dimension = GetFreeDimension();
            SpawnPoint = new Vector3(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );

            LobbiesByIndex[entity.Id] = this;
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
                DieAfterOutsideMapLimitTime: entity.DieAfterOutsideMapLimitTime,
                InLobbyWithMaps: this is Arena
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
            _dimensionsUsed.Remove(Dimension);

            foreach (TDSPlayer character in Players.ToArray())
            {
                RemovePlayer(character);
            }

            DbContext.Remove(LobbyEntity);
            await DbContext.SaveChangesAsync();
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
    }
}