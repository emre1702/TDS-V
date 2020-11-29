using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Maps;

namespace TDS.Server.Handler.GangSystem
{
    public class GangActionAreasHandler
    {
        private readonly List<IBaseGangActionArea> _gangAreas = new List<IBaseGangActionArea>();

        private readonly TDSDbContext _dbContext;
        private readonly ILoggingHandler _loggingHandler;
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly IServiceProvider _serviceProvider;

        public GangActionAreasHandler(TDSDbContext dbContext, MapsLoadingHandler mapsLoadingHandler, EventsHandler eventsHandler,
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _mapsLoadingHandler = mapsLoadingHandler;
            _loggingHandler = loggingHandler;
            _serviceProvider = serviceProvider;

            eventsHandler.MapsLoaded += LoadGangActionAreas;
        }



        /// <summary>
        /// Returns the gang action area by Id / MapId. MapId is used as Id!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IBaseGangActionArea? GetById(int id)
        {
            lock (_gangAreas)
            {
                return _gangAreas.FirstOrDefault(a => a.Entity?.MapId == id);
            }
        }

        private void LoadGangActionAreas()
        {
            var entities = _dbContext.GangActionAreas.Include(a => a.Map).Include(a => a.OwnerGang).ToList();
            CreateMissingGangActionAreas(entities);
            foreach (var entity in entities)
            {
                var map = _mapsLoadingHandler.GetGangActionAreaMap(entity.MapId);
                if (map is null)
                {
                    _loggingHandler.LogError($"Gang action area with Map {entity.Map.Name} ({entity.MapId}) has no map file in default maps folder!", Environment.StackTrace);
                    continue;
                }
                var area = ActivatorUtilities.CreateInstance<IBaseGangActionArea>(_serviceProvider, entity, map);
                lock (_gangAreas) { _gangAreas.Add(area); }
            }
            // Don't need it anymore
            _dbContext.Dispose();
        }

        private void CreateMissingGangActionAreas(List<GangActionAreas> gangActionArea)
        {
            var mapsWithoutGangActionArea = _mapsLoadingHandler.GetGangActionAreaMapsWithoutGangActionArea(gangActionArea);

            var newEntities = new List<GangActionAreas>();
            foreach (var map in mapsWithoutGangActionArea)
            {
                var entity = new GangActionAreas
                {
                    MapId = map.BrowserSyncedData.Id,
                    OwnerGangId = -1
                };
                newEntities.Add(entity);
                _dbContext.GangActionAreas.Add(entity);
                gangActionArea.Add(entity);
            }

            _dbContext.SaveChanges();

            foreach (var entity in newEntities)
            {
                _dbContext.Entry(entity).Reference(e => e.Map).Load();
            }
        }
    }
}
