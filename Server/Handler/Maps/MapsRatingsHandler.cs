using GTANetworkAPI;
using System;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models.Map;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.Challenge;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Maps
{
    public class MapsRatingsHandler : DatabaseEntityWrapper
    {
        private readonly MapCreatorHandler _mapsCreatingHandler;
        private readonly MapsLoadingHandler _mapsLoadingHandler;

        public MapsRatingsHandler(
            EventsHandler eventsHandler,

            MapsLoadingHandler mapsLoadingHandler,
            MapCreatorHandler mapsCreatorHandler,
            TDSDbContext dbContext)
            : base(dbContext)
        {
            _mapsLoadingHandler = mapsLoadingHandler;
            _mapsCreatingHandler = mapsCreatorHandler;

            eventsHandler.PlayerLoggedIn += SendPlayerHisRatings;

            NAPI.ClientEvent.Register<ITDSPlayer, int, int>(ToServerEvent.SendMapRating, this, AddPlayerMapRating);
        }

        public async void AddPlayerMapRating(ITDSPlayer player, int mapId, int rating)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                int playerId = player.Entity!.Id;

                MapDto? map = _mapsLoadingHandler.GetMapById(mapId);
                if (map is null)
                    return;

                PlayerMapRatings? maprating = await ExecuteForDBAsync(async dbContext => 
                    await dbContext.PlayerMapRatings
                        .FindAsync(playerId, mapId)
                        .ConfigureAwait(false))
                    .ConfigureAwait(false);
                if (maprating is null)
                {
                    maprating = new PlayerMapRatings { PlayerId = playerId, MapId = mapId };
                    await ExecuteForDB(dbContext => dbContext.PlayerMapRatings.Add(maprating)).ConfigureAwait(false);
                    player.Challenges.AddToChallenge(ChallengeType.ReviewMaps);
                }
                maprating.Rating = (byte)rating;
                map.BrowserSyncedData.Rating = (byte)rating;

                await ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync().ConfigureAwait(false)).ConfigureAwait(false);

                lock (map.Ratings)
                {
                    map.Ratings.Add(maprating);
                }

                map.RatingAverage = map.Ratings.Average(r => r.Rating);

                if (map.Info.IsNewMap)
                    NAPI.Task.RunSafe(() => _mapsCreatingHandler.AddedMapRating(map));
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void SendPlayerHisRatings(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;
            if (!player.Entity.PlayerMapRatings.Any())
                return;

            var ratingsDict = player.Entity.PlayerMapRatings.ToDictionary(r => r.MapId, r => r.Rating);
            NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.LoadOwnMapRatings, Serializer.ToBrowser(ratingsDict)));
        }
    }
}
