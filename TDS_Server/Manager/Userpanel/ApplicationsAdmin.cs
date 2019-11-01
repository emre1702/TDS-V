using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Rest;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server.Manager.Userpanel
{
    class ApplicationsAdmin
    {

        public static async Task<string?> GetData(TDSPlayer player)
        {
            try
            {
                if (player.AdminLevel.Level != (short)EAdminLevel.Administrator)
                    return null;

                using var dbContext = new TDSNewContext();

                var apps = await dbContext.Applications
                    .Where(a => !a.Closed && DateTime.UtcNow < a.CreateTime.AddDays(ServerConstants.CloseApplicationAfterDays))
                    .Include(a => a.Player)
                    .Select(a => new
                    {
                        ID = a.Id,
                        a.CreateTime,
                        PlayerName = a.Player.Name
                    })
                    .OrderBy(a => a.ID)
                    .ToListAsync();

                var appsToSend = apps.Select(a => new
                {
                    a.ID,
                    CreateTime = player.GetLocalDateTime(a.CreateTime).ToString(Constants.DateTimeOffsetFormat),
                    a.PlayerName
                });

                return JsonConvert.SerializeObject(apps);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
                return null;
            }
            
        }

        public static async Task SendApplicationData(TDSPlayer player, int applicationId)
        {
            if (player.AdminLevel.Level != (short)EAdminLevel.Administrator)
                return;

            using var dbContext = new TDSNewContext();

            int creatorId = await dbContext.Applications              
                .Where(a => a.Id == applicationId)
                .Select(a => a.PlayerId)
                .FirstOrDefaultAsync();

            if (creatorId == default)
                return;

            var answers = await dbContext.ApplicationAnswers
                .Where(a => a.ApplicationId == applicationId)
                .ToDictionaryAsync(a => a.QuestionId, a => a.Answer);
            var questionsJson = ApplicationUser.AdminQuestions;

            var stats = await PlayerStats.GetPlayerStats(creatorId, false, player);

            string json = JsonConvert.SerializeObject(new { Answers = answers, Questions = questionsJson, Stats = stats });
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.LoadApplicationDataForAdmin, json);
            
        }

        public static async Task SendInvitation(TDSPlayer player, int applicationId, string message)
        {
            if (player.AdminLevel.Level != (short)EAdminLevel.Administrator)
                return;

            using var dbContext = new TDSNewContext();

            var invitation = new ApplicationInvitations
            {
                AdminId = player.Entity!.Id,
                ApplicationId = applicationId,
                Message = message
            };
            dbContext.ApplicationInvitations.Add(invitation);
            
            await dbContext.SaveChangesAsync();

            int playerId = await dbContext.Applications.Where(a => a.Id == applicationId).Select(a => a.PlayerId).FirstOrDefaultAsync();
            if (playerId == default)
                return;

            var target = Player.Player.GetPlayerByID(playerId);
            if (target != null)
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, string.Format(target.Language.YOU_GOT_INVITATION_BY, player.DisplayName));
                NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.SENT_APPLICATION_TO, target.DisplayName));
            }
            else
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.SENT_APPLICATION);
            }
        }
    }
}
