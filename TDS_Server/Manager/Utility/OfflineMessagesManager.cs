using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using TDS_Server.Entity;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Utility {

    static class OfflineMessagesManager {

        #region AddOfflineMessage
        public static void AddOfflineMessage(uint playerid, uint sourceid, string message)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Offlinemessages msg = new Offlinemessages()
                {
                    TargetId = playerid,
                    SourceId = sourceid,
                    Message = message
                };
                dbcontext.Offlinemessages.AddAsync(msg);
                dbcontext.SaveChangesAsync();
            }
        }
        #endregion

        public static async void CheckOfflineMessages(Client player)
        {
            Players entity = player.GetEntity();

            using (var dbcontext = new TDSNewContext())
            {
                dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                int amountnewentries = await dbcontext
                    .Offlinemessages
                    .Where(msg => msg.SourceId == entity.Id && msg.AlreadyLoadedOnce)
                    .CountAsync();

                if (amountnewentries > 0)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player,
                        Utils.GetReplaced(player.GetLang().GOT_UNREAD_OFFLINE_MESSAGES, entity.OfflinemessagesSource.Count, amountnewentries.ToString())
                    );
                }
            }
        }
	}
}
