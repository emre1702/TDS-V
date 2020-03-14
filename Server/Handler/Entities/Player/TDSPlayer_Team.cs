using TDS_Server.Handler.Entities.Utility;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private Team? _team;

        public Team? Team
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

    }
}
