using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.PlayerHandlers
{
    public class TDSPlayerHandler : ITDSPlayerHandler
    {
        private readonly ILoggingHandler _loggingHandler;
        private readonly NameCheckHelper _nameCheckHelper;
        private readonly ConcurrentDictionary<ushort, ITDSPlayer> _tdsPlayerRemoteIdCache = new ConcurrentDictionary<ushort, ITDSPlayer>();

        public TDSPlayerHandler(
            NameCheckHelper nameCheckHelper,
            EventsHandler eventsHandler,
            ILoggingHandler loggingHandler)
        {
            _nameCheckHelper = nameCheckHelper;
            _loggingHandler = loggingHandler;

            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.LanguageChange, this, OnLanguageChange);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.WeaponShot, this, OnWeaponShot);

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOutBefore += EventsHandler_PlayerLoggedOutBefore;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOutAfter;
            eventsHandler.Minute += UpdatePlayers;
        }

        public int AmountLoggedInPlayers => LoggedInPlayers.Count;
        public ICollection<ITDSPlayer> LoggedInPlayers => _tdsPlayerRemoteIdCache.Values;

        public ITDSPlayer? GetPlayer(int playerId)
        {
            return _tdsPlayerRemoteIdCache.Values.FirstOrDefault(p => p.Id == playerId);
        }

        public ITDSPlayer? GetIfLoggedIn(ushort remoteId)
        {
            _tdsPlayerRemoteIdCache.TryGetValue(remoteId, out ITDSPlayer? player);
            return player;
        }

        public ITDSPlayer? FindTDSPlayer(string name)
        {
            var suffix = SharedConstants.ServerTeamSuffix.Trim();
            if (name.StartsWith(suffix))
                name = name.Substring(suffix.Length);
            name = name.Trim();

            foreach (var player in _tdsPlayerRemoteIdCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.EqualsName))
                    return player;
            }

            foreach (var player in _tdsPlayerRemoteIdCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.ContainsName))
                    return player;
            }

            foreach (var player in _tdsPlayerRemoteIdCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.EqualsScName))
                    return player;
            }

            foreach (var player in _tdsPlayerRemoteIdCache.Values)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.ContainsScName))
                    return player;
            }

            return null;
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            lock (_tdsPlayerRemoteIdCache) 
                _tdsPlayerRemoteIdCache[player.RemoteId] = player;
        }

        private void EventsHandler_PlayerLoggedOutAfter(ITDSPlayer player)
        {
            _tdsPlayerRemoteIdCache.TryRemove(player.RemoteId, out _);
        }

        private ValueTask EventsHandler_PlayerLoggedOutBefore(ITDSPlayer player)
        {
            return player.DatabaseHandler.SaveData(true);
        }

        private void OnLanguageChange(ITDSPlayer player, int language)
        {
            if (!Enum.IsDefined(typeof(Language), language))
                return;

            player.LanguageHandler.EnumValue = (Language)language;
        }

        private void OnWeaponShot(ITDSPlayer player)
        {
            player.WeaponStats.AddWeaponShot(player.CurrentWeapon);
        }

        private void ReduceMuteTime(ITDSPlayer player)
        {
            if (!player.MuteHandler.MuteTime.HasValue || player.MuteHandler.MuteTime == 0)
                return;

            if (--player.MuteHandler.MuteTime != 0)
                return;

            player.MuteHandler.MuteTime = null;
            player.SendNotification(player.Language.MUTE_EXPIRED);
        }

        private void ReduceVoiceMuteTime(ITDSPlayer player)
        {
            if (!player.MuteHandler.VoiceMuteTime.HasValue || player.MuteHandler.VoiceMuteTime == 0)
                return;

            if (--player.MuteHandler.VoiceMuteTime != 0)
                return;

            player.MuteHandler.VoiceMuteTime = null;
            NAPI.Task.RunSafe(() => player.SendNotification(player.Language.VOICE_MUTE_EXPIRED));

            if (player.Team is null || player.Team.IsSpectator)
                return;

            player.Team.Players.DoInMain(target =>
            {
                if (!target.Relations.HasRelationTo(player, PlayerRelation.Block))
                    player.Voice.SetVoiceTo(target, true);
            });
        }

        private void UpdatePlayers(int _)
        {
            foreach (var player in LoggedInPlayers)
            {
                try
                {
                    ++player.PlayTime.Minutes;
                    ReduceMuteTime(player);
                    ReduceVoiceMuteTime(player);
                    player.MapsVoting.CheckReduceMapBoughtCounter();

                    player.DatabaseHandler.CheckSaveData();
                }
                catch (Exception ex)
                {
                    _loggingHandler.LogError(ex, player);
                }
            }
        }
    }
}
