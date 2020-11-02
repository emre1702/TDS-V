using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Server
{
    public class ResourceStopHandler
    {

        private readonly ChallengesHelper _challengesHelper;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ServerStatsHandler _serverStatsHandler;
        private readonly ITDSPlayerHandler _tdsPlayersHandler;
        private bool _isFirstResourceStopCheck = true;
        private bool _resourceStopped = false;

        public ResourceStopHandler(EventsHandler eventsHandler, LangHelper langHelper, ILoggingHandler loggingHandler, ChallengesHelper challengesHelper, ServerStatsHandler serverStatsHandler,
            LobbiesHandler lobbiesHandler, ITDSPlayerHandler tdsPlayerHandler)
        {
            _langHelper = langHelper;
            _loggingHandler = loggingHandler;
            _challengesHelper = challengesHelper;
            _serverStatsHandler = serverStatsHandler;
            _lobbiesHandler = lobbiesHandler;
            _tdsPlayersHandler = tdsPlayerHandler;

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Console.CancelKeyPress += CurrentDomain_ProcessExit;

            eventsHandler.Hour += CheckHourForResourceRestart;
            eventsHandler.ResourceStop += OnResourceStop;
        }

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
            NAPI.Task.RunSafe(() =>
            {
                DeleteEntityList(NAPI.Pools.GetAllBlips());
                DeleteEntityList(NAPI.Pools.GetAllCheckpoints());
                DeleteEntityList(NAPI.Pools.GetAllColShapes());
                DeleteEntityList(NAPI.Pools.GetAllDummyEntities());
                DeleteEntityList(NAPI.Pools.GetAllMarkers());
                DeleteEntityList(NAPI.Pools.GetAllPeds());
                DeleteEntityList(NAPI.Pools.GetAllPickups());
                DeleteEntityList(NAPI.Pools.GetAllTextLabels());
                DeleteEntityList(NAPI.Pools.GetAllVehicles());
                DeleteEntityList(NAPI.Pools.GetAllObjects());
            });
        }

        private void DeleteEntityList(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
                NAPI.Entity.DeleteEntity(entity);
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
                var tasks = new List<Task>
                {
                    _loggingHandler.SaveTask(),
                    _serverStatsHandler.SaveTask(),
                    _lobbiesHandler.SaveAll(),
                };

                foreach (var player in _tdsPlayersHandler.LoggedInPlayers)
                {
                    try
                    {
                        tasks.Add(player.DatabaseHandler.SaveData(true).AsTask());
                    }
                    catch (Exception ex)
                    {
                        _loggingHandler.LogError(ex, player);
                    }
                }

                Console.WriteLine("Saving all in database ...");
                Task.WaitAll(tasks.ToArray(), System.Threading.Timeout.Infinite);
                Console.WriteLine("Saved all in database.");
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

    }
}
