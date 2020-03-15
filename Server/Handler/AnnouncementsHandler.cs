using System;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler
{
    class AnnouncementsHandler : IAnnouncementsHandler
    {
        public string Json { get; }

        public AnnouncementsHandler(TDSDbContext dbContext, Serializer serializer)
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

            Json = serializer.ToBrowser(data);
        }
    }
}
