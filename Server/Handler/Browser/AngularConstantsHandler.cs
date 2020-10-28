using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;

namespace TDS_Server.Handler.Browser
{
    public class AngularConstantsProvider
    {
        private readonly ISettingsHandler _settingsHandler;
        private readonly IAnnouncementsHandler _announcementsHandler;
        private readonly IChangelogsHandler _changelogsHandler;
        private readonly ILoggingHandler _loggingHandler;

        public AngularConstantsProvider(ISettingsHandler settingsHandler, IAnnouncementsHandler announcementsHandler, IChangelogsHandler changelogsHandler,
            ILoggingHandler loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _announcementsHandler = announcementsHandler;
            _changelogsHandler = changelogsHandler;
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
                    AnnouncementsJson = _announcementsHandler.Json,
                    ChangelogsJson = _changelogsHandler.Json,
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
