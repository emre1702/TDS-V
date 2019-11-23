using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Instance.Utility
{
    internal class Team
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
                index: Entity.Index,
                name: Entity.Name,
                color: new ColorDto(Entity.ColorR, Entity.ColorG, Entity.ColorB),
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
            Players.Add(player);
            player.Client.SetSkin(Entity.SkinHash != 0 ? (PedHash)Entity.SkinHash : player.FreemodeSkin);
        }

        public void RemovePlayer(TDSPlayer player)
        {
            Players.Remove(player);
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.ClearTeamPlayers);
        }

        public void ClearPlayers()
        {
            FuncIterate((player, team) =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.ClearTeamPlayers);
                foreach (var target in Players)
                {
                    if (target == player)
                        continue;
                    target.Client.DisableVoiceTo(player.Client);
                    player.Client.DisableVoiceTo(target.Client);
                }
            });
            Players.Clear();
        }

        public void SyncAddedPlayer(TDSPlayer player)
        {
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.PlayerJoinedTeam, player.Client.Handle.Value);
                if (!player.HasRelationTo(target, EPlayerRelation.Block) && !target.IsVoiceMuted)
                    target.Client.EnableVoiceTo(player.Client);
                if(!target.HasRelationTo(player, EPlayerRelation.Block) && !player.IsVoiceMuted)
                    player.Client.EnableVoiceTo(target.Client);
            }
        }

        public void SyncRemovedPlayer(TDSPlayer player)
        {
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.PlayerLeftTeam, player.Client.Handle.Value);
                target.Client.DisableVoiceTo(player.Client);
                player.Client.DisableVoiceTo(target.Client);
            }
        }

        public void SyncAllPlayers()
        {
            string json = Serializer.ToClient(Players.Select(p => p.Client.Value));
            foreach (var player in Players)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SyncTeamPlayers, json);
                foreach (var target in Players)
                {
                    if (target == player)
                        continue;
                    if (!player.HasRelationTo(target, EPlayerRelation.Block) && !target.IsVoiceMuted)
                        target.Client.EnableVoiceTo(player.Client);
                    if (!target.HasRelationTo(player, EPlayerRelation.Block) && !player.IsVoiceMuted)
                        player.Client.EnableVoiceTo(target.Client);
                }
            }
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

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Team team && team.Entity.Id == this.Entity.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}