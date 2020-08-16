using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Gamemodes.ArmsRace
{
    partial class ArmsRace
    {
        #region Public Methods

        public override bool CanJoinDuringRound(ITDSPlayer player, ITeam team) => true;

        public override void StartRoundCountdown()
        {
            Lobby.AmountLifes = short.MaxValue;
        }

        public override void StopRound()
        {
            Lobby.AmountLifes = (Lobby.Entity.FightSettings?.AmountLifes ?? 0);
        }

        #endregion Public Methods

        #region Private Methods

        private bool CheckRoundEnd(ITDSPlayer killer)
        {
            if (!GetNextWeapon(killer, out WeaponHash? weaponHash))
                return false;

            // WeaponHash == null => Round end
            if (weaponHash.HasValue)
                return false;

            Lobby.CurrentRoundEndBecauseOfPlayer = killer;
            Lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.PlayerWon);
            return true;
        }

        #endregion Private Methods
    }
}
