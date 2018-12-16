using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS.Entity;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        public static readonly Dictionary<uint, Lobby> LobbiesByIndex = new Dictionary<uint, Lobby>();
        private static readonly HashSet<uint> dimensionsUsed = new HashSet<uint> { 0 };

        protected readonly Lobbies LobbyEntity;

        public uint Id { get => LobbyEntity.Id; }
        public bool IsOfficial { get => LobbyEntity.IsOfficial; }
        public int StartTotalHP { get => LobbyEntity.StartArmor + LobbyEntity.StartHealth; }

        protected readonly uint Dimension;
        protected readonly Vector3 SpawnPoint;

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
            foreach (Teams team in entity.Teams)
            {
                Teams[team.Index] = team;
                TeamPlayers[team.Index] = new List<TDSPlayer>();
            }
        }

        public virtual void Start()
        {

        }

        protected virtual void Remove()
        {
            LobbiesByIndex.Remove(LobbyEntity.Id);
            dimensionsUsed.Remove(Dimension);

            foreach (TDSPlayer character in players)
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
            return players.Count == 0;
        }
    }
}
