namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayerAPI
    {
        #region Public Methods

        bool GetEntityPlayerIsFreeAimingAt(ref int targetEntity);

        bool GetPlayerInvincible();

        void SetPlayerHealthRechargeMultiplier(float regenRate);

        void SetPlayerInvincible(bool toggle);

        void SetPlayerLockon(bool toggle);

        void SetPlayerTargetingMode(int targetingMode);

        void SetPlayerTeam(int team);

        #endregion Public Methods
    }
}
