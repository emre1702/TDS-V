using GTANetworkAPI;
using TDS_Common.Enum;
using TDS_Server.Entity;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Player
{

    static class Register
    {

        public static async void RegisterPlayer(Client player, string password, string email)
        {
            using (TDSNewContext context = new TDSNewContext())
            {
                Players dbplayer = new Players
                {
                    Name = player.Name,
                    Scname = player.SocialClubName,
                    Password = Utils.HashPWServer(password),
#warning TODO: Make that nullable at client
                    Email = email,

                    IsVip = false
                };
                dbplayer.PlayerSettings = new PlayerSettings {
                    AllowDataTransfer = false,
#warning TODO: Add AllowDataTransfer to playersettings to set at register-window (and later in settings)
                    HitsoundOn = true,
                    Language = (byte)ELanguage.English
                };
                dbplayer.PlayerStats = new PlayerStats
                {
                    LoggedIn = false
                };
                context.Players.Add(dbplayer);
                await context.SaveChangesAsync();

                RestLogsManager.Log(Enum.ELogType.Register, player, true);

                Login.LoginPlayer(player, dbplayer.Id, password);
            }
        }
    }

}
