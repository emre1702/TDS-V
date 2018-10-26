namespace TDS.Manager.Utility
{

    using System.Collections.Generic;
    using GTANetworkAPI;
    using TDS.Entity;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using TDS.Enum;
    using TDS.Manager.Player;
    using System;
    using TDS.Interface;

    class AdminLevel
    {
        public Dictionary<ELanguage, string> Names = new Dictionary<ELanguage, string>();
        public List<Client> PlayersOnline = new List<Client>();
    }

    static class Admin
    {

        public static List<AdminLevel> AdminLevels = new List<AdminLevel>();

        [ServerEvent(Event.ResourceStart)]
        public async static void AdminResourceStart()
        {
            using (var dbcontext = new TDSNewContext())
            {
                AdminLevels = await dbcontext.Adminlevels.OrderBy(lvl => lvl.Level).Select(lvl => new AdminLevel()).ToListAsync();

                foreach (var entry in await dbcontext.Adminlevelnames.ToListAsync())
                {
                    AdminLevels[entry.Level].Names[(ELanguage)entry.Language] = entry.Name;
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

        public static void CallMethodForAdmins(Action<Client> func, uint minadminlvl = 1)
        {
            for (uint lvl = minadminlvl; lvl < AdminLevels.Count; ++lvl)
            {
                foreach (Client player in AdminLevels[(int)lvl].PlayersOnline)
                {
                    func(player);
                }
            }
        }

        public static void SendChatMessageToAdmins(Func<ILanguage, string> propertygetter, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Chat.SendChatMessageToPlayer(player, propertygetter(player.GetLang())));
        }

        public static void SendLangNotificationToAdmins<T>(Func<ILanguage, string> propertygetter, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Notification.SendNotificationToPlayer(player, propertygetter(player.GetLang())));
        }
    }

}
