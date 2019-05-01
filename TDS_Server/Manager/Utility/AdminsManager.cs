namespace TDS_Server.Manager.Utility
{
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TDS_Common.Enum;
    using TDS_Server.Dto;
    using TDS_Server.Entity;
    using TDS_Server.Instance.Player;
    using TDS_Server.Interface;

    internal static class AdminsManager
    {
        public static Dictionary<byte, AdminLevelDto> AdminLevels = new Dictionary<byte, AdminLevelDto>();

        public static async Task Init(TDSNewContext dbcontext)
        {
            AdminLevels = await dbcontext.AdminLevels
                .OrderBy(lvl => lvl.Level)
                .Select(lvl => new AdminLevelDto
                            (
                                lvl.Level,
                                "!{" + lvl.ColorR + "|" + lvl.ColorG + "|" + lvl.ColorB + "}"
                            ))
                .AsNoTracking()
                .ToDictionaryAsync(lvl => lvl.Level, lvl => lvl);

            foreach (var entry in await dbcontext.AdminLevelNames.ToListAsync())
            {
                AdminLevels[entry.Level].Names[(ELanguage)entry.Language] = entry.Name;
            }
        }

        public static void SetOnline(TDSPlayer player)
        {
            if (AdminLevels.ContainsKey(player.AdminLevel.Level))
            {
                AdminLevels[player.AdminLevel.Level].PlayersOnline.Add(player);
            }
        }

        public static void SetOffline(TDSPlayer player)
        {
            if (AdminLevels.ContainsKey(player.AdminLevel.Level))
            {
                AdminLevels[player.AdminLevel.Level].PlayersOnline.Remove(player);
            }
        }

        public static void CallMethodForAdmins(Action<TDSPlayer> func, byte minadminlvl = 1)
        {
            for (byte lvl = minadminlvl; lvl < AdminLevels.Count; ++lvl)
            {
                foreach (TDSPlayer player in AdminLevels[lvl].PlayersOnline)
                {
                    func(player);
                }
            }
        }

        public static void SendChatMessageToAdmins(string msg, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Chat.SendChatMessageToPlayer(player.Client, msg));
        }

        public static void SendLangChatMessageToAdmins(Func<ILanguage, string> propertygetter, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Chat.SendChatMessageToPlayer(player.Client, propertygetter(player.GetLang())));
        }

        public static void SendLangNotificationToAdmins<T>(Func<ILanguage, string> propertygetter, uint minadminlvl = 1)
        {
            CallMethodForAdmins(player => NAPI.Notification.SendNotificationToPlayer(player.Client, propertygetter(player.Language)));
        }
    }
}