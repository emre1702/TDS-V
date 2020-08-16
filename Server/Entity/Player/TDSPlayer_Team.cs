using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Properties

        public ITeam? Team { get; private set; }

        public int TeamIndex => Team?.Entity.Index ?? 0;

        #endregion Public Properties

        #region Public Methods

        public void SetTeam(ITeam? team, bool forceIsNew)
        {
            if (team != Team || forceIsNew)
            {
                Team?.RemovePlayer(this);
                team?.AddPlayer(this);
                SendEvent(ToClientEvent.PlayerTeamChange, team?.Entity.Name ?? "-");

                Team = team;
                SetStreamSyncedMetaData(PlayerDataKey.TeamIndex.ToString(), team?.Entity.Index ?? -1);
            }
        }

        #endregion Public Methods
    }
}
