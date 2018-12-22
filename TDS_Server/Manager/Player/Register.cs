using GTANetworkAPI;
using TDS_Server.Entity;
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
#warning TODO: Check if password is already hashed to SHA512
                    Password = Utils.ToSHA512(password),
#warning TODO: Make that nullable at client
                    Email = email,

                    IsVip = false
                };
#warning TODO: Check if adding them to dbplayer is enough 
                dbplayer.Playerlobbystats.Add(new Playerlobbystats());
                dbplayer.Playersettings = new Playersettings {
                    Id = dbplayer.Id,
#warning TODO: Check if we need that with Id = dbplayer.Id
                    AllowDataTransfer = false,
#warning TODO: Add AllowDataTransfer to playersettings to set at register-window (and later in settings)
                    HitsoundOn = true
                };
                dbplayer.Playerstats = new Playerstats
#warning TODO: Do we need Id = ... here?
                {
                    LoggedIn = false
                };
                context.Players.Add(dbplayer);
                await context.SaveChangesAsync();

                Logs.Rest.Log(Enum.ELogType.Register, player, true);

                Login.LoginPlayer(player, dbplayer.Id, password);
            }
        }
    }

}
