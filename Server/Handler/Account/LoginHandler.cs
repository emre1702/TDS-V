﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Player.Settings;
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

        public LoginHandler(
            DatabasePlayerHelper databasePlayerHandler,
            LangHelper langHelper,
            EventsHandler eventsHandler,
            ISettingsHandler settingsHandler,
            DataSyncHandler dataSyncHandler,
            ILoggingHandler loggingHandler,
            ServerStartHandler serverStartHandler,
            AngularConstantsProvider angularConstantsProvider)
        {
            _databasePlayerHandler = databasePlayerHandler;
            _langHelper = langHelper;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _dataSyncHandler = dataSyncHandler;
            _loggingHandler = loggingHandler;
            _serverStartHandler = serverStartHandler;
            _angularConstantsProvider = angularConstantsProvider;

            _eventsHandler.PlayerRegistered += EventsHandler_PlayerRegistered;

            NAPI.ClientEvent.Register<ITDSPlayer, string, string>(ToServerEvent.TryLogin, this, TryLogin);
        }

        public async Task LoginPlayer(ITDSPlayer player, int id, string? password)
        {
            bool worked = await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                var rightPassword = await GetPlayerPassword(dbContext, id).ConfigureAwait(false);
                if (rightPassword is null)
                {
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST));
                    return false;
                }

                if (password is { } && !Utils.IsPasswordValid(password, rightPassword))
                {
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.WRONG_PASSWORD));
                    return false;
                }

                var entity = await LoadPlayer(dbContext, id).ConfigureAwait(false);
                if (entity is null)
                    return false;

                CreateMissingTables(entity);
                entity.PlayerStats.LoggedIn = true;
                entity.PlayerStats.LastLoginTimestamp = DateTime.UtcNow;
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                await LoadCollections(dbContext, entity).ConfigureAwait(false);

                player.DatabaseHandler.Entity = entity;

                return true;
            }).ConfigureAwait(false);

            if (!worked || player.Entity == null)
                return;

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
        }

        public async void TryLogin(ITDSPlayer player, string username, string password)
        {
            if (player.TryingToLoginRegister)
                return;
            player.TryingToLoginRegister = true;
            try
            {
                await _serverStartHandler.LoadingTask.Task.ConfigureAwait(false);

                int id = await _databasePlayerHandler.GetPlayerIDByName(username).ConfigureAwait(false);
                if (id != 0)
                {
                    await LoginPlayer(player, id, password).ConfigureAwait(false);
                }
                else
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
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

        private void CreateMissingTables(Players entity)
        {
            if (entity.KillInfoSettings is null)
                entity.KillInfoSettings = new PlayerKillInfoSettings { ShowIcon = true };
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
                    .Include(p => p.PlayerSettings.Scoreboard)
                    .Include(p => p.PlayerSettings.Voice)
                    .Include(p => p.PlayerClothes)
                    .Include(p => p.ThemeSettings)
                    .Include(p => p.KillInfoSettings)

                   .FirstOrDefaultAsync(p => p.Id == playerId);

        private async Task LoadCollections(TDSDbContext dbContext, Players entity)
        {
            await dbContext.Entry(entity).Reference(e => e.CharDatas).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.CharDatas).Collection(e => e.AppearanceData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.CharDatas).Collection(e => e.FeaturesData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.CharDatas).Collection(e => e.GeneralData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.CharDatas).Collection(e => e.HairAndColorsData).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity.CharDatas).Collection(e => e.HeritageData).LoadAsync().ConfigureAwait(false);

            await dbContext.Entry(entity).Collection(e => e.OfflinemessagesTarget).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.PlayerMapRatings).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.PlayerMapFavourites).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.PlayerRelationsTarget).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.Challenges).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.WeaponStats).LoadAsync().ConfigureAwait(false);
            await dbContext.Entry(entity).Collection(e => e.WeaponBodypartStats).LoadAsync().ConfigureAwait(false);
        }
    }
}
