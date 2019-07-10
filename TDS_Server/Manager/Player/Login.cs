using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Language;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Player
{
    internal static class Login
    {
        public static async void LoginPlayer(Client player, int id, string password)
        {
            while (!TDSNewContext.IsConfigured)
                await Task.Delay(1000);
            using var dbcontext = new TDSNewContext();
            Players entity = await dbcontext.Players
                .Include(p => p.PlayerStats)
                .Include(p => p.PlayerTotalStats)
                .Include(p => p.PlayerSettings)
                .Include(p => p.OfflinemessagesTarget)
                .Include(p => p.PlayerMapRatings)
                .Include(p => p.PlayerMapFavourites)
                .Include(p => p.PlayerRelationsTarget)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                NAPI.Notification.SendNotificationToPlayer(player, LangUtils.GetLang(typeof(English)).ACCOUNT_DOESNT_EXIST);
                return;
            }

            if (Utils.HashPWServer(password) != entity.Password)
            {
                NAPI.Notification.SendNotificationToPlayer(player, player.GetLang().WRONG_PASSWORD);
                return;
            }

            Workaround.SetPlayerTeam(player, 1);  // To be able to use custom damagesystem
            entity.PlayerStats.LoggedIn = true;

            SyncedPlayerSettingsDto syncedPlayerSettings = new SyncedPlayerSettingsDto
            {
                Language = entity.PlayerSettings.Language,
                Hitsound = entity.PlayerSettings.Hitsound,
                Bloodscreen = entity.PlayerSettings.Bloodscreen,
                FloatingDamageInfo = entity.PlayerSettings.FloatingDamageInfo
            };

            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.RegisterLoginSuccessful, entity.AdminLvl,
                JsonConvert.SerializeObject(SettingsManager.SyncedSettings), JsonConvert.SerializeObject(syncedPlayerSettings));

            TDSPlayer character = player.GetChar();
            character.Entity = entity;

            if (entity.AdminLvl > 0)
                AdminsManager.SetOnline(character);
            character.Gang = Gang.GetFromId(entity.GangId ?? 0);

            await dbcontext.SaveChangesAsync();

            if (character.ChatLoaded)
                OfflineMessagesManager.CheckOfflineMessages(character);

            MapsRatings.SendPlayerHisRatings(character);
            LobbyEvents.JoinLobbyEvent(player, 0, 0);

            MapFavourites.LoadPlayerFavourites(character);

            RestLogsManager.Log(ELogType.Login, player, true);

            CustomEventManager.SetPlayerLoggedIn(character);

            LangUtils.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.Name));
        }
    }
}