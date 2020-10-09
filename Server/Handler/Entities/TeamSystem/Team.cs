using GTANetworkAPI;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.TeamSystem
{
    public class Team : ITeam
    {
        private Teams _entity;

        public Team(Teams entity)
        {
            _entity = entity;

            ChatColor = "!$" + Entity.ColorR + "|" + Entity.ColorG + "|" + Entity.ColorB + "$";

            if (!IsSpectator)
            {
                SpectateablePlayers = new List<ITDSPlayer>();
                AlivePlayers = new List<ITDSPlayer>();
            }

            SyncedTeamData = new SyncedTeamDataDto
            (
                index: Entity.Index,
                name: Entity.Name,
                color: new ColorDto(Entity.ColorR, Entity.ColorG, Entity.ColorB),
                amountPlayers: new SyncedTeamPlayerAmountDto()
            );
        }

        public List<ITDSPlayer>? AlivePlayers { get; set; }

        public string ChatColor { get; private set; }

        public Teams Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                ChatColor = "!$" + Entity.ColorR + "|" + Entity.ColorG + "|" + Entity.ColorB + "$";
            }
        }

        public bool IsSpectator => Entity.Index == 0;
        public List<ITDSPlayer> Players { get; private set; } = new List<ITDSPlayer>();
        public int SpawnCounter { get; set; }
        public List<ITDSPlayer>? SpectateablePlayers { get; set; }
        public SyncedTeamDataDto SyncedTeamData { get; set; }

        public static bool operator !=(Team? a, Team? b)
        {
            return !(a == b);
        }

        public static bool operator ==(Team? a, Team? b)
        {
            if (a is null)
                return (b is null);
            if (b is null)
                return false;
            return a.Entity.Id == b.Entity.Id;
        }

        public void AddPlayer(ITDSPlayer player)
        {
            Players.Add(player);
            player.SetSkin(Entity.SkinHash != 0 ? (PedHash)Entity.SkinHash : player.FreemodeSkin);
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Team team && team.Entity.Id == this.Entity.Id;
        }

        public bool Equals([AllowNull] ITeam other)
        {
            return _entity.Id == other?.Entity.Id;
        }

        public void FuncIterate(Action<ITDSPlayer> func)
        {
            foreach (var player in Players)
            {
                func(player);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void RemoveAlivePlayer(ITDSPlayer player)
        {
            if (AlivePlayers is null)
                return;

            AlivePlayers.Remove(player);
            SyncedTeamData.AmountPlayers.AmountAlive = (uint)AlivePlayers.Count;
        }

        public void RemovePlayer(ITDSPlayer player)
        {
            Players.Remove(player);
        }

        public void SyncAddedPlayer(ITDSPlayer player)
        {
            string json = Serializer.ToClient(Players.Select(p => p.RemoteId).ToList());
            player.TriggerEvent(ToClientEvent.SyncTeamPlayers, json);
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                target.TriggerEvent(ToClientEvent.PlayerJoinedTeam, player.RemoteId);
                if (!player.HasRelationTo(target, PlayerRelation.Block) && !target.IsVoiceMuted)
                    target.SetVoiceTo(player, true);
                if (!target.HasRelationTo(player, PlayerRelation.Block) && !player.IsVoiceMuted)
                    player.SetVoiceTo(target, true);
            }
        }

        public void SyncAllPlayers()
        {
            string json = Serializer.ToClient(Players.Select(p => p.RemoteId).ToList());
            foreach (var player in Players)
            {
                player.TriggerEvent(ToClientEvent.SyncTeamPlayers, json);
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

        public void SyncRemovedPlayer(ITDSPlayer player)
        {
            player.ResetVoiceToAndFrom();

            NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Where(p => p != player).ToArray(), ToClientEvent.PlayerLeftTeam, player.RemoteId);
            player.TriggerEvent(ToClientEvent.ClearTeamPlayers);
        }

        public void SendMessage(string message)
        {
            foreach (var player in Players)
            {
                player.SendChatMessage(message);
            }
        }

        public ITDSPlayer? GetNearestPlayer(Vector3 position)
            => Players.MinBy(p => p.Position.DistanceTo(position)).FirstOrDefault();
    }
}
