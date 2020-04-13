using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.Player;
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
        private readonly DatabasePlayerHelper _databasePlayerHandler;
        private readonly LangHelper _langHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ServerStartHandler _serverStartHandler;

        public LoginHandler(
            DatabasePlayerHelper databasePlayerHandler,
            LangHelper langHelper,
            EventsHandler eventsHandler,
            Serializer serializer,
            ISettingsHandler settingsHandler,
            IServiceProvider serviceProvider,
            DataSyncHandler dataSyncHandler,
            ILoggingHandler loggingHandler,
            ServerStartHandler serverStartHandler)
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

            _eventsHandler.PlayerRegistered += EventsHandler_PlayerRegistered;
        }

        public async void TryLogin(ITDSPlayer player, string username, string password)
        {
            if (!_serverStartHandler.IsReadyForLogin)
            {
                player.SendNotification(player.Language.TRY_AGAIN_LATER);
                return;
            }

            int id = await _databasePlayerHandler.GetPlayerIDByName(username);
            if (id != 0)
            {
                await LoginPlayer(player, id, password);
            }
            else
                player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST);
        }

        public async Task LoginPlayer(ITDSPlayer iplayer, int id, string? password)
        {
            if (!(iplayer is TDSPlayer player))
                return;
            if (player.ModPlayer is null)
                return;


            bool worked = await player.ExecuteForDBAsync(async (dbContext) =>
            {
                player.Entity = await dbContext.Players
                   .Include(p => p.PlayerStats)
                   .Include(p => p.PlayerTotalStats)
                   .Include(p => p.PlayerSettings)
                   .Include(p => p.OfflinemessagesTarget)
                   .Include(p => p.PlayerMapRatings)
                   .Include(p => p.PlayerMapFavourites)
                   .Include(p => p.PlayerRelationsTarget)
                   .Include(p => p.PlayerClothes)
                   .Include(p => p.Challenges)
                   .FirstOrDefaultAsync(p => p.Id == id);

                if (player.Entity is null)
                {
                    player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST);
                    dbContext.Dispose();
                    return false;
                }

                if (password is { } && Utils.HashPWServer(password) != player.Entity.Password)
                {
                    player.SendNotification(player.Language.WRONG_PASSWORD);
                    dbContext.Dispose();
                    return false;
                }

                player.ModPlayer.Name = player.Entity.Name;
                //Workaround.SetPlayerTeam(player, 1);  // To be able to use custom damagesystem
                player.Entity.PlayerStats.LoggedIn = true;
                player.Entity.PlayerStats.LastLoginTimestamp = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
                return true;
            });

            if (!worked || player.Entity == null)
                return;

            var angularConstantsData = ActivatorUtilities.CreateInstance<AngularConstantsDataDto>(_serviceProvider, player);

            player.SendEvent(ToClientEvent.LoginSuccessful,
                _serializer.ToClient(_settingsHandler.SyncedSettings),
                _serializer.ToClient(player.Entity.PlayerSettings),
                _serializer.ToBrowser(angularConstantsData)
                );

            _dataSyncHandler.SetData(player, PlayerDataKey.MapsBoughtCounter, PlayerDataSyncMode.Player, player.Entity.PlayerStats.MapsBoughtCounter);
            _dataSyncHandler.SetData(player, PlayerDataKey.Name, PlayerDataSyncMode.Player, player.Entity.Name);

            _eventsHandler.OnPlayerLogin(player);

            _loggingHandler.LogRest(LogType.Login, player, true);

            _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.DisplayName));
        }

        private async void EventsHandler_PlayerRegistered(ITDSPlayer player, Players dbPlayer)
        {
            await LoginPlayer(player, dbPlayer.Id, null);
        }
    }
}
