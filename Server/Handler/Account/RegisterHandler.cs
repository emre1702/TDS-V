using System.ComponentModel.DataAnnotations;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Server;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.Core.Manager.PlayerManager
{
    public class RegisterHandler : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly IModAPI _modAPI;
        private readonly ServerStartHandler _serverStartHandler;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public RegisterHandler(IModAPI modAPI, TDSDbContext dbContext, ILoggingHandler loggingHandler, EventsHandler eventsHandler,
            DatabasePlayerHelper databasePlayerHelper, ServerStartHandler serverStartHandler,
            TDSPlayerHandler tdsPlayerHandler)
            : base(dbContext, loggingHandler)
        {
            (_modAPI, _eventsHandler, _databasePlayerHelper, _serverStartHandler) = (modAPI, eventsHandler, databasePlayerHelper, serverStartHandler);
            _tdsPlayerHandler = tdsPlayerHandler;

            modAPI.ClientEvent.Add<IPlayer, string, string, string>(ToServerEvent.TryRegister, this, TryRegister);
        }

        #endregion Public Constructors

        #region Public Methods

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
                Password = Utils.HashPasswordServer(password),
                Email = email,
                IsVip = false,
                AdminLvl = SharedUtils.GetRandom<short>(0, 1, 2, 3)        // DEBUG
            };
            if (dbPlayer is null)
                return;

            dbPlayer.PlayerSettings = new PlayerSettings
            {
                AllowDataTransfer = false,
                Language = Language.English,
                Hitsound = true,
                Bloodscreen = true,
                FloatingDamageInfo = true,
                ShowConfettiAtRanking = true,
                CheckAFK = true,
                WindowsNotifications = false,
                HideDirtyChat = false,
                ShowCursorOnChatOpen = true,
                Voice3D = false,
                VoiceAutoVolume = false,
                HideChatInfo = false,
                UseDarkTheme = true
            };
            dbPlayer.PlayerStats = new PlayerStats
            {
                LoggedIn = false
            };
            dbPlayer.PlayerTotalStats = new PlayerTotalStats();
            dbPlayer.PlayerClothes = new PlayerClothes();

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Players.Add(dbPlayer);
                await dbContext.SaveChangesAsync();
            });

            LoggingHandler.LogRest(LogType.Register, player, true);

            _eventsHandler.OnPlayerRegister(player, dbPlayer);

            //Todo: Implement that
            // _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_REGISTERED, username));
        }

        public async void TryRegister(IPlayer modPlayer, string username, string password, string email)
        {
            var player = _tdsPlayerHandler.GetNotLoggedIn(modPlayer);
            if (player is null)
                return;
            if (player.TryingToLoginRegister)
                return;

            player.TryingToLoginRegister = true;
            try
            {
                if (!_serverStartHandler.IsReadyForLogin)
                {
                    _modAPI.Thread.RunInMainThread(() => player.SendNotification(player.Language.TRY_AGAIN_LATER));
                    return;
                }

                if (player.ModPlayer is null)
                    return;
                if (username.Length < 3 || username.Length > 20)
                    return;
                string? scName = null;
                _modAPI.Thread.RunInMainThread(() => scName = player.ModPlayer.SocialClubName);
                if (await _databasePlayerHelper.DoesPlayerWithScnameExist(scName!))
                    return;
                if (await _databasePlayerHelper.DoesPlayerWithNameExist(username))
                {
                    _modAPI.Thread.RunInMainThread(() => player.SendNotification(player.Language.PLAYER_WITH_NAME_ALREADY_EXISTS));
                    return;
                }
                char? invalidChar = Utils.CheckNameValid(username);
                if (invalidChar.HasValue)
                {
                    _modAPI.Thread.RunInMainThread(()
                        => player.SendNotification(string.Format(player.Language.CHAR_IN_NAME_IS_NOT_ALLOWED, invalidChar.Value)));
                    return;
                }
                RegisterPlayer(player, username, password, email.Length != 0 ? email : null);
            }
            finally
            {
                player.TryingToLoginRegister = false;
            }
        }

        #endregion Public Methods
    }
}
