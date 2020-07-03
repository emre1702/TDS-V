using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class ArmsRace
    {
        #region Public Methods

        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
            if (killer == player)
                return;
            if (CheckRoundEnd(killer))
                return;

            GiveNextWeapon(killer);
        }

        #endregion Public Methods
    }
}
