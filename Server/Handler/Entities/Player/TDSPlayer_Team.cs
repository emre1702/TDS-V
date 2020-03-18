using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.Utility;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private ITeam? _team;

        public ITeam? Team
        {
            get => _team;
            set
            {
                if (value != _team)
                {
                    _team?.RemovePlayer(this);
                    value?.AddPlayer(this);
                    _modAPI.Sync.SendEvent(this, ToClientEvent.PlayerTeamChange, value?.Entity.Name ?? "-");

                    _team = value;
                }
            }
        }

        public int TeamIndex => Team?.Entity.Index ?? 0;

    }
}
