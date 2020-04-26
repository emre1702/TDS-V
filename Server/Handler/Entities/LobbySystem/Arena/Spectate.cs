using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        private readonly Dictionary<ITDSPlayer, TDSTimer> _removeSpectatorsTimer = new Dictionary<ITDSPlayer, TDSTimer>();

        protected override void SpectateOtherSameTeam(ITDSPlayer character, bool next = true)
        {
            if (CurrentRoundStatus == RoundStatus.Countdown || CurrentRoundStatus == RoundStatus.Round)
                base.SpectateOtherSameTeam(character, next);
        }

        protected override void SpectateOtherAllTeams(ITDSPlayer character, bool next = true)
        {
            if (CurrentRoundStatus == RoundStatus.Countdown || CurrentRoundStatus == RoundStatus.Round)
                base.SpectateOtherAllTeams(character, next);
        }

        private void PlayerCantBeSpectatedAnymore(ITDSPlayer player)
        {
            if (_removeSpectatorsTimer.ContainsKey(player))
                _removeSpectatorsTimer.Remove(player);
            player.Team?.SpectateablePlayers?.Remove(player);

            if (player.Spectators.Any())
            {
                foreach (ITDSPlayer spectator in player.Spectators.ToList())  // ToList because the list gets changed in both methods
                {
                    SpectateNext(spectator, true);
                }
            }
        }

        private void MakeSurePlayerSpectatesAnyone(ITDSPlayer player)
        {
            if (player.Spectates is { })
                return;

            SpectateNext(player, true);
        }
    }
}
