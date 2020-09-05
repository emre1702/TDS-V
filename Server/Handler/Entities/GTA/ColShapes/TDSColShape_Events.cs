using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.ColShapes
{
    partial class TDSColShape
    {
        public override event PlayerEnteredExitedColShape? PlayerEntered;

        public override event PlayerEnteredExitedColShape? PlayerExited;

        private void AddEvents()
        {
            OnEntityEnterColShape += TDSColShape_OnEntityEnterColShape;
            OnEntityExitColShape += TDSColShape_OnEntityExitColShape;
        }

        private void TDSColShape_OnEntityExitColShape(GTANetworkAPI.ColShape colShape, GTANetworkAPI.Player client)
        {
            if (colShape != this || !(client is ITDSPlayer player))
                return;
            PlayerEntered?.Invoke(player);
        }

        private void TDSColShape_OnEntityEnterColShape(GTANetworkAPI.ColShape colShape, GTANetworkAPI.Player client)
        {
            if (colShape != this || !(client is ITDSPlayer player))
                return;
            PlayerExited?.Invoke(player);
        }
    }
}
