using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Player;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Server
{
    public class ResourceStopHandler
    {
        #region Private Fields

        private readonly ChallengesHelper _challengesHelper;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly ServerStatsHandler _serverStatsHandler;
        private readonly ITDSPlayerHandler _tdsPlayersHandler;
        private bool _isFirstResourceStopCheck = true;
        private bool _resourceStopped = false;

        #endregion Private Fields

        #region Public Constructors

        public ResourceStopHandler(EventsHandler eventsHandler, LangHelper langHelper, ILoggingHandler loggingHandler, ChallengesHelper challengesHelper, ServerStatsHandler serverStatsHandler,
            LobbiesHandler lobbiesHandler, ITDSPlayerHandler tdsPlayerHandler, IModAPI modAPI)
        {
            _langHelper = langHelper;
            _loggingHandler = loggingHandler;
            _challengesHelper = challengesHelper;
            _serverStatsHandler = serverStatsHandler;
            _lobbiesHandler = lobbiesHandler;
            _tdsPlayersHandler = tdsPlayerHandler;
            _modAPI = modAPI;

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Console.CancelKeyPress += CurrentDomain_ProcessExit;

            eventsHandler.Hour += CheckHourForResourceRestart;
            eventsHandler.ResourceStop += OnResourceStop;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CheckHourForResourceRestart(int _)
        {
            if (_isFirstResourceStopCheck)
            {
                _isFirstResourceStopCheck = false;
                return;
            }

            if (DateTime.UtcNow.Hour != 5)
                return;

            ResourceRestartCountdown(3, true);

            return;
        }

        public void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            OnResourceStop();
            Environment.Exit(0);
        }

        #endregion Public Methods

        #region Private Methods

        private void ExecuteResourceRestart()
        {
            if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Monday)
            {
                _challengesHelper.ClearWeeklyChallenges();
            }

            OnResourceStop();
            Process.GetCurrentProcess().Kill();
        }

        private void OnResourceStop()
        {
            if (_resourceStopped)
                return;
            _resourceStopped = true;
            SaveAllInDatabase();
            RemoveAllCreated();
        }

        private void RemoveAllCreated()
        {
            _modAPI.Pool.RemoveAll();
        }

        private void ResourceRestartCountdown(int counter, bool isMinute)
        {
            if (isMinute)
            {
                if (--counter > 1)
                {
                    _langHelper.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_MINUTES, counter));
                    _ = new TDSTimer(() => ResourceRestartCountdown(counter, true), 60 * 1000, 1);
                }
                else if (counter == 1)
                {
                    _langHelper.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_MINUTES, counter));
                    _ = new TDSTimer(() => ResourceRestartCountdown(counter, true), 45 * 1000, 1);
                }
                else
                {
                    _langHelper.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_SECONDS, 15));
                    _ = new TDSTimer(() => ResourceRestartCountdown(10, false), 5000, 1);
                }
            }
            else // is second
            {
                if (--counter > 0)
                {
                    _langHelper.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_SECONDS, 15));
                    _ = new TDSTimer(() => ResourceRestartCountdown(counter, false), 1000, 1, true);
                }
                else
                {
                    ExecuteResourceRestart();
                }
            }
        }

        private void SaveAllInDatabase()
        {
            try
            {
                List<Task> tasks = new List<Task>
                {
                    _loggingHandler.SaveTask(),
                    _serverStatsHandler.SaveTask(),
                    _lobbiesHandler.SaveAll(),
                };

                foreach (ITDSPlayer player in _tdsPlayersHandler.LoggedInPlayers)
                {
                    try
                    {
                        tasks.Add(player.SaveData(true).AsTask());
                    }
                    catch (Exception ex)
                    {
                        _loggingHandler.LogError(ex, player);
                    }
                }

                Task.WaitAll(tasks.ToArray(), System.Threading.Timeout.Infinite);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}
