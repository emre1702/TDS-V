﻿using System.Diagnostics.CodeAnalysis;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler.Helper;
using TDS.Shared.Data.Models;

namespace TDS.Server.TeamsSystem
{
    public class Team : ITeam
    {
        public ITeamChat Chat { get; }
        public ITeamPlayers Players { get; }
        public ITeamSync Sync { get; }

        public bool IsSpectator => Entity.Index == 0;
        public int SpawnCounter { get; set; }

        public Teams Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                Chat.InitColor();
            }
        }

#nullable disable
        public SyncedTeamDataDto SyncedData { get; set; }

        private Teams _entity;
#nullable enable

        public Team(ITeamChat chat, ITeamPlayers players, ITeamSync sync)
        {
            Chat = chat;
            Players = players;
            Sync = sync;
        }

        public void Init(Teams entity)
        {
            _entity = entity;

            Chat.Init(this);
            Players.Init(this);
            Sync.Init(this);

            SyncedData = new SyncedTeamDataDto
            (
                index: Entity.Index,
                name: Entity.Name,
                color: new ColorDto(Entity.ColorR, Entity.ColorG, Entity.ColorB),
                amountPlayers: new SyncedTeamPlayerAmountDto()
            );
        }

        public static bool operator !=(Team a, Team b)
        {
            return !(a == b);
        }

        public static bool operator ==(Team a, Team b)
        {
            if (a is null)
                return b is null;
            if (b is null)
                return false;
            return a.Entity.Id == b.Entity.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Team team && team.Entity.Id == Entity.Id;
        }

        public bool Equals([AllowNull] ITeam other)
        {
            return _entity.Id == other?.Entity.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
