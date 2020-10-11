using GTANetworkAPI;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Server.PlayersSystem
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
                _player.Entity.PlayerStats.Money = value;
                NAPI.Task.Run(() => _dataSyncHandler.SetData(_player, PlayerDataKey.Money, DataSyncMode.Player, value));
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
                if (money > 0 && _player.Entity?.PlayerTotalStats != null)
                    _player.Entity.PlayerTotalStats.Money += money;
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
