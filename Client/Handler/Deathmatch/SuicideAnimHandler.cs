using System;
using System.Collections.Generic;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Handler.Events;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Deathmatch
{
    public class SuicideAnimHandler : ServiceBase
    {
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly UtilsHandler _utilsHandler;
        private string _animName;
        private float _animTime;
        private bool _shotFired;

        public SuicideAnimHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender, UtilsHandler utilsHandler)
            : base(loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _utilsHandler = utilsHandler;

            Add(ToClientEvent.ApplySuicideAnimation, OnApplySuicideAnimationMethod);
        }

        public void ApplyAnimation(ITDSPlayer player, string animName, float animTime)
        {
            try
            {
                Logging.LogWarning("ApplyAnimation 1");
                player.TaskPlayAnim("MP_SUICIDE", animName, 8f, 0, -1, 0, 0, false, false, false);
                Logging.LogWarning("ApplyAnimation 2");

                if (player != RAGE.Elements.Player.LocalPlayer)
                    return;

                _animName = animName;
                _animTime = animTime;
                _shotFired = false;
                Tick += OnRender;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, "ApplyAnimation");
            }
        }

        private void OnApplySuicideAnimationMethod(object[] args)
        {
            ushort playerHandle = Convert.ToUInt16(args[0]);
            string animName = (string)args[1];
            float animTime = Convert.ToSingle(args[2]);

            ITDSPlayer player = _utilsHandler.GetPlayerByHandleValue(playerHandle);
            if (player == null)
                return;

            ApplyAnimation(player, animName, animTime);
        }

        private void OnRender(List<TickNametagData> _)
        {
            if (!RAGE.Elements.Player.LocalPlayer.IsPlayingAnim("MP_SUICIDE", _animName, 3))
            {
                Tick -= OnRender;
                return;
            }

            if (_animName == "PISTOL" && !_shotFired && RAGE.Elements.Player.LocalPlayer.HasAnimEventFired(RAGE.Game.Misc.GetHashKey("Fire")))
            {
                _shotFired = true;
                _remoteEventsSender.Send(ToServerEvent.SuicideShoot);
            }

            if (RAGE.Elements.Player.LocalPlayer.GetAnimCurrentTime("MP_SUICIDE", _animName) >= _animTime)
            {
                Tick -= OnRender;
                _remoteEventsSender.Send(ToServerEvent.SuicideKill);
            }
        }
    }
}
