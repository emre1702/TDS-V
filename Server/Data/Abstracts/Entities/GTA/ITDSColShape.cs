using GTANetworkAPI;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
#nullable enable

    public abstract class ITDSColShape : ColShape
    {
        public delegate void PlayerEnteredExitedColShape(ITDSPlayer player);

        public abstract event PlayerEnteredExitedColShape? PlayerEntered;

        public abstract event PlayerEnteredExitedColShape? PlayerExited;

        public ITDSColShape(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
