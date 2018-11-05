using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        public static readonly Dictionary<uint, Lobby> LobbiesByIndex = new Dictionary<uint, Lobby>();
        private static readonly HashSet<uint> dimensionsUsed = new HashSet<uint> { 0 };

        protected readonly Lobbies entity;

        public uint Id { get => entity.Id; }
        public bool IsOfficial { get => entity.IsOfficial; }

        protected readonly uint dimension;
        protected readonly Vector3 spawnPoint;

        public Lobby(Lobbies entity)
        {
            this.entity = entity;

            dimension = GetFreeDimension();
            spawnPoint = new Vector3(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );

            LobbiesByIndex[entity.Id] = this;
            dimensionsUsed.Add(dimension);
        }

        protected virtual void Remove()
        {
            LobbiesByIndex.Remove(entity.Id);
            dimensionsUsed.Remove(dimension);
        }

        private static uint GetFreeID()
        {
            uint tryid = 0;
            while (LobbiesByIndex.ContainsKey(tryid))
                ++tryid;
            return tryid;
        }

        private static uint GetFreeDimension()
        {
            uint tryid = 0;
            while (dimensionsUsed.Contains(tryid))
                ++tryid;
            return tryid;
        }

        private bool IsEmpty()
        {
            return players.Count == 0;
        }
    }
}
