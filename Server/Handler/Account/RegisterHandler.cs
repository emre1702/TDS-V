using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Server;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;

namespace TDS_Server.Core.Manager.PlayerManager
{
    public class RegisterHandler : DatabaseEntityWrapper
    {
        private readonly EventsHandler _eventsHandler;
        private readonly LangHelper _langHelper;
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly ServerStartHandler _serverStartHandler;

        public RegisterHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler, EventsHandler eventsHandler, LangHelper langHelper,
            DatabasePlayerHelper databasePlayerHelper, ServerStartHandler serverStartHandler) 
            : base(dbContext, loggingHandler)
            => (_eventsHandler, _langHelper, _databasePlayerHelper, _serverStartHandler) = (eventsHandler, langHelper, databasePlayerHelper, serverStartHandler);

        public async void RegisterPlayer(ITDSPlayer player, string username, string password, string? email)
        {
            if (player.ModPlayer is null)
                return;
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                email = null;
            if (int.TryParse(username, out int result))
                return;

            Players dbPlayer = new Players
            {
                Name = username,
                SCName = player.ModPlayer.SocialClubName,
                Password = Utils.HashPWServer(password),
                Email = email,
                IsVip = false,
                AdminLvl = SharedUtils.GetRandom<short>(0, 1, 2, 3)        // DEBUG
            };

            dbPlayer.PlayerSettings = new PlayerSettings
            {
                AllowDataTransfer = false,
                Language = Language.English,
                Hitsound = true,
                Bloodscreen = true,
                FloatingDamageInfo = true,
                ShowConfettiAtRanking = true
            };
            dbPlayer.PlayerStats = new PlayerStats
            {
                LoggedIn = false
            };
            dbPlayer.PlayerTotalStats = new PlayerTotalStats();
            dbPlayer.PlayerClothes = new PlayerClothes
            {
                IsMale = SharedUtils.GetRandom(true, false)
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Players.Add(dbPlayer);
                await dbContext.SaveChangesAsync();
            });

            LoggingHandler.LogRest(LogType.Register, player, true);

            _eventsHandler.OnPlayerRegister(player);

            //Todo: Implement that
            // _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_REGISTERED, username));
        }

        
        public async void TryRegister(ITDSPlayer player, string username, string password, string email)
        {
            if (!_serverStartHandler.IsReadyForLogin)
            {
                player.SendNotification(player.Language.TRY_AGAIN_LATER);
                return;
            }
                
            if (player.ModPlayer is null)
                return;
            if (username.Length < 3 || username.Length > 20)
                return;
            if (await _databasePlayerHelper.DoesPlayerWithScnameExist(player.ModPlayer.SocialClubName))
                return;
            if (await _databasePlayerHelper.DoesPlayerWithNameExist(username))
            {
                player.SendNotification(player.Language.PLAYER_WITH_NAME_ALREADY_EXISTS);
                return;
            }
            char? invalidChar = Utils.CheckNameValid(username);
            if (invalidChar.HasValue)
            {
                player.SendNotification(string.Format(player.Language.CHAR_IN_NAME_IS_NOT_ALLOWED, invalidChar.Value));
                return;
            }
            RegisterPlayer(player, username, password, email.Length != 0 ? email : null);
        }
    }
}
