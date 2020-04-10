using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class PlainMainBrowserHandler : BrowserHandlerBase
    {
        private bool _roundEndReasonShowing;

        public PlainMainBrowserHandler(IModAPI modAPI, Serializer serializer) 
            : base(modAPI, serializer, Constants.MainBrowserPath)
        {
            CreateBrowser();
            //SetReady(); Only for Angular
        }


        public void OnLoadOwnMapRatings(string datajson)
        {
            ExecuteStr($"loadMyMapRatings(`{datajson}`);");
        }

        public void ShowBloodscreen()
        {
            ExecuteFast("c");
        }

        public void PlaySound(string soundname)
        {
            ExecuteFast("a", soundname);
        }

        public void PlayHitsound()
        {
            ExecuteFast("b");
        }

        public void AddKillMessage(string msg)
        {
            ExecuteFast("d", msg);
        }

        public void SendAlert(string msg)
        {
            ExecuteStr($"alert('{msg}');");
        }

        public void ShowRoundEndReason(string reason, int mapId)
        {
            _roundEndReasonShowing = true;
            ExecuteStr($"showRoundEndReason(`{reason}`, {mapId});");
        }

        public void HidRoundEndReason()
        {
            if (!_roundEndReasonShowing)
                return;
            ExecuteStr("hidRoundEndReason();");
            _roundEndReasonShowing = false;
        }

        public void StartBombTick(int msToDetonate, int startAtMs)
        {
            ExecuteStr($"startBombTickSound({msToDetonate}, {startAtMs})");
        }

        public void StopBombTick()
        {
            ExecuteStr("stopBombTickSound()");
        }

        public void StartPlayerTalking(string name)
        {
            ExecuteFast("e", name);
        }

        public void StopPlayerTalking(string name)
        {
            ExecuteFast("f", name);
        }
    }
}
