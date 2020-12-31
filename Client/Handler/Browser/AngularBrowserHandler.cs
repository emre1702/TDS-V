using RAGE.Elements;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Interfaces;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Utility;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Browser
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
            eventsHandler.AngularCooldownReturn += ShowCooldownCallback;
            eventsHandler.RoundEnded += _ => ResetMapVoting();
            eventsHandler.ChatInputToggled += ToggleChatOpened;

            Add(FromBrowserEvent.GetHashedPassword, OnGetHashedPassword);
            Add(ToClientEvent.ToBrowserEvent, OnToBrowserEventMethod);
            Add(ToClientEvent.FromBrowserEventReturn, OnFromBrowserEventReturnMethod);

            OnPlayerStartTalking += EventHandler_PlayerStartTalking;
            OnPlayerStopTalking += EventHandler_PlayerStopTalking;

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
            ExecuteFast(eventName, ret);
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

        public void LoadSettings(string json)
        {
            ExecuteFast(ToBrowserEvent.LoadSettings, json);
        }

        public void LoadLanguage(ILanguage language)
        {
            ExecuteFast(ToBrowserEvent.LoadLanguage, (int)language.Enum);
        }

        public void LoadMapForMapCreator(string json)
        {
            ExecuteFast(ToBrowserEvent.LoadMapForMapCreator, json);
        }

        internal void AddKillMessage(string killInfoJson)
        {
            ExecuteFast(ToBrowserEvent.AddKillMessage, killInfoJson);
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

        public void ShowCooldownCallback(string eventName)
        {
            // Do this to stop the callback waiting
            ExecuteFast(eventName, "Cooldown");
            ShowCooldown();
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

        public void ToggleCharCreator(bool toggle)
        {
            ExecuteFast(ToBrowserEvent.ToggleCharCreator, toggle);
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

        public void EventHandler_PlayerStartTalking(Player modPlayer)
        {
            if (!(modPlayer is ITDSPlayer player))
                return;
            ExecuteFast(ToBrowserEvent.AddUserTalking, player.RemoteId, player.DisplayName);
        }

        public void EventHandler_PlayerStopTalking(Player modPlayer)
        {
            if (!(modPlayer is ITDSPlayer player))
                return;
            ExecuteFast(ToBrowserEvent.RemoveUserTalking, player.RemoteId);
        }

        private void OnToBrowserEventMethod(object[] args)
        {
            try
            {
                switch (args.Length)
                {
                    case 1:
                        FromServerToBrowser((string)args[0]);
                        break;

                    case 2:
                        FromServerToBrowser((string)args[0], args[1]);
                        break;

                    case 3:
                        FromServerToBrowser((string)args[0], args[1], args[2]);
                        break;

                    case 4:
                        FromServerToBrowser((string)args[0], args[1], args[2], args[3]);
                        break;

                    case 5:
                        FromServerToBrowser((string)args[0], args[1], args[2], args[3], args[4]);
                        break;

                    case 6:
                        FromServerToBrowser((string)args[0], args[1], args[2], args[3], args[4], args[5]);
                        break;

                    case 7:
                        FromServerToBrowser((string)args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                        break;

                    case 8:
                        FromServerToBrowser((string)args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                        break;

                    case 9:
                        FromServerToBrowser((string)args[0], args[1], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                        break;

                    case 10:
                        FromServerToBrowser((string)args[0], args[1], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, "OnToBrowser: " + (string)args[0]);
            }
        }
    }
}