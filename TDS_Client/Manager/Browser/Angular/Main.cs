using RAGE.Ui;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Manager.Utility;
using TDS_Common.Enum;
using TDS_Common.Default;
using TDS_Client.Enum;
using RAGE.Elements;
using System.Linq;
using TDS_Common.Manager.Utility;
using System;
using System.Diagnostics;

namespace TDS_Client.Manager.Browser.Angular
{
    static class Main
    {
        public static HtmlWindow Browser { get; set; }
        private readonly static LinkedList<Action> _executeList = new LinkedList<Action>();
        private static bool _isReady;

        private static void Execute(string eventName, params object[] args)
        {
            string execStr = Shared.GetExecStr(eventName, args);
            if (!_isReady)
                _executeList.AddLast(() => Browser.ExecuteJs(execStr));
            else
                Browser.ExecuteJs(execStr);
        }

        private static void ExecuteFast(string eventName, params object[] args)
        {
            if (!_isReady)
                _executeList.AddLast(() => Browser.Call(eventName, args));
            else
                Browser.Call(eventName, args);
        }

        public static void Start(string angularConstantsDataJson, string challengesJson)
        {
            if (Browser != null)
                return;

            RAGE.Chat.SafeMode = false;

            _executeList.AddFirst(() => Execute(DToBrowserEvent.InitLoadAngular, angularConstantsDataJson, challengesJson));

            Browser = new HtmlWindow(ClientConstants.AngularMainBrowserPath);
            Browser.MarkAsChat();
        }

        public static void SetReady()
        {
            _isReady = true;
            foreach (var exec in _executeList)
            {
                exec();
            }
            _executeList.Clear();
        }

        public static void LoadLanguage(ELanguage language)
        {
            Execute(DToBrowserEvent.LoadLanguage, (int)language);
        }

        public static void OpenMapMenu(string mapsListJson)
        {
            Execute(DToBrowserEvent.OpenMapMenu, mapsListJson);
        }

        public static void CloseMapMenu()
        {
            Execute(DToBrowserEvent.CloseMapMenu);
        }

        public static void AddMapToVoting(string mapVoteJson)
        {
            Execute(DToBrowserEvent.AddMapToVoting, mapVoteJson);
        }

        public static void SetMapVotes(int mapId, int amountVotes)
        {
            Execute(DToBrowserEvent.SetMapVotes, mapId, amountVotes);
        }

        public static void LoadMapVoting(string mapVotesJson)
        {
            Execute(DToBrowserEvent.LoadMapVoting, mapVotesJson);
        }

        public static void ResetMapVoting()
        {
            Execute(DToBrowserEvent.ResetMapVoting);
        }

        public static void LoadFavoriteMaps(string mapFavoritesJson)
        {
            Execute(DToBrowserEvent.LoadFavoriteMaps, mapFavoritesJson);
        }

        public static void ToggleChatInput(bool activated)
        {
            Execute(DToBrowserEvent.ToggleChatInput, activated);
        }

        public static void ToggleChatInput(bool activated, string startWith)
        {
            Execute(DToBrowserEvent.ToggleChatInput, activated, startWith);
        }

        public static void ToggleTeamOrderModus(bool activated)
        {
            Execute(DToBrowserEvent.ToggleTeamOrderModus, activated);
        }

        public static void ToggleChatOpened(bool activated)
        {
            Execute(DToBrowserEvent.ToggleChatOpened, activated);
        }

        public static void ToggleFreeroam(bool activated)
        {
            Execute(DToBrowserEvent.ToggleFreeroam, activated);
        }

        public static void ToggleMapCreator(bool activated)
        {
            Execute(DToBrowserEvent.ToggleMapCreator, activated);
        }

        public static void ToggleLobbyChoiceMenu(bool activated)
        {
            CursorManager.Visible = activated;
            Execute(DToBrowserEvent.ToggleLobbyChoice, activated);
        }

        public static void SendMapCreatorReturn(int err)
        {
            Execute(DFromBrowserEvent.SendMapCreatorData, err);
        }

        public static void SaveMapCreatorReturn(int err)
        {
            Execute(DFromBrowserEvent.SaveMapCreatorData, err);
        }

        public static void LoadMapNamesForMapCreator(string json)
        {
            Execute(DToServerEvent.LoadMapNamesToLoadForMapCreator, json);
        }

        public static void LoadMapForMapCreator(string json)
        {
            Execute(DToServerEvent.LoadMapForMapCreator, json);
        }

        public static void SyncInFightLobby(bool b)
        {
            Execute(DToBrowserEvent.ToggleInFightLobby, b);
        }

        public static void CreateCustomLobbyReturn(string errorOrEmpty)
        {
            Execute(DFromBrowserEvent.CreateCustomLobby, errorOrEmpty);
        }

        public static void AddCustomLobby(string json)
        {
            Execute(DToBrowserEvent.AddCustomLobby, json);
        }

        public static void RemoveCustomLobby(int lobbyId)
        {
            Execute(DToBrowserEvent.RemoveCustomLobby, lobbyId);
        }

        public static void SyncAllCustomLobbies(string json)
        {
            Execute(DToBrowserEvent.SyncAllCustomLobbies, json);
        }

        public static void LeaveCustomLobbyMenu()
        {
            Execute(DToBrowserEvent.LeaveCustomLobbyMenu);
        }

        public static void SyncTeamChoiceMenuData(string teamsJson, bool isRandomTeams)
        {
            ToggleTeamChoiceMenu(true);
            Execute(DToBrowserEvent.SyncTeamChoiceMenuData, teamsJson, isRandomTeams);
        }

        public static void ToggleTeamChoiceMenu(bool boolean)
        {
            Execute(DToBrowserEvent.ToggleTeamChoiceMenu, boolean);
        }

        public static void ToggleUserpanel(bool boolean)
        {
            Execute(DToBrowserEvent.ToggleUserpanel, boolean);
        }

        public static void LoadUserpanelData(int type, string json)
        {
            Execute(DToServerEvent.LoadUserpanelData, type, json);
        }

        public static void ShowCooldown()
        {
            Execute(DToBrowserEvent.ShowCooldown);
        }

        public static void AddPositionToMapCreatorBrowser(int id, EMapCreatorPositionType type, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, 
            object info, ushort ownerRemoteId)
        {
            Execute(DToBrowserEvent.AddPositionToMapCreatorBrowser, id, (int)type, posX, posY, posZ, rotX, rotY, rotZ, ownerRemoteId, info);
        }

        public static void RemovePositionInMapCreatorBrowser(int id, EMapCreatorPositionType type)
        {
            Execute(DToBrowserEvent.RemovePositionInMapCreatorBrowser, id, (int)type);
        }

        public static void ShowRankings(string rankingsJson)
        {
            Execute(DToBrowserEvent.ShowRankings, rankingsJson);
        }

        public static void HideRankings()
        {
            Execute(DToBrowserEvent.HideRankings);
        }

        public static void RefreshAdminLevel(int adminLevel)
        {
            Execute(DToBrowserEvent.RefreshAdminLevel, adminLevel);
        }

        public static void LoadApplicationDataForAdmin(string json)
        {
            Execute(DToServerEvent.LoadApplicationDataForAdmin, json);
        }

        public static void GetSupportRequestData(string json)
        {
            Execute(DToServerEvent.GetSupportRequestData, json);
        }

        public static void SetSupportRequestClosed(int requestId, bool closed)
        {
            Execute(DToBrowserEvent.SetSupportRequestClosed, requestId, closed);
        }

        public static void SyncNewSupportRequestMessage(int requestId, string messageJson)
        {
            Execute(DToBrowserEvent.SyncNewSupportRequestMessage, requestId, messageJson);
        }

        public static void SyncMoney(int money)
        {
            Execute(DToBrowserEvent.SyncMoney, money);
        }

        public static void SyncMapPriceData(int mapBuyCounter)
        {
            Execute(DToBrowserEvent.SyncMapPriceData, mapBuyCounter);
        }

        public static void SyncIsLobbyOwner(bool obj)
        {
            Execute(DToBrowserEvent.SyncIsLobbyOwner, obj);
        }

        public static void MapCreatorSyncData(int mapInfoType, object data)
        {
            Execute(DToBrowserEvent.MapCreatorSyncData, mapInfoType, data);
        }

        public static void GetHashedPasswordReturn(string hashedPassword)
        {
            Execute(DFromBrowserEvent.GetHashedPassword, hashedPassword);
        }

        public static void ToggleRoundStats(bool toggle)
        {
            Execute(DToBrowserEvent.ToggleRoundStats, toggle);
        }

        public static void ToggleHUD(bool toggle)
        {
            Execute(DToBrowserEvent.ToggleHUD, toggle);
        }

        public static void SyncHUDDataChange(EHUDDataType type, int value)
        {
            ExecuteFast(DToBrowserEvent.SyncHUDDataChange, (int)type, value);
            //Execute(DToBrowserEvent.SyncHUDDataChange, type, value);
        }


        public static void FromServerToBrowser(string eventName, params object[] args)
        {
            Execute(eventName, args);
        }

        public static void FromBrowserEventReturn(string eventName, object ret)
        {
            Execute(DToServerEvent.FromBrowserEvent, eventName, ret);
        }

        public static void AddNameForChat(string name)
        {
            Execute(DToBrowserEvent.AddNameForChat, name);
        }

        public static void LoadNamesForChat(List<Player> players)
        {
            IEnumerable<string> names = players.Select(p => p.Name);
            Execute(DToBrowserEvent.LoadNamesForChat, Serializer.ToBrowser(names));
        }

        public static void RemoveNameForChat(string name)
        {
            Execute(DToBrowserEvent.RemoveNameForChat, name);
        }

        public static void SyncUsernameChange(string name)
        {
            Execute(DToBrowserEvent.SyncUsernameChange, name);
        }
    }

}
