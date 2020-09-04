using GTANetworkAPI;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSVehicle : Vehicle
    {

        public ITDSVehicle(NetHandle netHandle): base(netHandle) {}

        public abstract void SetInvincible(bool toggle, ITDSPlayer forPlayer);
    }
}
