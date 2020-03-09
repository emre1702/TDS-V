using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Interfaces;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Core.Instance.Utility
{
    public class Team
    {
        private Teams _entity;

        public Teams Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                ChatColor = "!$" + Entity.ColorR + "|" + Entity.ColorG + "|" + Entity.ColorB + "$";
            }
        }

        public string ChatColor { get; private set; }
        public List<TDSPlayer> Players { get; private set; } = new List<TDSPlayer>();
        public List<TDSPlayer>? SpectateablePlayers { get; set; }
        public List<TDSPlayer>? AlivePlayers { get; set; }
        public SyncedTeamDataDto SyncedTeamData { get; set; }
        public int SpawnCounter { get; set; }

        public bool IsSpectator => Entity.Index == 0;

        public Team(Teams entity)
        {
            _entity = entity;
            ChatColor = "!$" + Entity.ColorR + "|" + Entity.ColorG + "|" + Entity.ColorB + "$";

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
            player.Player!.SetSkin(Entity.SkinHash != 0 ? (PedHash)Entity.SkinHash : player.FreemodeSkin);
        }

        public void RemovePlayer(TDSPlayer player)
        {
            Players.Remove(player);
            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.ClearTeamPlayers);
        }

        public void ClearPlayers()
        {
            FuncIterate((player, team) =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.ClearTeamPlayers);
                foreach (var target in Players)
                {
                    if (target == player)
                        continue;
                    target.Player!.DisableVoiceTo(player.Player);
                    player.Player!.DisableVoiceTo(target.Player);
                }
            });
            Players.Clear();
        }

        public void SyncAddedPlayer(TDSPlayer player)
        {
            string json = Serializer.ToClient(Players.Select(p => p.Player!.Handle.Value));
            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.SyncTeamPlayers, json);
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                NAPI.ClientEvent.TriggerClientEvent(target.Player, DToClientEvent.PlayerJoinedTeam, player.Player!.Handle.Value);
                if (!player.HasRelationTo(target, EPlayerRelation.Block) && !target.IsVoiceMuted)
                    target.Player!.EnableVoiceTo(player.Player);
                if(!target.HasRelationTo(player, EPlayerRelation.Block) && !player.IsVoiceMuted)
                    player.Player.EnableVoiceTo(target.Player);
            }
        }

        public void SyncRemovedPlayer(TDSPlayer player)
        {
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                NAPI.ClientEvent.TriggerClientEvent(target.Player, DToClientEvent.PlayerLeftTeam, player.Player!.Handle.Value);
                target.Player!.DisableVoiceTo(player.Player);
                player.Player.DisableVoiceTo(target.Player);
            }
        }

        public void SyncAllPlayers()
        {
            string json = Serializer.ToClient(Players.Select(p => p.Player!.Handle.Value));
            foreach (var player in Players)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.SyncTeamPlayers, json);
                foreach (var target in Players)
                {
                    if (target == player)
                        continue;
                    if (!player.HasRelationTo(target, EPlayerRelation.Block) && !target.IsVoiceMuted)
                        target.Player!.EnableVoiceTo(player.Player);
                    if (!target.HasRelationTo(player, EPlayerRelation.Block) && !player.IsVoiceMuted)
                        player.Player!.EnableVoiceTo(target.Player);
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
