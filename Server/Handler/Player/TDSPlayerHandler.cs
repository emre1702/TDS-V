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
using TDS_Shared.Manager.Utility;
using static System.Collections.Generic.Dictionary<ulong, TDS_Server.Data.Interfaces.ITDSPlayer>;

namespace TDS_Server.Handler.Player
{
    public class TDSPlayerHandler
    {
        public ValueCollection LoggedInPlayers => _tdsPlayerCache.Values;
        public int AmountLoggedInPlayers => LoggedInPlayers.Count;

        private readonly Dictionary<ulong, ITDSPlayer> _tdsPlayerCache = new Dictionary<ulong, ITDSPlayer>();
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

            eventsHandler.PlayerLoggedOutBefore += EventsHandler_PlayerLoggedOutBefore; ;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOutAfter;
            eventsHandler.Minute += UpdatePlayers;
        }

        public ITDSPlayer Get(IPlayer modPlayer)
        {
            var playerId = modPlayer.SocialClubId;

            if (!_tdsPlayerCache.ContainsKey(playerId))
            {
                var tdsPlayer = ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider, modPlayer);
                _tdsPlayerCache[playerId] = tdsPlayer;
            }

            return _tdsPlayerCache[playerId];
        }

        public ITDSPlayer? GetIfLoggedIn(IPlayer modPlayer)
        {
            var playerId = modPlayer.SocialClubId;

            if (!_tdsPlayerCache.ContainsKey(playerId))
                return null;

            var player = _tdsPlayerCache[playerId];
            if (!player.LoggedIn)
                return null;

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

        private ValueTask EventsHandler_PlayerLoggedOutBefore(ITDSPlayer player)
        {
            return player.SaveData(true);
        }

        private void EventsHandler_PlayerLoggedOutAfter(ITDSPlayer player)
        {
            _tdsPlayerCache.Remove(player.SocialClubId);
        }

        private void UpdatePlayers(ulong _)
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
