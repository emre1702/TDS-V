using GTANetworkAPI;
using System;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Stats;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Timer
{
    internal class MinuteTimer
    {
        private static int _counter = 0;

        public static async void Execute()
        {
            if (!ResourceStart.ResourceStarted)
                return;

            // int currenttick = Environment.TickCount;
            ++_counter;

            try
            {
                SavePlayers();

                await ServerTotalStatsManager.Save();
                await ServerDailyStatsManager.Save();

                // log-save //
                if (_counter % SettingsManager.SaveLogsCooldownMinutes == 0)
                {
                    await LogsManager.Save();
                }

                if (_counter % SettingsManager.SaveSeasonsCooldownMinutes == 0)
                {
                    //Season.SaveSeason();
                }

                var date = DateTime.UtcNow;
                NAPI.World.SetTime(date.Hour, date.Minute, date.Second);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.Message, Environment.StackTrace);
            }
        }

        private static void SavePlayers()
        {
            foreach (var player in Player.Player.GetAllTDSPlayer())
            {
                try
                {
                    ++player.PlayMinutes;
                    ReduceMuteTime(player);
                    ReduceVoiceMuteTime(player);
                    player.CheckReduceMapBoughtCounter();

                    player.CheckSaveData();
                }
                catch (Exception ex)
                {
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace, player);
                }
            }
        }

        private static void ReduceMuteTime(TDSPlayer player)
        {
            if (!player.MuteTime.HasValue || player.MuteTime == 0)
                return;

            if (--player.MuteTime != 0)
                return;

            player.MuteTime = null;
            player.SendNotification(player.Language.MUTE_EXPIRED);
        }

        private static void ReduceVoiceMuteTime(TDSPlayer player)
        {
            if (!player.VoiceMuteTime.HasValue || player.VoiceMuteTime == 0)
                return;

            if (--player.VoiceMuteTime != 0)
                return;
            
            player.VoiceMuteTime = null;
            player.SendNotification(player.Language.VOICE_MUTE_EXPIRED);

            if (player.Team is null || player.Team.IsSpectator)
                return;

            foreach (var target in player.Team.Players)
            {
                if (!target.HasRelationTo(player, EPlayerRelation.Block))
                    player.Client?.EnableVoiceTo(target.Client);
            }
        }
    }
}