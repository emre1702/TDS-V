using System;
using System.Threading.Tasks;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
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
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly LangHelper _langHelper;
        private readonly ILoggingHandler _loggingHandler;
        private readonly Serializer _serializer;
        private readonly ServerStartHandler _serverStartHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISettingsHandler _settingsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public LoginHandler(
            DatabasePlayerHelper databasePlayerHandler,
            LangHelper langHelper,
            EventsHandler eventsHandler,
            Serializer serializer,
            ISettingsHandler settingsHandler,
            IServiceProvider serviceProvider,
            DataSyncHandler dataSyncHandler,
            ILoggingHandler loggingHandler,
            ServerStartHandler serverStartHandler,
            ITDSPlayerHandler tdsPlayerHandler)
        {
            _databasePlayerHandler = databasePlayerHandler;
            _langHelper = langHelper;
            _eventsHandler = eventsHandler;
            _serializer = serializer;
            _settingsHandler = settingsHandler;
            _serviceProvider = serviceProvider;
            _dataSyncHandler = dataSyncHandler;
            _loggingHandler = loggingHandler;
            _serverStartHandler = serverStartHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            _eventsHandler.PlayerRegistered += EventsHandler_PlayerRegistered;

            NAPI.ClientEvent.Register<ITDSPlayer, string, string>(ToServerEvent.TryLogin, this, TryLogin);
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task LoginPlayer(ITDSPlayer player, int id, string? password)
        {
            bool worked = await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                Players? entity = await dbContext.Players
                    .Include(p => p.PlayerStats)
                    .Include(p => p.PlayerTotalStats)
                    .Include(p => p.PlayerSettings)
                    .Include(p => p.PlayerClothes)
                    .Include(p => p.ThemeSettings)

                   .FirstOrDefaultAsync(p => p.Id == id);

                if (entity is null)
                {
                    NAPI.Task.Run(() => player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST));
                    return false;
                }

                if (password is { } && !Utils.IsPasswordValid(password, entity.Password))
                {
                    NAPI.Task.Run(() => player.SendNotification(player.Language.WRONG_PASSWORD));
                    return false;
                }

                NAPI.Task.Run(() =>
                {
                    player.Name = entity.Name;
                    //Workaround.SetPlayerTeam(player, 1);  // To be able to use custom damagesystem
                });

                entity.PlayerStats.LoggedIn = true;
                entity.PlayerStats.LastLoginTimestamp = DateTime.UtcNow;

                if (entity.ThemeSettings is null)
                    entity.ThemeSettings = new PlayerThemeSettings() { UseDarkTheme = true };
                await dbContext.SaveChangesAsync();

                await dbContext.Entry(entity).Reference(e => e.CharDatas).LoadAsync();
                await dbContext.Entry(entity.CharDatas).Collection(e => e.AppearanceData).LoadAsync();
                await dbContext.Entry(entity.CharDatas).Collection(e => e.FeaturesData).LoadAsync();
                await dbContext.Entry(entity.CharDatas).Collection(e => e.GeneralData).LoadAsync();
                await dbContext.Entry(entity.CharDatas).Collection(e => e.HairAndColorsData).LoadAsync();
                await dbContext.Entry(entity.CharDatas).Collection(e => e.HeritageData).LoadAsync();

                await dbContext.Entry(entity).Collection(e => e.OfflinemessagesTarget).LoadAsync();
                await dbContext.Entry(entity).Collection(e => e.PlayerMapRatings).LoadAsync();
                await dbContext.Entry(entity).Collection(e => e.PlayerMapFavourites).LoadAsync();
                await dbContext.Entry(entity).Collection(e => e.PlayerRelationsTarget).LoadAsync();
                await dbContext.Entry(entity).Collection(e => e.Challenges).LoadAsync();
                await dbContext.Entry(entity).Collection(e => e.WeaponStats).LoadAsync();
                await dbContext.Entry(entity).Collection(e => e.WeaponBodypartStats).LoadAsync();

                await NAPI.Task.RunWait(() => player.Entity = entity);

                return true;
            });

            if (!worked || player.Entity == null)
                return;

            var angularConstantsData = ActivatorUtilities.CreateInstance<AngularConstantsDataDto>(_serviceProvider, player);

            var syncedSettingsJson = _serializer.ToClient(_settingsHandler.SyncedSettings);
            var playerSettingsJson = _serializer.ToClient(player.Entity.PlayerSettings);
            var playerThemeSettingsJson = _serializer.ToClient(player.Entity.ThemeSettings);
            var angularContentsJson = _serializer.ToBrowser(angularConstantsData);

            NAPI.Task.Run(() =>
            {
                player.TriggerEvent(ToClientEvent.LoginSuccessful,
                    syncedSettingsJson,
                    playerSettingsJson,
                    playerThemeSettingsJson,
                    angularContentsJson
                );

                _dataSyncHandler.SetData(player, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, player.Entity.PlayerStats.MapsBoughtCounter);
                _dataSyncHandler.SetData(player, PlayerDataKey.Name, DataSyncMode.Player, player.Entity.Name);

                _eventsHandler.OnPlayerLogin(player);

                _loggingHandler.LogRest(LogType.Login, player, true);

                _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.DisplayName));
            });
        }

        public async void TryLogin(ITDSPlayer player, string username, string password)
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

                int id = await _databasePlayerHandler.GetPlayerIDByName(username);
                if (id != 0)
                {
                    await LoginPlayer(player, id, password);
                }
                else
                    NAPI.Task.Run(() => player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST));
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
