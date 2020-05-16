using System;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Userpanel;
using TDS_Server.Data;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Core;
using TDS_Server.Data.Interfaces.ModAPI;
using System.Collections.Generic;
using TDS_Server.Data.Utility;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSettingsSpecialHandler
    {
        private readonly IModAPI _modAPI;
        private readonly ISettingsHandler _settingsHandler;
        private readonly Serializer _serializer;
        private readonly ILoggingHandler _loggingHandler;
        private readonly DataSyncHandler _dataSyncHandler;

        public UserpanelSettingsSpecialHandler(ISettingsHandler settingsHandler, Serializer serializer, ILoggingHandler loggingHandler, 
            DataSyncHandler dataSyncHandler, IModAPI modAPI) 
            => (_modAPI, _settingsHandler, _serializer, _loggingHandler, _dataSyncHandler) 
            = (modAPI, settingsHandler, serializer, loggingHandler, dataSyncHandler);

        public string? GetData(ITDSPlayer player)
        {
            if (player.Entity is null)
                return null;

            var lastUsernameChange = player.Entity.PlayerStats.LastFreeUsernameChange;
            var data = new UserpanelSettingsSpecialDataDto
            {
                Username = player.Entity.Name,
                Email = player.Entity.Email,
                UsernameBuyInCooldown = lastUsernameChange.HasValue && lastUsernameChange.Value.AddDays(_settingsHandler.ServerSettings.UsernameChangeCooldownDays) > DateTime.UtcNow

            };
            return _serializer.ToBrowser(data);
        }

        public async Task<object?> SetData(ITDSPlayer player, ArraySegment<object> args)
        {
            if (player.Entity is null)
                return "Unknown error";

            var type = (UserpanelSettingsSpecialType)Convert.ToInt32(args[0]);
            string value = Convert.ToString(args[1])!;
            string password = Convert.ToString(args[2])!;

            if (Utils.HashPasswordServer(password) != player.Entity.Password)
            {
                return player.Language.WRONG_PASSWORD;
            }

            int? paid = null;
            DateTime? lastFreeUsernameChange = player.Entity.PlayerStats.LastFreeUsernameChange;

            object? oldValue = null;
            switch (type)
            {
                case UserpanelSettingsSpecialType.Username:
                    if (!player.Entity.PlayerStats.LastFreeUsernameChange.HasValue
                        || player.Entity.PlayerStats.LastFreeUsernameChange.Value.AddDays(_settingsHandler.ServerSettings.UsernameChangeCooldownDays) < DateTime.UtcNow)
                    {
                        player.Entity.PlayerStats.LastFreeUsernameChange = DateTime.UtcNow;
                    }
                    else
                    {
                        if (player.Money < _settingsHandler.ServerSettings.UsernameChangeCost)
                        {
                            return player.Language.NOT_ENOUGH_MONEY;
                        }
                        paid = _settingsHandler.ServerSettings.UsernameChangeCost;
                        _modAPI.Thread.RunInMainThread(() => player.GiveMoney(-_settingsHandler.ServerSettings.UsernameChangeCost));
                    }
                    oldValue = player.Entity.Name;
                    player.Entity.Name = value;
                    break;
                case UserpanelSettingsSpecialType.Password:
                    oldValue = player.Entity.Password;
                    player.Entity.Password = Utils.HashPasswordServer(value);
                    break;
                case UserpanelSettingsSpecialType.Email:
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
                _loggingHandler.LogError(ex, player);
                if (paid.HasValue)
                    _modAPI.Thread.RunInMainThread(() => player.GiveMoney(paid.Value));
                if (lastFreeUsernameChange != player.Entity.PlayerStats.LastFreeUsernameChange)
                    player.Entity.PlayerStats.LastFreeUsernameChange = lastFreeUsernameChange;

                if (oldValue is { })
                {
                    switch (type)
                    {
                        case UserpanelSettingsSpecialType.Username:
                            player.ModPlayer!.Name = (string)oldValue;
                            break;
                        case UserpanelSettingsSpecialType.Password:
                            player.Entity.Password = (string)oldValue;
                            break;
                        case UserpanelSettingsSpecialType.Email:
                            player.Entity.Email = (string)oldValue;
                            break;
                    }

                }

                return "Unknown error";
            }

            switch (type)
            {
                case UserpanelSettingsSpecialType.Username:
                    player.ModPlayer!.Name = value;
                    _modAPI.Thread.RunInMainThread(() => _dataSyncHandler.SetData(player, PlayerDataKey.Name, DataSyncMode.Player, value));
                    break;
            }

            return string.Empty;
        }
    }
}
