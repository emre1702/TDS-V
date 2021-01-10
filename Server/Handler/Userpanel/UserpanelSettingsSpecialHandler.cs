using GTANetworkAPI;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Userpanel;
using TDS.Server.Data.Utility;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Userpanel;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelSettingsSpecialHandler : IUserpanelSettingsSpecialHandler
    {
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ILoggingHandler _loggingHandler;

        private readonly ISettingsHandler _settingsHandler;

        public UserpanelSettingsSpecialHandler(ISettingsHandler settingsHandler, ILoggingHandler loggingHandler,
            DataSyncHandler dataSyncHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            (_settingsHandler, _loggingHandler, _dataSyncHandler) = (settingsHandler, loggingHandler, dataSyncHandler);

            remoteBrowserEventsHandler.Add(ToServerEvent.SaveSpecialSettingsChange, SetData);
        }

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

        private async Task<object?> SetData(RemoteBrowserEventArgs args)
        {
            var player = args.Player;
            if (player.Entity is null)
                return "Unknown error";

            var type = (UserpanelSettingsSpecialType)Convert.ToInt32(args.Args[0]);
            string value = Convert.ToString(args.Args[1])!;
            string password = Convert.ToString(args.Args[2])!;

            if (!Utils.IsPasswordValid(password, player.Entity.Password))
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
                await player.DatabaseHandler.SaveData().ConfigureAwait(false);
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
                    NAPI.Task.RunSafe(() => _dataSyncHandler.SetData(player, PlayerDataKey.Name, DataSyncMode.Player, value));
                    break;
            }

            return string.Empty;
        }
    }
}