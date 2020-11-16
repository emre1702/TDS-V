using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models.GangWindow;
using TDS.Shared.Core;

namespace TDS.Server.Handler.GangSystem.GangWindow
{
    public class GangWindowMainMenuHandler
    {
        public string? GetMainData(ITDSPlayer player)
        {
            if (!player.IsInGang || player.GangRank is null)
                return "";

            var gangEntity = player.Gang.Entity;

            var data = new GangMainData
            {
                GangData = new SyncedGangData
                {
                    Name = gangEntity.Name,
                    Short = gangEntity.Short,
                    Color = gangEntity.Color,
                    BlipColor = gangEntity.BlipColor,
                    Id = gangEntity.Id,
                    OwnerId = gangEntity.OwnerId ?? -1
                },
                PlayerGangData = new SyncedPlayerGangData
                {
                    Rank = player.GangRank.Rank,
                    IsGangOwner = player.IsGangOwner
                },
                HighestRank = gangEntity.Ranks.Max(r => r.Rank)
            };

            return Serializer.ToBrowser(data);
        }
    }
}
