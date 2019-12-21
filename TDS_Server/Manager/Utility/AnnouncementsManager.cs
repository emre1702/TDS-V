using System;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    class AnnouncementsManager
    {
        public static string Json { get; private set; } = string.Empty;

        public static void LoadAnnouncements(TDSDbContext dbContext)
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

            Json = Serializer.ToBrowser(data);
        }
    }
}
