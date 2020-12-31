using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Player.Settings;
using TDS.Server.Handler.Appearance;
using TDS.Server.Handler.Browser;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.Server;
using TDS.Server.Handler.Sync;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Account
{
    public class LoginHandler
    {
        private readonly DatabasePlayerHelper _databasePlayerHandler;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly LangHelper _langHelper;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ServerStartHandler _serverStartHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly AngularConstantsProvider _angularConstantsProvider;
        private readonly PlayerBodyHandler _playerCharHandler;

        public LoginHandler(
            DatabasePlayerHelper databasePlayerHandler,
            LangHelper langHelper,
            EventsHandler eventsHandler,
            ISettingsHandler settingsHandler,
            DataSyncHandler dataSyncHandler,
            ILoggingHandler loggingHandler,
            ServerStartHandler serverStartHandler,
            AngularConstantsProvider angularConstantsProvider,
            PlayerBodyHandler playerCharHandler)
        {
            _databasePlayerHandler = databasePlayerHandler;
            _langHelper = langHelper;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _dataSyncHandler = dataSyncHandler;
            _loggingHandler = loggingHandler;
            _serverStartHandler = serverStartHandler;
            _angularConstantsProvider = angularConstantsProvider;
            _playerCharHandler = playerCharHandler;

            _eventsHandler.PlayerRegistered += EventsHandler_PlayerRegistered;

            NAPI.ClientEvent.Register<ITDSPlayer, string, string>(ToServerEvent.TryLogin, this, TryLogin);
        }

        public async Task<string> LoginPlayer(ITDSPlayer player, int id, string? password)
        {
            string? errMsgKey = await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                var rightPassword = await GetPlayerPassword(dbContext, id).ConfigureAwait(false);
                if (rightPassword is null)
                    return "AccountDoesntExist";

                if (password is { } && !Utils.IsPasswordValid(password, rightPassword))
                    return "WrongPassword";

                var entity = await LoadPlayer(dbContext, id).ConfigureAwait(false);
                if (entity is null)
                    return "AccountDoesntExist";

                await CreateMissingTables(player, entity);
                entity.PlayerStats.LoggedIn = true;
                entity.PlayerStats.LastLoginTimestamp = DateTime.UtcNow;
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                await LoadCollections(dbContext, entity).ConfigureAwait(false);

                player.DatabaseHandler.Entity = entity;

                return null;
            }).ConfigureAwait(false);

            if (errMsgKey is { } || player.Entity == null)
            {
                if (errMsgKey is { })
                    return errMsgKey;
                LoggingHandler.Instance.LogError("errMsgKey is null but player entity is also null?!", Environment.StackTrace, source: player);
                return "ErrorInfo";
            }

            player.Database.SetPlayerSource(player);
            var angularConstantsData = _angularConstantsProvider.Get(player);

            var syncedSettingsJson = Serializer.ToClient(_settingsHandler.SyncedSettings);
            var angularContentsJson = Serializer.ToBrowser(angularConstantsData, true);

            await NAPI.Task.RunWait(player.Init);
            player.Name = player.Entity.Name;

            NAPI.Task.RunSafe(() =>
            {
                player.TriggerEvent(ToClientEvent.LoginSuccessful,
                    syncedSettingsJson,
                    angularContentsJson
                );

                _dataSyncHandler.SetData(player, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, player.Entity.PlayerStats.MapsBoughtCounter);
                _dataSyncHandler.SetData(player, PlayerDataKey.Name, DataSyncMode.Player, player.Entity.Name);
            });

            _eventsHandler.OnPlayerLogin(player);
            _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.DisplayName));
            _loggingHandler.LogRest(LogType.Login, player, true);
            return "";
        }

        public async void TryLogin(ITDSPlayer player, string username, string password)
        {
            var errMsgKey = await TryLoginWithReturn(player, username, password);
            NAPI.Task.RunSafe(() => player.TriggerBrowserEvent(ToBrowserEvent.TryLogin, errMsgKey ?? ""));
        }

        public async Task<string?> TryLoginWithReturn(ITDSPlayer player, string username, string password)
        {
            if (player.TryingToLoginRegister)
                return "Cooldown";
            player.TryingToLoginRegister = true;
            try
            {
                await _serverStartHandler.LoadingTask.Task.ConfigureAwait(false);

                int id = await _databasePlayerHandler.GetPlayerIDByName(username).ConfigureAwait(false);
                if (id != 0)
                {
                    var errMsgKey = await LoginPlayer(player, id, password).ConfigureAwait(false);
                    return errMsgKey;
                }
                else
                    return "AccountDoesntExist";
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
                return "ErrorInfo";
            }
            finally
            {
                player.TryingToLoginRegister = false;
            }
        }

        private async void EventsHandler_PlayerRegistered(ITDSPlayer player, Players dbPlayer)
        {
            await LoginPlayer(player, dbPlayer.Id, null).ConfigureAwait(false);
        }

        private Task<string?> GetPlayerPassword(TDSDbContext dbContext, int playerId)
            => dbContext.Players.Where(p => p.Id == playerId).Select(p => p.Password).FirstOrDefaultAsync() as Task<string?>;

        private async ValueTask CreateMissingTables(ITDSPlayer player, Players entity)
        {
            if (entity.KillInfoSettings is null)
                entity.KillInfoSettings = new PlayerKillInfoSettings { ShowIcon = true };
            if (entity.BodyDatas is null)
                await _playerCharHandler.InitPlayerChar((player, entity));
        }

        private Task<Players?> LoadPlayer(TDSDbContext dbContext, int playerId)
            => dbContext.Players
                    .Include(p => p.PlayerStats)
                    .Include(p => p.PlayerTotalStats)
                    .Include(p => p.PlayerSettings)
                    .Include(p => p.PlayerSettings.Chat)
                    .Include(p => p.PlayerSettings.CooldownsAndDurations)
                    .Include(p => p.PlayerSettings.FightEffect)
                    .Include(p => p.PlayerSettings.General)
                    .Include(p => p.PlayerSettings.Info)
                    .Include(p => p.PlayerSettings.IngameColors)
                    .Include(p => p.PlayerSettings.Hud)
                    .Include(p => p.PlayerSettings.Scoreboard)
                    .Include(p => p.PlayerSettings.Voice)
                    .Include(p => p.PlayerClothes)
                    .Include(p => p.ThemeSettings)
                    .Include(p => p.KillInfoSettings)
                    .Include(p => p.BodyDatas)
                    .Include(p => p.PlayerLobbyStats)

                   .FirstOrDefaultAsync(p => p.Id == playerId);

        private async Task LoadCollections(TDSDbContext dbContext, Players entity)
        {
            await dbContext.Entry(entity.BodyDatas).Collection(e => e.AppearanceData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.BodyDatas).Collection(e => e.FeaturesData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.BodyDatas).Collection(e => e.GeneralData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.BodyDatas).Collection(e => e.HairAndColorsData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.BodyDatas).Collection(e => e.HeritageData).LoadAsync().ConfigureAwait(false);

            await dbContext.Entry(entity).Collection(e => e.OfflinemessagesTarget).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.PlayerMapRatings).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.PlayerMapFavourites).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.PlayerRelationsTarget).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.PlayerRelationsPlayer).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.Challenges).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.WeaponStats).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.WeaponBodypartStats).LoadAsync().ConfigureAwait(false);
        }
    }
}