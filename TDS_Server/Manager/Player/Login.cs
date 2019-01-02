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

    static class Login
    {

        public static async void LoginPlayer(Client player, uint id, string password)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Players entity = await dbcontext.Players
                                        .Include(p => p.Playerstats)
                                        .Include(p => p.Playersettings)
                                        .Include(p => p.OfflinemessagesTarget)
                                        .Include(p => p.Playermapratings)
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

                if (entity.AdminLvl > 0)
                    AdminsManager.SetOnline(player);

                player.Team = 1;        // To be able to use custom damagesystem
                entity.Playerstats.LoggedIn = true;

                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.RegisterLoginSuccessful, entity.AdminLvl, JsonConvert.SerializeObject(SettingsManager.SyncedSettings));
                LobbyEvents.JoinLobbyEvent(player, 0, 0);

                await dbcontext.SaveChangesAsync();

                TDSPlayer character = player.GetChar();
                MapsManager.SendPlayerHisRatings(character);

                character.Entity = entity;

                //Gang.CheckPlayerGang(character);

                RestLogsManager.Log(Enum.ELogType.Login, player, true);    
            }
        }
    }

}
