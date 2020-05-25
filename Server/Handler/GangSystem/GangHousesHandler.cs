using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.GangSystem;

namespace TDS_Server.Handler.GangSystem
{
    public class GangHousesHandler
    {
        #region Public Fields

        public List<GangHouse> Houses = new List<GangHouse>();

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<int, List<GangHouse>> _evelFreeHouses = new Dictionary<int, List<GangHouse>>();
        private readonly List<GangHouse> _occupiedHouses = new List<GangHouse>();
        private List<GangHouses> _houseEntities = new List<GangHouses>();

        #endregion Private Fields

        #region Public Constructors

        public GangHousesHandler(TDSDbContext dbContext, GangLevelsHandler gangLevelsHandler, GangsHandler gangsHandler,
            IServiceProvider serviceProvider)
        {
            LoadHouses(dbContext, gangLevelsHandler, gangsHandler, serviceProvider);
        }

        #endregion Public Constructors

        #region Public Methods

        public void LoadHouses(TDSDbContext dbContext, GangLevelsHandler gangLevelsHandler, GangsHandler gangsHandler,
            IServiceProvider serviceProvider)
        {
            _houseEntities = dbContext.GangHouses.Include(gh => gh.OwnerGang).AsNoTracking().ToList();

            foreach (var houseEntity in _houseEntities)
            {
                int cost = gangLevelsHandler.Levels.TryGetValue(houseEntity.NeededGangLevel, out GangLevelSettings? level) ? level.HousePrice : int.MaxValue;
                var house = ActivatorUtilities.CreateInstance<GangHouse>(serviceProvider, houseEntity, cost);
                Houses.Add(house);

                if (house.Entity.OwnerGang is null)
                {
                    if (!_evelFreeHouses.TryGetValue(house.Entity.NeededGangLevel, out List<GangHouse>? list))
                    {
                        list = new List<GangHouse>();
                        _evelFreeHouses.Add(house.Entity.NeededGangLevel, list);
                    }

                    list.Add(house);
                }
                else
                {
                    _occupiedHouses.Add(house);
                    var gang = gangsHandler.GetById(houseEntity.OwnerGang.Id);
                    gang.House = house;
                }
            }
        }

        #endregion Public Methods
    }
}
