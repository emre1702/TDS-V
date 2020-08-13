using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Methods

        public void SetEntityInvincible(IVehicle vehicle, bool invincible)
        {
            vehicle.SetInvincible(invincible, this);
        }

        #endregion Public Methods
    }
}
