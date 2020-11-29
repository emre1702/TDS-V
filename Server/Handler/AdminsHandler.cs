using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Events;

namespace TDS.Server.Handler
{
    public class AdminsHandler
    {
        public AdminLevelDto LowestLevel
        {
            get
            {
                lock (_adminLevels)
                {
                    return _adminLevels[0];
                }
            }
        }

        public AdminLevelDto HighestLevel
        {
            get
            {
                lock (_adminLevels)
                {
                    return _adminLevels.Values.MaxBy(a => a.Level).First();
                }
            }
        }

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

        public List<ITDSPlayer> GetAllAdmins()
        {
            lock (_adminLevels)
            {
                return _adminLevels.Values
                    .SelectMany(entry => entry.PlayersOnline)
                    .ToList();
            }
        }

        public List<ITDSPlayer> GetAllAdminsSorted()
        {
            lock (_adminLevels)
            {
                return _adminLevels.Values
                    .SelectMany(entry => entry.PlayersOnline)
                    .OrderByDescending(player => player.Admin.Level.Level)
                    .ThenBy(player => player.Name)
                    .ToList();
            }
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
