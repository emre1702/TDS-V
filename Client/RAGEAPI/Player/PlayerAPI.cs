using TDS_Client.Data.Interfaces.ModAPI.Player;

namespace TDS_Client.RAGEAPI.Player
{
    internal class PlayerAPI : IPlayerAPI
    {
        #region Public Methods

        public bool GetEntityPlayerIsFreeAimingAt(ref int targetEntity)
        {
            return RAGE.Game.Player.GetEntityPlayerIsFreeAimingAt(ref targetEntity);
        }

        public bool GetPlayerInvincible()
        {
            return RAGE.Game.Player.GetPlayerInvincible();
        }

        public void SetPlayerHealthRechargeMultiplier(float regenRate)
        {
            RAGE.Game.Player.SetPlayerHealthRechargeMultiplier(regenRate);
        }

        public void SetPlayerInvincible(bool toggle)
        {
            RAGE.Game.Player.SetPlayerInvincible(toggle);
        }

        public void SetPlayerLockon(bool toggle)
        {
            RAGE.Game.Player.SetPlayerLockon(toggle);
        }

        public void SetPlayerTargetingMode(int targetingMode)
        {
            RAGE.Game.Player.SetPlayerTargetingMode(targetingMode);
        }

        public void SetPlayerTeam(int team)
        {
            RAGE.Game.Player.SetPlayerTeam(team);
        }

        #endregion Public Methods
    }
}
