using System;
using System.Linq;
using System.Web;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler
{
    public class AnnouncementsHandler
    {
        private readonly string _json;

        public AnnouncementsHandler(TDSDbContext dbContext, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            var data = dbContext.Announcements
                .OrderByDescending(a => a.Id)
                .Take(20)
                .Select(a => new AngularAnnouncementData
                {
                    DaysAgo = (DateTime.UtcNow - a.Created).Days,
                    Text = a.Text
                })
                .ToList();

            _json = HttpUtility.JavaScriptStringEncode(Serializer.ToBrowser(data));

            remoteBrowserEventsHandler.Add(ToServerEvent.LoadAnnouncements, (_) => _json);
        }
    }
}