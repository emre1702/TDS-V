using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Userpanel;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Player;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Userpanel
{
    class UserpanelApplicationsAdminHandler : DatabaseEntityWrapper
    {
        private readonly UserpanelPlayerStatsHandler _userpanelPlayerStatsHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly Serializer _serializer;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public UserpanelApplicationsAdminHandler(UserpanelPlayerStatsHandler userpanelPlayerStatsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler, 
            ISettingsHandler settingsHandler, Serializer serializer, TDSPlayerHandler tdsPlayerHandler) 
            : base(dbContext, loggingHandler)
            => (_userpanelPlayerStatsHandler, _settingsHandler, _serializer, _tdsPlayerHandler) = (userpanelPlayerStatsHandler, settingsHandler, serializer, tdsPlayerHandler);

        public async Task<string?> GetData(ITDSPlayer player)
        {
            try
            {
                if (player.AdminLevel.Level == (short)AdminLevel.User)
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
                    .ToListAsync());

                var appsToSend = apps.Select(a => new AppToSendData
                {
                    ID = a.ID,
                    CreateTime = player.GetLocalDateTimeString(a.CreateTime),
                    PlayerName = a.PlayerName
                });

                return _serializer.ToBrowser(appsToSend);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, player);
                return null;
            }

        }

        public async Task SendApplicationData(ITDSPlayer player, int applicationId)
        {
            if (player.AdminLevel.Level == (short)AdminLevel.User)
                return;

            int creatorId = await ExecuteForDBAsync(async dbContext 
                => await  dbContext.Applications
                    .Where(a => a.Id == applicationId)
                    .Select(a => a.PlayerId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false));

            if (creatorId == default)
                return;

            var answers = await ExecuteForDBAsync(async dbContext 
                => await dbContext.ApplicationAnswers
                    .Where(a => a.ApplicationId == applicationId)
                    .ToDictionaryAsync(a => a.QuestionId, a => a.Answer)
                    .ConfigureAwait(false));
            var questionsJson = UserpanelApplicationUserHandler.AdminQuestions;

            var stats = await _userpanelPlayerStatsHandler.GetPlayerStats(creatorId, false, player).ConfigureAwait(false);

            bool alreadyInvited = await ExecuteForDBAsync(async dbContext
                => await dbContext.ApplicationInvitations
                    .AnyAsync(i => i.ApplicationId == applicationId && i.AdminId == player.Entity!.Id)
                    .ConfigureAwait(false));

            string json = Serializer.ToBrowser(new ApplicationData
            {
                ApplicationID = applicationId,
                Answers = answers,
                Questions = questionsJson,
                Stats = stats,
                AlreadyInvited = alreadyInvited
            });
            player.SendEvent(ToClientEvent.LoadApplicationDataForAdmin, json);

        }

        public async Task<object?> SendInvitation(ITDSPlayer player, object[] args)
        {
            if (player.AdminLevel.Level != (short)AdminLevel.Administrator)
                return null;

            if (args.Length < 2)
                return null;
            int? applicationId;
            if ((applicationId = Utils.GetInt(args[0])) == null)
                return null;
            if (!(args[1] is string message))
                return null;

            var invitation = new ApplicationInvitations
            {
                AdminId = player.Entity!.Id,
                ApplicationId = applicationId.Value,
                Message = message
            };

            int playerId = await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.ApplicationInvitations.Add(invitation);

                await dbContext.SaveChangesAsync();

                return await dbContext.Applications.Where(a => a.Id == applicationId).Select(a => a.PlayerId).FirstOrDefaultAsync();
            });
           
            if (playerId == default)
                return null;

            var target = _tdsPlayerHandler.GetIfExists(playerId);
            if (target is { })
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
