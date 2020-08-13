using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Entity.TeamSystem
{
    public class Team : ITeam
    {
        #region Private Fields
        private readonly Serializer _serializer;
        private Teams _entity;

        #endregion Private Fields

        #region Public Constructors

        public Team(Teams entity, Serializer serializer)
        {
            _serializer = serializer;
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

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

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

        public void FuncIterate(Action<ITDSPlayer, ITeam> func)
        {
            foreach (var player in Players)
            {
                func(player, this);
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
            player.SendEvent(ToClientEvent.SyncTeamPlayers, Players.ToArray());
            foreach (var target in Players)
            {
                if (target == player)
                    continue;
                target.SendEvent(ToClientEvent.PlayerJoinedTeam, player);
                if (!player.HasRelationTo(target, PlayerRelation.Block) && !target.IsVoiceMuted)
                    target.SetVoiceTo(player, true);
                if (!target.HasRelationTo(player, PlayerRelation.Block) && !player.IsVoiceMuted)
                    player.SetVoiceTo(target, true);
            }
        }

        public void SyncAllPlayers()
        {
            var array = Players.ToArray();
            foreach (var player in Players)
            {
                player.SendEvent(ToClientEvent.SyncTeamPlayers, array);
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
            foreach (var target in Players)
            {
                if (target != player)
                    target.SendEvent(ToClientEvent.PlayerLeftTeam, player);
            }

            player.SendEvent(ToClientEvent.ClearTeamPlayers);
        }

        #endregion Public Methods
    }
}
