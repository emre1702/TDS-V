using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Stats;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Player;
using TDS_Shared.Instance;

namespace TDS_Server.Handler
{
    public class ResourceStopHandler
    {
        private bool _resourceStopped = false;
        private bool _isFirstResourceStopCheck = true;

        private readonly LangHelper _langHelper;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ChallengesHelper _challengesHelper;
        private readonly ServerStatsHandler _serverStatsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly TDSPlayerHandler _tdsPlayersHandler;
        private readonly IModAPI _modAPI;

        public ResourceStopHandler(EventsHandler eventsHandler, LangHelper langHelper, ILoggingHandler loggingHandler, ChallengesHelper challengesHelper, ServerStatsHandler serverStatsHandler,
            LobbiesHandler lobbiesHandler, TDSPlayerHandler tdsPlayerHandler, IModAPI modAPI)
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


        public void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            OnResourceStop();
            Environment.Exit(0);
        }

        public void CheckHourForResourceRestart(ulong _)
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


        private void ExecuteResourceRestart()
        {
            if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Monday)
            {
                _challengesHelper.ClearWeeklyChallenges();
            }

            OnResourceStop();
            Process.GetCurrentProcess().Kill();
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

        private void RemoveAllCreated()
        {
            _modAPI.Pool.RemoveAll();
        }

        private void OnResourceStop()
        {
            if (_resourceStopped)
                return;
            _resourceStopped = true;
            SaveAllInDatabase();
            RemoveAllCreated();
        }
    }
}
