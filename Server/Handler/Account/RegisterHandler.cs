﻿using GTANetworkAPI;
using GTANetworkMethods;
using System.ComponentModel.DataAnnotations;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
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
using TDS_Shared.Default;

namespace TDS_Server.Core.Manager.PlayerManager
{
    public class RegisterHandler : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly LangHelper _langHelper;
        private readonly ServerStartHandler _serverStartHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public RegisterHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler, EventsHandler eventsHandler,
            DatabasePlayerHelper databasePlayerHelper, ServerStartHandler serverStartHandler, LangHelper langHelper,
            ITDSPlayerHandler tdsPlayerHandler)
            : base(dbContext, loggingHandler)
        {
            (_eventsHandler, _databasePlayerHelper, _serverStartHandler) = (eventsHandler, databasePlayerHelper, serverStartHandler);
            _langHelper = langHelper;
            _tdsPlayerHandler = tdsPlayerHandler;

            NAPI.ClientEvent.Register<ITDSPlayer, string, string, string, int>(ToServerEvent.TryRegister, this, TryRegister);
        }

        #endregion Public Constructors

        #region Public Methods

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
                IsVip = false,
                AdminLvl = SharedUtils.GetRandom<short>(0, 1, 2, 3)        // DEBUG
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

            LoggingHandler.LogRest(LogType.Register, player, true);

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
                if (!_serverStartHandler.IsReadyForLogin)
                {
                    NAPI.Task.Run(() => player.SendNotification(player.Language.TRY_AGAIN_LATER));
                    return;
                }

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
            finally
            {
                player.TryingToLoginRegister = false;
            }
        }

        #endregion Public Methods
    }
}
