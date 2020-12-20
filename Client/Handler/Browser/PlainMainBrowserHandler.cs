using RAGE.Elements;
using System;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Models;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Browser
{
    public class PlainMainBrowserHandler : BrowserHandlerBase
    {
        private readonly RemoteEventsSender _remoteEventsSender;
        private bool _roundEndReasonShowing;

        public PlainMainBrowserHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender, EventsHandler eventsHandler)
            : base(loggingHandler, Constants.MainBrowserPath)
        {
            _remoteEventsSender = remoteEventsSender;

            CreateBrowser();
            //SetReady(); Only for Angular

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.MapChanged += HideRoundEndReason;
            eventsHandler.RoundStarted += _ => HideRoundEndReason();
            eventsHandler.CountdownStarted += _ => HideRoundEndReason();

            Add(FromBrowserEvent.SendMapRating, OnBrowserSendMapRatingMethod);
            Add(ToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(ToClientEvent.PlayCustomSound, OnPlayCustomSoundMethod);
        }

        public override void CreateBrowser()
        {
            base.CreateBrowser();
            Browser.ExecuteJs($"mp.trigger('{FromBrowserEvent.Created}', 'PlainMain')");
        }

        public void HideRoundEndReason()
        {
            try
            {
                if (!_roundEndReasonShowing)
                    return;
                ExecuteStr("hideRoundEndReason();");
                _roundEndReasonShowing = false;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void OnLoadOwnMapRatings(string datajson)
        {
            ExecuteStr($"loadMyMapRatings(`{datajson}`);");
        }

        public void PlayHitsound()
        {
            ExecuteFast("b");
        }

        public void PlaySound(string soundname)
        {
            ExecuteFast("a", soundname);
        }

        public void SendAlert(string msg)
        {
            ExecuteStr($"alert('{msg}');");
        }

        public void ShowBloodscreen()
        {
            ExecuteFast("c");
        }

        public void ShowRoundEndReason(string reason, int mapId)
        {
            _roundEndReasonShowing = true;
            ExecuteStr($"showRoundEndReason(`{reason}`, {mapId});");
        }

        public void StartBombTick(int msToDetonate, int startAtMs)
        {
            ExecuteStr($"startBombTickSound({msToDetonate}, {startAtMs})");
        }

        public void StopBombTick()
        {
            ExecuteStr("stopBombTickSound()");
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            HideRoundEndReason();
        }

        private void OnBrowserSendMapRatingMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            int rating = Convert.ToInt32(args[1]);
            _remoteEventsSender.Send(ToServerEvent.SendMapRating, mapId, rating);
        }

        private void OnLoadOwnMapRatingsMethod(object[] args)
        {
            string datajson = (string)args[0];
            OnLoadOwnMapRatings(datajson);
        }

        private void OnPlayCustomSoundMethod(object[] args)
        {
            string soundName = (string)args[0];
            PlaySound(soundName);
        }
    }
}