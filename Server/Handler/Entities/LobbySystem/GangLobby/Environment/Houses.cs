using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.GangSystem;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {

        private readonly List<GangHouse> _occupiedHouses = new List<GangHouse>();
        private readonly Dictionary<int, List<GangHouse>> _levelFreeHouses = new Dictionary<int, List<GangHouse>>();

        private void LoadHouses()
        {
            
            List<GangHouses> houseEntities = ExecuteForDB(dbContext =>
            {
                return dbContext.GangHouses.Include(gh => gh.OwnerGang).ToList();
            }).Result;

            foreach (var houseEntity in houseEntities)
            {
                // GangHouses entity, int cost, ILobby lobby, 
                int cost = _levels.TryGetValue(houseEntity.NeededGangLevel, out GangLevelSettings? level) ? level.HousePrice : int.MaxValue;
                var house = ActivatorUtilities.CreateInstance<GangHouse>(_serviceProvider, houseEntity, this, cost);

                if (house.Entity.OwnerGang is { })
                {
                    if (!_levelFreeHouses.TryGetValue(house.Entity.NeededGangLevel, out List<GangHouse>? list))
                    {
                        list = new List<GangHouse>();
                        _levelFreeHouses.Add(house.Entity.NeededGangLevel, list);
                    }

                    list.Add(house);
                }
                else
                {
                    _occupiedHouses.Add(house);
                }
               
            }
        }
    }
}
