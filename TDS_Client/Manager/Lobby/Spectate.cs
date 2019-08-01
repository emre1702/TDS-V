using RAGE.Elements;
using System;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

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
                    CameraManager.SpectateCam.Activate();
                else if (value == null)
                    CameraManager.SpectateCam.Deactivate();

                _spectatingEntity = value;

                if (value != null)
                    CameraManager.SpectateCam.Spectate(value);
                    
                 CameraManager.SpectateCam.Render(true, ClientConstants.DefaultSpectatePlayerChangeEaseTime);  
            }
        }

        private static PedBase _spectatingEntity;
        private static bool _binded;

        private static void Next(ConsoleKey _)
        {
            EventsSender.Send(DToServerEvent.SpectateNext, true);
        }

        private static void Previous(ConsoleKey _)
        {
            EventsSender.Send(DToServerEvent.SpectateNext, false);
        }

        public static void Start()
        {
            if (_binded)
                return;
            _binded = true;

            BindManager.Add(ConsoleKey.RightArrow, Next);
            BindManager.Add(ConsoleKey.D, Next);
            BindManager.Add(ConsoleKey.LeftArrow, Previous);
            BindManager.Add(ConsoleKey.A, Previous);
        }

        public static void Stop()
        {
            if (!_binded)
                return;
            _binded = false;

            BindManager.Remove(ConsoleKey.RightArrow, Next);
            BindManager.Remove(ConsoleKey.D, Next);
            BindManager.Remove(ConsoleKey.LeftArrow, Previous);
            BindManager.Remove(ConsoleKey.A, Previous);
        }
    }
}