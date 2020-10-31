using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Shared.Core;

namespace TDS_Server.Handler.GangSystem.GangWindow
{
    public class GangWindowRanksLevelsHandler
    {
        public List<GangRanks>? GetRanks(ITDSPlayer player)
        {
            if (player.Entity is null)
                return null;
            if (player.Gang.Entity.Id <= 0)
                return null;

            var data = player.Gang.Entity.Ranks.OrderBy(g => g.Rank).ToList();

            return data;
        }

        public string? GetRanksJson(ITDSPlayer player)
        {
            var data = GetRanks(player);
            if (data is null)
                return null;
            return Serializer.ToBrowser(data);
        }

        public async Task<object?> Modify(ITDSPlayer player, string json)
        {
            var fromList = Serializer.FromBrowser<List<GangRanks>>(json);

            await player.Gang.Database.ExecuteForDBAsync(async dbContext =>
            {
                var rank0 = player.Gang.Entity.Ranks.First(r => r.Rank == 0);
                foreach (var rank in player.Gang.Entity.Ranks)
                {
                    var newRank = fromList.FirstOrDefault(r => r.Id == rank.Id);
                    if (newRank is { })
                        CopyValues(newRank, rank, (short)fromList.IndexOf(newRank));
                    else
                    {
                        var playersWithThatRank = player.Gang.Entity.Members.Where(m => m.RankId == rank.Id);
                        foreach (var playerWithThatRank in playersWithThatRank)
                        {
                            playerWithThatRank.RankId = rank0.Rank;
                            playerWithThatRank.Rank = rank0;
                        }

                        player.Gang.Entity.Ranks.Remove(rank);
                    }
                }
                await dbContext.SaveChangesAsync();

                for (short rank = 0; rank < fromList.Count; ++rank)
                {
                    var rankEntity = fromList[rank];

                    if (rankEntity.Id != -1)
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
