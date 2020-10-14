﻿using GTANetworkAPI;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Models.Userpanel;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSettingsSpecialHandler : IUserpanelSettingsSpecialHandler
    {
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ILoggingHandler _loggingHandler;

        private readonly ISettingsHandler _settingsHandler;

        public UserpanelSettingsSpecialHandler(ISettingsHandler settingsHandler, ILoggingHandler loggingHandler,
            DataSyncHandler dataSyncHandler)
            => (_settingsHandler, _loggingHandler, _dataSyncHandler)
            = (settingsHandler, loggingHandler, dataSyncHandler);

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
            return Serializer.ToBrowser(data);
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
                        player.MoneyHandler.GiveMoney(-_settingsHandler.ServerSettings.UsernameChangeCost);
                    }
                    oldValue = player.Entity.Name;
                    player.Entity.Name = value;

                    if (player.IsInGang)
                        player.Gang.Entity.Members.First(m => m.PlayerId == player.Entity.Id).Name = player.Entity.Name;
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
                await player.DatabaseHandler.SaveData();
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex, player);
                if (paid.HasValue)
                    player.MoneyHandler.GiveMoney(paid.Value);
                if (lastFreeUsernameChange != player.Entity.PlayerStats.LastFreeUsernameChange)
                    player.Entity.PlayerStats.LastFreeUsernameChange = lastFreeUsernameChange;

                if (oldValue is { })
                {
                    switch (type)
                    {
                        case UserpanelSettingsSpecialType.Username:
                            player.Name = (string)oldValue;
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
                    player.Name = value;
                    NAPI.Task.Run(() => _dataSyncHandler.SetData(player, PlayerDataKey.Name, DataSyncMode.Player, value));
                    break;
            }

            return string.Empty;
        }
    }
}
