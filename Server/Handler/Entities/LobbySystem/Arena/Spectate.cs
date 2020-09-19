using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        #region Private Fields

        private readonly Dictionary<ITDSPlayer, TDSTimer> _removeSpectatorsTimer = new Dictionary<ITDSPlayer, TDSTimer>();

        #endregion Private Fields

        #region Protected Methods

        protected override void SpectateOtherAllTeams(ITDSPlayer player, bool next = true)
        {
            if (CurrentRoundStatus == RoundStatus.Countdown || CurrentRoundStatus == RoundStatus.Round)
                base.SpectateOtherAllTeams(player, next);
        }

        protected override void SpectateOtherSameTeam(ITDSPlayer player, bool next = true, bool ignoreSource = false)
        {
            if (CurrentRoundStatus == RoundStatus.Countdown || CurrentRoundStatus == RoundStatus.Round)
                base.SpectateOtherSameTeam(player, next, ignoreSource);
        }

        #endregion Protected Methods

        #region Private Methods

        private void MakeSurePlayerSpectatesAnyone(ITDSPlayer player)
        {
            if (player.Spectates is { } && player.Spectates != player)
                return;

            SpectateNext(player, true);
        }

        private void PlayerCantBeSpectatedAnymore(ITDSPlayer player)
        {
            if (_removeSpectatorsTimer.ContainsKey(player))
                _removeSpectatorsTimer.Remove(player);
            player.Team?.SpectateablePlayers?.Remove(player);

            if (player.HasSpectators())
            {
                foreach (ITDSPlayer spectator in player.GetSpectators())  // ToList because the list gets changed in both methods
                {
                    SpectateNext(spectator, true);
                }
            }
        }

        #endregion Private Methods
    }
}
