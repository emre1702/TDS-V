using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Dto.Challlenge;
using TDS_Server.Enums;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Language;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.PlayerManager
{
    internal static class Login
    {
        public static async void LoginPlayer(Player client, int id, string password)
        {
            while (!TDSDbContext.IsConfigured)
                await Task.Delay(1000);
            TDSPlayer player = client.GetChar(true);

            player.InitDbContext();
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
                    player.SendNotification(LangUtils.GetLang(typeof(English)).ACCOUNT_DOESNT_EXIST);
                    dbContext.Dispose();
                    return false;
                }

                if (Utils.HashPWServer(password) != player.Entity.Password)
                {
                    player.SendNotification(player.Language.WRONG_PASSWORD);
                    dbContext.Dispose();
                    return false;
                }

                client.Name = player.Entity.Name;
                //Workaround.SetPlayerTeam(player, 1);  // To be able to use custom damagesystem
                player.Entity.PlayerStats.LoggedIn = true;
                await dbContext.SaveChangesAsync();
                return true;
            });
            
            if (!worked || player.Entity == null)
                return;

            if (player.Entity.PlayerClothes is null)
                player.Entity.PlayerClothes = new TDS_Server_DB.Entity.Player.PlayerClothes { IsMale = CommonUtils.GetRandom(true, false) };

            /*#region Add weekly challenges and reload
            if (player.Entity.Challenges.Count == 0)
            {
                await ChallengeManager.AddWeeklyChallenges(player.Entity);
                await player.ExecuteForDBAsync(dbContext => 
                {
                    player.Entity.Challenges = null;
                    dbContext.Entry(player.Entity).Reference(p => p.Challenges).IsLoaded = false;
                    return dbContext.Entry(player.Entity).Reference(p => p.Challenges).LoadAsync();
                });
            }
            player.InitChallengesDict();
            #endregion*/


            var angularConstantsData = AngularConstantsDataDto.Get(player);

            NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.RegisterLoginSuccessful, 
                Serializer.ToClient(SettingsManager.SyncedSettings), 
                Serializer.ToClient(player.Entity.PlayerSettings), 
                Serializer.ToBrowser(angularConstantsData),
                GetChallengesJson(player));

            PlayerDataSync.SetData(player, EPlayerDataKey.MapsBoughtCounter, EPlayerDataSyncMode.Player, player.Entity.PlayerStats.MapsBoughtCounter);

            player.Gang = Gang.GetPlayerGang(player);
            player.GangRank = Gang.GetPlayerGangRank(player);

            if (player.ChatLoaded)
                OfflineMessagesManager.CheckOfflineMessages(player);

            MapsRatings.SendPlayerHisRatings(player);
            EventsHandler.JoinLobbyEvent(client, LobbyManager.MainMenu.Id);

            MapFavourites.LoadPlayerFavourites(player);

            RestLogsManager.Log(ELogType.Login, client, true);

            CustomEventManager.SetPlayerLoggedIn(player);

            LangUtils.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.DisplayName));
        }

        private static string GetChallengesJson(TDSPlayer player)
        {
            var result = player.Entity!.Challenges
                .GroupBy(c => c.Frequency)
                .Select(g => new ChallengeGroupDto 
                {
                    Frequency = g.Key,
                    Challenges = g.Select(c => new ChallengeDto
                    {
                        Type = c.Challenge,
                        Amount = c.Amount,
                        CurrentAmount = c.CurrentAmount
                    })
                });

            return Serializer.ToBrowser(result);
        }
    }
}
