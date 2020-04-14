﻿using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw.Dx;
using TDS_Shared.Core;

namespace TDS_Client.Handler
{
    public class TimerHandler : ServiceBase
    {
        public int ElapsedMs;

        public TimerHandler(IModAPI modAPI, LoggingHandler loggingHandler, DxHandler dxHandler)
            : base(modAPI, loggingHandler)
        {
            ElapsedMs = ModAPI.Misc.GetGameTimer();

            TDSTimer.Init(modAPI.Chat.Output, () => ModAPI.Misc.GetGameTimer());
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(_ => TDSTimer.OnUpdateFunc()));
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(_ => RefreshElapsedMs()));

            new TDSTimer(dxHandler.RefreshResolution, 10000, 0);
        }

        private void RefreshElapsedMs()
        {
            ElapsedMs = ModAPI.Misc.GetGameTimer();
        }
    }
}