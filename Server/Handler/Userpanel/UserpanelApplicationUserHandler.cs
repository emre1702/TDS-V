using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Userpanel;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Handler.Userpanel
{
    public class AdminQuestionData
    {
        [JsonProperty("2")]
        public UserpanelAdminQuestionAnswerType AnswerType { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("1")]
        public string Question { get; set; } = string.Empty;
    }

    public class AdminQuestionsData
    {
        [JsonProperty("0")]
        public string AdminName { get; set; } = string.Empty;

        [JsonProperty("1")]
        public IEnumerable<AdminQuestionData>? Questions { get; set; }
    }

    public class ApplicationUserData
    {
        [JsonProperty("2")]
        public string AdminQuestions { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime CreateDateTime { get; set; }

        [JsonProperty("0")]
        public string? CreateTime { get; set; }

        [JsonProperty("1")]
        public IEnumerable<ApplicationUserInvitationData>? Invitations { get; set; }
    }

    public class ApplicationUserInvitationData
    {
        [JsonProperty("1")]
        public string? AdminName { get; set; }

        [JsonProperty("2")]
        public string? AdminSCName { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("3")]
        public string? Message { get; set; }
    }

    public class UserpanelApplicationUserHandler : DatabaseEntityWrapper, IUserpanelApplicationUserHandler
    {
        private readonly BonusBotConnectorClient _bonusbotConnectorClient;
        private readonly OfflineMessagesHandler _offlineMessagesHandler;

        private readonly ISettingsHandler _settingsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public UserpanelApplicationUserHandler(TDSDbContext dbContext,
            ISettingsHandler settingsHandler, BonusBotConnectorClient bonusbotConnectorClient, ITDSPlayerHandler tdsPlayerHandler,
            OfflineMessagesHandler offlineMessagesHandler, EventsHandler eventsHandler) : base(dbContext)
        {
            _settingsHandler = settingsHandler;
            _bonusbotConnectorClient = bonusbotConnectorClient;
            _tdsPlayerHandler = tdsPlayerHandler;
            _offlineMessagesHandler = offlineMessagesHandler;

            LoadAdminQuestions();

            eventsHandler.Hour += DeleteTooLongClosedApplications;
        }

        public string AdminQuestions { get; set; } = string.Empty;

        public async Task<object?> AcceptInvitation(ITDSPlayer player, ArraySegment<object> args)
        {
            if (args.Count == 0)
                return null;

            int? invitationId;
            if ((invitationId = Utils.GetInt(args[0])) == null)
                return null;

            var invitation = await ExecuteForDBAsync(async dbContext
                => await dbContext.ApplicationInvitations
                    .Include(i => i.Admin)
                    .ThenInclude(a => a.PlayerSettings)
                    .Where(i => i.Id == invitationId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (invitation == null)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED));
                return null;
            }

            var application = await ExecuteForDBAsync(async dbContext =>
                await dbContext.Applications
                    .Include(a => a.Player)
                    .Where(a => a.Id == invitation.ApplicationId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (application.PlayerId != player.Entity!.Id)
            {
                LoggingHandler.Instance.LogError($"{player.Name ?? "?"} tried to accept an invitation from {invitation.Admin.Name}, but for {application.Player.Name}.",
                    Environment.StackTrace, null, player);
                return null;
            }

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Remove(invitation);
                application.Closed = true;
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            player.Entity.AdminLeaderId = invitation.AdminId;
            player.Entity.AdminLvl = 1;
            await player.DatabaseHandler.SaveData().ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
            {
                player.SendChatMessage(string.Format(player.Language.YOU_ACCEPTED_TEAM_INVITATION, invitation.Admin.Name));

                ITDSPlayer? admin = _tdsPlayerHandler.Get(invitation.AdminId);
                if (admin != null)
                {
                    admin.SendChatMessage(string.Format(admin.Language.PLAYER_ACCEPTED_YOUR_INVITATION, player.DisplayName));
                }
                else
                {
                    _offlineMessagesHandler.Add(invitation.Admin, player.Entity, "I've accepted your team application.");
                }
            });

            return null;
        }

        public async Task<object?> CreateApplication(ITDSPlayer player, ArraySegment<object> args)
        {
            string answersJson = (string)args[0];
            var answers = Serializer.FromBrowser<Dictionary<int, string>>(answersJson);

            var application = new Applications
            {
                PlayerId = player.Entity!.Id,
                Closed = false
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Applications.Add(application);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                foreach (var entry in answers)
                {
                    var answer = new ApplicationAnswers
                    {
                        ApplicationId = application.Id,
                        QuestionId = entry.Key,
                        Answer = entry.Value
                    };
                    dbContext.ApplicationAnswers.Add(answer);
                }

                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                await dbContext.Entry(application).Reference(a => a.Player).LoadAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            _bonusbotConnectorClient.ChannelChat?.SendAdminApplication(application, player);
            return null;
        }

        public async void DeleteTooLongClosedApplications(int _)
        {
            try
            {
                await ExecuteForDBAsync(async dbContext =>
                {
                    await dbContext.Applications
                       .Where(a => a.CreateTime.AddDays(_settingsHandler.ServerSettings.DeleteApplicationAfterDays) < DateTime.UtcNow)
                       .ForEachAsync(a => dbContext.Applications.Remove(a))
                       .ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async Task<string> GetData(ITDSPlayer player)
        {
            var application = await player.Database.ExecuteForDBAsync(async dbContext =>
                await dbContext.Applications
                    .FirstOrDefaultAsync(a => a.PlayerId == player.Entity!.Id)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (application == null)
            {
                return Serializer.ToBrowser(new ApplicationUserData { AdminQuestions = AdminQuestions });
            }

            if (application.CreateTime.AddDays(_settingsHandler.ServerSettings.DeleteApplicationAfterDays) < DateTime.UtcNow)
            {
                await player.Database.ExecuteForDBAsync(async (dbContext) =>
                {
                    dbContext.Remove(application);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);

                return Serializer.ToBrowser(new ApplicationUserData { AdminQuestions = AdminQuestions });
            }

            var applicationData = await player.Database.ExecuteForDBAsync(async (dbContext) =>
                await dbContext.Applications.Where(a => a.PlayerId == player.Entity!.Id)
                    .Include(a => a.Invitations)
                    .ThenInclude(i => i.Admin)
                    .Select(a => new ApplicationUserData
                    {
                        CreateDateTime = a.CreateTime,
                        Invitations = a.Invitations.Select(i =>
                            new ApplicationUserInvitationData
                            {
                                ID = i.Id,
                                AdminName = i.Admin.Name,
                                AdminSCName = i.Admin.SCName,
                                Message = i.Message
                            })
                    })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (applicationData == default)
            {
                return Serializer.ToBrowser(new ApplicationUserData { AdminQuestions = AdminQuestions });
            }

            return Serializer.ToBrowser(new ApplicationUserData { CreateTime = player.Timezone.GetLocalDateTimeString(applicationData.CreateDateTime), Invitations = applicationData.Invitations });
        }

        public async Task<object?> RejectInvitation(ITDSPlayer player, ArraySegment<object> args)
        {
            if (args.Count == 0)
                return null;

            int? invitationId;
            if ((invitationId = Utils.GetInt(args[0])) == null)
                return null;

            var invitation = await ExecuteForDBAsync(async dbContext =>
                await dbContext.ApplicationInvitations
                    .Include(i => i.Admin)
                    .ThenInclude(a => a.PlayerSettings)
                    .Where(i => i.Id == invitationId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (invitation == null)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED));
                return null;
            }

            var application = await ExecuteForDBAsync(async dbContext =>
                await dbContext.Applications
                    .Include(a => a.Player)
                    .Where(a => a.Id == invitation.ApplicationId)
                    .Select(a => new { PlayerName = a.Player.Name, a.PlayerId })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (application.PlayerId != player.Entity!.Id)
            {
                LoggingHandler.Instance.LogError($"{player.Name ?? "?"} tried to reject an invitation from {invitation.Admin.Name}, but for {application.PlayerName}.", Environment.StackTrace, null, player);
                return null;
            }

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Remove(invitation);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
            {
                player.SendChatMessage(string.Format(player.Language.YOU_REJECTED_TEAM_INVITATION, invitation.Admin.Name));

                ITDSPlayer? admin = _tdsPlayerHandler.Get(invitation.AdminId);
                if (admin != null)
                {
                    admin.SendChatMessage(string.Format(admin.Language.PLAYER_REJECTED_YOUR_INVITATION, player.DisplayName));
                }
                else
                {
                    _offlineMessagesHandler.Add(invitation.Admin, player.Entity, "I rejected your team application.");
                }
            });

            return null;
        }

        private async void LoadAdminQuestions()
        {
            try
            {
                var list = await ExecuteForDB(dbContext => dbContext.ApplicationQuestions.Include(q => q.Admin).Select(e => new
                {
                    AdminName = e.Admin.Name,
                    ID = e.Id,
                    e.Question,
                    e.AnswerType
                }).ToList()
                .GroupBy(g => g.AdminName)
                .Select(g => new AdminQuestionsData
                {
                    AdminName = g.Key,
                    Questions = g.Select(q => new AdminQuestionData
                    {
                        ID = q.ID,
                        Question = q.Question,
                        AnswerType = q.AnswerType
                    })
                }))
                .ConfigureAwait(false);

                AdminQuestions = Serializer.ToBrowser(list);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }
    }
}
