using GTANetworkAPI;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TDS_Shared.Data.Enums;
using TDS_Common.Manager.Utility;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Core.Manager.PlayerManager
{
    internal static class Register
    {
        public static async void RegisterPlayer(Player player, string username, string password, string? email)
        {
            while (!TDSDbContext.IsConfigured)
                await Task.Delay(1000);
            using TDSDbContext dbContext = new TDSDbContext();
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                email = null;
            if (int.TryParse(username, out int result)) 
                return;

            Players dbplayer = new Players
            {
                Name = username,
                SCName = player.SocialClubName,
                Password = Utils.HashPWServer(password),
                Email = email,
                IsVip = false,
                AdminLvl = CommonUtils.GetRandom<short>(0, 1, 2, 3)        // DEBUG
            };
            
            dbplayer.PlayerSettings = new PlayerSettings
            {
                AllowDataTransfer = false,
                Language = Language.English,
                Hitsound = true,
                Bloodscreen = true,
                FloatingDamageInfo = true,
                ShowConfettiAtRanking = true
            };
            dbplayer.PlayerStats = new PlayerStats
            {
                LoggedIn = false
            };
            dbplayer.PlayerTotalStats = new PlayerTotalStats();
            dbplayer.PlayerClothes = new PlayerClothes
            {
                IsMale = CommonUtils.GetRandom(true, false)
            };
            dbContext.Players.Add(dbplayer);
            await dbContext.SaveChangesAsync();

            await ChallengeManager.AddForeverChallenges(dbplayer);
            RestLogsManager.Log(ELogType.Register, player, true);
            CustomEventManager.SetPlayerRegistered(player);

            Login.LoginPlayer(player, dbplayer.Id, password);
        }
    }
}
