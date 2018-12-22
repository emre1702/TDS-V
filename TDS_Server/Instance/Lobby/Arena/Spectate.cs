using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        private readonly Dictionary<TDSPlayer, Timer> removeSpectatorsTimer = new Dictionary<TDSPlayer, Timer>();

        protected override void SpectateOtherSameTeam(TDSPlayer character, bool next = true)
        {
            if (currentRoundStatus == ERoundStatus.Countdown || currentRoundStatus == ERoundStatus.Round)
                base.SpectateOtherSameTeam(character, next);
        }

        protected override void SpectateOtherAllTeams(TDSPlayer character, bool next = true)
        {
            if (currentRoundStatus == ERoundStatus.Countdown || currentRoundStatus == ERoundStatus.Round)
                base.SpectateOtherAllTeams(character, next);
        }

        private void PlayerCantBeSpectatedAnymore(TDSPlayer character)
        {
            if (removeSpectatorsTimer.ContainsKey(character))
                removeSpectatorsTimer.Remove(character);
            SpectateablePlayers[character.Team.Index].Remove(character);

            if (character.Spectators.Any())
            {
                foreach (TDSPlayer spectator in character.Spectators.ToList())  // ToList because the list gets changed in both methods
                {
                    if (spectator.Team.IsSpectatorTeam)
                        SpectateOtherAllTeams(spectator);
                    else
                        SpectateOtherSameTeam(spectator);
                }
            }
            
        }
    }
}
