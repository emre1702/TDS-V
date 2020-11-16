using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.PlayersSystem
{
    public class MapsVoting : IPlayerMapsVoting
    {
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ISettingsHandler _settingsHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public MapsVoting(DataSyncHandler dataSyncHandler, ISettingsHandler settingsHandler)
        {
            _dataSyncHandler = dataSyncHandler;
            _settingsHandler = settingsHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void CheckReduceMapBoughtCounter()
        {
            if (!(_player.Entity?.PlayerStats is { } stats))
                return;
            if (stats.MapsBoughtCounter <= 1)
                return;
            if (DateTime.UtcNow >= stats.LastMapsBoughtCounterReduce.AddMinutes(_settingsHandler.ServerSettings.ReduceMapsBoughtCounterAfterMinute))
            {
                stats.LastMapsBoughtCounterReduce = DateTime.UtcNow;
                --stats.MapsBoughtCounter;
                NAPI.Task.RunSafe(() => _dataSyncHandler.SetData(_player, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, stats.MapsBoughtCounter));
            }
        }

        public void SetBoughtMap(int price)
        {
            _player.MoneyHandler.GiveMoney(-price);
            ++_player.Entity!.PlayerStats.MapsBoughtCounter;
            if (_player.LobbyStats is { })
                ++_player.LobbyStats.TotalMapsBought;
            _player.Challenges.AddToChallenge(ChallengeType.BuyMaps);
            NAPI.Task.RunSafe(() =>
                _dataSyncHandler.SetData(_player, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, _player.Entity.PlayerStats.MapsBoughtCounter));
        }
    }
}
