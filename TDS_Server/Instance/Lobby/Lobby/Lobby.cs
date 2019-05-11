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

        protected readonly Lobbies LobbyEntity;

        public int Id => LobbyEntity.Id;
        public string Name => LobbyEntity.Name;
        public bool IsOfficial => LobbyEntity.IsOfficial;
        public string CreatorName => LobbyEntity.OwnerNavigation.Name;
        public string OwnerName => CreatorName;
        public int StartTotalHP => LobbyEntity.StartArmor + LobbyEntity.StartHealth;

        protected readonly uint Dimension;
        protected readonly Vector3 SpawnPoint;

        private readonly SyncedLobbySettingsDto _syncedLobbySettings;

        public Lobby(Lobbies entity)
        {
            LobbyEntity = entity;

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
            LobbiesByIndex.Remove(LobbyEntity.Id);
            _dimensionsUsed.Remove(Dimension);

            foreach (TDSPlayer character in Players)
            {
                RemovePlayer(character);
            }

            using TDSNewContext dbcontext = new TDSNewContext();
            dbcontext.Remove(LobbyEntity);
            await dbcontext.SaveChangesAsync();
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