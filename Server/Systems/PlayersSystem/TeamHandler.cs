using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.PlayersSystem
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

                NAPI.Task.RunSafe(() =>
                {
                    _player.TriggerEvent(ToClientEvent.PlayerTeamChange, team?.Entity.Name ?? "-");
                    _dataSyncHandler.SetData(_player, PlayerDataKey.TeamIndex, DataSyncMode.Lobby, team?.Entity.Index ?? -1);
                });
            }
        }
    }
}
