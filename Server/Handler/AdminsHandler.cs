using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler
{
    public class AdminsHandler
    {
        private readonly Dictionary<short, AdminLevelDto> _adminLevels  = new Dictionary<short, AdminLevelDto>();

        public AdminsHandler(TDSDbContext dbContext, EventsHandler eventsHandler)
        {
            _adminLevels = dbContext.AdminLevels
                .OrderBy(lvl => lvl.Level)
                .Select(lvl => new AdminLevelDto
                            (
                                lvl.Level,
                                "!$" + lvl.ColorR + "|" + lvl.ColorG + "|" + lvl.ColorB + "$"
                            ))
                .ToDictionary(lvl => lvl.Level, lvl => lvl);

            foreach (var entry in dbContext.AdminLevelNames.ToList())
            {
                _adminLevels[entry.Level].Names[entry.Language] = entry.Name;
            }

            eventsHandler.PlayerLoggedIn += SetOnline;
            eventsHandler.PlayerLoggedOut += SetOffline;
        }

        public AdminLevelDto GetLevel(short adminLvl)
        {
            lock (_adminLevels)
            {
                return _adminLevels[adminLvl];
            }
        }

        public AdminLevelDto GetLowestLevel()
        {
            lock (_adminLevels)
            {
                return _adminLevels[0];
            }
        }

        public AdminLevelDto GetHighestLevel()
        {
            lock (_adminLevels)
            {
                return _adminLevels.Values.MaxBy(a => a.Level).First();
            }
        }

        public void CallMethodForAdmins(Action<ITDSPlayer> func, byte minadminlvl = 1)
        {
            lock (_adminLevels)
            {
                for (byte lvl = minadminlvl; lvl < _adminLevels.Count; ++lvl)
                {
                    foreach (ITDSPlayer player in _adminLevels[lvl].PlayersOnline)
                    {
                        func(player);
                    }
                }
            }
        }

        public void SendMessage(string msg, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendChatMessage(msg), minadminlvl);
        }

        public void SendMessage(Func<ILanguage, string> propertygetter, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendChatMessage(propertygetter(player.Language)), minadminlvl);
        }

        public void SendNotification(Func<ILanguage, string> propertygetter, byte minadminlvl = 1)
        {
            CallMethodForAdmins(player => player.SendNotification(propertygetter(player.Language)), minadminlvl);
        }

        private void SetOffline(ITDSPlayer player)
        {
            lock (_adminLevels)
            {
                if (_adminLevels.ContainsKey(player.Admin.Level.Level))
                {
                    _adminLevels[player.Admin.Level.Level].PlayersOnline.Remove(player);
                }
            }
        }

        private void SetOnline(ITDSPlayer player)
        {
            lock (_adminLevels)
            {
                if (_adminLevels.ContainsKey(player.Admin.Level.Level))
                {
                    _adminLevels[player.Admin.Level.Level].PlayersOnline.Add(player);
                }
            }
        }
    }
}
