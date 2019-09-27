using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
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
            while (!TDSNewContext.IsConfigured)
                await Task.Delay(1000);
            TDSPlayer character = player.GetChar();
            character.InitDbContext();

            character.Entity = await character.DbContext.Players
                .Include(p => p.PlayerStats)
                .Include(p => p.PlayerTotalStats)
                .Include(p => p.PlayerSettings)
                .Include(p => p.OfflinemessagesTarget)
                .Include(p => p.PlayerMapRatings)
                .Include(p => p.PlayerMapFavourites)
                .Include(p => p.PlayerRelationsTarget)
                .Include(p => p.PlayerWeaponComponents)
                .Include(p => p.PlayerWeaponTints)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (character.Entity is null)
            {
                NAPI.Notification.SendNotificationToPlayer(player, LangUtils.GetLang(typeof(English)).ACCOUNT_DOESNT_EXIST);
                character.DbContext.Dispose();
                return;
            }

            if (Utils.HashPWServer(password) != character.Entity.Password)
            {
                NAPI.Notification.SendNotificationToPlayer(player, player.GetLang().WRONG_PASSWORD);
                character.DbContext.Dispose();
                return;
            }

            player.Name = character.Entity.Name;
            //Workaround.SetPlayerTeam(player, 1);  // To be able to use custom damagesystem
            character.Entity.PlayerStats.LoggedIn = true;
            await character.DbContext.SaveChangesAsync();

            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.RegisterLoginSuccessful, character.Entity.AdminLvl,
                JsonConvert.SerializeObject(SettingsManager.SyncedSettings), JsonConvert.SerializeObject(character.Entity.PlayerSettings));

            if (character.Entity.AdminLvl > 0)
                AdminsManager.SetOnline(character);
            character.Gang = Gang.GetFromId(character.Entity.GangId ?? 0);

            if (character.ChatLoaded)
                OfflineMessagesManager.CheckOfflineMessages(character);

            MapsRatings.SendPlayerHisRatings(character);
            EventsHandler.JoinLobbyEvent(player, 0);

            MapFavourites.LoadPlayerFavourites(character);

            RestLogsManager.Log(ELogType.Login, player, true);

            CustomEventManager.SetPlayerLoggedIn(character);

            LangUtils.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_IN, player.Name));
        }
    }
}