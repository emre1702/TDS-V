using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Models;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Account
{
    class LoginHandler
    {
        private readonly DatabasePlayerHelper _databasePlayerHandler;
        private readonly ChallengesHelper _challengesHelper;
        private readonly LangHelper _langHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly Serializer _serializer;
        private readonly SettingsHandler _settingsHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly LoggingHandler _loggingHandler;

        public LoginHandler(
            DatabasePlayerHelper databasePlayerHandler,
            ChallengesHelper challengesHelper,
            LangHelper langHelper,
            EventsHandler eventsHandler,
            Serializer serializer,
            SettingsHandler settingsHandler,
            IServiceProvider serviceProvider,
            DataSyncHandler dataSyncHandler,
            LoggingHandler loggingHandler)
            => (_databasePlayerHandler, _challengesHelper, _langHelper, _eventsHandler, _serializer, _settingsHandler, _serviceProvider, _dataSyncHandler, _loggingHandler)
            = (databasePlayerHandler, challengesHelper, langHelper, eventsHandler, serializer, settingsHandler, serviceProvider, dataSyncHandler, loggingHandler);


        //[RemoteEvent(DToServerEvent.TryLogin)]
        public async void TryLogin(TDSPlayer player, string username, string password)
        {
            int id = await _databasePlayerHandler.GetPlayerIDByName(username);
            if (id != 0)
            {
                await LoginPlayer(player, id, password);
            }
            else
                player.SendNotification(player.Language.ACCOUNT_DOESNT_EXIST);
        }

        public async Task LoginPlayer(TDSPlayer player, int id, string password)
        {
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

                if (Utils.HashPWServer(password) != player.Entity.Password)
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

            player.SendEvent(ToClientEvent.RegisterLoginSuccessful,
                _serializer.ToClient(_settingsHandler.SyncedSettings),
                _serializer.ToClient(player.Entity.PlayerSettings),
                _serializer.ToBrowser(angularConstantsData)
                );

            _dataSyncHandler.SetData(player, PlayerDataKey.MapsBoughtCounter, PlayerDataSyncMode.Player, player.Entity.PlayerStats.MapsBoughtCounter);
            _dataSyncHandler.SetData(player, PlayerDataKey.Name, PlayerDataSyncMode.Player, player.Entity.Name);

            _eventsHandler.OnPlayerLogin(player);

            _loggingHandler.LogRest(LogType.Login, player, true);


            MapsRatings.SendPlayerHisRatings(player);

            MapFavourites.LoadPlayerFavourites(player);



            _langHelper.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.DisplayName));
        }
    }
}
