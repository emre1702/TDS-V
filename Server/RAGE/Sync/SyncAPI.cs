using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.RAGE
{
    class SyncAPI : ISyncAPI
    {

        public void SendEvent(ITDSPlayer player, string eventName, params object[] args)
        {
            if (!(player.ModPlayer is Player modPlayer))
                return;
            NAPI.ClientEvent.TriggerClientEvent(modPlayer._instance, eventName, args);
        }
    }
}
