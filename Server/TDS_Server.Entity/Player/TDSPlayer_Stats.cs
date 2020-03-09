using System;
using TDS_Shared.Enum;
using TDS_Shared.Enum.Challenge;
using TDS_Server.Enums;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        public PlayerTotalStats? TotalStats => Entity?.PlayerTotalStats;

        public int Money
        {
            get => Entity?.PlayerStats.Money ?? 0;
            set
            {
                if (Entity is null)
                    return;
                Entity.PlayerStats.Money = value;
                PlayerDataSync.SetData(this, EPlayerDataKey.Money, EPlayerDataSyncMode.Player, value);
            }
        }

        public int PlayMinutes
        {
            get => Entity?.PlayerStats.PlayTime ?? 0;
            set
            {
                if (Entity is null)
                    return;
                int addToPlayTime = value - Entity.PlayerStats.PlayTime;
                Entity.PlayerStats.PlayTime = value;
                if (addToPlayTime > 0)
                    AddToChallenge(EChallengeType.PlayTime, addToPlayTime);
            }
        }


        #region Money

        public void GiveMoney(int money)
        {
            if (money >= 0 || Money > money * -1)
            {
                Money += money;
                if (money > 0 && TotalStats != null)
                    TotalStats.Money += money;
            }
            else
                if (Player is { })
                    ErrorLogsManager.Log($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
                                    Environment.StackTrace, Player);
                else
                ErrorLogsManager.Log($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
                                Environment.StackTrace);
        }

        public void GiveMoney(uint money)
        {
            GiveMoney((int)money);
        }

        #endregion Money

        public void CheckReduceMapBoughtCounter()
        {
            if (Entity is null)
                return;
            if (Entity.PlayerStats.MapsBoughtCounter <= 1)
                return;
            if (DateTime.UtcNow >= Entity.PlayerStats.LastMapsBoughtCounterReduce.AddMinutes(SettingsManager.ServerSettings.ReduceMapsBoughtCounterAfterMinute))
            {
                Entity.PlayerStats.LastMapsBoughtCounterReduce = DateTime.UtcNow;
                --Entity.PlayerStats.MapsBoughtCounter;
                PlayerDataSync.SetData(this, EPlayerDataKey.MapsBoughtCounter, EPlayerDataSyncMode.Player, Entity.PlayerStats.MapsBoughtCounter);
            }
        }
    }
}
