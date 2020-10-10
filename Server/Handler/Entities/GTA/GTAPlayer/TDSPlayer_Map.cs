using TDS_Server.Data.Enums;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        public override void SetBoughtMap(int price)
        {
            GiveMoney(-price);
            ++Entity!.PlayerStats.MapsBoughtCounter;
            if (LobbyStats is { })
                ++LobbyStats.TotalMapsBought;
            AddToChallenge(ChallengeType.BuyMaps);
            _dataSyncHandler.SetData(this, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, Entity.PlayerStats.MapsBoughtCounter);
        }
    }
}
