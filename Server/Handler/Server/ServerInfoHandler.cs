using BonusBotConnector.Client;
using System;
using System.Linq;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Interfaces.Handlers;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Bonusbot;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler.Server
{
    public class ServerInfoHandler
    {
        #region Private Fields

        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly BonusbotSettings? _bonusBotSettings;
        private readonly DateTime _dateTime;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ServerStatsHandler _serverStatsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public ServerInfoHandler(EventsHandler eventsHandler, TDSDbContext dbContext, LobbiesHandler lobbiesHandler, ITDSPlayerHandler tdsPlayerHandler,
            BonusBotConnectorClient bonusBotConnectorClient, ServerStatsHandler serverStatsHandler)
        {
            _dateTime = DateTime.UtcNow;

            _bonusBotSettings = dbContext.BonusbotSettings.FirstOrDefault();

            _lobbiesHandler = lobbiesHandler;
            _tdsPlayerHandler = tdsPlayerHandler;
            _bonusBotConnectorClient = bonusBotConnectorClient;
            _serverStatsHandler = serverStatsHandler;

            if (_bonusBotSettings is { } && _bonusBotConnectorClient.ServerInfos is { })
                eventsHandler.Second += EventsHandler_Second;
        }

        #endregion Public Constructors

        #region Private Methods

        private void EventsHandler_Second(int counter)
        {
            if (_bonusBotSettings is null)
                return;

            if (counter % _bonusBotSettings.RefreshServerStatsFrequencySec == 0)
            {
                //Todo: Read Server.cfg and get port & name
                var request = new RAGEServerStatsRequest
                {
                    PlayerAmountInArena = _lobbiesHandler.Arena.Players.Count,
                    PlayerAmountInCustomLobby = _lobbiesHandler.Lobbies.Where(p => !p.IsOfficial).Sum(l => l.Players.Count),
                    PlayerAmountInGangLobby = _lobbiesHandler.Lobbies.Where(p => p is IGangLobby || (p is IArena arena && arena.IsGangActionLobby)).Sum(l => l.Players.Count),
                    PlayerAmountInMainMenu = _tdsPlayerHandler.LoggedInPlayers.Where(p => p.Lobby is null || p.Lobby.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu).Count(),
                    PlayerAmountOnline = _tdsPlayerHandler.LoggedInPlayers.Count,
                    ServerPort = 0,
                    Version = "1.0.0",   // Todo: Save Version somewhere else
                    ServerName = "?",
                    RefreshFrequencySec = _bonusBotSettings.RefreshServerStatsFrequencySec,
                    PlayerPeakToday = _serverStatsHandler.DailyStats.PlayerPeak,
                    OnlineSince = Utils.GetUniversalDateTimeString(_dateTime)
                };

                _bonusBotConnectorClient.ServerInfos?.Refresh(request);
            }
        }

        #endregion Private Methods
    }
}
