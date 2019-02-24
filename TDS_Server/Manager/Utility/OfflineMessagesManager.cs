using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Utility {

    static class OfflineMessagesManager {

        #region AddOfflineMessage
        public static async void AddOfflineMessage(uint playerid, uint sourceid, string message)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Offlinemessages msg = new Offlinemessages()
                {
                    TargetId = playerid,
                    SourceId = sourceid,
                    Message = message
                };
                await dbcontext.Offlinemessages.AddAsync(msg);
                await dbcontext.SaveChangesAsync();
            }
        }
        #endregion

        public static async void CheckOfflineMessages(TDSPlayer player)
        {
            using (var dbcontext = new TDSNewContext())
            {
                int amountnewentries = await dbcontext
                    .Offlinemessages
                    .Where(msg => msg.SourceId == player.Entity.Id && msg.AlreadyLoadedOnce)
                    .AsNoTracking()
                    .CountAsync();
                int amountentries = await dbcontext
                    .Offlinemessages
                    .AsNoTracking()
                    .CountAsync();


                if (amountnewentries > 0)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player.Client,
                        Utils.GetReplaced(player.Language.GOT_UNREAD_OFFLINE_MESSAGES, amountentries.ToString(), amountnewentries.ToString())
                    );
                }
            }
        }
	}
}
