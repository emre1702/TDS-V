using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Player.Char;
using TDS.Server.Database.Entity.Player.Settings;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.Server;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Account
{
    public class RegisterHandler
    {
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly LangHelper _langHelper;
        private readonly ServerStartHandler _serverStartHandler;

        public RegisterHandler(EventsHandler eventsHandler,
            DatabasePlayerHelper databasePlayerHelper, ServerStartHandler serverStartHandler, LangHelper langHelper)
        {
            (_eventsHandler, _databasePlayerHelper, _serverStartHandler) = (eventsHandler, databasePlayerHelper, serverStartHandler);
            _langHelper = langHelper;

            NAPI.ClientEvent.Register<ITDSPlayer, string, string, string, int>(ToServerEvent.TryRegister, this, TryRegister);
        }

        public async void RegisterPlayer(ITDSPlayer player, string username, string password, string? email, Language language, string scName, ulong scId)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                email = null;
            if (int.TryParse(username, out int result))
                return;

            var dbPlayer = CreatePlayerEntity(username, password, email, scName, scId);
            dbPlayer.PlayerSettings = CreatePlayerSettingsEntity(language);
            dbPlayer.PlayerStats = CreatePlayerStatsEntity();
            dbPlayer.PlayerTotalStats = new PlayerTotalStats();
            dbPlayer.PlayerClothes = new PlayerClothes();
            dbPlayer.ThemeSettings = CreatePlayerThemeSettingsEntity();
            dbPlayer.KillInfoSettings = CreatePlayerKillInfoSettingsEntity();

            await player.Database.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Players.Add(dbPlayer);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

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
                await NAPI.Task.RunWait(player.Init);

                await _serverStartHandler.LoadingTask.Task.ConfigureAwait(false);

                if (username.Length < 3 || username.Length > 20)
                    return;

                if (await _databasePlayerHelper.DoesPlayerWithScnameExist(player.SocialClubName).ConfigureAwait(false))
                    return;
                if (await _databasePlayerHelper.DoesPlayerWithNameExist(username).ConfigureAwait(false))
                {
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.PLAYER_WITH_NAME_ALREADY_EXISTS));
                    return;
                }
                char? invalidChar = Utils.CheckNameValid(username);
                if (invalidChar.HasValue)
                {
                    NAPI.Task.RunSafe(()
                        => player.SendNotification(string.Format(player.Language.CHAR_IN_NAME_IS_NOT_ALLOWED, invalidChar.Value)));
                    return;
                }
                RegisterPlayer(player, username, password, email.Length != 0 ? email : null, (Language)language, player.SocialClubName, player.SocialClubId);
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

        private Players CreatePlayerEntity(string username, string password, string? email, string scName, ulong scId)
            => new Players
            {
                Name = username,
                SCId = scId,
                SCName = scName,
                Password = Utils.HashPasswordServer(password),
                Email = email,
                IsVip = false
            };

        private PlayerSettings CreatePlayerSettingsEntity(Language language)
            => new PlayerSettings
            {
                General = new PlayerGeneralSettings 
                { 
                    AllowDataTransfer = false,
                    Language = language,
                    CheckAFK = true,
                    WindowsNotifications = false,
                    ShowConfettiAtRanking = true,
                },
                FightEffect = new PlayerFightEffectSettings
                {
                    Hitsound = true,
                    Bloodscreen = true,
                    FloatingDamageInfo = true
                },
                Chat = new PlayerChatSettings
                {
                    HideDirtyChat = false,
                    ShowCursorOnChatOpen = true,
                    HideChatInfo = false
                },
                Voice = new PlayerVoiceSettings
                {
                    Voice3D = false,
                    VoiceAutoVolume = false,
                },
                Info = new PlayerInfoSettings
                {
                    ShowCursorInfo = true,
                    ShowLobbyLeaveInfo = true
                }
            };

        private PlayerStats CreatePlayerStatsEntity()
            => new PlayerStats
            {
                LoggedIn = false
            };

        private PlayerThemeSettings CreatePlayerThemeSettingsEntity()
           => new PlayerThemeSettings
           {
               UseDarkTheme = true
           };

        private PlayerKillInfoSettings CreatePlayerKillInfoSettingsEntity()
           => new PlayerKillInfoSettings
           {
               ShowIcon = true
           };


        /* 
         * charDatas.GeneralData.Add(new PlayerCharGeneralDatas { Slot = slot, SyncedData = new CharCreateGeneralData { Slot = slot, IsMale = isMale } });
            charDatas.HeritageData.Add(new PlayerCharHeritageDatas
            {
                Slot = slot,
                SyncedData = new CharCreateHeritageData
                {
                    Slot = slot,
                    FatherIndex = 0,
                    MotherIndex = 21,
                    ResemblancePercentage = isMale ? 1 : 0,
                    SkinTonePercentage = isMale ? 1 : 0
                }
            });
            charDatas.FeaturesData.Add(new PlayerCharFeaturesDatas { Slot = slot, SyncedData = new CharCreateFeaturesData { Slot = slot } });
            charDatas.AppearanceData.Add(new PlayerCharAppearanceDatas { Slot = slot, SyncedData = new CharCreateAppearanceData { Slot = slot } });
            charDatas.HairAndColorsData.Add(new PlayerCharHairAndColorsDatas { Slot = slot, SyncedData = new CharCreateHairAndColorsData { Slot = slot } });*/
    }
}
