using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.GangWindow;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;

namespace TDS_Server.Handler.GangSystem.GangWindow
{
    public class GangWindowRanksLevelsHandler
    {
        private readonly Serializer _serializer;

        public GangWindowRanksLevelsHandler(Serializer serializer)
        {
            _serializer = serializer;
        }

        public string? GetRanks(ITDSPlayer player)
        {
            if (player.Entity is null)
                return null;
            if (player.Gang.Entity.Id <= 0)
                return null;

            var data = player.Gang.Entity.Ranks.OrderBy(g => g.Rank).ToList();
            foreach (var rank in data)
            {
                rank.OriginalRank = rank.Rank;
            }

            return _serializer.ToBrowser(data);
        }

        public async Task<object?> Modify(ITDSPlayer player, string json)
        {
            var fromList = _serializer.FromBrowser<List<GangRanks>>(json);

            await player.Gang.ExecuteForDBAsync(async dbContext =>
            {

                foreach (var rank in player.Gang.Entity.Ranks)
                {
                    var newRank = fromList.FirstOrDefault(r => r.OriginalRank == rank.Rank);
                    if (newRank is { })
                        CopyValues(newRank, rank, (short)fromList.IndexOf(newRank));
                    else 
                        player.Gang.Entity.Ranks.Remove(rank);
                }
                await dbContext.SaveChangesAsync();

                for (short rank = 0; rank < fromList.Count; ++rank)
                {
                    var rankEntity = fromList[rank];

                    if (rankEntity.OriginalRank.HasValue)
                        continue;

                    player.Gang.Entity.Ranks.Add(new GangRanks
                    {
                        Rank = rank,
                        Color = rankEntity.Color,
                        Name = rankEntity.Name
                    });
                }

                await dbContext.SaveChangesAsync();
            });


            return "";
        }

        private void CopyValues(GangRanks from, GangRanks to, short rank)
        {
            to.Name = from.Name;
            to.Rank = rank;
            to.Color = from.Color;
        }
    }
}
