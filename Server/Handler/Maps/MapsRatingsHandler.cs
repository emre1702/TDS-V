using GTANetworkAPI;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Maps
{
    public class MapsRatingsHandler : DatabaseEntityWrapper
    {
        private readonly MapCreatorHandler _mapsCreatingHandler;
        private readonly MapsLoadingHandler _mapsLoadingHandler;

        public MapsRatingsHandler(
            EventsHandler eventsHandler,

            MapsLoadingHandler mapsLoadingHandler,
            MapCreatorHandler mapsCreatorHandler,
            TDSDbContext dbContext,
            ILoggingHandler loggingHandler,
            ITDSPlayerHandler tdsPlayerHandler)
            : base(dbContext, loggingHandler)
        {
            _mapsLoadingHandler = mapsLoadingHandler;
            _mapsCreatingHandler = mapsCreatorHandler;

            eventsHandler.PlayerLoggedIn += SendPlayerHisRatings;

            NAPI.ClientEvent.Register<ITDSPlayer, int, int>(ToServerEvent.SendMapRating, this, AddPlayerMapRating);
        }

        public async void AddPlayerMapRating(ITDSPlayer player, int mapId, int rating)
        {
            if (!player.LoggedIn)
                return;
            int playerId = player.Entity!.Id;

            MapDto? map = _mapsLoadingHandler.GetMapById(mapId);
            if (map is null)
                return;

            PlayerMapRatings? maprating = await ExecuteForDBAsync(async dbContext => await dbContext.PlayerMapRatings.FindAsync(playerId, mapId));
            if (maprating is null)
            {
                maprating = new PlayerMapRatings { PlayerId = playerId, MapId = mapId };
                await ExecuteForDB(dbContext => dbContext.PlayerMapRatings.Add(maprating));
                player.Challenges.AddToChallenge(ChallengeType.ReviewMaps);
            }
            maprating.Rating = (byte)rating;
            map.BrowserSyncedData.Rating = (byte)rating;

            await ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync());

            map.Ratings.Add(maprating);
            map.RatingAverage = map.Ratings.Average(r => r.Rating);

            if (map.Info.IsNewMap)
                NAPI.Task.Run(() => _mapsCreatingHandler.AddedMapRating(map));
        }

        public void SendPlayerHisRatings(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;
            if (!player.Entity.PlayerMapRatings.Any())
                return;

            var ratingsDict = player.Entity.PlayerMapRatings.ToDictionary(r => r.MapId, r => r.Rating);
            NAPI.Task.Run(() => player.TriggerEvent(ToClientEvent.LoadOwnMapRatings, Serializer.ToBrowser(ratingsDict)));
        }
    }
}
