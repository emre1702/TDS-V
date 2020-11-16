using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.GamemodesSystem.Players;
using TDS.Server.GangGamemodesSystem.Gamemodes;

namespace TDS.Server.GangGamemodesSystem.Players
{
    public class GangGangwarPlayers : GangwarPlayers, IGangGangwarGamemodePlayers
    {
        private ITDSPlayer? _attackLeader;
        private readonly IGangGangwarGamemode _gamemode;

        public GangGangwarPlayers(IGangGangwarGamemode gamemode) 
            => _gamemode = gamemode;

        protected override async ValueTask PlayerJoinedAfter((ITDSPlayer Player, int TeamIndex) data)
        {
            await base.PlayerJoinedAfter(data);

            if (_attackLeader is null && data.TeamIndex == (int)GangActionLobbyTeamIndex.Attacker)
                SetAttackLeader(data.Player);
        }

        protected override async ValueTask PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            await base.PlayerLeftAfter(data);

            if (data.Player == _attackLeader && _gamemode.Teams.Attacker.Players.Amount > 0)
            {
                var newAttackLeader = _gamemode.Teams.Attacker.Players.GetRandom();
                SetAttackLeader(newAttackLeader);
            }
        }

        private void SetAttackLeader(ITDSPlayer attackLeader)
        {
            _attackLeader = attackLeader;

            // Todo: Send him infos or open menu or whatever
        }
    }
}
