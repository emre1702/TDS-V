﻿using GTANetworkAPI;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        private ITeam? _team;

        public override ITeam? Team => _team;
        public override int TeamIndex => Team?.Entity.Index ?? 0;

        public override void SetTeam(ITeam? team, bool forceIsNew)
        {
            if (team != Team || forceIsNew)
            {
                Team?.RemovePlayer(this);
                team?.AddPlayer(this);
                TriggerEvent(ToClientEvent.PlayerTeamChange, team?.Entity.Name ?? "-");

                _team = team;
                _dataSyncHandler.SetData(this, PlayerDataKey.TeamIndex, DataSyncMode.Lobby, team?.Entity.Index ?? -1);
            }
        }
    }
}