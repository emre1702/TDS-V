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

        public void AddNameForChat(string name)
        {
            Execute(ToBrowserEvent.AddNameForChat, name);
        }

        public void AddPositionToMapCreatorBrowser(int id, MapCreatorPositionType type, float posX, float posY, float posZ, float rotX, float rotY, float rotZ,
            object info, ushort ownerRemoteId)
        {
            Execute(ToBrowserEvent.AddPositionToMapCreatorBrowser, id, (int)type, posX, posY, posZ, rotX, rotY, rotZ, ownerRemoteId, info);
        }

        public void CloseMapMenu()
        {
            Execute(ToBrowserEvent.CloseMapMenu);
        }

        public void FromBrowserEventReturn(string eventName, object ret)
        {
            Execute(ToServerEvent.FromBrowserEvent, eventName, ret);
        }

        public void FromServerToBrowser(string eventName, params object[] args)
        {
            Execute(eventName, args);
        }

        public void GetHashedPasswordReturn(string hashedPassword)
        {
            Execute(FromBrowserEvent.GetHashedPassword, hashedPassword);
        }

        public void HideRankings()
        {
            Execute(ToBrowserEvent.HideRankings);
        }

        public void LoadChatSettings(float width, float maxHeight, float fontSize, bool hideDirtyChat, bool hideChatInfo, float chatInfoFontSize, int chatInfoAnimationTimeMs)
        {
            Execute(ToBrowserEvent.LoadChatSettings, width, maxHeight, fontSize, hideDirtyChat, hideChatInfo, chatInfoFontSize, chatInfoAnimationTimeMs);
        }

        public void LoadLanguage(ILanguage language)
        {
            Execute(ToBrowserEvent.LoadLanguage, (int)language.Enum);
        }

        public void LoadMapForMapCreator(string json)
        {
            Execute(ToBrowserEvent.LoadMapForMapCreator, json);
        }

        public void LoadNamesForChat(List<ITDSPlayer> players)
        {
            IEnumerable<string> names = players.Select(p => p.Name);
            Execute(ToBrowserEvent.LoadNamesForChat, Serializer.ToBrowser(names));
        }

        public void LoadUserpanelData(int type, string json)
        {
            Execute("sb13", type, json);
        }

        public void OpenMapMenu(string mapsListJson)
        {
            Execute(ToBrowserEvent.OpenMapMenu, mapsListJson);
        }

        public void RefreshAdminLevel(int adminLevel)
        {
            Execute(ToBrowserEvent.RefreshAdminLevel, adminLevel);
        }

        public void RemoveNameForChat(string name)
        {
            Execute(ToBrowserEvent.RemoveNameForChat, name);
        }

        public void RemovePositionInMapCreatorBrowser(int id, MapCreatorPositionType type)
        {
            Execute(ToBrowserEvent.RemovePositionInMapCreatorBrowser, id, (int)type);
        }

        public void RemoveTeamPositionInMapCreatorBrowser(int teamNumber)
        {
            Execute(ToBrowserEvent.RemoveTeamPositionsInMapCreatorBrowser, teamNumber);
        }

        public void ResetMapVoting()
        {
            Execute(ToBrowserEvent.ResetMapVoting);
        }

        public override void SetReady(params object[] args)
        {
            base.SetReady(args);
        }

        public void ShowCooldown()
        {
            Execute(ToBrowserEvent.ShowCooldown);
        }

        public void ShowRankings(string rankingsJson)
        {
            Execute(ToBrowserEvent.ShowRankings, rankingsJson);
        }

        public void SyncHudDataChange(HudDataType type, int value)
        {
            ExecuteFast(ToBrowserEvent.SyncHudDataChange, (int)type, value);
            //Execute(ToBrowserEvent.SyncHUDDataChange, type, value);
        }

        public void SyncInFightLobby(bool b)
        {
            Execute(ToBrowserEvent.ToggleInFightLobby, b);
        }

        public void SyncIsLobbyOwner(bool obj)
        {
            Execute(ToBrowserEvent.SyncIsLobbyOwner, obj);
        }

        public void SyncMapPriceData(int mapBuyCounter)
        {
            Execute(ToBrowserEvent.SyncMapPriceData, mapBuyCounter);
        }

        public void SyncMoney(int money)
        {
            Execute(ToBrowserEvent.SyncMoney, money);
        }

        public void SyncTeamChoiceMenuData(string teamsJson, bool isRandomTeams)
        {
            ToggleTeamChoiceMenu(true);
            Execute(ToBrowserEvent.SyncTeamChoiceMenuData, teamsJson, isRandomTeams);
        }

        public void SyncUsernameChange(string name)
        {
            Execute(ToBrowserEvent.SyncUsernameChange, name);
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
            Execute(ToBrowserEvent.ToggleChatInput, activated);
        }

        public void ToggleChatInput(bool activated, string startWith)
        {
            Execute(ToBrowserEvent.ToggleChatInput, activated, startWith);
        }

        public void ToggleChatOpened(bool activated)
        {
            Execute(ToBrowserEvent.ToggleChatOpened, activated);
        }

        public void ToggleFreeroam(bool activated)
        {
            Execute(ToBrowserEvent.ToggleFreeroam, activated);
        }

        public void ToggleHUD(bool toggle)
        {
            Execute(ToBrowserEvent.ToggleHUD, toggle);
        }

        public void ToggleInfo(InfoType type, bool toggle)
        {
            ExecuteFast(ToBrowserEvent.ToggleInfo, (int)type, toggle);
        }

        public void ToggleLobbyChoiceMenu(bool activated)
        {
            _eventsHandler.OnCursorToggleRequested(activated);
            Execute(ToBrowserEvent.ToggleLobbyChoice, activated);
        }

        public void ToggleMapCreator(bool activated)
        {
            Execute(ToBrowserEvent.ToggleMapCreator, activated);
        }

        public void ToggleRoundStats(bool toggle)
        {
            Logging.LogWarning(toggle.ToString(), "AngularBrowserHandler.ToggleRoundStats");
            Execute(ToBrowserEvent.ToggleRoundStats, toggle);
        }

        public void ToggleTeamChoiceMenu(bool boolean)
        {
            Execute(ToBrowserEvent.ToggleTeamChoiceMenu, boolean);
        }

        public void ToggleTeamOrderModus(bool activated)
        {
            Execute(ToBrowserEvent.ToggleTeamOrderModus, activated);
        }

        public void ToggleUserpanel(bool boolean)
        {
            Execute(ToBrowserEvent.ToggleUserpanel, boolean);
        }

        public void ToggleGangWindow(bool boolean)
        {
            Execute(ToBrowserEvent.ToggleGangWindow, boolean);
        }

        internal void MapCreatorSyncCurrentMapToServer(int tdsPlayerId, int idCounter)
        {
            Execute(ToBrowserEvent.MapCreatorSyncCurrentMapToServer, tdsPlayerId, idCounter);
        }

        internal void SyncThemeSettings(string dataJson)
        {
            Execute(ToBrowserEvent.LoadThemeSettings, dataJson);
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
