using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw.Dx;
using TDS_Shared.Core;

namespace TDS_Client.Handler
{
    public class TimerHandler
    {
        public int ElapsedMs;

        private readonly IModAPI _modAPI;

        public TimerHandler(IModAPI modAPI, DxHandler dxHandler)
        {
            _modAPI = modAPI;
            ElapsedMs = _modAPI.Misc.GetGameTimer();

            TDSTimer.Init(modAPI.Chat.Output, () => _modAPI.Misc.GetGameTimer());
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(_ => TDSTimer.OnUpdateFunc()));
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(_ => RefreshElapsedMs()));

            new TDSTimer(dxHandler.RefreshResolution, 10000, 0);
        }

        private void RefreshElapsedMs()
        {
            ElapsedMs = _modAPI.Misc.GetGameTimer();
        }
    }
}
