using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        public static readonly Dictionary<uint, Lobby> LobbiesByIndex = new Dictionary<uint, Lobby>();
        private static readonly HashSet<uint> dimensionsUsed = new HashSet<uint> { 0 };

        protected readonly Lobbies entity;

        public uint Id { get => this.entity.Id; }
        public bool IsOfficial { get => this.entity.IsOfficial; }

        private readonly uint dimension;
        protected readonly Vector3 spawnPoint;

        public Lobby (Lobbies entity)
        {
            this.entity = entity;

            this.dimension = GetFreeDimension();
            this.spawnPoint = new Vector3(
                this.entity.DefaultSpawnX, 
                this.entity.DefaultSpawnY, 
                this.entity.DefaultSpawnZ
            );

            LobbiesByIndex[this.entity.Id] = this;
            dimensionsUsed.Add(this.dimension);
        }

        protected virtual void Remove()
        {
            LobbiesByIndex.Remove(this.entity.Id);
            dimensionsUsed.Remove(this.dimension);
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
            return this.players.Count == 0;
        }
    }
}
