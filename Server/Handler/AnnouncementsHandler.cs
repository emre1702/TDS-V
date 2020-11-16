using System;
using System.Linq;
using System.Web;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;

namespace TDS.Server.Handler
{
    public class AnnouncementsHandler : IAnnouncementsHandler
    {
        public string Json { get; }

        public AnnouncementsHandler(TDSDbContext dbContext)
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

            Json = HttpUtility.JavaScriptStringEncode(Serializer.ToBrowser(data));
        }
    }
}
