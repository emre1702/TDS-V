﻿using System;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Shared.Core;

namespace TDS_Server.Handler
{
    public class AnnouncementsHandler : IAnnouncementsHandler
    {
        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public string Json { get; }

        #endregion Public Properties
    }
}
