using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Browser
{
    public class AngularBrowserHandler : BrowserHandlerBase
    {
        private readonly EventsHandler _eventsHandler;

        public AngularBrowserHandler(LoggingHandler loggingHandler,
            EventsHandler eventsHandler)
            : base(loggingHandler, Constants.AngularMainBrowserPath)
        {
            _eventsHandler = eventsHandler;

            RAGE.Chat.SafeMode = false;
            RAGE.Chat.Show(false);

            eventsHandler.InFightStatusChanged += ToggleRoundStats;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.AngularCooldown += ShowCooldown;
            eventsHandler.RoundEnded += _ => ResetMapVoting();
            eventsHandler.ChatInputToggled += ToggleChatOpened;

            RAGE.Events.Add(FromBrowserEvent.GetHashedPassword, OnGetHashedPassword);
            RAGE.Events.Add(ToClientEvent.ToBrowserEvent, OnToBrowserEventMethod);
            RAGE.Events.Add(ToClientEvent.FromBrowserEventReturn, OnFromBrowserEventReturnMethod);

            CreateBrowser();
        }

        public override void CreateBrowser()
        {
            base.CreateBrowser();
            Browser.ExecuteJs($"mp.trigger('{FromBrowserEvent.Created}', 'Angular')");
        }

        public void AddNameForChat(string name)
        {
            ExecuteFast(ToBrowserEvent.AddNameForChat, name);
        }

        public void AddPositionToMapCreatorBrowser(int id, MapCreatorPositionType type, float posX, float posY, float posZ, float rotX, float rotY, float rotZ,
            object info, ushort ownerRemoteId)
        {
            ExecuteFast(ToBrowserEvent.AddPositionToMapCreatorBrowser, id, (int)type, posX, posY, posZ, rotX, rotY, rotZ, ownerRemoteId, info);
        }

        public void CloseMapMenu()
        {
            ExecuteFast(ToBrowserEvent.CloseMapMenu);
        }

        public void FromBrowserEventReturn(string eventName, object ret)
        {
            ExecuteFast(ToServerEvent.FromBrowserEvent, eventName, ret);
        }

        public void FromServerToBrowser(string eventName, params object[] args)
        {
            ExecuteFast(eventName, args);
        }

        public void GetHashedPasswordReturn(string hashedPassword)
        {
            ExecuteFast(FromBrowserEvent.GetHashedPassword, hashedPassword);
        }

        public void HideRankings()
        {
            ExecuteFast(ToBrowserEvent.HideRankings);
        }

        public void LoadChatSettings(float width, float maxHeight, float fontSize, bool hideDirtyChat, bool hideChatInfo, float chatInfoFontSize, int chatInfoAnimationTimeMs)
        {
            ExecuteFast(ToBrowserEvent.LoadChatSettings, width, maxHeight, fontSize, hideDirtyChat, hideChatInfo, chatInfoFontSize, chatInfoAnimationTimeMs);
        }

        public void LoadLanguage(ILanguage language)
        {
            ExecuteFast(ToBrowserEvent.LoadLanguage, (int)language.Enum);
        }

        public void LoadMapForMapCreator(string json)
        {
            ExecuteFast(ToBrowserEvent.LoadMapForMapCreator, json);
        }

        public void LoadNamesForChat(List<ITDSPlayer> players)
        {
            IEnumerable<string> names = players.Select(p => p.Name);
            ExecuteFast(ToBrowserEvent.LoadNamesForChat, Serializer.ToBrowser(names));
        }

        public void LoadUserpanelData(int type, string json)
        {
            ExecuteFast("sb13", type, json);
        }

        public void OpenMapMenu(string mapsListJson)
        {
            ExecuteFast(ToBrowserEvent.OpenMapMenu, mapsListJson);
        }

        public void RefreshAdminLevel(int adminLevel)
        {
            ExecuteFast(ToBrowserEvent.RefreshAdminLevel, adminLevel);
        }

        public void RemoveNameForChat(string name)
        {
            ExecuteFast(ToBrowserEvent.RemoveNameForChat, name);
        }

        public void RemovePositionInMapCreatorBrowser(int id, MapCreatorPositionType type)
        {
            ExecuteFast(ToBrowserEvent.RemovePositionInMapCreatorBrowser, id, (int)type);
        }

        public void RemoveTeamPositionInMapCreatorBrowser(int teamNumber)
        {
            ExecuteFast(ToBrowserEvent.RemoveTeamPositionsInMapCreatorBrowser, teamNumber);
        }

        public void ResetMapVoting()
        {
            ExecuteFast(ToBrowserEvent.ResetMapVoting);
        }

        public override void SetReady(params object[] args)
        {
            base.SetReady(args);
        }

        public void ShowCooldown()
        {
            ExecuteFast(ToBrowserEvent.ShowCooldown);
        }

        public void ShowRankings(string rankingsJson)
        {
            ExecuteFast(ToBrowserEvent.ShowRankings, rankingsJson);
        }

        public void SyncHudDataChange(HudDataType type, int value)
        {
            ExecuteFast(ToBrowserEvent.SyncHudDataChange, (int)type, value);
            //ExecuteFast(ToBrowserEvent.SyncHUDDataChange, type, value);
        }

        public void SyncInFightLobby(bool b)
        {
            ExecuteFast(ToBrowserEvent.ToggleInFightLobby, b);
        }

        public void SyncIsLobbyOwner(bool obj)
        {
            ExecuteFast(ToBrowserEvent.SyncIsLobbyOwner, obj);
        }

        public void SyncMapPriceData(int mapBuyCounter)
        {
            ExecuteFast(ToBrowserEvent.SyncMapPriceData, mapBuyCounter);
        }

        public void SyncMoney(int money)
        {
            ExecuteFast(ToBrowserEvent.SyncMoney, money);
        }

        public void SyncTeamChoiceMenuData(string teamsJson, bool isRandomTeams)
        {
            ToggleTeamChoiceMenu(true);
            ExecuteFast(ToBrowserEvent.SyncTeamChoiceMenuData, teamsJson, isRandomTeams);
        }

        public void SyncUsernameChange(string name)
        {
            ExecuteFast(ToBrowserEvent.SyncUsernameChange, name);
        }

        public void ToggleCharCreator(bool toggle, string dataJson = "")
        {
            ExecuteFast(ToBrowserEvent.ToggleCharCreator, toggle, dataJson);
        }

        public void SyncGangId(int gangId)
        {
            ExecuteFast(ToBrowserEvent.SyncGangId, gangId);
        }

        public void ToggleChatInput(bool activated)
        {
            ExecuteFast(ToBrowserEvent.ToggleChatInput, activated);
        }

        public void ToggleChatInput(bool activated, string startWith)
        {
            ExecuteFast(ToBrowserEvent.ToggleChatInput, activated, startWith);
        }

        public void ToggleChatOpened(bool activated)
        {
            ExecuteFast(ToBrowserEvent.ToggleChatOpened, activated);
        }

        public void ToggleFreeroam(bool activated)
        {
            ExecuteFast(ToBrowserEvent.ToggleFreeroam, activated);
        }

        public void ToggleHUD(bool toggle)
        {
            ExecuteFast(ToBrowserEvent.ToggleHUD, toggle);
        }

        public void ToggleInfo(InfoType type, bool toggle)
        {
            ExecuteFast(ToBrowserEvent.ToggleInfo, (int)type, toggle);
        }

        public void ToggleLobbyChoiceMenu(bool activated)
        {
            _eventsHandler.OnCursorToggleRequested(activated);
            ExecuteFast(ToBrowserEvent.ToggleLobbyChoice, activated);
        }

        public void ToggleMapCreator(bool activated)
        {
            ExecuteFast(ToBrowserEvent.ToggleMapCreator, activated);
        }

        public void ToggleRoundStats(bool toggle)
        {
            Logging.LogWarning(toggle.ToString(), "AngularBrowserHandler.ToggleRoundStats");
            ExecuteFast(ToBrowserEvent.ToggleRoundStats, toggle);
        }

        public void ToggleTeamChoiceMenu(bool boolean)
        {
            ExecuteFast(ToBrowserEvent.ToggleTeamChoiceMenu, boolean);
        }

        public void ToggleTeamOrderModus(bool activated)
        {
            ExecuteFast(ToBrowserEvent.ToggleTeamOrderModus, activated);
        }

        public void ToggleUserpanel(bool boolean)
        {
            ExecuteFast(ToBrowserEvent.ToggleUserpanel, boolean);
        }

        public void ToggleGangWindow(bool boolean)
        {
            ExecuteFast(ToBrowserEvent.ToggleGangWindow, boolean);
        }

        internal void MapCreatorSyncCurrentMapToServer(int tdsPlayerId, int idCounter)
        {
            ExecuteFast(ToBrowserEvent.MapCreatorSyncCurrentMapToServer, tdsPlayerId, idCounter);
        }

        internal void SyncThemeSettings(string dataJson)
        {
            ExecuteFast(ToBrowserEvent.LoadThemeSettings, dataJson);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            ResetMapVoting();
        }

        private void OnFromBrowserEventReturnMethod(object[] args)
        {
            string eventName = (string)args[0];
            object ret = args[1];
            FromBrowserEventReturn(eventName, ret);
        }

        private void OnGetHashedPassword(object[] args)
        {
            string pw = Convert.ToString(args[0]);
            GetHashedPasswordReturn(SharedUtils.HashPWClient(pw));
        }

        private void OnToBrowserEventMethod(object[] args)
        {
            string eventName = (string)args[0];
            FromServerToBrowser(eventName, args.Skip(1).ToArray());
        }
    }
}
