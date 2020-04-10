using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Deathmatch
{
    public class SuicideAnimHandler
    {
        private bool _shotFired;
        private string _animName;
        private float _animTime;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly IModAPI _modAPI;
        private readonly RemoteEventsSender _remoteEventsSender;

        public SuicideAnimHandler(IModAPI modAPI, RemoteEventsSender remoteEventsSender)
        {
            _tickEventMethod = new EventMethodData<TickDelegate>(OnRender);

            _modAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;
        }

        public void ApplyAnimation(IPlayer player, string animName, float animTime)
        {
            player.TaskPlayAnim("MP_SUICIDE", animName, 8f, 0, -1, 0, 0, false, false, false);

            if (player != _modAPI.LocalPlayer)
                return;

            _animName = animName;
            _animTime = animTime;
            _shotFired = false;
            _modAPI.Event.Tick.Add(_tickEventMethod);
        }

        private void OnRender(ulong _)
        {
            if (!_modAPI.LocalPlayer.IsPlayingAnim("MP_SUICIDE", _animName, 3))
            {
                _modAPI.Event.Tick.Remove(_tickEventMethod);
                return;
            }

            if (_animName == "PISTOL" && !_shotFired && _modAPI.LocalPlayer.HasAnimEventFired(_modAPI.Misc.GetHashKey("Fire")))
            {
                _shotFired = true;
                _remoteEventsSender.Send(ToServerEvent.SuicideShoot);
            }

            if (_modAPI.LocalPlayer.GetAnimCurrentTime("MP_SUICIDE", _animName) >= _animTime)
            {
                _modAPI.Event.Tick.Remove(_tickEventMethod);
                _remoteEventsSender.Send(ToServerEvent.SuicideKill);
            }
        }
    }
}
