using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.PlayersSystem
{
    public class TeamHandler : IPlayerTeamHandler
    {
        public ITeam? Team { get; private set; }

        private readonly DataSyncHandler _dataSyncHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public TeamHandler(DataSyncHandler dataSyncHandler)
        {
            _dataSyncHandler = dataSyncHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void SetTeam(ITeam? team, bool forceIsNew)
        {
            if (team != _player.Team || forceIsNew)
            {
                _player.Team?.Players.Remove(_player);
                team?.Players.Add(_player);

                Team = team;

                NAPI.Task.Run(() =>
                {
                    _player.TriggerEvent(ToClientEvent.PlayerTeamChange, team?.Entity.Name ?? "-");
                    _dataSyncHandler.SetData(_player, PlayerDataKey.TeamIndex, DataSyncMode.Lobby, team?.Entity.Index ?? -1);
                });
            }
        }
    }
}
