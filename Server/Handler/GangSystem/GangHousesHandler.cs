using AltV.Net.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.GangSystem
{
    public class GangHousesHandler : DatabaseEntityWrapper
    {
        #region Public Fields

        public List<IGangHouse> Houses = new List<IGangHouse>();

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<int, List<IGangHouse>> _levelFreeHouses = new Dictionary<int, List<IGangHouse>>();
        private readonly List<IGangHouse> _occupiedHouses = new List<IGangHouse>();
        private List<GangHouses> _houseEntities = new List<GangHouses>();

        private readonly GangLevelsHandler _gangLevelsHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly GangsHandler _gangsHandler;
        private readonly EventsHandler _eventsHandler;

        #endregion Private Fields

        #region Public Constructors

        public GangHousesHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler, GangLevelsHandler gangLevelsHandler, GangsHandler gangsHandler,
            IServiceProvider serviceProvider, EventsHandler eventsHandler) : base(dbContext, loggingHandler)
        {
            _gangLevelsHandler = gangLevelsHandler;
            _serviceProvider = serviceProvider;
            _gangsHandler = gangsHandler;
            _eventsHandler = eventsHandler;

            LoadHouses(dbContext);
        }

        #endregion Public Constructors

        #region Public Methods

        public void LoadHouses(TDSDbContext dbContext)
        {
            _houseEntities = dbContext.GangHouses.Include(gh => gh.OwnerGang).AsNoTracking().ToList();

            foreach (var houseEntity in _houseEntities)
            {
                LoadHouse(houseEntity);
            }
        }


        public async void AddHouse(Position position, float rotation, byte neededGangLevel, int creatorId)
        {
            var entity = new GangHouses
            {
                CreatorId = creatorId,
                NeededGangLevel = neededGangLevel,
                PosX = position.X,
                PosY = position.Y,
                PosZ = position.Z,
                Rot = rotation
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.GangHouses.Add(entity);
                await dbContext.SaveChangesAsync();
            });

            LoadHouse(entity);
        }

        private void LoadHouse(GangHouses entity)
        {
            int cost = _gangLevelsHandler.Levels.TryGetValue(entity.NeededGangLevel, out GangLevelSettings? level) ? level.HousePrice : int.MaxValue;
            var house = ActivatorUtilities.CreateInstance<IGangHouse>(_serviceProvider, entity, cost);
            Houses.Add(house);

            if (house.Entity.OwnerGang is null)
            {
                if (!_levelFreeHouses.TryGetValue(house.Entity.NeededGangLevel, out List<IGangHouse>? list))
                {
                    list = new List<IGangHouse>();
                    _levelFreeHouses.Add(house.Entity.NeededGangLevel, list);
                }

                list.Add(house);
            }
            else
            {
                _occupiedHouses.Add(house);
                var gang = _gangsHandler.GetById(entity.OwnerGang.Id);
                gang.House = house;
            }

            _eventsHandler.OnGangHouseLoaded(house);
        }

        #endregion Public Methods
    }
}
