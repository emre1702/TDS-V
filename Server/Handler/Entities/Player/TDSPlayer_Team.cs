using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
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
                _dataSyncHandler.SetData(this, PlayerDataKey.TeamIndex, DataSyncMode.Lobby, team?.Entity.Index ?? -1);
            }
        }

    }
}
