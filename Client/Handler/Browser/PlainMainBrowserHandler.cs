using RAGE.Elements;
using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler.Browser
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

            OnPlayerStartTalking += EventHandler_PlayerStartTalking;
            OnPlayerStopTalking += EventHandler_PlayerStopTalking;
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

        public void StartPlayerTalking(string name)
        {
            ExecuteFast("e", name);
        }

        public void StopBombTick()
        {
            ExecuteStr("stopBombTickSound()");
        }

        public void StopPlayerTalking(string name)
        {
            ExecuteFast("f", name);
        }

        private void EventHandler_PlayerStartTalking(Player player)
        {
            StartPlayerTalking(player.Name);
        }

        private void EventHandler_PlayerStopTalking(Player player)
        {
            StopPlayerTalking(player.Name);
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