using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS.Server.Data.RoundEndReasons;

namespace TDS.Server.GamemodesSystem.Rounds
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
                // If attacker dies it's handled by target
                DeathRoundEndReason deathRoundEndReason => deathRoundEndReason.WinnerTeam != _gamemode.Teams.Attacker,
                NewPlayerRoundEndReason _ => false,
                // Handled by target
                LobbyEmptyRoundEndReason _ => false,    

                _ => true
            };
    }
}
