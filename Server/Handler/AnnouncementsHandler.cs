﻿using System;
using System.Linq;
using System.Web;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;

namespace TDS_Server.Handler
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
