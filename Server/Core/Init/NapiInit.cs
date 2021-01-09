using GTANetworkAPI;
using System;

namespace TDS.Server.Core.Init
{
    public class NapiInit
    {
        public void Init()
        {
            NAPI.Server.SetAutoSpawnOnConnect(false);
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);

            var date = DateTime.UtcNow;
            NAPI.World.SetTime(date.Hour, date.Minute, date.Second);
        }
    }
}