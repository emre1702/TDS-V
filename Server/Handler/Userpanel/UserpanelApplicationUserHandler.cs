using BonusBotConnector.Client;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Userpanel;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Handler.Userpanel
{
    public class AdminQuestionData
    {
        #region Public Properties

        [JsonProperty("2")]
        public UserpanelAdminQuestionAnswerType AnswerType { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("1")]
        public string Question { get; set; } = string.Empty;

        #endregion Public Properties
    }

    public class AdminQuestionsData
    {
        #region Public Properties

        [JsonProperty("0")]
        public string AdminName { get; set; } = string.Empty;

        [JsonProperty("1")]
        public IEnumerable<AdminQuestionData>? Questions { get; set; }

        #endregion Public Properties
    }

    public class ApplicationUserData
    {
        #region Public Properties

        [JsonProperty("2")]
        public string AdminQuestions { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime CreateDateTime { get; set; }

        [JsonProperty("0")]
        public string? CreateTime { get; set; }

        [JsonProperty("1")]
        public IEnumerable<ApplicationUserInvitationData>? Invitations { get; set; }

        #endregion Public Properties
    }

    public class ApplicationUserInvitationData
    {
        #region Public Properties

        [JsonProperty("1")]
        public string? AdminName { get; set; }

        [JsonProperty("2")]
        public string? AdminSCName { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("3")]
        public string? Message { get; set; }

        #endregion Public Properties
    }

    public class UserpanelApplicationUserHandler : DatabaseEntityWrapper, IUserpanelApplicationUserHandler
    {
        #region Private Fields

        private readonly BonusBotConnectorClient _bonusbotConnectorClient;
        private readonly IModAPI _modAPI;
        private readonly OfflineMessagesHandler _offlineMessagesHandler;
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelApplicationUserHandler(IModAPI modAPI, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer,
            ISettingsHandler settingsHandler, BonusBotConnectorClient bonusbotConnectorClient, TDSPlayerHandler tdsPlayerHandler,
            OfflineMessagesHandler offlineMessagesHandler, EventsHandler eventsHandler) : base(dbContext, loggingHandler)
        {
            _modAPI = modAPI;
            _serializer = serializer;
            _settingsHandler = settingsHandler;
            _bonusbotConnectorClient = bonusbotConnectorClient;
            _tdsPlayerHandler = tdsPlayerHandler;
            _offlineMessagesHandler = offlineMessagesHandler;

            LoadAdminQuestions();

            eventsHandler.Hour += DeleteTooLongClosedApplications;
        }

        #endregion Public Constructors

        #region Public Properties

        public string AdminQuestions { get; set; } = string.Empty;

        #endregion Public Properties

        #region Public Methods

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
                .FirstOrDefaultAsync());
            if (invitation == null)
            {
                _modAPI.Thread.QueueIntoMainThread(() => player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED));
                return null;
            }

            var application = await ExecuteForDBAsync(async dbContext =>
                await dbContext.Applications.Include(a => a.Player).Where(a => a.Id == invitation.ApplicationId).FirstOrDefaultAsync());
            if (application.PlayerId != player.Entity!.Id)
            {
                LoggingHandler.LogError($"{player.ModPlayer?.Name ?? "?"} tried to accept an invitation from {invitation.Admin.Name}, but for {application.Player.Name}.",
                    Environment.StackTrace, null, player);
                return null;
            }

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Remove(invitation);
                application.Closed = true;
                await dbContext.SaveChangesAsync();
            });

            player.Entity.AdminLeaderId = invitation.AdminId;
            player.Entity.AdminLvl = 1;
            await player.SaveData();

            _modAPI.Thread.QueueIntoMainThread(() =>
            {
                player.SendMessage(string.Format(player.Language.YOU_ACCEPTED_TEAM_INVITATION, invitation.Admin.Name));

                ITDSPlayer? admin = _tdsPlayerHandler.GetIfExists(invitation.AdminId);
                if (admin != null)
                {
                    admin.SendMessage(string.Format(admin.Language.PLAYER_ACCEPTED_YOUR_INVITATION, player.DisplayName));
                }
                else
                {
                    _offlineMessagesHandler.AddOfflineMessage(invitation.Admin, player.Entity, "I've accepted your team application.");
                }
            });

            return null;
        }

        public async Task<object?> CreateApplication(ITDSPlayer player, ArraySegment<object> args)
        {
            string answersJson = (string)args[0];
            var answers = _serializer.FromBrowser<Dictionary<int, string>>(answersJson);

            var application = new Applications
            {
                PlayerId = player.Entity!.Id,
                Closed = false
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Applications.Add(application);
                await dbContext.SaveChangesAsync();

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

                await dbContext.SaveChangesAsync();

                await dbContext.Entry(application).Reference(a => a.Player).LoadAsync();
            });

            _bonusbotConnectorClient.ChannelChat?.SendAdminApplication(application, player);
            return null;
        }

        public async void DeleteTooLongClosedApplications(int _)
        {
            await ExecuteForDB(async dbContext =>
            {
                await dbContext.Applications
                   .Where(a => a.CreateTime.AddDays(_settingsHandler.ServerSettings.DeleteApplicationAfterDays) < DateTime.UtcNow)
                   .ForEachAsync(a => dbContext.Applications.Remove(a));
                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<string> GetData(ITDSPlayer player)
        {
            var application = await player.ExecuteForDBAsync(async dbContext =>
                await dbContext.Applications.FirstOrDefaultAsync(a => a.PlayerId == player.Entity!.Id));

            if (application == null)
            {
                return _serializer.ToBrowser(new ApplicationUserData { AdminQuestions = AdminQuestions });
            }

            if (application.CreateTime.AddDays(_settingsHandler.ServerSettings.DeleteApplicationAfterDays) < DateTime.UtcNow)
            {
                await player.ExecuteForDBAsync(async (dbContext) =>
                {
                    dbContext.Remove(application);
                    await dbContext.SaveChangesAsync();
                });

                return _serializer.ToBrowser(new ApplicationUserData { AdminQuestions = AdminQuestions });
            }

            var applicationData = await player.ExecuteForDBAsync(async (dbContext) =>
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
                    .FirstOrDefaultAsync());

            if (applicationData == default)
            {
                return _serializer.ToBrowser(new ApplicationUserData { AdminQuestions = AdminQuestions });
            }

            return _serializer.ToBrowser(new ApplicationUserData { CreateTime = player.GetLocalDateTimeString(applicationData.CreateDateTime), Invitations = applicationData.Invitations });
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
                    .FirstOrDefaultAsync());
            if (invitation == null)
            {
                _modAPI.Thread.QueueIntoMainThread(() => player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED));
                return null;
            }

            var application = await ExecuteForDBAsync(async dbContext =>
                await dbContext.Applications
                    .Include(a => a.Player)
                    .Where(a => a.Id == invitation.ApplicationId)
                    .Select(a => new { PlayerName = a.Player.Name, a.PlayerId })
                    .FirstOrDefaultAsync());
            if (application.PlayerId != player.Entity!.Id)
            {
                LoggingHandler.LogError($"{player.ModPlayer?.Name ?? "?"} tried to reject an invitation from {invitation.Admin.Name}, but for {application.PlayerName}.", Environment.StackTrace, null, player);
                return null;
            }

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Remove(invitation);
                await dbContext.SaveChangesAsync();
            });

            _modAPI.Thread.QueueIntoMainThread(() =>
            {
                player.SendMessage(string.Format(player.Language.YOU_REJECTED_TEAM_INVITATION, invitation.Admin.Name));

                ITDSPlayer? admin = _tdsPlayerHandler.GetIfExists(invitation.AdminId);
                if (admin != null)
                {
                    admin.SendMessage(string.Format(admin.Language.PLAYER_REJECTED_YOUR_INVITATION, player.DisplayName));
                }
                else
                {
                    _offlineMessagesHandler.AddOfflineMessage(invitation.Admin, player.Entity, "I rejected your team application.");
                }
            });

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private async void LoadAdminQuestions()
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
            }));

            AdminQuestions = _serializer.ToBrowser(list);
        }

        #endregion Private Methods
    }
}
