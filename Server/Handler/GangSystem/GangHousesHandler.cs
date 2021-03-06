﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Entities.GangSystem;
using TDS.Server.Handler.Events;

namespace TDS.Server.Handler.GangSystem
{
    public class GangHousesHandler : DatabaseEntityWrapper
    {
        public List<GangHouse> Houses { get; } = new List<GangHouse>();

        private readonly Dictionary<int, List<GangHouse>> _levelFreeHouses = new Dictionary<int, List<GangHouse>>();
        private readonly List<GangHouse> _occupiedHouses = new List<GangHouse>();

        private List<GangHouses> _houseEntities = new List<GangHouses>();

        private readonly GangLevelsHandler _gangLevelsHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly GangsHandler _gangsHandler;
        private readonly EventsHandler _eventsHandler;

        public GangHousesHandler(TDSDbContext dbContext, GangLevelsHandler gangLevelsHandler, GangsHandler gangsHandler,
            IServiceProvider serviceProvider, EventsHandler eventsHandler) : base(dbContext)
        {
            _gangLevelsHandler = gangLevelsHandler;
            _serviceProvider = serviceProvider;
            _gangsHandler = gangsHandler;
            _eventsHandler = eventsHandler;

            LoadHouses(dbContext);
        }

        public void LoadHouses(TDSDbContext dbContext)
        {
            _houseEntities = dbContext.GangHouses.Include(gh => gh.OwnerGang).AsNoTracking().ToList();

            foreach (var houseEntity in _houseEntities)
            {
                LoadHouse(houseEntity);
            }
        }

        public List<GangHouse> GetLoadedHouses()
        {
            lock (Houses)
            {
                return Houses.ToList();
            }
        }

        public async Task AddHouse(Vector3 position, float rotation, byte neededGangLevel, int creatorId)
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
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            LoadHouse(entity);
        }

        private void LoadHouse(GangHouses entity)
        {
            int cost = _gangLevelsHandler.Levels.TryGetValue(entity.NeededGangLevel, out GangLevelSettings? level) ? level.HousePrice : int.MaxValue;
            var house = ActivatorUtilities.CreateInstance<GangHouse>(_serviceProvider, entity, cost);
            lock (Houses) { Houses.Add(house); }

            if (house.Entity.OwnerGang is null)
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
                var gang = _gangsHandler.GetById(entity.OwnerGang.Id);
                gang.HouseHandler.House = house;
            }

            _eventsHandler.OnGangHouseLoaded(house);
        }
    }
}
