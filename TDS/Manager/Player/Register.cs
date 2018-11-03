using GTANetworkAPI;
using TDS.Entity;
using TDS.Manager.Utility;

namespace TDS.Manager.Player
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
                    Password = Utils.ToSHA512(password),      // TODO: Check if password is already hashed to SHA512
                    Email = email,        // TODO: Make that nullable at client
                    IsVip = false
                };
                // TODO: Check if adding them to dbplayer is enough 
                dbplayer.Playerlobbystats.Add(new Playerlobbystats());
                dbplayer.Playersettings = new Playersettings {
                    Id = dbplayer.Id,  // TODO: Check if we need that with Id = dbplayer.Id
                    AllowDataTransfer = false,   // TODO: Add AllowDataTransfer to playersettings to set at register-window (and later in settings)
                    HitsoundOn = true
                };
                dbplayer.Playerstats = new Playerstats  //TODO: Do we need Id = ... here?
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
