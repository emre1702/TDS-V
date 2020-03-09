using GTANetworkAPI;
using TDS_Shared.Default;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Entity.Player
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
                    NAPI.ClientEvent.TriggerClientEvent(Player, DToClientEvent.PlayerTeamChange, value?.Entity.Name ?? "-");

                    _team = value;
                }
            }
        }

    }
}
