using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public void SetEntityInvincible(ITDSVehicle vehicle, bool invincible)
        {
            vehicle.Vehicle?.SetInvincible(invincible, this);
        }
    }
}
