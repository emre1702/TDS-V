using GTANetworkAPI;
using System;
using TDS_Server.Data.Enums;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        public override int Money
        {
            get => Entity?.PlayerStats.Money ?? 0;
            set
            {
                if (Entity is null)
                    return;
                Entity.PlayerStats.Money = value;
                NAPI.Task.Run(() => _dataSyncHandler.SetData(this, PlayerDataKey.Money, DataSyncMode.Player, value));
            }
        }

        public override int PlayMinutes
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

        public override PlayerTotalStats? TotalStats => Entity?.PlayerTotalStats;

        public override void CheckReduceMapBoughtCounter()
        {
            if (Entity is null)
                return;
            if (Entity.PlayerStats.MapsBoughtCounter <= 1)
                return;
            if (DateTime.UtcNow >= Entity.PlayerStats.LastMapsBoughtCounterReduce.AddMinutes(_settingsHandler.ServerSettings.ReduceMapsBoughtCounterAfterMinute))
            {
                Entity.PlayerStats.LastMapsBoughtCounterReduce = DateTime.UtcNow;
                --Entity.PlayerStats.MapsBoughtCounter;
                NAPI.Task.Run(() => _dataSyncHandler.SetData(this, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, Entity.PlayerStats.MapsBoughtCounter));
            }
        }

        public override void GiveMoney(int money)
        {
            if (money >= 0 || Money > money * -1)
            {
                Money += money;
                if (money > 0 && TotalStats != null)
                    TotalStats.Money += money;
            }
            else
                _loggingHandler.LogError($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
                                Environment.StackTrace, null, this);
        }

        public override void GiveMoney(uint money)
        {
            GiveMoney((int)money);
        }
    }
}
