using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Player
{
    public class TDSPlayerHandler
    {
        #region Private Fields

        private readonly ILoggingHandler _loggingHandler;
        private readonly NameCheckHelper _nameCheckHelper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<IPlayer, ITDSPlayer> _tdsPlayerCache = new ConcurrentDictionary<IPlayer, ITDSPlayer>();
        private readonly ConcurrentDictionary<ushort, ITDSPlayer> _tdsPlayerRemoteIdCache = new ConcurrentDictionary<ushort, ITDSPlayer>();

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public int AmountLoggedInPlayers => LoggedInPlayers.Count;
        public ICollection<ITDSPlayer> LoggedInPlayers => _tdsPlayerCache.Values;

        #endregion Public Properties

        #region Public Methods

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

        public ITDSPlayer? GetIfExists(int playerId)
        {
            return _tdsPlayerCache.Values.FirstOrDefault(p => p.Id == playerId);
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

        public ITDSPlayer GetNotLoggedIn(IPlayer modPlayer)
        {
            if (!_tdsPlayerCache.TryGetValue(modPlayer, out ITDSPlayer? tdsPlayer) || tdsPlayer.LoggedIn)
            {
                tdsPlayer = ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider);
                tdsPlayer.ModPlayer = modPlayer;
                _tdsPlayerCache[modPlayer] = tdsPlayer;
            }

            return tdsPlayer;
        }

        #endregion Public Methods

        #region Internal Methods

        internal ITDSPlayer? FindTDSPlayer(string name)
        {
            var suffix = SharedConstants.ServerTeamSuffix.Trim();
            if (name.StartsWith(suffix))
                name = name.Substring(suffix.Length);
            name = name.Trim();

            foreach (var player in _tdsPlayerCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.EqualsName))
                    return player;
            }

            foreach (var player in _tdsPlayerCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.ContainsName))
                    return player;
            }

            foreach (var player in _tdsPlayerCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.EqualsScName))
                    return player;
            }

            foreach (var player in _tdsPlayerCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.ContainsScName))
                    return player;
            }

            return null;
        }

        #endregion Internal Methods

        #region Private Methods

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            _tdsPlayerRemoteIdCache[player.RemoteId] = player;
        }

        private void EventsHandler_PlayerLoggedOutAfter(ITDSPlayer player)
        {
            if (player.ModPlayer is { })
                _tdsPlayerCache.TryRemove(player.ModPlayer, out _);
            _tdsPlayerRemoteIdCache.TryRemove(player.RemoteId, out _);
        }

        private ValueTask EventsHandler_PlayerLoggedOutBefore(ITDSPlayer player)
        {
            return player.SaveData(true);
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

        #endregion Private Methods
    }
}
