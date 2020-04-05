using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Client.Manager.Browser.Angular
{
    public class AngularBrowserHandler : BrowserHandlerBase
    {
        private readonly SettingsHandler _settingsHandler;
        private readonly CursorHandler _cursorHandler;

        public AngularBrowserHandler(IModAPI modAPI, SettingsHandler settingsHandler, CursorHandler cursorHandler, Serializer serializer)
            : base(modAPI, serializer, Constants.AngularMainBrowserPath)
        {
            _settingsHandler = settingsHandler;
            _cursorHandler = cursorHandler;

            modAPI.Chat.SafeMode = false;
            modAPI.Chat.Show(false);

            CreateBrowser();
            Browser.MarkAsChat();
        }

        public override void SetReady(params object[] args)
        {
            base.SetReady(args);

            SendWelcomeMessage();
        }


        private void SendWelcomeMessage()
        {
            ModAPI.Chat.Output("#o#__________________________________________");
            ModAPI.Chat.Output(string.Join("#n#", _settingsHandler.Language.WELCOME_MESSAGE));
            ModAPI.Chat.Output("#o#__________________________________________");
        }

        public void LoadLanguage(ILanguage language)
        {
            Execute(ToBrowserEvent.LoadLanguage, (int)language.Enum);
        }

        public void OpenMapMenu(string mapsListJson)
        {
            Execute(ToBrowserEvent.OpenMapMenu, mapsListJson);
        }

        public void CloseMapMenu()
        {
            Execute(ToBrowserEvent.CloseMapMenu);
        }

        public void AddMapToVoting(string mapVoteJson)
        {
            Execute(ToBrowserEvent.AddMapToVoting, mapVoteJson);
        }

        public void SetMapVotes(int mapId, int amountVotes)
        {
            Execute(ToBrowserEvent.SetMapVotes, mapId, amountVotes);
        }

        public void LoadMapVoting(string mapVotesJson)
        {
            Execute(ToBrowserEvent.LoadMapVoting, mapVotesJson);
        }

        public void ResetMapVoting()
        {
            Execute(ToBrowserEvent.ResetMapVoting);
        }

        public void LoadFavoriteMaps(string mapFavoritesJson)
        {
            Execute(ToBrowserEvent.LoadFavoriteMaps, mapFavoritesJson);
        }

        public void ToggleChatInput(bool activated)
        {
            Execute(ToBrowserEvent.ToggleChatInput, activated);
        }

        public void ToggleChatInput(bool activated, string startWith)
        {
            Execute(ToBrowserEvent.ToggleChatInput, activated, startWith);
        }

        public void ToggleTeamOrderModus(bool activated)
        {
            Execute(ToBrowserEvent.ToggleTeamOrderModus, activated);
        }

        public void ToggleChatOpened(bool activated)
        {
            Execute(ToBrowserEvent.ToggleChatOpened, activated);
        }

        public void ToggleFreeroam(bool activated)
        {
            Execute(ToBrowserEvent.ToggleFreeroam, activated);
        }

        public void ToggleMapCreator(bool activated)
        {
            Execute(ToBrowserEvent.ToggleMapCreator, activated);
        }

        public void ToggleLobbyChoiceMenu(bool activated)
        {
            _cursorHandler.Visible = activated;
            Execute(ToBrowserEvent.ToggleLobbyChoice, activated);
        }

        public void SendMapCreatorReturn(int err)
        {
            Execute(FromBrowserEvent.SendMapCreatorData, err);
        }

        public void SaveMapCreatorReturn(int err)
        {
            Execute(FromBrowserEvent.SaveMapCreatorData, err);
        }

        public void LoadMapNamesForMapCreator(string json)
        {
            Execute(ToServerEvent.LoadMapNamesToLoadForMapCreator, json);
        }

        public void LoadMapForMapCreator(string json)
        {
            Execute(ToServerEvent.LoadMapForMapCreator, json);
        }

        public void SyncInFightLobby(bool b)
        {
            Execute(ToBrowserEvent.ToggleInFightLobby, b);
        }

        public void CreateCustomLobbyReturn(string errorOrEmpty)
        {
            Execute(FromBrowserEvent.CreateCustomLobby, errorOrEmpty);
        }

        public void AddCustomLobby(string json)
        {
            Execute(ToBrowserEvent.AddCustomLobby, json);
        }

        public void RemoveCustomLobby(int lobbyId)
        {
            Execute(ToBrowserEvent.RemoveCustomLobby, lobbyId);
        }

        public void SyncAllCustomLobbies(string json)
        {
            Execute(ToBrowserEvent.SyncAllCustomLobbies, json);
        }

        public void LeaveCustomLobbyMenu()
        {
            Execute(ToBrowserEvent.LeaveCustomLobbyMenu);
        }

        public void SyncTeamChoiceMenuData(string teamsJson, bool isRandomTeams)
        {
            ToggleTeamChoiceMenu(true);
            Execute(ToBrowserEvent.SyncTeamChoiceMenuData, teamsJson, isRandomTeams);
        }

        public void ToggleTeamChoiceMenu(bool boolean)
        {
            Execute(ToBrowserEvent.ToggleTeamChoiceMenu, boolean);
        }

        public void ToggleUserpanel(bool boolean)
        {
            Execute(ToBrowserEvent.ToggleUserpanel, boolean);
        }

        public void LoadUserpanelData(int type, string json)
        {
            Execute(ToServerEvent.LoadUserpanelData, type, json);
        }

        public void ShowCooldown()
        {
            Execute(ToBrowserEvent.ShowCooldown);
        }

        public void AddPositionToMapCreatorBrowser(int id, MapCreatorPositionType type, float posX, float posY, float posZ, float rotX, float rotY, float rotZ,
            object info, ushort ownerRemoteId)
        {
            Execute(ToBrowserEvent.AddPositionToMapCreatorBrowser, id, (int)type, posX, posY, posZ, rotX, rotY, rotZ, ownerRemoteId, info);
        }

        public void RemovePositionInMapCreatorBrowser(int id, MapCreatorPositionType type)
        {
            Execute(ToBrowserEvent.RemovePositionInMapCreatorBrowser, id, (int)type);
        }

        public void ShowRankings(string rankingsJson)
        {
            Execute(ToBrowserEvent.ShowRankings, rankingsJson);
        }

        public void HideRankings()
        {
            Execute(ToBrowserEvent.HideRankings);
        }

        public void RefreshAdminLevel(int adminLevel)
        {
            Execute(ToBrowserEvent.RefreshAdminLevel, adminLevel);
        }

        public void LoadApplicationDataForAdmin(string json)
        {
            Execute(ToServerEvent.LoadApplicationDataForAdmin, json);
        }

        public void GetSupportRequestData(string json)
        {
            Execute(ToServerEvent.GetSupportRequestData, json);
        }

        public void SetSupportRequestClosed(int requestId, bool closed)
        {
            Execute(ToBrowserEvent.SetSupportRequestClosed, requestId, closed);
        }

        public void SyncNewSupportRequestMessage(int requestId, string messageJson)
        {
            Execute(ToBrowserEvent.SyncNewSupportRequestMessage, requestId, messageJson);
        }

        public void SyncMoney(int money)
        {
            Execute(ToBrowserEvent.SyncMoney, money);
        }

        public void SyncMapPriceData(int mapBuyCounter)
        {
            Execute(ToBrowserEvent.SyncMapPriceData, mapBuyCounter);
        }

        public void SyncIsLobbyOwner(bool obj)
        {
            Execute(ToBrowserEvent.SyncIsLobbyOwner, obj);
        }

        public void MapCreatorSyncData(int mapInfoType, object data)
        {
            Execute(ToBrowserEvent.MapCreatorSyncData, mapInfoType, data);
        }

        public void GetHashedPasswordReturn(string hashedPassword)
        {
            Execute(FromBrowserEvent.GetHashedPassword, hashedPassword);
        }

        public void ToggleRoundStats(bool toggle)
        {
            Execute(ToBrowserEvent.ToggleRoundStats, toggle);
        }

        public void ToggleHUD(bool toggle)
        {
            Execute(ToBrowserEvent.ToggleHUD, toggle);
        }

        public void SyncHUDDataChange(HudDataType type, int value)
        {
            ExecuteFast(ToBrowserEvent.SyncHudDataChange, (int)type, value);
            //Execute(ToBrowserEvent.SyncHUDDataChange, type, value);
        }


        public void FromServerToBrowser(string eventName, params object[] args)
        {
            Execute(eventName, args);
        }

        public void FromBrowserEventReturn(string eventName, object ret)
        {
            Execute(ToServerEvent.FromBrowserEvent, eventName, ret);
        }

        public void AddNameForChat(string name)
        {
            Execute(ToBrowserEvent.AddNameForChat, name);
        }

        public void LoadNamesForChat(List<IPlayer> players)
        {
            IEnumerable<string> names = players.Select(p => p.Name);
            Execute(ToBrowserEvent.LoadNamesForChat, Serializer.ToBrowser(names));
        }

        public void RemoveNameForChat(string name)
        {
            Execute(ToBrowserEvent.RemoveNameForChat, name);
        }

        public void SyncUsernameChange(string name)
        {
            Execute(ToBrowserEvent.SyncUsernameChange, name);
        }
    }

}
