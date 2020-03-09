using System;
using System.Threading.Tasks;
using TDS_Common.Enum;
using TDS_Common.Enum.Userpanel;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Userpanel;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Core.Manager.Userpanel
{
    static class SettingsSpecial
    {
        public static string? GetData(TDSPlayer player)
        {
            if (player.Entity is null)
                return null;

            var lastUsernameChange = player.Entity.PlayerStats.LastFreeUsernameChange;
            var data = new UserpanelSettingsSpecialDataDto
            {
                Username = player.Entity.Name,
                Email = player.Entity.Email,
                UsernameBuyInCooldown = lastUsernameChange.HasValue && lastUsernameChange.Value.AddDays(SettingsManager.ServerSettings.UsernameChangeCooldownDays) > DateTime.UtcNow

            };
            return Serializer.ToBrowser(data);
        }

        public static async Task<object?> SetData(TDSPlayer player, object[] args)
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
            
            object? oldValue = null;
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
                    oldValue = player.Entity.Name;
                    player.Entity.Name = value;
                    break;
                case EUserpanelSettingsSpecialType.Password:
                    oldValue = player.Entity.Password;
                    player.Entity.Password = Utils.HashPWServer(value);
                    break;
                case EUserpanelSettingsSpecialType.Email:
                    oldValue = player.Entity.Email;
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

                if (oldValue is { })
                {
                    switch (type)
                    {
                        case EUserpanelSettingsSpecialType.Username:
                            player.Player!.Name = (string)oldValue;
                            break;
                        case EUserpanelSettingsSpecialType.Password:
                            player.Entity.Password = (string)oldValue;
                            break;
                        case EUserpanelSettingsSpecialType.Email:
                            player.Entity.Email = (string)oldValue;
                            break;
                    }

                }
                
                return "Unknown error";
            }

            switch (type)
            {
                case EUserpanelSettingsSpecialType.Username:
                    player.Player!.Name = value;
                    PlayerDataSync.SetData(player, EPlayerDataKey.Name, EPlayerDataSyncMode.Player, value);
                    break;
            }

            return string.Empty;
        }
    }
}
