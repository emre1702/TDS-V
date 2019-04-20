using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
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
        public string ChatColor { get; private set; }
        public List<TDSPlayer> Players { get; private set; } = new List<TDSPlayer>();
        public List<TDSPlayer>? SpectateablePlayers { get; set; }
        public List<TDSPlayer>? AlivePlayers { get; set; }
        public SyncedTeamDataDto SyncedTeamData { get; set; }
        public int SpawnCounter;

        public bool IsSpectator => Entity.Index == 0;

        public Team(Teams entity)
        {
            _entity = entity;
            ChatColor = "!{" + Entity.ColorR + "|" + Entity.ColorG + "|" + Entity.ColorB + "}";

            if (!IsSpectator)
            {
                SpectateablePlayers = new List<TDSPlayer>();
                AlivePlayers = new List<TDSPlayer>();
            }

            SyncedTeamData = new SyncedTeamDataDto
            (
                index: (int)Entity.Index,
                name: Entity.Name,
                color: System.Drawing.Color.FromArgb(Entity.ColorR, Entity.ColorG, Entity.ColorB),
                amountPlayers: new SyncedTeamPlayerAmountDto()
            );
        }

        public void FuncIterate(Action<TDSPlayer, Team> func)
        {
            foreach (var player in Players)
            {
                func(player, this);
            }
        }

        public void AddPlayer(TDSPlayer player)
        {
            foreach (var target in Players)
            {
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.PlayerJoinedTeam, player.Client);
            }
            Players.Add(player);
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SyncTeamPlayers, JsonConvert.SerializeObject(Players.Select(p => p.Client.Value)));
        }

        public void RemovePlayer(TDSPlayer player)
        {
            Players.Remove(player);
            foreach (var target in Players)
            {
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.PlayerLeftTeam, player.Client);
            }
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.ClearTeamPlayers);
        }

        public void ClearPlayers()
        {
            FuncIterate((player, team) =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.ClearTeamPlayers);
            });
            Players.Clear();
        }

        public static bool operator ==(Team? a, Team? b)
        {
            if (a is null)
                return (b is null);
            if (b is null)
                return false;
            return a.Entity.Id == b.Entity.Id;
        }

        public static bool operator !=(Team? a, Team? b)
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
