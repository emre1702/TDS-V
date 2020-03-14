using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Shared.Manager.Utility;
using static System.Collections.Generic.Dictionary<ulong, TDS_Server.Handler.Entities.Player.TDSPlayer>;

namespace TDS_Server.Handler.Player
{
    public class TDSPlayerHandler
    {
        public ValueCollection LoggedInPlayers => _tdsPlayerCache.Values;

        private readonly Dictionary<ulong, TDSPlayer> _tdsPlayerCache = new Dictionary<ulong, TDSPlayer>();
        private readonly Queue<(ulong, DateTime)> _removeFromCacheAtTimeQueue = new Queue<(ulong, DateTime)>();
        private readonly NameCheckHelper _nameCheckHelper;
        private readonly IServiceProvider _serviceProvider;

        public TDSPlayerHandler(
            NameCheckHelper nameCheckHelper, 
            IServiceProvider serviceProvider, 
            EventsHandler eventsHandler)
        {
            _nameCheckHelper = nameCheckHelper;
            _serviceProvider = serviceProvider;

            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
            eventsHandler.OnMinute += RemoveOldCaches;
        }

        public TDSPlayer GetTDSPlayer(IPlayer modPlayer)
        {
            var playerId = modPlayer.SocialClubId;

            if (!_tdsPlayerCache.ContainsKey(playerId))
            {
                var tdsPlayer = ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider, modPlayer);
                _tdsPlayerCache[playerId] = tdsPlayer;
            }

            return _tdsPlayerCache[playerId];
        }

        private void RemoveOldCaches(int _)
        {
            if (_removeFromCacheAtTimeQueue.Count == 0)
                return;

            var dateNow = DateTime.Now;
            foreach (var entry in _removeFromCacheAtTimeQueue)
            {
                if (dateNow >= entry.Item2)
                    break;
                _tdsPlayerCache.Remove(entry.Item1);
            }
            _removeFromCacheAtTimeQueue.Clear();
        }

        internal TDSPlayer? FindTDSPlayer(string name)
        {
            foreach (var player in _tdsPlayerCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.Equals))
                    return player;
            }

            foreach (var player in _tdsPlayerCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.Contains))
                    return player;
            }

            return null;
        }

        private void EventsHandler_PlayerLoggedOut(TDSPlayer player)
        {
            _removeFromCacheAtTimeQueue.Enqueue((player.SocialClubId, DateTime.Now.AddMinutes(Constants.RemoveTDSPlayerMinutesAfterLoggedOut)));
        }
    }
}
