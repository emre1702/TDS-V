using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Entity.Player;
using TDS_Server.Handler.Helper;

namespace TDS_Server.Handler.Player
{
    public class TDSPlayerHandler
    {
        private readonly Dictionary<ulong, ITDSPlayer> _tdsPlayerCache = new Dictionary<ulong, ITDSPlayer>();
        private readonly Queue<(ulong, long)> _removeFromCacheAtTimeQueue = new Queue<(ulong, long)>();
        private readonly NameCheckHelper _nameCheckHelper;

        public TDSPlayerHandler(NameCheckHelper nameCheckHelper)
            => _nameCheckHelper = nameCheckHelper;

        public ITDSPlayer GetTDSPlayer(IPlayer modPlayer)
        {
            var playerId = modPlayer.SocialClubId;

            if (!_tdsPlayerCache.ContainsKey(playerId))
            {
                var tdsPlayer = new TDSPlayer(modPlayer);
                _tdsPlayerCache[playerId] = tdsPlayer;
            }

            return _tdsPlayerCache[playerId];
        }

        //Todo: Add in Minute Timer
        private void RemoveOldCaches()
        {
            if (_removeFromCacheAtTimeQueue.Count == 0)
                return;

            var ticksNow = DateTime.Now.Ticks;
            foreach (var entry in _removeFromCacheAtTimeQueue)
            {
                if (ticksNow > entry.Item2)
                    break;
                _tdsPlayerCache.Remove(entry.Item1);
            }
            _removeFromCacheAtTimeQueue.Clear();
        }

        internal ITDSPlayer? FindTDSPlayer(string name)
        {
            foreach (var player in _tdsPlayerCache.Values)
            {
                ;
            }
        }

        private static bool IsNameValid
    }
}
