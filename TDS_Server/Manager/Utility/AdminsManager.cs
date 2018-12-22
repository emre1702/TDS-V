namespace TDS_Server.Manager.Utility
{

    using System.Collections.Generic;
    using GTANetworkAPI;
    using TDS_Server.Entity;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using TDS_Server.Enum;
    using TDS_Server.Manager.Player;
    using System;
    using TDS_Server.Interface;

    class AdminLevel
    {
        public sbyte Level;
        public string FontColor;
        public Dictionary<ELanguage, string> Names = new Dictionary<ELanguage, string>();
        public List<Client> PlayersOnline = new List<Client>();
    }

    static class AdminsManager
    {

        public static Dictionary<sbyte, AdminLevel> AdminLevels = new Dictionary<sbyte, AdminLevel>();

        [ServerEvent(Event.ResourceStart)]
        public async static void AdminResourceStart()
        {
            using (var dbcontext = new TDSNewContext())
            {
                AdminLevels = await dbcontext.Adminlevels
                    .OrderBy(lvl => lvl.Level)
                    .Select(lvl => new AdminLevel {
                                    Level = (sbyte)lvl.Level,
                                    FontColor = "{" + lvl.ColorR + "|" + lvl.ColorG + "|" + lvl.ColorB + "}"
                                })
                    .ToDictionaryAsync(lvl => lvl.Level, lvl => lvl);

                foreach (var entry in await dbcontext.Adminlevelnames.ToListAsync())
                {
                    AdminLevels[(sbyte)entry.Level].Names[(ELanguage)entry.Language] = entry.Name;
                }
            }
        }

        public static void SetOnline(Client player)
        {
            Players character = player.GetEntity();
            if (AdminLevels.Count >= character.AdminLvl)
            {
                AdminLevels[character.AdminLvl].PlayersOnline.Add(player);
            }
        }

        public static void SetOffline(Client player)
        {
            Players character = player.GetEntity();
            if (AdminLevels.Count >= character.AdminLvl)
            {
                AdminLevels[character.AdminLvl].PlayersOnline.Remove(player);
            }
        }

        public static void CallMethodForAdmins(Action<Client> func, sbyte minadminlvl = 1)
        {
            for (sbyte lvl = minadminlvl; lvl < AdminLevels.Count; ++lvl)
            {
                foreach (Client player in AdminLevels[lvl].PlayersOnline)
                {
                    func(player);
                }
            }
        }

        public static void SendChatMessageToAdmins(string msg, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Chat.SendChatMessageToPlayer(player, msg));
        }

        public static void SendLangChatMessageToAdmins(Func<ILanguage, string> propertygetter, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Chat.SendChatMessageToPlayer(player, propertygetter(player.GetLang())));
        }

        public static void SendLangNotificationToAdmins<T>(Func<ILanguage, string> propertygetter, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Notification.SendNotificationToPlayer(player, propertygetter(player.GetLang())));
        }
    }

}
