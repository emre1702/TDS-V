﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Userpanel.Application;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Userpanel;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelApplicationsAdminHandler : DatabaseEntityWrapper, IUserpanelApplicationsAdminHandler
    {
        private readonly ISettingsHandler _settingsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly UserpanelApplicationUserHandler _userpanelApplicationUserHandler;
        private readonly UserpanelPlayerGeneralStatsHandler _userpanelPlayerStatsHandler;

        public UserpanelApplicationsAdminHandler(UserpanelPlayerGeneralStatsHandler userpanelPlayerStatsHandler, UserpanelApplicationUserHandler userpanelApplicationUserHandler,
            TDSDbContext dbContext, ISettingsHandler settingsHandler, ITDSPlayerHandler tdsPlayerHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(dbContext)
        {
            (_userpanelPlayerStatsHandler, _settingsHandler, _tdsPlayerHandler, _userpanelApplicationUserHandler)
              = (userpanelPlayerStatsHandler, settingsHandler, tdsPlayerHandler, userpanelApplicationUserHandler);

            remoteBrowserEventsHandler.Add(ToServerEvent.SendApplicationInvite, SendInvitation, player => player.Admin.Level.Level == (short)AdminLevel.Administrator);
            remoteBrowserEventsHandler.Add(ToServerEvent.LoadApplicationDataForAdmin, SendApplicationData, player => player.Admin.Level.Level != (short)AdminLevel.User);
        }

        public async Task<string?> GetData(ITDSPlayer player)
        {
            try
            {
                if (player.Admin.Level.Level == (short)AdminLevel.User)
                    return null;

                var apps = await ExecuteForDBAsync(async dbContext => await dbContext.Applications
                        .Where(a => !a.Closed && DateTime.UtcNow < a.CreateTime.AddDays(_settingsHandler.ServerSettings.CloseApplicationAfterDays))
                        .Include(a => a.Player)
                        .Select(a => new
                        {
                            ID = a.Id,
                            a.CreateTime,
                            PlayerName = a.Player.Name
                        })
                        .OrderBy(a => a.ID)
                        .ToListAsync()
                        .ConfigureAwait(false))
                    .ConfigureAwait(false);

                var appsToSend = apps.Select(a => new AppToSendData
                {
                    ID = a.ID,
                    CreateTime = player.Timezone.GetLocalDateTimeString(a.CreateTime),
                    PlayerName = a.PlayerName
                });

                return Serializer.ToBrowser(appsToSend);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, player);
                return null;
            }
        }

        private async Task<object?> SendApplicationData(RemoteBrowserEventArgs args)
        {
            int applicationId = (int)args.Args[0];

            int creatorId = await ExecuteForDBAsync(async dbContext
                => await dbContext.Applications
                    .Where(a => a.Id == applicationId)
                    .Select(a => a.PlayerId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (creatorId == default)
                return null;

            var answers = await ExecuteForDBAsync(async dbContext
                => await dbContext.ApplicationAnswers
                    .Where(a => a.ApplicationId == applicationId)
                    .ToDictionaryAsync(a => a.QuestionId, a => a.Answer)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            var questionsJson = _userpanelApplicationUserHandler.AdminQuestions;

            var stats = await _userpanelPlayerStatsHandler.GetPlayerGeneralStats(creatorId, false, args.Player).ConfigureAwait(false);

            bool alreadyInvited = await ExecuteForDBAsync(async dbContext
                => await dbContext.ApplicationInvitations
                    .AnyAsync(i => i.ApplicationId == applicationId && i.AdminId == args.Player.Entity!.Id)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            string json = Serializer.ToBrowser(new ApplicationData
            {
                ApplicationID = applicationId,
                Answers = answers,
                Questions = questionsJson,
                Stats = stats,
                AlreadyInvited = alreadyInvited
            });

            return json;
        }

        private async Task<object?> SendInvitation(RemoteBrowserEventArgs args)
        {
            if (args.Args.Count < 2)
                return null;
            int? applicationId;
            if ((applicationId = Utils.GetInt(args.Args[0])) == null)
                return null;
            if (args.Args[1] is not string message)
                return null;

            var invitation = new ApplicationInvitations
            {
                AdminId = args.Player.Entity!.Id,
                ApplicationId = applicationId.Value,
                Message = message
            };

            int playerId = await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.ApplicationInvitations.Add(invitation);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                return await dbContext.Applications.Where(a => a.Id == applicationId).Select(a => a.PlayerId).FirstOrDefaultAsync().ConfigureAwait(false);
            })
                .ConfigureAwait(false);

            if (playerId == default)
                return null;

            NAPI.Task.RunSafe(() =>
            {
                var target = _tdsPlayerHandler.GetPlayer(playerId);
                if (target is { })
                {
                    target.SendChatMessage(string.Format(target.Language.YOU_GOT_INVITATION_BY, args.Player.DisplayName));
                    args.Player.SendChatMessage(string.Format(args.Player.Language.SENT_APPLICATION_TO, target.DisplayName));
                }
                else
                {
                    args.Player.SendChatMessage(args.Player.Language.SENT_APPLICATION);
                }
            });

            return null;
        }
    }
}