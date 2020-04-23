using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.Utility;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public ITeam? Team { get; private set; }

        public int TeamIndex => Team?.Entity.Index ?? 0;

        public void SetTeam(ITeam? team, bool forceIsNew)
        {
            if (team != Team || forceIsNew)
            {
                Team?.RemovePlayer(this);
                team?.AddPlayer(this);
                _modAPI.Sync.SendEvent(this, ToClientEvent.PlayerTeamChange, team?.Entity.Name ?? "-");

                Team = team;
            }
        }

    }
}
