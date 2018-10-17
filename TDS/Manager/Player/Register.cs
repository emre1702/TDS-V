using GTANetworkAPI;

namespace TDS.Manager.Player
{

    static class Register
    {

        public static void RegisterPlayer(Client player, string password, string email)
        {
            using (Entities.TDSNewContext context = new Entities.TDSNewContext())
            {
                Entities.Players dbplayer = new Entities.Players
                {
                    Name = player.Name,
                    Scname = player.SocialClubName,
                    Password = Utility.ToSHA512(password),      // TODO: Check if password is already hashed to SHA512
                    Email = email,
                    AllowDataTransfer = false,           // TODO: Add AllowDataTransfer to set at register-window (and later in settings)

                };
                dbplayer.Playerlobbystats.Add(new Entities.Playerlobbystats());
                dbplayer.Playersettings = new Entities.Playersettings();
                context.Players.Add(dbplayer);
                context.SaveChanges();


            }

            /*Account.AddAccount(player.SocialClubName, uid);

            logs.Log.Register(player);

            Login.LoginPlayer(player.GetChar(), uid); */
        }
    }

}
