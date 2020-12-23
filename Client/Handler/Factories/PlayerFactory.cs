using TDS.Client.Handler.Entities.GTA;
using TDS.Client.Handler.Sync;

namespace TDS.Client.Handler.Factories
{
    public class PlayerFactory
    {
        public PlayerFactory(DataSyncHandler dataSyncHandler)
        {
            RAGE.Elements.Entities.Players.CreateEntity = (ushort id, ushort remoteId) => new TDSPlayer(id, remoteId, dataSyncHandler);
            RAGE.Elements.Player.RecreateLocal();
        }
    }
}
