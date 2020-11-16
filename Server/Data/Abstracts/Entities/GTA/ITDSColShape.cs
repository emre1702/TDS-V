using GTANetworkAPI;

namespace TDS.Server.Data.Abstracts.Entities.GTA
{
#nullable enable

    public abstract class ITDSColshape : ColShape
    {
        public delegate void PlayerEnteredExitedColShape(ITDSPlayer player);

        public abstract event PlayerEnteredExitedColShape? PlayerEntered;

        public abstract event PlayerEnteredExitedColShape? PlayerExited;

        protected ITDSColshape(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
