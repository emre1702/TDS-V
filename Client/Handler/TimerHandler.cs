using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw.Dx;
using TDS_Shared.Core;

namespace TDS_Client.Manager.Utility
{
    public class TimerHandler
    {
        private ulong ElapsedMs => TDSTimer.ElapsedMs;

        public TimerHandler(IModAPI modAPI, DxHandler dxHandler)
        {
            TDSTimer.Init(modAPI.Chat.Output);
            modAPI.Event.Tick.Add(new EventMethodData<Action>(TDSTimer.OnUpdateFunc));

            new TDSTimer(dxHandler.RefreshResolution, 10000, 0);
        }
    }
}
