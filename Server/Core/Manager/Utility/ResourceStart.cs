using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Threading.Tasks;
using TDS_Server.Database.Entity;

namespace TDS_Server.Core.Manager.Utility
{
    internal class ResourceStart : Script
    {

        private async void LoadAll()
        {
            try
            {
                if (!CodeMistakesChecker.EverythingsAlright())
                {
                    NAPI.Resource.StopResource("tds");
                    return;
                }

                HourTimer.Execute();

                await AdminsManager.Init(dbContext).ConfigureAwait(true);
                Workaround.Init();
                await CommandsManager.LoadCommands(dbContext).ConfigureAwait(true);
                Damagesys.LoadDefaults(dbContext);

                await BansManager.Get().RemoveExpiredBans().ConfigureAwait(true);



                Normal.Init(dbContext);
                Bomb.Init(dbContext);
                Sniper.Init(dbContext);
                Gangwar.Init(dbContext);

                await Gang.LoadAll(dbContext).ConfigureAwait(true);
                await LobbyManager.LoadAllLobbies(dbContext).ConfigureAwait(true);
                GangwarAreasManager.LoadGangwarAreas(dbContext);

                Userpanel.Main.Init(dbContext);
                InvitationManager.Init();

                ResourceStarted = true;

                Account.Init();

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

        
    }
}
