using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server_DB.Entity.Log;

namespace TDS_Server.Core.Manager.Logs
{
    static class KillLogsManager
    {
        public static void Log(TDSPlayer player, TDSPlayer killer, uint weapon)
        {
            var log = new LogKills
            {
                KillerId = killer.Entity!.Id,
                DeadId = player.Entity!.Id,
                WeaponId = weapon
            };
            LogsManager.AddLog(log);
        }
    }
}
