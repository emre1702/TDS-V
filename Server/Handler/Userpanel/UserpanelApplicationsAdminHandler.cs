using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Userpanel;
using TDS_Server.Handler.Entities;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Userpanel
{
    public class ApplicationData
    {
        #region Public Properties

        [JsonProperty("4")]
        public bool AlreadyInvited { get; set; }

        [JsonProperty("1")]
        public Dictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();

        [JsonProperty("0")]
        public int ApplicationID { get; set; }

        [JsonProperty("2")]
        public string Questions { get; set; } = string.Empty;

        [JsonProperty("3")]
        public PlayerUserpanelGeneralStatsDataDto? Stats { get; set; }

        #endregion Public Properties
    }

    public class AppToSendData
    {
        #region Public Properties

        [JsonProperty("1")]
        public string CreateTime { get; set; } = string.Empty;

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("2")]
        public string PlayerName { get; set; } = string.Empty;

        #endregion Public Properties
    }

    public class UserpanelApplicationsAdminHandler : DatabaseEntityWrapper, IUserpanelApplicationsAdminHandler
    {
        #region Private Fields

        private readonly ISettingsHandler _settingsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly UserpanelApplicationUserHandler _userpanelApplicationUserHandler;
        private readonly UserpanelPlayerGeneralStatsHandler _userpanelPlayerStatsHandler;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelApplicationsAdminHandler(UserpanelPlayerGeneralStatsHandler userpanelPlayerStatsHandler, UserpanelApplicationUserHandler userpanelApplicationUserHandler,
            TDSDbContext dbContext, ILoggingHandler loggingHandler, ISettingsHandler settingsHandler, ITDSPlayerHandler tdsPlayerHandler)
            : base(dbContext, loggingHandler)
            => (_userpanelPlayerStatsHandler, _settingsHandler, _tdsPlayerHandler, _userpanelApplicationUserHandler)
            = (userpanelPlayerStatsHandler, settingsHandler, tdsPlayerHandler, userpanelApplicationUserHandler);

        #endregion Public Constructors

        #region Public Methods

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
                    .ToListAsync());

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
                LoggingHandler.LogError(ex, player);
                return null;
            }
        }

        public async Task<object?> SendApplicationData(ITDSPlayer player, ArraySegment<object> args)
        {
            int applicationId = (int)args[0];

            if (player.Admin.Level.Level == (short)AdminLevel.User)
                return null;

            int creatorId = await ExecuteForDBAsync(async dbContext
                => await dbContext.Applications
                    .Where(a => a.Id == applicationId)
                    .Select(a => a.PlayerId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false));

            if (creatorId == default)
                return null;

            var answers = await ExecuteForDBAsync(async dbContext
                => await dbContext.ApplicationAnswers
                    .Where(a => a.ApplicationId == applicationId)
                    .ToDictionaryAsync(a => a.QuestionId, a => a.Answer)
                    .ConfigureAwait(false));
            var questionsJson = _userpanelApplicationUserHandler.AdminQuestions;

            var stats = await _userpanelPlayerStatsHandler.GetPlayerGeneralStats(creatorId, false, player).ConfigureAwait(false);

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

            return json;
        }

        public async Task<object?> SendInvitation(ITDSPlayer player, ArraySegment<object> args)
        {
            if (player.Admin.Level.Level != (short)AdminLevel.Administrator)
                return null;

            if (args.Count < 2)
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

            NAPI.Task.Run(() =>
            {
                var target = _tdsPlayerHandler.Get(playerId);
                if (target is { })
                {
                    target.SendChatMessage(string.Format(target.Language.YOU_GOT_INVITATION_BY, player.DisplayName));
                    player.SendChatMessage(string.Format(player.Language.SENT_APPLICATION_TO, target.DisplayName));
                }
                else
                {
                    player.SendChatMessage(player.Language.SENT_APPLICATION);
                }
            });

            return null;
        }

        #endregion Public Methods
    }
}
