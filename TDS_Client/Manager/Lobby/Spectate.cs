using RAGE.Elements;
using System;
using TDS_Client.Enum;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Manager.Lobby
{
    internal static class Spectate
    {
        public static PedBase SpectatingEntity
        {
            get => _spectatingEntity;
            set
            {
                if (value == _spectatingEntity)
                    return;
                if (_spectatingEntity == null) 
                    Start();
                else if (value == null)
                    Stop();

                _spectatingEntity = value;

                if (value != null)
                    CameraManager.SpectateCam.Spectate(value);
                    
                CameraManager.SpectateCam.Render(true, ClientConstants.DefaultSpectatePlayerChangeEaseTime);  
             }
        }

        private static PedBase _spectatingEntity;
        private static bool _binded;

        private static void Next(EKey _)
        {
            EventsSender.Send(DToServerEvent.SpectateNext, true);
        }

        private static void Previous(EKey _)
        {
            EventsSender.Send(DToServerEvent.SpectateNext, false);
        }

        public static void Start()
        {
            if (_binded)
                return;
            _binded = true;

            Death.PlayerSpawn();
            CameraManager.SpectateCam.Activate();

            BindManager.Add(EKey.RightArrow, Next);
            BindManager.Add(EKey.D, Next);
            BindManager.Add(EKey.LeftArrow, Previous);
            BindManager.Add(EKey.A, Previous);
        }

        public static void Stop()
        {
            if (!_binded)
                return;
            _binded = false;

            CameraManager.SpectateCam.Deactivate();

            BindManager.Remove(EKey.RightArrow, Next);
            BindManager.Remove(EKey.D, Next);
            BindManager.Remove(EKey.LeftArrow, Previous);
            BindManager.Remove(EKey.A, Previous);
        }
    }
}