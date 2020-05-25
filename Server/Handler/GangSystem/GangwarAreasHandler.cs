using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Maps;

namespace TDS_Server.Handler.GangSystem
{
    public class GangwarAreasHandler
    {
        #region Private Fields

        private readonly TDSDbContext _dbContext;
        private readonly ILoggingHandler _loggingHandler;
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly IServiceProvider _serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public GangwarAreasHandler(TDSDbContext dbContext, MapsLoadingHandler mapsLoadingHandler, EventsHandler eventsHandler, ILoggingHandler loggingHandler, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _mapsLoadingHandler = mapsLoadingHandler;
            _loggingHandler = loggingHandler;
            _serviceProvider = serviceProvider;

            eventsHandler.MapsLoaded += LoadGangwarAreas;
        }

        #endregion Public Constructors

        #region Public Properties

        public List<GangwarArea> GangwarAreas { get; set; } = new List<GangwarArea>();

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the Gangwar area by Id / MapId. MapId is used as Id!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GangwarArea? GetById(int id)
        {
            return GangwarAreas.FirstOrDefault(a => a.Entity?.MapId == id);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadGangwarAreas()
        {
            var entities = _dbContext.GangwarAreas.Include(a => a.Map).Include(a => a.OwnerGang).ToList();
            foreach (var entity in entities)
            {
                var map = _mapsLoadingHandler.DefaultMaps.FirstOrDefault(m => m.Info.Type == MapType.Gangwar && m.BrowserSyncedData.Id == entity.MapId);
                if (map is null)
                {
                    _loggingHandler.LogError($"GangwarArea with Map {entity.Map.Name} ({entity.MapId}) has no map file in default maps folder!", Environment.StackTrace);
                    continue;
                }
                var area = ActivatorUtilities.CreateInstance<GangwarArea>(_serviceProvider, entity, map);
                GangwarAreas.Add(area);
            }
        }

        #endregion Private Methods
    }
}
