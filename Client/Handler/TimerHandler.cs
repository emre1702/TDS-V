using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw.Dx;
using TDS_Shared.Core;

namespace TDS_Client.Handler
{
    public class TimerHandler
    {
        public ulong ElapsedMs => TDSTimer.ElapsedMs;

        public TimerHandler(IModAPI modAPI, DxHandler dxHandler)
        {
            TDSTimer.Init(modAPI.Chat.Output);
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(_ => TDSTimer.OnUpdateFunc()));

            new TDSTimer(dxHandler.RefreshResolution, 10000, 0);
        }
    }
}
