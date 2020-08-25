using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Browser
{
    public class AngularBrowserHandler
    {
        #region Public Methods



        public void LoadFavoriteMaps(string mapFavoritesJson)
        {
            Execute(ToBrowserEvent.LoadFavoriteMaps, mapFavoritesJson);
        }

        public void LoadMapForMapCreator(string json)
        {
            Execute(ToBrowserEvent.LoadMapForMapCreator, json);
        }

        public void LoadNamesForChat(List<IPlayer> players)
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

        public void SyncTeamChoiceMenuData(string teamsJson, bool isRandomTeams)
        {
            ToggleTeamChoiceMenu(true);
            Execute(ToBrowserEvent.SyncTeamChoiceMenuData, teamsJson, isRandomTeams);
        }

        public void ToggleCharCreator(bool toggle, string dataJson = "")
        {
            ExecuteFast(ToBrowserEvent.ToggleCharCreator, toggle, dataJson);
        }

        public void ToggleChatInput(bool activated)
        {
            Execute(ToBrowserEvent.ToggleChatInput, activated);
        }

        public void ToggleChatInput(bool activated, string startWith)
        {
            Execute(ToBrowserEvent.ToggleChatInput, activated, startWith);
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

        #endregion Public Methods

        #region Internal Methods

        internal void MapCreatorSyncCurrentMapToServer(int tdsPlayerId, int idCounter)
        {
            Execute(ToBrowserEvent.MapCreatorSyncCurrentMapToServer, tdsPlayerId, idCounter);
        }

        internal void SyncThemeSettings(string dataJson)
        {
            Execute(ToBrowserEvent.LoadThemeSettings, dataJson);
        }

        #endregion Internal Methods

    }
}
