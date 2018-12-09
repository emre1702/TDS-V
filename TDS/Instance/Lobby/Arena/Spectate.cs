using System;
using System.Collections.Generic;
using System.Text;
using TDS.Enum;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class Arena
    {
        protected override void SpectateNextSameTeam(Character character)
        {
            base.SpectateNextSameTeam(character);
        }

        protected override void SpectateNextAllTeams(Character character)
        {
            if (currentRoundStatus == ERoundStatus.Countdown || currentRoundStatus == ERoundStatus.Round)
                base.SpectateNextAllTeams(character);
        }
    }
}
