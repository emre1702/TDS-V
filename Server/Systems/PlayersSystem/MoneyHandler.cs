using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;

namespace TDS.Server.PlayersSystem
{
    public class MoneyHandler : IPlayerMoneyHandler
    {
        public int Money
        {
            get => _player.Entity?.PlayerStats.Money ?? 0;
            set
            {
                if (_player.Entity is null)
                    return;
                var addedMoney = value - _player.Entity.PlayerStats.Money;
                _player.Entity.PlayerStats.Money = value;
                if (addedMoney > 0 && _player.Entity?.PlayerTotalStats is { } totalStats)
                    totalStats.Money += addedMoney;
                NAPI.Task.RunSafe(() => _dataSyncHandler.SetData(_player, PlayerDataKey.Money, DataSyncMode.Player, value));
            }
        }

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ILoggingHandler _loggingHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public MoneyHandler(DataSyncHandler dataSyncHandler, ILoggingHandler loggingHandler)
        {
            _dataSyncHandler = dataSyncHandler;
            _loggingHandler = loggingHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void GiveMoney(int money)
        {
            if (money >= 0 || Money > money * -1)
            {
                Money += money;
            }
            else
                _loggingHandler.LogError($"Should have went to minus money! Current: {Money} | Substracted money: {money}",
                                Environment.StackTrace, null, _player);
        }

        public void GiveMoney(uint money)
        {
            GiveMoney((int)money);
        }
    }
}
