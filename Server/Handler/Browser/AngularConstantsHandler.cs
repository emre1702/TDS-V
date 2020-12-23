using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;

namespace TDS.Server.Handler.Browser
{
    public class AngularConstantsProvider
    {
        private readonly ISettingsHandler _settingsHandler;
        private readonly ILoggingHandler _loggingHandler;

        public AngularConstantsProvider(ISettingsHandler settingsHandler, ILoggingHandler loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _loggingHandler = loggingHandler;
        }

        public AngularConstantsDataDto Get(ITDSPlayer player)
        {
            try
            {
                return new AngularConstantsDataDto
                {
                    TDSId = player.Id,
                    RemoteId = player.RemoteId,
                    UsernameChangeCost = _settingsHandler.ServerSettings.UsernameChangeCost,
                    UsernameChangeCooldownDays = _settingsHandler.ServerSettings.UsernameChangeCooldownDays,
                    MapBuyBasePrice = _settingsHandler.ServerSettings.MapBuyBasePrice,
                    MapBuyCounterMultiplicator = _settingsHandler.ServerSettings.MapBuyCounterMultiplicator,
                    Username = player.Entity?.Name ?? "?",
                    SCName = player.Entity?.SCName ?? "?"
                };
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
                return new AngularConstantsDataDto();
            }
        }
    }
}
