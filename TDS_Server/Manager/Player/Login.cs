﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Enums;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Language;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Player
{
    internal static class Login
    {
        public static async void LoginPlayer(Client player, int id, string password)
        {
            while (!TDSDbContext.IsConfigured)
                await Task.Delay(1000);
            TDSPlayer character = player.GetChar();

            character.InitDbContext();
            bool worked = await character.ExecuteForDBAsync(async (dbContext) =>
            {
                character.Entity = await dbContext.Players
                   .Include(p => p.PlayerStats)
                   .Include(p => p.PlayerTotalStats)
                   .Include(p => p.PlayerSettings)
                   .Include(p => p.OfflinemessagesTarget)
                   .Include(p => p.PlayerMapRatings)
                   .Include(p => p.PlayerMapFavourites)
                   .Include(p => p.PlayerRelationsTarget)
                   .Include(p => p.PlayerClothes)
                   .FirstOrDefaultAsync(p => p.Id == id);

                if (character.Entity is null)
                {
                    NAPI.Notification.SendNotificationToPlayer(player, LangUtils.GetLang(typeof(English)).ACCOUNT_DOESNT_EXIST);
                    dbContext.Dispose();
                    return false;
                }

                if (Utils.HashPWServer(password) != character.Entity.Password)
                {
                    NAPI.Notification.SendNotificationToPlayer(player, player.GetLang().WRONG_PASSWORD);
                    dbContext.Dispose();
                    return false;
                }

                player.Name = character.Entity.Name;
                //Workaround.SetPlayerTeam(player, 1);  // To be able to use custom damagesystem
                character.Entity.PlayerStats.LoggedIn = true;
                await dbContext.SaveChangesAsync();
                return true;
            });
            
            if (!worked || character.Entity == null)
                return;

            if (character.Entity.PlayerClothes is null)
                character.Entity.PlayerClothes = new TDS_Server_DB.Entity.Player.PlayerClothes { IsMale = CommonUtils.GetRandom(true, false) };

            var angularConstantsData = AngularConstantsDataDto.Get(character);

            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.RegisterLoginSuccessful, 
                Serializer.ToClient(SettingsManager.SyncedSettings), Serializer.ToClient(character.Entity.PlayerSettings), Serializer.ToBrowser(angularConstantsData));

            PlayerDataSync.SetData(character, EPlayerDataKey.MapsBoughtCounter, EPlayerDataSyncMode.Player, character.Entity.PlayerStats.MapsBoughtCounter);

            character.Gang = Gang.GetPlayerGang(character);
            character.GangRank = Gang.GetPlayerGangRank(character);

            if (character.ChatLoaded)
                OfflineMessagesManager.CheckOfflineMessages(character);

            MapsRatings.SendPlayerHisRatings(character);
            EventsHandler.JoinLobbyEvent(player, LobbyManager.MainMenu.Id);

            MapFavourites.LoadPlayerFavourites(character);

            RestLogsManager.Log(ELogType.Login, player, true);

            CustomEventManager.SetPlayerLoggedIn(character);

            LangUtils.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, character.DisplayName));
        }
    }
}