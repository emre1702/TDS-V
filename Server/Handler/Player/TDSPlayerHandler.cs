using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Enums;
using TDS_Shared.Core;
using System.Collections.Concurrent;

namespace TDS_Server.Handler.Player
{
    public class TDSPlayerHandler
    {
        public ICollection<ITDSPlayer> LoggedInPlayers => _tdsPlayerCache.Values;
        public int AmountLoggedInPlayers => LoggedInPlayers.Count;

        private readonly ConcurrentDictionary<IPlayer, ITDSPlayer> _tdsPlayerCache = new ConcurrentDictionary<IPlayer, ITDSPlayer>();
        private readonly ConcurrentDictionary<ushort, ITDSPlayer> _tdsPlayerRemoteIdCache = new ConcurrentDictionary<ushort, ITDSPlayer>();
        private readonly NameCheckHelper _nameCheckHelper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggingHandler _loggingHandler;

        public TDSPlayerHandler(
            NameCheckHelper nameCheckHelper, 
            IServiceProvider serviceProvider, 
            EventsHandler eventsHandler,
            ILoggingHandler loggingHandler)
        {
            _nameCheckHelper = nameCheckHelper;
            _serviceProvider = serviceProvider;
            _loggingHandler = loggingHandler;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOutBefore += EventsHandler_PlayerLoggedOutBefore;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOutAfter;
            eventsHandler.Minute += UpdatePlayers;
        }

        public ITDSPlayer Get(IPlayer modPlayer)
        {
            if (!_tdsPlayerCache.TryGetValue(modPlayer, out ITDSPlayer? tdsPlayer))
            {
                tdsPlayer = ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider);
                tdsPlayer.ModPlayer = modPlayer;
                _tdsPlayerCache[modPlayer] = tdsPlayer;
            }

            return tdsPlayer;
        }

        public ITDSPlayer GetNew(IPlayer modPlayer)
        {
            ITDSPlayer tdsPlayer = ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider);
            tdsPlayer.ModPlayer = modPlayer;
            _tdsPlayerCache[modPlayer] = tdsPlayer;

            return tdsPlayer;
        }

        public ITDSPlayer? GetIfLoggedIn(IPlayer modPlayer)
        {
            if (!_tdsPlayerCache.ContainsKey(modPlayer))
                return null;

            var player = _tdsPlayerCache[modPlayer];
            if (!player.LoggedIn)
                return null;

            return player;
        }

        public ITDSPlayer? GetIfLoggedIn(ushort remoteId)
        {
            _tdsPlayerRemoteIdCache.TryGetValue(remoteId, out ITDSPlayer? player);
            return player;
        }

        public ITDSPlayer? GetIfExists(int playerId)
        {
            return _tdsPlayerCache.Values.FirstOrDefault(p => p.Id == playerId);
        }

        internal ITDSPlayer? FindTDSPlayer(string name)
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

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            _tdsPlayerRemoteIdCache[player.RemoteId] = player;
        }

        private ValueTask EventsHandler_PlayerLoggedOutBefore(ITDSPlayer player)
        {
            return player.SaveData(true);
        }

        private void EventsHandler_PlayerLoggedOutAfter(ITDSPlayer player)
        {
            if (player.ModPlayer is { })
                _tdsPlayerCache.TryRemove(player.ModPlayer, out _);
            _tdsPlayerRemoteIdCache.TryRemove(player.RemoteId, out _);
        }

        private void UpdatePlayers(int _)
        {
            foreach (var player in LoggedInPlayers)
            {
                try
                {
                    ++player.PlayMinutes;
                    ReduceMuteTime(player);
                    ReduceVoiceMuteTime(player);
                    player.CheckReduceMapBoughtCounter();

                    player.CheckSaveData();
                }
                catch (Exception ex)
                {
                    _loggingHandler.LogError(ex, player);
                }
            }
        }

        private void ReduceMuteTime(ITDSPlayer player)
        {
            if (!player.MuteTime.HasValue || player.MuteTime == 0)
                return;

            if (--player.MuteTime != 0)
                return;

            player.MuteTime = null;
            player.SendNotification(player.Language.MUTE_EXPIRED);
        }

        private void ReduceVoiceMuteTime(ITDSPlayer player)
        {
            if (!player.VoiceMuteTime.HasValue || player.VoiceMuteTime == 0)
                return;

            if (--player.VoiceMuteTime != 0)
                return;

            player.VoiceMuteTime = null;
            player.SendNotification(player.Language.VOICE_MUTE_EXPIRED);

            if (player.Team is null || player.Team.IsSpectator)
                return;

            foreach (var target in player.Team.Players)
            {
                if (!target.HasRelationTo(player, PlayerRelation.Block))
                    player.SetVoiceTo(target, true);
            }
        }
    }
}
