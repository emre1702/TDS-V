﻿using AltV.Net;
using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Server;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Account
{
    public class LoginHandler
    {
        #region Private Fields

        private readonly DatabasePlayerHelper _databasePlayerHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly LangHelper _langHelper;
        private readonly ILoggingHandler _loggingHandler;
        private readonly Serializer _serializer;
        private readonly ServerStartHandler _serverStartHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISettingsHandler _settingsHandler;

        #endregion Private Fields

        #region Public Constructors

        public LoginHandler(
            DatabasePlayerHelper databasePlayerHandler,
            LangHelper langHelper,
            EventsHandler eventsHandler,
            Serializer serializer,
            ISettingsHandler settingsHandler,
            IServiceProvider serviceProvider,
            ILoggingHandler loggingHandler,
            ServerStartHandler serverStartHandler)
        {
            _databasePlayerHandler = databasePlayerHandler;
            _langHelper = langHelper;
            _eventsHandler = eventsHandler;
            _serializer = serializer;
            _settingsHandler = settingsHandler;
            _serviceProvider = serviceProvider;
            _loggingHandler = loggingHandler;
            _serverStartHandler = serverStartHandler;

            _eventsHandler.PlayerRegistered += EventsHandler_PlayerRegistered;

            AltAsync.OnClient<ITDSPlayer, string, string, Task>(ToServerEvent.TryLogin, TryLogin);
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task LoginPlayer(ITDSPlayer player, int id, string? password)
        {
            bool worked = await player.ExecuteForDBAsync(async (dbContext) =>
            {
                Players? entity = await dbContext.Players
                    .Include(p => p.PlayerStats)
                    .Include(p => p.PlayerTotalStats)
                    .Include(p => p.PlayerSettings)
                    .Include(p => p.OfflinemessagesTarget)
                    .Include(p => p.PlayerMapRatings)
                    .Include(p => p.PlayerMapFavourites)
                    .Include(p => p.PlayerRelationsTarget)
                    .Include(p => p.PlayerClothes)
                    .Include(p => p.Challenges)
                    .Include(p => p.CharDatas)
                    .Include(p => p.CharDatas.GeneralData)
                    .Include(p => p.CharDatas.HeritageData)
                    .Include(p => p.CharDatas.FeaturesData)
                    .Include(p => p.CharDatas.AppearanceData)
                    .Include(p => p.CharDatas.HairAndColorsData)
                    .Include(p => p.ThemeSettings)
                    .Include(p => p.WeaponStats)
                    .Include(p => p.WeaponBodypartStats)

                   .FirstOrDefaultAsync(p => p.Id == id);

                await AltAsync.Do(() => player.Entity = entity);

                if (entity is null)
                {
                    await AltAsync.Do(() => player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST));
                    return false;
                }

                if (password is { } && !Utils.IsPasswordValid(password, entity.Password))
                {
                    await AltAsync.Do(() => player.SendNotification(player.Language.WRONG_PASSWORD));
                    return false;
                }

                await AltAsync.Do(() =>
                {
                    //Todo: Implement setting the name
                    //player.name = entity.Name;
                    //Workaround.SetPlayerTeam(player, 1);  // To be able to use custom damagesystem
                });

                entity.PlayerStats.LoggedIn = true;
                entity.PlayerStats.LastLoginTimestamp = DateTime.UtcNow;

                if (entity.ThemeSettings is null)
                    entity.ThemeSettings = new PlayerThemeSettings() { UseDarkTheme = true };
                await dbContext.SaveChangesAsync();
                return true;
            });

            if (!worked || player.Entity == null)
                return;

            var angularConstantsData = ActivatorUtilities.CreateInstance<AngularConstantsDataDto>(_serviceProvider, player);

            var syncedSettingsJson = _serializer.ToClient(_settingsHandler.SyncedSettings);
            var playerSettingsJson = _serializer.ToClient(player.Entity.PlayerSettings);
            var playerThemeSettingsJson = _serializer.ToClient(player.Entity.ThemeSettings);
            var angularContentsJson = _serializer.ToBrowser(angularConstantsData);

            await AltAsync.Do(() =>
            {
                player.SendEvent(ToClientEvent.LoginSuccessful,
                    syncedSettingsJson,
                    playerSettingsJson,
                    playerThemeSettingsJson,
                    angularContentsJson
                );

                player.SetClientMetaData(PlayerDataKey.MapsBoughtCounter.ToString(), player.Entity.PlayerStats.MapsBoughtCounter);
                player.SetClientMetaData(PlayerDataKey.Name.ToString(), player.Entity.Name);

                _eventsHandler.OnPlayerLogin(player);

                _loggingHandler.LogRest(LogType.Login, player, true);

                _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.DisplayName));
            });
        }

        public async Task TryLogin(ITDSPlayer player, string username, string password)
        {
            if (player.TryingToLoginRegister)
                return;
            player.TryingToLoginRegister = true;
            try
            {
                if (!_serverStartHandler.IsReadyForLogin)
                {
                    await AltAsync.Do(() => player.SendNotification(player.Language.TRY_AGAIN_LATER));
                    return;
                }

                int id = await _databasePlayerHandler.GetPlayerIDByName(username);
                if (id != 0)
                {
                    await LoginPlayer(player, id, password);
                }
                else
                    await AltAsync.Do(() => player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST));
            }
            finally
            {
                player.TryingToLoginRegister = false;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private async void EventsHandler_PlayerRegistered(ITDSPlayer player, Players dbPlayer)
        {
            await LoginPlayer(player, dbPlayer.Id, null);
        }

        #endregion Private Methods
    }
}
