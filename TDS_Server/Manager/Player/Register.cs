﻿using GTANetworkAPI;
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
            using (TDSNewContext dbContext = new TDSNewContext())
            {
                Players dbplayer = new Players
                {
                    Name = player.Name,
                    Scname = player.SocialClubName,
                    Password = Utils.HashPWServer(password),
                    //Todo Make that nullable at client
                    Email = email,
                    IsVip = false
                };
                dbplayer.PlayerSettings = new PlayerSettings {
                    AllowDataTransfer = false,
                    //Todo Add AllowDataTransfer to playersettings to set at register-window (and later in settings)
                    Language = (byte)ELanguage.English,
                    Hitsound = true,
                    Bloodscreen = true,
                    FloatingDamageInfo = true
                    
                };
                dbplayer.PlayerStats = new PlayerStats
                {
                    LoggedIn = false
                };
                dbContext.Players.Add(dbplayer);
                await dbContext.SaveChangesAsync();

                RestLogsManager.Log(Enum.ELogType.Register, player, true);

                Login.LoginPlayer(player, dbplayer.Id, password);
            }
        }
    }

}
