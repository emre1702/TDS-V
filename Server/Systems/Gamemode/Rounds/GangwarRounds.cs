using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.RoundEndReasons;

namespace TDS_Server.GamemodesSystem.Rounds
{
    public class GangwarRounds : BaseGamemodeRounds
    {
        private readonly IGangwarGamemode _gamemode;

        public GangwarRounds(IGangwarGamemode gamemode)
        {
            _gamemode = gamemode;
        }

        public override bool CanEndRound(IRoundEndReason roundEndReason)
            => roundEndReason switch
            {
                // If attacker dies it's handled by Target
                DeathRoundEndReason deathRoundEndReason => deathRoundEndReason.WinnerTeam != _gamemode.Teams.Attacker,
                NewPlayerRoundEndReason _ => false,

                _ => true
            };
    }
}
