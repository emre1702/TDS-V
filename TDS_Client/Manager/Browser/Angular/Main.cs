﻿using Newtonsoft.Json;
using Player = RAGE.Elements.Player;
using RAGE.Ui;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Manager.Utility;
using TDS_Common.Enum;
using TDS_Common.Default;
using TDS_Client.Enum;

namespace TDS_Client.Manager.Browser.Angular
{
    static class Main
    {
        public static HtmlWindow Browser { get; set; }
        private readonly static Queue<string> _executeQueue = new Queue<string>();

        private static void Execute(string eventName, params object[] args)
        {
            string execStr = Shared.GetExecStr(eventName, args);
            if (Browser == null)
                _executeQueue.Enqueue(execStr);
            else
                Browser.ExecuteJs(execStr);
        }

        public static void Start(int adminLevel)
        {
            Browser = new HtmlWindow(ClientConstants.AngularMainBrowserPath);

            Execute(DToBrowserEvent.InitLoadAngular, adminLevel);
            foreach (var execStr in _executeQueue)
            {
                Browser.ExecuteJs(execStr);
            }
            _executeQueue.Clear();
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
            Execute(DToClientEvent.SyncTeamChoiceMenuData, teamsJson, isRandomTeams);
        }

        public static void ToggleTeamChoiceMenu(bool boolean)
        {
            Execute(DToClientEvent.ToggleTeamChoiceMenu, boolean);
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

        public static void AddPositionToMapCreatorBrowser(int id, EMapCreatorPositionType type, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, object info)
        {
            Execute(DToBrowserEvent.AddPositionToMapCreatorBrowser, id, (int)type, posX, posY, posZ, rotX, rotY, rotZ, info);
        }

        internal static void RemovePositionInMapCreatorBrowser(int id, EMapCreatorPositionType type)
        {
            Execute(DToBrowserEvent.RemovePositionInMapCreatorBrowser, id, (int)type);
        }
    }

}