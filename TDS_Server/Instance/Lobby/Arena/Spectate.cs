using System.Collections.Generic;
using System.Linq;
using TDS_Common.Instance.Utility;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        private readonly Dictionary<TDSPlayer, TDSTimer> _removeSpectatorsTimer = new Dictionary<TDSPlayer, TDSTimer>();

        protected override void SpectateOtherSameTeam(TDSPlayer character, bool next = true)
        {
            if (_currentRoundStatus == ERoundStatus.Countdown || _currentRoundStatus == ERoundStatus.Round)
                base.SpectateOtherSameTeam(character, next);
        }

        protected override void SpectateOtherAllTeams(TDSPlayer character, bool next = true)
        {
            if (_currentRoundStatus == ERoundStatus.Countdown || _currentRoundStatus == ERoundStatus.Round)
                base.SpectateOtherAllTeams(character, next);
        }

        private void PlayerCantBeSpectatedAnymore(TDSPlayer character)
        {
            if (_removeSpectatorsTimer.ContainsKey(character))
                _removeSpectatorsTimer.Remove(character);
            character.Team?.SpectateablePlayers?.Remove(character);

            if (character.Spectators.Any())
            {
                foreach (TDSPlayer spectator in character.Spectators.ToList())  // ToList because the list gets changed in both methods
                {
                    SpectateNext(spectator, true);
                }
            }
        }
    }
}