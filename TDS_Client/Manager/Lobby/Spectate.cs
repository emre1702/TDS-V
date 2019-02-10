using RAGE;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    static class Spectate
    {
        private static bool binded;

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
            if (binded)
                return;
            binded = true;

            BindManager.Add(ConsoleKey.RightArrow, Next);
            BindManager.Add(ConsoleKey.D, Next);
            BindManager.Add(ConsoleKey.LeftArrow, Previous);
            BindManager.Add(ConsoleKey.A, Previous);
        }

        public static void Stop()
        {
            if (!binded)
                return;
            binded = false;

            BindManager.Remove(ConsoleKey.RightArrow, Next);
            BindManager.Remove(ConsoleKey.D, Next);
            BindManager.Remove(ConsoleKey.LeftArrow, Previous);
            BindManager.Remove(ConsoleKey.A, Previous);
        }
    }
}
