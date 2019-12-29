using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TDS_Common.Dto;
using TDS_Server.Instance;
using TDS_Server.Instance.GameModes;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Commands;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Stats;
using TDS_Server.Manager.Timer;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    internal class ResourceStart : Script
    {
        public static bool ResourceStarted { get; private set; }

        public ResourceStart()
        {
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);
            var date = DateTime.UtcNow;
            NAPI.World.SetTime(date.Hour, date.Minute, date.Second);
            LoadAll();
        }

        private static string? GetLocalIPv4()
        {
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties adapterProperties = item.GetIPProperties();

                    if (adapterProperties.GatewayAddresses.Any())
                    {
                        foreach (UnicastIPAddressInformation ip in adapterProperties.UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }

            return null;
        }

        private async void LoadAll()
        {
            try
            {
                BonusBotConnector_Server.Program.Main();

                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                AppDomain.CurrentDomain.ProcessExit += ResourceStop.CurrentDomain_ProcessExit;

                SettingsManager.LoadLocal();
                ClothesManager.Init();

                using var dbcontext = new TDSDbContext(SettingsManager.ConnectionString);
                dbcontext.Database.Migrate();
                var connection = (NpgsqlConnection)dbcontext.Database.GetDbConnection();
                connection.Open();
                connection.ReloadTypes();

                BonusBotConnector_Client.Main.Init(dbcontext, ErrorLogsManager.LogFromBonusBot);

                var playerStats = await dbcontext.PlayerStats.Where(s => s.LoggedIn).ToListAsync().ConfigureAwait(true);
                foreach (var stat in playerStats)
                {
                    stat.LoggedIn = false;
                }
                await dbcontext.SaveChangesAsync().ConfigureAwait(true);
                dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                ServerDailyStatsManager.Init();
                ServerTotalStatsManager.Init();

                await SettingsManager.Load(dbcontext).ConfigureAwait(true);

                HourTimer.Execute();

                await AdminsManager.Init(dbcontext).ConfigureAwait(true);
                Workaround.Init();
                await CommandsManager.LoadCommands(dbcontext).ConfigureAwait(true);
                Damagesys.LoadDefaults(dbcontext);

                await BansManager.Get().RemoveExpiredBans().ConfigureAwait(true);

                await MapsLoader.LoadDefaultMaps(dbcontext).ConfigureAwait(true);
                await MapCreator.LoadNewMaps(dbcontext).ConfigureAwait(true);
                await MapCreator.LoadSavedMaps(dbcontext).ConfigureAwait(true);
                await MapCreator.LoadNeedCheckMaps(dbcontext).ConfigureAwait(true);

                Normal.Init(dbcontext);
                Bomb.Init(dbcontext);
                Sniper.Init(dbcontext);

                await Gang.LoadAll(dbcontext).ConfigureAwait(true);
                await LobbyManager.LoadAllLobbies(dbcontext).ConfigureAwait(true);
                GangwarAreasManager.LoadGangwarAreas(dbcontext);

                Userpanel.Main.Init(dbcontext);
                InvitationManager.Init();

                ResourceStarted = true;

                Account.Init();

                HourTimer.CreateTimer();
                MinuteTimer.CreateTimer();
                SecondTimer.CreateTimer();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(ReadInput);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }

        [ServerEvent(Event.UnhandledException)]
        public void OnUnhandledException(Exception ex)
        {
            try
            {
                ErrorLogsManager.Log("Unhandled exception: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, (TDSPlayer?)null);
            }
            catch
            {
                // ignored
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                ErrorLogsManager.Log("CurrentDomain_UnhandledException: " 
                    +  ((Exception)e.ExceptionObject).GetBaseException().Message, 
                    ((Exception)e.ExceptionObject).StackTrace ?? Environment.StackTrace, 
                    (TDSPlayer?)null);
            }
            catch
            {
                // ignored
            }
        }

        private void ReadInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Length == 0)
                    continue;
                if (input[0] == '/')
                    input = input.Substring(1);

                var consolePlayer = new TDSPlayer(null) 
                {
                    IsConsole = true
                };

                NAPI.Task.Run(() => CommandsManager.UseCommand(consolePlayer, input));
                
            }
        }
    }
}