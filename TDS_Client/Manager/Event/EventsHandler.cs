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
            Dx.RefreshResolution();
            CameraManager.Init();
            Streaming.RequestNamedPtfxAsset("scr_xs_celebration");
            Misc.SetWeatherTypeNowPersist("CLEAR");
            Misc.SetWind(0);

            // CLEAR_GPS_CUSTOM_ROUTE
            RAGE.Game.Invoker.Invoke(0xE6DE0561D9232A64);
        }

        public static void OnLocalPlayerDataChange(EPlayerDataKey key, object obj)
        {
            switch (key)
            {
                case EPlayerDataKey.Money:
                    Stats.StatSetInt(Misc.GetHashKey("SP0_TOTAL_CASH"), (int)obj, false);
                    break;
                case EPlayerDataKey.AdminLevel:
                    if (Browser.Angular.Main.Browser == null)
                        Browser.Angular.Main.Start((int)obj);
                    else
                        Browser.Angular.Main.RefreshAdminLevel((int)obj);
                    break;
                case EPlayerDataKey.LoggedIn:
                    TickManager.Add(() => Ui.ShowHudComponentThisFrame((int)HudComponent.Cash));
                    break;
            }  
        }
    }
}