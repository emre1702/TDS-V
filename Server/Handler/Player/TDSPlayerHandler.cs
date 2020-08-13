using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Handlers;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Player
{
    public class TDSPlayerHandler : ITDSPlayerHandler
    {
        #region Fields

        private readonly ILoggingHandler _loggingHandler;
        private readonly NameCheckHelper _nameCheckHelper;

        #endregion Fields

        #region Constructors

        public TDSPlayerHandler(
            NameCheckHelper nameCheckHelper,
            EventsHandler eventsHandler,
            ILoggingHandler loggingHandler)
        {
            _nameCheckHelper = nameCheckHelper;
            _loggingHandler = loggingHandler;

            Alt.OnClient<ITDSPlayer, int>(ToServerEvent.LanguageChange, OnLanguageChange);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.WeaponShot, OnWeaponShot);

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerLoggedOutBefore += EventsHandler_PlayerLoggedOutBefore;
            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOutAfter;
            eventsHandler.Minute += UpdatePlayers;
        }

        #endregion Constructors

        #region Properties

        public List<ITDSPlayer> LoggedInPlayers { get; set; } = new List<ITDSPlayer>();

        #endregion Properties

        #region Methods

        public ITDSPlayer? FindTDSPlayer(string name)
        {
            var suffix = SharedConstants.ServerTeamSuffix.Trim();
            if (name.StartsWith(suffix))
                name = name.Substring(suffix.Length);
            name = name.Trim();

            foreach (var player in LoggedInPlayers)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.EqualsName))
                    return player;
            }

            foreach (var player in LoggedInPlayers)
            {
                if (_nameCheckHelper.IsName(player, name, IsNameCheckLevel.ContainsName))
                    return player;
            }

            return null;
        }

        public ITDSPlayer? Get(int playerId)
        {
            return LoggedInPlayers.FirstOrDefault(p => p.Id == playerId);
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            LoggedInPlayers.Add(player);
            player.LoggedIn = true;
        }

        private void EventsHandler_PlayerLoggedOutAfter(ITDSPlayer player)
        {
            if (player.LoggedIn)
                LoggedInPlayers.Remove(player);
            player.LoggedIn = false;
        }

        private ValueTask EventsHandler_PlayerLoggedOutBefore(ITDSPlayer player)
        {
            return player.SaveData(true);
        }

        private void OnLanguageChange(ITDSPlayer player, int language)
        {
            if (!Enum.IsDefined(typeof(Language), language))
                return;

            player.LanguageEnum = (Language)language;
        }

        private void OnWeaponShot(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            player.AddWeaponShot(player.CurrentWeapon, null, null, false);
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

        #endregion Methods
    }
}
