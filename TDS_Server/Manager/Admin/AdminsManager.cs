﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Dto;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Interfaces;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Admin
{
    internal static class AdminsManager
    {
        public static Dictionary<short, AdminLevelDto> AdminLevels = new Dictionary<short, AdminLevelDto>();

        public static async Task Init(TDSDbContext dbcontext)
        {
            AdminLevels = await dbcontext.AdminLevels
                .OrderBy(lvl => lvl.Level)
                .Select(lvl => new AdminLevelDto
                            (
                                lvl.Level,
                                "!$" + lvl.ColorR + "|" + lvl.ColorG + "|" + lvl.ColorB + "$"
                            ))
                .ToDictionaryAsync(lvl => lvl.Level, lvl => lvl);

            foreach (var entry in await dbcontext.AdminLevelNames.ToListAsync())
            {
                AdminLevels[entry.Level].Names[entry.Language] = entry.Name;
            }

            CustomEventManager.OnPlayerLoggedIn += SetOnline;
            CustomEventManager.OnPlayerLoggedOut += SetOffline;
        }

        private static void SetOnline(TDSPlayer player)
        {
            if (AdminLevels.ContainsKey(player.AdminLevel.Level))
            {
                AdminLevels[player.AdminLevel.Level].PlayersOnline.Add(player);
            }
        }

        private static void SetOffline(TDSPlayer player)
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

        public static void SendChatMessageToAdmins(string msg, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendMessage(msg), minadminlvl);
        }

        public static void SendLangChatMessageToAdmins(Func<ILanguage, string> propertygetter, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendMessage(propertygetter(player.GetLang())), minadminlvl);
        }

        public static void SendLangNotificationToAdmins(Func<ILanguage, string> propertygetter, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendNotification(propertygetter(player.Language)), minadminlvl);
        }
    }
}