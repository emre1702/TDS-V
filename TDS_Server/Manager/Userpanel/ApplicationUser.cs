using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server.Manager.Userpanel
{
    class ApplicationUser
    {
        public static string AdminQuestions { get; set; } = string.Empty;

        public static void LoadAdminQuestions(TDSDbContext dbContext)
        {
            var list = dbContext.ApplicationQuestions.Include(q => q.Admin).Select(e => new 
            {
                AdminName = e.Admin.Name,
                ID = e.Id,
                e.Question,
                e.AnswerType
            }).ToList()
            .GroupBy(g => g.AdminName)
            .Select(g => new {
                AdminName = g.Key,
                Questions = g.Select(q => new
                {
                    q.ID,
                    q.Question,
                    q.AnswerType
                })
            });

            AdminQuestions = JsonConvert.SerializeObject(list);
        }

        public static async Task<string> GetData(TDSPlayer player)
        {
            var application = await player.ExecuteForDBAsync((dbContext) =>
            {
                return dbContext.Applications.FirstOrDefaultAsync(a => a.PlayerId == player.Entity!.Id);
            });

            if (application == null)
            {
                return JsonConvert.SerializeObject(new { AdminQuestions });
            }

            if (application.CreateTime.AddDays(SettingsManager.ServerSettings.DeleteApplicationAfterDays) < DateTime.UtcNow)
            {
                await player.ExecuteForDBAsync((dbContext) =>
                {
                    dbContext.Remove(application);
                    return dbContext.SaveChangesAsync();
                });

                return JsonConvert.SerializeObject(new { AdminQuestions });
            }

            var applicationData = await player.ExecuteForDBAsync((dbContext) => 
                dbContext.Applications.Where(a => a.PlayerId == player.Entity!.Id)
                    .Include(a => a.Invitations)
                    .ThenInclude(i => i.Admin)
                    .Select(a => new { a.CreateTime, Invitations = a.Invitations.Select(i => 
                        new { 
                            ID = i.Id, 
                            AdminName = i.Admin.Name, 
                            AdminSCName = i.Admin.SCName,
                            i.Message 
                        }
                    )})
                    .FirstOrDefaultAsync());

            if (applicationData == default)
            {
                return JsonConvert.SerializeObject(new { AdminQuestions });
            }

            return JsonConvert.SerializeObject(new { CreateTime = player.GetLocalDateTimeString(applicationData.CreateTime), applicationData.Invitations });
        }

        public static async void CreateApplication(TDSPlayer player, string answersJson)
        {
            var answers = JsonConvert.DeserializeObject<Dictionary<int, string>>(answersJson);

            using var dbContext = new TDSDbContext();

            var application = new Applications
            {
                PlayerId = player.Entity!.Id,
                Closed = false
            };
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
        }

        public static async void AcceptInvitation(TDSPlayer player, int invitationId)
        {
            using var dbContext = new TDSDbContext();

            var invitation = await dbContext.ApplicationInvitations
                .Include(i => i.Admin)
                .Where(i => i.Id == invitationId)
                .FirstOrDefaultAsync();
            if (invitation == null)
            {
                NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED);
                return;
            }

            var application = await dbContext.Applications.Include(a => a.Player).Where(a => a.Id == invitation.ApplicationId).FirstOrDefaultAsync();
            if (application.PlayerId != player.Entity!.Id)
            {
                ErrorLogsManager.Log($"{player.Client.Name} tried to accept an invitation from {invitation.Admin.Name}, but for {application.Player.Name}.", Environment.StackTrace, player);
                return;
            }

            dbContext.Remove(invitation);
            application.Closed = true;
            await dbContext.SaveChangesAsync();

            player.Entity.AdminLeaderId = invitation.AdminId;
            player.Entity.AdminLvl = 1;
            await player.SaveData();

            NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.YOU_ACCEPTED_INVITATION, invitation.Admin.Name));

            TDSPlayer? admin = Player.Player.GetPlayerByID(invitation.AdminId);
            if (admin != null)
            {
                NAPI.Chat.SendChatMessageToPlayer(admin.Client, string.Format(admin.Language.PLAYER_ACCEPTED_YOUR_INVITATION, player.DisplayName));
            } 
            else
            {
                OfflineMessagesManager.AddOfflineMessage(invitation.AdminId, player.Entity.Id, "I've accepted your team application.");
            }
        }

        public static async void RejectInvitation(TDSPlayer player, int invitationId)
        {
            using var dbContext = new TDSDbContext();

            var invitation = await dbContext.ApplicationInvitations
                .Include(i => i.Admin)
                .Where(i => i.Id == invitationId)
                .FirstOrDefaultAsync();
            if (invitation == null)
            {
                NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED);
                return;
            }

            var application = await dbContext.Applications
                .Include(a => a.Player)
                .Where(a => a.Id == invitation.ApplicationId)
                .Select(a => new { PlayerName = a.Player.Name, a.PlayerId })
                .FirstOrDefaultAsync();
            if (application.PlayerId != player.Entity!.Id)
            {
                ErrorLogsManager.Log($"{player.Client.Name} tried to reject an invitation from {invitation.Admin.Name}, but for {application.PlayerName}.", Environment.StackTrace, player);
                return;
            }

            dbContext.Remove(invitation);
            await dbContext.SaveChangesAsync();

            NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.YOU_REJECTED_INVITATION, invitation.Admin.Name));

            TDSPlayer? admin = Player.Player.GetPlayerByID(invitation.AdminId);
            if (admin != null)
            {
                NAPI.Chat.SendChatMessageToPlayer(admin.Client, string.Format(admin.Language.PLAYER_REJECTED_YOUR_INVITATION, player.DisplayName));
            }
            else
            {
                OfflineMessagesManager.AddOfflineMessage(invitation.AdminId, player.Entity.Id, "I rejected your team application.");
            }
        }

        public static async Task DeleteTooLongClosedApplications()
        {
            using var dbContext = new TDSDbContext();

            var apps = await dbContext.Applications
                .Where(a => a.CreateTime.AddDays(SettingsManager.ServerSettings.DeleteApplicationAfterDays) < DateTime.UtcNow)
                .ToListAsync();

            if (apps.Any())
            {
                dbContext.Applications.RemoveRange(apps);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
