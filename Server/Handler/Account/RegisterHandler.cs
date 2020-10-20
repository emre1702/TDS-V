using GTANetworkAPI;
using System;
using System.ComponentModel.DataAnnotations;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Server;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Account
{
    public class RegisterHandler : DatabaseEntityWrapper
    {
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly LangHelper _langHelper;
        private readonly ServerStartHandler _serverStartHandler;

        public RegisterHandler(TDSDbContext dbContext, EventsHandler eventsHandler,
            DatabasePlayerHelper databasePlayerHelper, ServerStartHandler serverStartHandler, LangHelper langHelper)
            : base(dbContext)
        {
            (_eventsHandler, _databasePlayerHelper, _serverStartHandler) = (eventsHandler, databasePlayerHelper, serverStartHandler);
            _langHelper = langHelper;

            NAPI.ClientEvent.Register<ITDSPlayer, string, string, string, int>(ToServerEvent.TryRegister, this, TryRegister);
        }

        public async void RegisterPlayer(ITDSPlayer player, string username, string password, string? email, Language language)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                email = null;
            if (int.TryParse(username, out int result))
                return;

            var (scId, scName) = await NAPI.Task.RunWait(() => (player.SocialClubId, player.SocialClubName));

            var dbPlayer = new Players
            {
                Name = username,
                SCId = scId,
                SCName = scName,
                Password = Utils.HashPasswordServer(password),
                Email = email,
                IsVip = false
            };
            if (dbPlayer is null)
                return;

            dbPlayer.PlayerSettings = new PlayerSettings
            {
                AllowDataTransfer = false,
                Language = language,
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
                ShowCursorInfo = true,
                ShowLobbyLeaveInfo = true
            };
            dbPlayer.PlayerStats = new PlayerStats
            {
                LoggedIn = false
            };
            dbPlayer.PlayerTotalStats = new PlayerTotalStats();
            dbPlayer.PlayerClothes = new PlayerClothes();
            dbPlayer.ThemeSettings = new PlayerThemeSettings
            {
                UseDarkTheme = true
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Players.Add(dbPlayer);
                await dbContext.SaveChangesAsync();
            });

            LoggingHandler.Instance.LogRest(LogType.Register, player, true);

            _eventsHandler.OnPlayerRegister(player, dbPlayer);

            _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_REGISTERED, username));
        }

        public async void TryRegister(ITDSPlayer player, string username, string password, string email, int language)
        {
            if (player.TryingToLoginRegister)
                return;

            player.TryingToLoginRegister = true;
            try
            {
                await _serverStartHandler.LoadingTask.Task;

                if (username.Length < 3 || username.Length > 20)
                    return;
                var scName = await NAPI.Task.RunWait(() => player.SocialClubName);

                if (await _databasePlayerHelper.DoesPlayerWithScnameExist(scName))
                    return;
                if (await _databasePlayerHelper.DoesPlayerWithNameExist(username))
                {
                    NAPI.Task.Run(() => player.SendNotification(player.Language.PLAYER_WITH_NAME_ALREADY_EXISTS));
                    return;
                }
                char? invalidChar = Utils.CheckNameValid(username);
                if (invalidChar.HasValue)
                {
                    NAPI.Task.Run(()
                        => player.SendNotification(string.Format(player.Language.CHAR_IN_NAME_IS_NOT_ALLOWED, invalidChar.Value)));
                    return;
                }
                RegisterPlayer(player, username, password, email.Length != 0 ? email : null, (Language)language);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
            finally
            {
                player.TryingToLoginRegister = false;
            }
        }
    }
}
