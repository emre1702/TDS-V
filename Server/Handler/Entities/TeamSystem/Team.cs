using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.TeamSystem
{
    public class Team : ITeam
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

        private Serializer _serializer;
        private IModAPI _modAPI;

        public Team(Serializer serializer, IModAPI modAPI, Teams entity)
        {
            _serializer = serializer;
            _modAPI = modAPI;
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
            player.ModPlayer?.SetSkin(Entity.SkinHash != 0 ? (PedHash)Entity.SkinHash : player.FreemodeSkin);
        }

        public void RemovePlayer(TDSPlayer player)
        {
            Players.Remove(player);
            player.SendEvent(ToClientEvent.ClearTeamPlayers);
        }

        public void ClearPlayers()
        {
            FuncIterate((player, team) =>
            {
                player.SendEvent(ToClientEvent.ClearTeamPlayers);
                foreach (var target in Players)
                {
                    if (target == player)
                        continue;
                    player.SetVoiceTo(target, false);
                    target.SetVoiceTo(player, false);
                }
            });
            Players.Clear();
        }

        public void SyncAddedPlayer(TDSPlayer player)
        {
            string json = _serializer.ToClient(Players.Select(p => p.RemoteId));
            player.SendEvent(ToClientEvent.SyncTeamPlayers, json);
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                target.SendEvent(ToClientEvent.PlayerJoinedTeam, player.RemoteId);
                if (!player.HasRelationTo(target, PlayerRelation.Block) && !target.IsVoiceMuted)
                    target.SetVoiceTo(player, true);
                if (!target.HasRelationTo(player, PlayerRelation.Block) && !player.IsVoiceMuted)
                    player.SetVoiceTo(target, true);
            }
        }

        public void SyncRemovedPlayer(TDSPlayer player)
        {
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                target.SendEvent(ToClientEvent.PlayerLeftTeam, player.RemoteId);
                target.SetVoiceTo(player, false);
                player.SetVoiceTo(target, false);
            }
        }

        public void SyncAllPlayers()
        {
            string json = _serializer.ToClient(Players.Select(p => p.RemoteId));
            foreach (var player in Players)
            {
                player.SendEvent(ToClientEvent.SyncTeamPlayers, json);
                foreach (var target in Players)
                {
                    if (target == player)
                        continue;
                    if (!player.HasRelationTo(target, PlayerRelation.Block) && !target.IsVoiceMuted)
                        target.SetVoiceTo(player, true);
                    if (!target.HasRelationTo(player, PlayerRelation.Block) && !player.IsVoiceMuted)
                        player.SetVoiceTo(target, true);
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
