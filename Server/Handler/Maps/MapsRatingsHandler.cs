using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Maps
{
    public class MapsRatingsHandler : DatabaseEntityWrapper
    {
        private Serializer _serializer;
        private MapsLoadingHandler _mapsLoadingHandler;
        private MapCreatorHandler _mapsCreatingHandler;

        public MapsRatingsHandler(
            EventsHandler eventsHandler, 
            Serializer serializer, 
            MapsLoadingHandler mapsLoadingHandler,
            MapCreatorHandler mapsCreatorHandler, 
            TDSDbContext dbContext, 
            ILoggingHandler loggingHandler)
            : base(dbContext, loggingHandler)
        {
            _serializer = serializer;
            _mapsLoadingHandler = mapsLoadingHandler;
            _mapsCreatingHandler = mapsCreatorHandler;

            eventsHandler.PlayerLoggedIn += SendPlayerHisRatings;
        }

        public async void AddPlayerMapRating(TDSPlayer player, int mapId, byte rating)
        {
            int playerId = player.Entity!.Id;

            MapDto? map = _mapsLoadingHandler.GetMapById(mapId);
            if (map is null)
                return;

            PlayerMapRatings? maprating = await ExecuteForDBAsync(async dbContext => await dbContext.PlayerMapRatings.FindAsync(playerId, mapId));
            if (maprating is null)
            {
                maprating = new PlayerMapRatings { PlayerId = playerId, MapId = mapId };
                await ExecuteForDB(dbContext => dbContext.PlayerMapRatings.Add(maprating));
                player.AddToChallenge(ChallengeType.ReviewMaps);
            }
            maprating.Rating = rating;
            map.BrowserSyncedData.Rating = rating;

            await ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync());
            
            map.Ratings.Add(maprating);
            map.RatingAverage = map.Ratings.Average(r => r.Rating);

            if (map.Info.IsNewMap)
                _mapsCreatingHandler.AddedMapRating(map);
        }

        public void SendPlayerHisRatings(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;
            if (!player.Entity.PlayerMapRatings.Any())
                return;

            var ratingsDict = player.Entity.PlayerMapRatings.ToDictionary(r => r.MapId, r => r.Rating);
            player.SendEvent(ToClientEvent.LoadOwnMapRatings, _serializer.ToBrowser(ratingsDict));
        }
    }
}
