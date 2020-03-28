using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public void SetEntityInvincible(IVehicle vehicle, bool invincible)
        {
            vehicle.SetInvincible(invincible, this);
        }
    }
}
