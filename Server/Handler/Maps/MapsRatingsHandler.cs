using AltV.Net.Async;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Maps
{
    public class MapsRatingsHandler : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly MapCreatorHandler _mapsCreatingHandler;
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly Serializer _serializer;

        #endregion Private Fields

        #region Public Constructors

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

            AltAsync.OnClient<ITDSPlayer, int, int, Task>(ToServerEvent.SendMapRating, AddPlayerMapRating);
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task AddPlayerMapRating(ITDSPlayer player, int mapId, int rating)
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
            maprating.Rating = (byte)rating;
            map.BrowserSyncedData.Rating = (byte)rating;

            await ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync());

            map.Ratings.Add(maprating);
            map.RatingAverage = map.Ratings.Average(r => r.Rating);

            if (map.Info.IsNewMap)
                await AltAsync.Do(() => _mapsCreatingHandler.AddedMapRating(map));
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

        #endregion Public Methods
    }
}
