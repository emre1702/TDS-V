﻿using GTANetworkAPI;
using System.ComponentModel.DataAnnotations;
using TDS_Common.Enum;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Player
{
    internal static class Register
    {
        public static async void RegisterPlayer(Client player, string password, string? email)
        {
            using TDSNewContext dbContext = new TDSNewContext();
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                email = null;

            Players dbplayer = new Players
            {
                Name = player.Name,
                SCName = player.SocialClubName,
                Password = Utils.HashPWServer(password),
                Email = email,
                IsVip = false
            };
            dbplayer.PlayerSettings = new PlayerSettings
            {
                AllowDataTransfer = false,
                //Todo Add AllowDataTransfer to playersettings to set at register-window (and later in settings)
                Language = ELanguage.English,
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

            RestLogsManager.Log(ELogType.Register, player, true);
            CustomEventManager.SetPlayerRegistered(player);

            Login.LoginPlayer(player, dbplayer.Id, password);
        }
    }
}