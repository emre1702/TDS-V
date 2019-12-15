using RAGE.Game;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Utility;
using TDS_Common.Enum;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        public EventsHandler()
        {
            LoadOnStart();
            AddRAGEEvents();
            AddFromServerEvents();
            AddFromBrowserEvents();
            AddWorkaroundEvents();
        }

        private void LoadOnStart()
        {
            Settings.LoadLanguageFromRAGE();
            ClientUtils.Init();
            Dx.RefreshResolution();
            CameraManager.Init();
            Streaming.RequestNamedPtfxAsset("scr_xs_celebration");
            Misc.SetWeatherTypeNowPersist("CLEAR");
            Misc.SetWind(0);
            Streaming.RequestAnimDict("MP_SUICIDE");
            Audio.SetAudioFlag("LoadMPData", true);

            // CLEAR_GPS_CUSTOM_ROUTE
            Invoker.Invoke(0xE6DE0561D9232A64);
        }
    }
}