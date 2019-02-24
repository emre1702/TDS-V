using System;
using System.Collections.Generic;
using TDS_Common.Dto;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Utility
{
    class Team
    {
        private Teams _entity;

        public Teams Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                ChatColor = "!{" + Entity.ColorR + "|" + Entity.ColorG + "|" + Entity.ColorB + "}";
            }
        }
        public string ChatColor;
        public List<TDSPlayer> Players { get; set; } = new List<TDSPlayer>();
        public List<TDSPlayer> SpectateablePlayers { get; set; }
        public List<TDSPlayer> AlivePlayers { get; set; }
        public SyncedTeamDataDto SyncedTeamData { get; set; }
        public int SpawnCounter;

        public Team(Teams entity)
        {
            Entity = entity;

            if (!IsSpectator)
            {
                SpectateablePlayers = new List<TDSPlayer>();
                AlivePlayers = new List<TDSPlayer>();
            }

            SyncedTeamData = new SyncedTeamDataDto()
            {
                Index = (int)Entity.Index,
                Name = Entity.Name,
                Color = System.Drawing.Color.FromArgb(Entity.ColorR, Entity.ColorG, Entity.ColorB),
                AmountPlayers = new SyncedTeamPlayerAmountDto()
            };
        }

        public void FuncIterate(Action<TDSPlayer, Team> func)
        {
            foreach (var player in Players)
            {
                func(player, this);
            }
        }

        public bool IsSpectator => Entity.Id == 0;

        public static bool operator ==(Team a, Team b)
        {
            if (a is null && b is null)
                return true;
            if (a is null)
                return false;
            if (b is null)
                return false;
            return a.Entity.Id == b.Entity.Id;
        }

        public static bool operator !=(Team a, Team b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is Team team && team.Entity.Id == this.Entity.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
