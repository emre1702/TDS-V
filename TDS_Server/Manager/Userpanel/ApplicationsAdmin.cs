using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server.Manager.Userpanel
{
    static class ApplicationsAdmin
    {

        public static async Task<string?> GetData(TDSPlayer player)
        {
            try
            {
                if (player.AdminLevel.Level == (short)EAdminLevel.User)
                    return null;

                using var dbContext = new TDSDbContext();

                var apps = await dbContext.Applications
                    .Where(a => !a.Closed && DateTime.UtcNow < a.CreateTime.AddDays(SettingsManager.ServerSettings.CloseApplicationAfterDays))
                    .Include(a => a.Player)
                    .Select(a => new
                    {
                        ID = a.Id,
                        a.CreateTime,
                        PlayerName = a.Player.Name
                    })
                    .OrderBy(a => a.ID)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var appsToSend = apps.Select(a => new AppToSendData
                {
                    ID = a.ID,
                    CreateTime = player.GetLocalDateTimeString(a.CreateTime),
                    PlayerName = a.PlayerName
                });

                return Serializer.ToBrowser(appsToSend);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
                return null;
            }
            
        }

        public static async Task SendApplicationData(TDSPlayer player, int applicationId)
        {
            if (player.AdminLevel.Level == (short)EAdminLevel.User)
                return;

            using var dbContext = new TDSDbContext();

            int creatorId = await dbContext.Applications              
                .Where(a => a.Id == applicationId)
                .Select(a => a.PlayerId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (creatorId == default)
                return;

            var answers = await dbContext.ApplicationAnswers
                .Where(a => a.ApplicationId == applicationId)
                .ToDictionaryAsync(a => a.QuestionId, a => a.Answer)
                .ConfigureAwait(false);
            var questionsJson = ApplicationUser.AdminQuestions;

            var stats = await PlayerStats.GetPlayerStats(creatorId, false, player).ConfigureAwait(false);

            bool alreadyInvited = await dbContext.ApplicationInvitations
                .AnyAsync(i => i.ApplicationId == applicationId && i.AdminId == player.Entity!.Id)
                .ConfigureAwait(false);

            string json = Serializer.ToBrowser(new ApplicationData
            { 
                ApplicationID = applicationId,
                Answers = answers, 
                Questions = questionsJson, 
                Stats = stats,
                AlreadyInvited = alreadyInvited
            });
            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.LoadApplicationDataForAdmin, json);
            
        }

        public static async Task<object?> SendInvitation(TDSPlayer player, object[] args)
        {
            if (player.AdminLevel.Level != (short)EAdminLevel.Administrator)
                return null;

            if (args.Length < 2)
                return null;
            int? applicationId;
            if ((applicationId = Utils.GetInt(args[0])) == null)
                return null;
            if (!(args[1] is string message))
                return null;

            using var dbContext = new TDSDbContext();

            var invitation = new ApplicationInvitations
            {
                AdminId = player.Entity!.Id,
                ApplicationId = applicationId.Value,
                Message = message
            };
            dbContext.ApplicationInvitations.Add(invitation);
            
            await dbContext.SaveChangesAsync().ConfigureAwait(true);

            int playerId = await dbContext.Applications.Where(a => a.Id == applicationId).Select(a => a.PlayerId).FirstOrDefaultAsync().ConfigureAwait(true);
            if (playerId == default)
                return null;

            var target = PlayerManager.PlayerManager.GetPlayerByID(playerId);
            if (target != null)
            {
                target.SendMessage(string.Format(target.Language.YOU_GOT_INVITATION_BY, player.DisplayName));
                player.SendMessage(string.Format(player.Language.SENT_APPLICATION_TO, target.DisplayName));
            }
            else
            {
                player.SendMessage(player.Language.SENT_APPLICATION);
            }

            return null;
        }
    }

    public class AppToSendData
    {
        [JsonProperty("0")]
        public int ID { get; set; }
        [JsonProperty("1")]
        public string CreateTime { get; set; } = string.Empty;
        [JsonProperty("2")]
        public string PlayerName { get; set; } = string.Empty;
    }

    public class ApplicationData
    {
        [JsonProperty("0")]
        public int ApplicationID { get; set; }
        [JsonProperty("1")]
        public Dictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();
        [JsonProperty("2")]
        public string Questions { get; set; } = string.Empty;
        [JsonProperty("3")]
        public PlayerUserpanelStatsDataDto? Stats { get; set; }
        [JsonProperty("4")]
        public bool AlreadyInvited { get; set; }
    }
}
