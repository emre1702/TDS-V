namespace TDS_Server.Manager.Player
{
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS_Server.Default;
    using TDS_Server.Entity;
    using TDS_Server.Instance.Language;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Utility;
    using TDS_Common.Default;
    using TDS_Server.Instance.Lobby;
    using TDS_Server.Manager.Logs;
    using TDS_Server.Manager.Maps;
    using Newtonsoft.Json;
    using TDS_Server.Enum;

    static class Login
    {

        public static async void LoginPlayer(Client player, uint id, string password)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Players entity = await dbcontext.Players
                                        .Include(p => p.PlayerStats)
                                        .Include(p => p.PlayerSettings)
                                        .Include(p => p.OfflinemessagesTarget)
                                        .Include(p => p.PlayerMapRatings)
                                        .Include(p => p.PlayerMapFavourites)
                                        .AsNoTracking()
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

                player.Team = 1;        // To be able to use custom damagesystem
                entity.PlayerStats.LoggedIn = true;

                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.RegisterLoginSuccessful, entity.AdminLvl, JsonConvert.SerializeObject(SettingsManager.SyncedSettings));

                TDSPlayer character = player.GetChar();
                character.Entity = entity;

                if (entity.AdminLvl > 0)
                    AdminsManager.SetOnline(character);

                dbcontext.PlayerStats.Attach(entity.PlayerStats);
                dbcontext.Entry(entity.PlayerStats).Property(x => x.LoggedIn).IsModified = true;
                await dbcontext.SaveChangesAsync();

                if (character.ChatLoaded)
                    OfflineMessagesManager.CheckOfflineMessages(character);

                MapsManager.SendPlayerHisRatings(character);
                LobbyEvents.JoinLobbyEvent(player, 0, 0);

                MapFavourites.LoadPlayerFavourites(character);

                //Gang.CheckPlayerGang(character);

                RestLogsManager.Log(ELogType.Login, player, true);    
            }
        }
    }

}
