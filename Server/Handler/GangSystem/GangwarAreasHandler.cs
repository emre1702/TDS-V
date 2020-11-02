﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.GangSystem.GangGamemodes.Gangwar;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Maps;

namespace TDS_Server.Handler.GangSystem
{
    public class GangwarAreasHandler
    {
        private readonly List<GangwarArea> _gangwarAreas = new List<GangwarArea>();

        private readonly TDSDbContext _dbContext;
        private readonly ILoggingHandler _loggingHandler;
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly IServiceProvider _serviceProvider;

        public GangwarAreasHandler(TDSDbContext dbContext, MapsLoadingHandler mapsLoadingHandler, EventsHandler eventsHandler,
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _mapsLoadingHandler = mapsLoadingHandler;
            _loggingHandler = loggingHandler;
            _serviceProvider = serviceProvider;

            eventsHandler.MapsLoaded += LoadGangwarAreas;
        }

        

        /// <summary>
        /// Returns the Gangwar area by Id / MapId. MapId is used as Id!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GangwarArea? GetById(int id)
        {
            lock (_gangwarAreas)
            {
                return _gangwarAreas.FirstOrDefault(a => a.Entity?.MapId == id);
            }
        }

        private void LoadGangwarAreas()
        {
            var entities = _dbContext.GangwarAreas.Include(a => a.Map).Include(a => a.OwnerGang).ToList();
            CreateMissingGangwarAreas(entities);
            foreach (var entity in entities)
            {
                var map = _mapsLoadingHandler.GetGangwarAreaMap(entity.MapId);
                if (map is null)
                {
                    _loggingHandler.LogError($"GangwarArea with Map {entity.Map.Name} ({entity.MapId}) has no map file in default maps folder!", Environment.StackTrace);
                    continue;
                }
                var area = ActivatorUtilities.CreateInstance<GangwarArea>(_serviceProvider, entity, map);
                lock(_gangwarAreas) { _gangwarAreas.Add(area); }
            }
            // Don't need it anymore
            _dbContext.Dispose();
        }

        private void CreateMissingGangwarAreas(List<GangwarAreas> gangwarAreas)
        {
            var mapsWithoutGangwarArea = _mapsLoadingHandler.GetGangwarMapsWithoutGangwarAreas(gangwarAreas);

            var newEntities = new List<GangwarAreas>();
            foreach (var map in mapsWithoutGangwarArea)
            {
                var entity = new GangwarAreas
                {
                    MapId = map.BrowserSyncedData.Id,
                    OwnerGangId = -1
                };
                newEntities.Add(entity);
                _dbContext.GangwarAreas.Add(entity);
                gangwarAreas.Add(entity);
            }

            _dbContext.SaveChanges();

            foreach (var entity in newEntities)
            {
                _dbContext.Entry(entity).Reference(e => e.Map).Load();
            }
        }
    }
}
