using System;
using TDS_Server.Data.Enums;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Properties

        public int Money
        {
            get => Entity?.PlayerStats.Money ?? 0;
            set
            {
                if (Entity is null)
                    return;
                Entity.PlayerStats.Money = value;
                _dataSyncHandler.SetData(this, PlayerDataKey.Money, DataSyncMode.Player, value);
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
                    AddToChallenge(ChallengeType.PlayTime, addToPlayTime);
            }
        }

        public PlayerTotalStats? TotalStats => Entity?.PlayerTotalStats;

        #endregion Public Properties

        #region Public Methods

        public void CheckReduceMapBoughtCounter()
        {
            if (Entity is null)
                return;
            if (Entity.PlayerStats.MapsBoughtCounter <= 1)
                return;
            if (DateTime.UtcNow >= Entity.PlayerStats.LastMapsBoughtCounterReduce.AddMinutes(_settingsHandler.ServerSettings.ReduceMapsBoughtCounterAfterMinute))
            {
                Entity.PlayerStats.LastMapsBoughtCounterReduce = DateTime.UtcNow;
                --Entity.PlayerStats.MapsBoughtCounter;
                _dataSyncHandler.SetData(this, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, Entity.PlayerStats.MapsBoughtCounter);
            }
        }

        public void GiveMoney(int money)
        {
            if (money >= 0 || Money > money * -1)
            {
                Money += money;
                if (money > 0 && TotalStats != null)
                    TotalStats.Money += money;
            }
            else
                LoggingHandler?.LogError($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
                                Environment.StackTrace, null, this);
        }

        public void GiveMoney(uint money)
        {
            GiveMoney((int)money);
        }

        #endregion Public Methods
    }
}
