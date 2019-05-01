using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Dto;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public static readonly Dictionary<uint, Lobby> LobbiesByIndex = new Dictionary<uint, Lobby>();
        private static readonly HashSet<uint> dimensionsUsed = new HashSet<uint> { 0 };

        protected readonly Lobbies LobbyEntity;

        public uint Id => LobbyEntity.Id;
        public string Name => LobbyEntity.Name;
        public bool IsOfficial => LobbyEntity.IsOfficial;
        public string CreatorName => LobbyEntity.OwnerNavigation.Name;
        public string OwnerName => CreatorName;
        public int StartTotalHP => LobbyEntity.StartArmor + LobbyEntity.StartHealth;

        protected readonly uint Dimension;
        protected readonly Vector3 SpawnPoint;

        private SyncedLobbySettingsDto syncedLobbySettings;

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
            dimensionsUsed.Add(Dimension);

            Teams = new Team[entity.Teams.Count];
            foreach (Teams teamEntity in entity.Teams)
            {
                Team team = new Team(teamEntity);
                Teams[team.Entity.Index] = team;
            }

            syncedLobbySettings = new SyncedLobbySettingsDto
            (
                Id: entity.Id,
                Name: entity.Name,
                BombDefuseTimeMs: entity.BombDefuseTimeMs,
                BombPlantTimeMs: entity.BombPlantTimeMs,
                SpawnAgainAfterDeathMs: entity.SpawnAgainAfterDeathMs,
                CountdownTime: entity.CountdownTime,
                RoundTime: entity.RoundTime,
                BombDetonateTimeMs: entity.BombDetonateTimeMs,
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
            dimensionsUsed.Remove(Dimension);

            foreach (TDSPlayer character in Players)
            {
                RemovePlayer(character);
            }

            using (TDSNewContext dbcontext = new TDSNewContext())
            {
                dbcontext.Remove(LobbyEntity);
                await dbcontext.SaveChangesAsync();
            }
        }

        private static uint GetFreeDimension()
        {
            uint tryid = 0;
            while (dimensionsUsed.Contains(tryid))
                ++tryid;
            return tryid;
        }

        protected bool IsEmpty()
        {
            return Players.Count == 0;
        }
    }
}