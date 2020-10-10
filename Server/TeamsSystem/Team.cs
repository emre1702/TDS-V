using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Models;

namespace TDS_Server.TeamsSystem
{
    public class Team : ITeam
    {
        public ITeamChat Chat { get; }
        public ITeamPlayers Players { get; }
        public ITeamSync Sync { get; }

        public bool IsSpectator => Entity.Index == 0;
        public int SpawnCounter { get; set; }

        public SyncedTeamDataDto SyncedData { get; set; }

        public Teams Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                Chat.InitColor();
            }
        }

        private Teams _entity;

        public Team(Teams entity, LangHelper langHelper)
        {
            _entity = entity;

            Chat = new Chat(this, langHelper);
            Players = new Players(this);
            Sync = new Sync(this);

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
