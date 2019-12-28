﻿using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.Player
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
                    NAPI.ClientEvent.TriggerClientEvent(Client, DToClientEvent.PlayerTeamChange, value?.Entity.Name ?? "-");

                    _team = value;
                }
            }
        }

    }
}
