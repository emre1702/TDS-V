using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using TDS.Entity;
using TDS.Manager.Player;

namespace TDS.Manager.Utility {

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
            }
        }
        #endregion

        public static void CheckOfflineMessages(Client player)
        {
            uint amountnewentries = 0;
            foreach (Offlinemessages msg in player.GetEntity().OfflinemessagesSource)
            {
                if (!msg.AlreadyLoadedOnce)
                {
                    ++amountnewentries;
                    msg.AlreadyLoadedOnce = true;
                }
            }
            if (amountnewentries > 0)
            {     
                NAPI.Chat.SendChatMessageToPlayer(player, 
                    Utils.GetReplaced(player.GetLang().GOT_OFFLINE_MESSAGE_WITH_NEW, amountnewentries.ToString())
                );
                using (var dbcontext = new TDSNewContext())
                {
                    dbcontext.SaveChangesAsync();
                }
            } else
                NAPI.Chat.SendChatMessageToPlayer(player, player.GetLang().GOT_OFFLINE_MESSAGE_WITHOUT_NEW);
        }
	}
}
