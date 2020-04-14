using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Deathmatch
{
    public class SuicideAnimHandler : ServiceBase
    {
        private bool _shotFired;
        private string _animName;
        private float _animTime;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly UtilsHandler _utilsHandler;

        public SuicideAnimHandler(IModAPI modAPI, LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender, UtilsHandler utilsHandler)
            : base(modAPI, loggingHandler)
        {
            _tickEventMethod = new EventMethodData<TickDelegate>(OnRender);

            _remoteEventsSender = remoteEventsSender;
            _utilsHandler = utilsHandler;

            modAPI.Event.Add(ToClientEvent.ApplySuicideAnimation, OnApplySuicideAnimationMethod);
        }

        public void ApplyAnimation(IPlayer player, string animName, float animTime)
        {
            player.TaskPlayAnim("MP_SUICIDE", animName, 8f, 0, -1, 0, 0, false, false, false);

            if (player != ModAPI.LocalPlayer)
                return;

            _animName = animName;
            _animTime = animTime;
            _shotFired = false;
            ModAPI.Event.Tick.Add(_tickEventMethod);
        }

        private void OnRender(int _)
        {
            if (!ModAPI.LocalPlayer.IsPlayingAnim("MP_SUICIDE", _animName))
            {
                ModAPI.Event.Tick.Remove(_tickEventMethod);
                return;
            }

            if (_animName == "PISTOL" && !_shotFired && ModAPI.LocalPlayer.HasAnimEventFired(ModAPI.Misc.GetHashKey("Fire")))
            {
                _shotFired = true;
                _remoteEventsSender.Send(ToServerEvent.SuicideShoot);
            }

            if (ModAPI.LocalPlayer.GetAnimCurrentTime("MP_SUICIDE", _animName) >= _animTime)
            {
                ModAPI.Event.Tick.Remove(_tickEventMethod);
                _remoteEventsSender.Send(ToServerEvent.SuicideKill);
            }
        }

        private void OnApplySuicideAnimationMethod(object[] args)
        {
            ushort playerHandle = Convert.ToUInt16(args[0]);
            string animName = (string)args[1];
            float animTime = Convert.ToSingle(args[2]);

            IPlayer player = _utilsHandler.GetPlayerByHandleValue(playerHandle);
            if (player == null)
                return;

            ApplyAnimation(player, animName, animTime);
        }
    }
}
