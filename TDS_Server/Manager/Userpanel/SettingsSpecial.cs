using System;
using System.Threading.Tasks;
using TDS_Common.Enum.Userpanel;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Userpanel;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Userpanel
{
    static class SettingsSpecial
    {
        public static string? GetData(TDSPlayer player)
        {
            if (player.Entity is null)
                return null;

            var data = new UserpanelSettingsSpecialDataDto
            {
                Username = player.Entity.Name,
                Email = player.Entity.Email
            };
            return Serializer.ToBrowser(data);
        }

        public static async Task<object?> SetData(TDSPlayer player, params object[] args)
        {
            if (player.Entity is null)
                return "Unknown error";

            var type = (EUserpanelSettingsSpecialType)Convert.ToInt32(args[0]);
            string value = Convert.ToString(args[1])!;
            string password = Convert.ToString(args[2])!;

            if (Utils.HashPWServer(password) != player.Entity.Password)
            {
                return player.Language.WRONG_PASSWORD;
            }

            int? paid = null;
            DateTime? lastFreeUsernameChange = player.Entity.PlayerStats.LastFreeUsernameChange;
            switch (type)
            {
                case EUserpanelSettingsSpecialType.Username:
                    if (!player.Entity.PlayerStats.LastFreeUsernameChange.HasValue 
                        || player.Entity.PlayerStats.LastFreeUsernameChange.Value.AddDays(SettingsManager.ServerSettings.UsernameChangeCooldownDays) < DateTime.UtcNow)
                    {
                        player.Entity.PlayerStats.LastFreeUsernameChange = DateTime.UtcNow;
                    }
                    else
                    {
                        if (player.Money < SettingsManager.ServerSettings.UsernameChangeCost)
                        {
                            return player.Language.NOT_ENOUGH_MONEY;
                        }
                        paid = SettingsManager.ServerSettings.UsernameChangeCost;
                        player.GiveMoney(-SettingsManager.ServerSettings.UsernameChangeCost);
                    }
                    player.Entity.Name = value;
                    break;
                case EUserpanelSettingsSpecialType.Password:
                    player.Entity.Password = value;
                    break;
                case EUserpanelSettingsSpecialType.Email:
                    player.Entity.Email = value;
                    break;
                default:
                    return "Unknown error";
            }

            try
            {
                await player.SaveData();
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
                if (paid.HasValue)
                    player.GiveMoney(paid.Value);
                if (lastFreeUsernameChange != player.Entity.PlayerStats.LastFreeUsernameChange)
                    player.Entity.PlayerStats.LastFreeUsernameChange = lastFreeUsernameChange;
                return "Unknown error";
            }
            
            return "";
        }
    }
}
