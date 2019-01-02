using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Dto;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;

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

            Teams = new Teams[entity.Teams.Count];
            TeamPlayers = new List<TDSPlayer>[entity.Teams.Count];
            SyncedTeamDatas = new SyncedTeamDataDto[entity.Teams.Count];
            foreach (Teams team in entity.Teams)
            {
                Teams[team.Index] = team;
                TeamPlayers[team.Index] = new List<TDSPlayer>();
                SyncedTeamDatas[team.Index] = new SyncedTeamDataDto()
                {
                    Index = (int)team.Index,
                    Name = team.Name,
                    Color = System.Drawing.Color.FromArgb(team.ColorR, team.ColorG, team.ColorB),
                    AmountPlayers = new SyncedTeamPlayerAmountDto()
                };
            }

            syncedLobbySettings = new SyncedLobbySettingsDto()
            {
                Id = entity.Id,
                BombDefuseTimeMs = entity.BombDefuseTimeMs,
                BombPlantTimeMs = entity.BombPlantTimeMs,
                SpawnAgainAfterDeathMs = entity.SpawnAgainAfterDeathMs,
                CountdownTime = entity.CountdownTime,
                RoundTime = entity.RoundTime,
                BombDetonateTimeMs = entity.BombDetonateTimeMs,
                DieAfterOutsideMapLimitTime = entity.DieAfterOutsideMapLimitTime,
                InLobbyWithMaps = this is Arena
            };
        }

        public virtual void Start()
        {

        }

        protected virtual void Remove()
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
            }
        }

#warning Todo check if thats needed after all lobbies implementation
        /*private static uint GetFreeID()
        {
            uint tryid = 0;
            while (LobbiesByIndex.ContainsKey(tryid))
                ++tryid;
            return tryid;
        }*/

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
