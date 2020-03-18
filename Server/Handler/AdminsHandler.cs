using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler
{
    public class AdminsHandler
    {
        public Dictionary<short, AdminLevelDto> AdminLevels = new Dictionary<short, AdminLevelDto>();

        public AdminsHandler(TDSDbContext dbContext, EventsHandler eventsHandler)
        {
            AdminLevels = dbContext.AdminLevels
                .OrderBy(lvl => lvl.Level)
                .Select(lvl => new AdminLevelDto
                            (
                                lvl.Level,
                                "!$" + lvl.ColorR + "|" + lvl.ColorG + "|" + lvl.ColorB + "$"
                            ))
                .ToDictionary(lvl => lvl.Level, lvl => lvl);

            foreach (var entry in dbContext.AdminLevelNames.ToList())
            {
                AdminLevels[entry.Level].Names[entry.Language] = entry.Name;
            }

            eventsHandler.PlayerLoggedIn += SetOnline;
            eventsHandler.PlayerLoggedOutBefore += SetOffline;
        }

        private void SetOnline(ITDSPlayer player)
        {
            if (AdminLevels.ContainsKey(player.AdminLevel.Level))
            {
                AdminLevels[player.AdminLevel.Level].PlayersOnline.Add(player);
            }
        }

        private void SetOffline(ITDSPlayer player)
        {
            if (AdminLevels.ContainsKey(player.AdminLevel.Level))
            {
                AdminLevels[player.AdminLevel.Level].PlayersOnline.Remove(player);
            }
        }

        public void CallMethodForAdmins(Action<ITDSPlayer> func, byte minadminlvl = 1)
        {
            for (byte lvl = minadminlvl; lvl < AdminLevels.Count; ++lvl)
            {
                foreach (ITDSPlayer player in AdminLevels[lvl].PlayersOnline)
                {
                    func(player);
                }
            }
        }

        public void SendMessage(string msg, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendMessage(msg), minadminlvl);
        }

        public void SendMessage(Func<ILanguage, string> propertygetter, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendMessage(propertygetter(player.Language)), minadminlvl);
        }

        public void SendNotification(Func<ILanguage, string> propertygetter, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendNotification(propertygetter(player.Language)), minadminlvl);
        }
    }
}
