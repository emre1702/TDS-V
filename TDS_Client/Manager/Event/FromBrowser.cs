using RAGE;
using RAGE.Game;
using System;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Enum.Userpanel;
using TDS_Common.Manager.Utility;
using static RAGE.Events;
using Player = RAGE.Elements.Player;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        private void AddFromBrowserEvents()
        {
            Add(DToServerEvent.AcceptInvitation, OnAcceptInvitationMethod);
            Add(DFromBrowserEvent.AddMapVote, OnAddMapVoteMethod);
            Add(DFromBrowserEvent.ChooseArenaToJoin, OnChooseArenaToJoinMethod);
            Add(DFromBrowserEvent.ChooseMapCreatorToJoin, OnChooseMapCreatorToJoinMethod);
            Add(DToServerEvent.ChooseTeam, OnChooseTeamMethod);
            Add(DFromBrowserEvent.CloseMapVotingMenu, OnCloseMapVotingMenuMethod);
            Add(DFromBrowserEvent.CloseUserpanel, OnCloseUserpanelMethod);
            Add(DFromBrowserEvent.CreateCustomLobby, OnCreateCustomLobbyMethod);
            Add(DFromBrowserEvent.GetHashedPassword, OnGetHashedPassword);
            Add(DToServerEvent.GetSupportRequestData, OnGetSupportRequestDataBrowserMethod);
            Add(DFromBrowserEvent.GetVehicle, OnGetVehicleMethod);
            Add(DFromBrowserEvent.HoldMapCreatorObject, OnHoldMapCreatorObjectMethod);
            Add(DFromBrowserEvent.InputStarted, OnInputStartedMethod);
            Add(DFromBrowserEvent.InputStopped, OnInputStoppedMethod);
            Add(DFromBrowserEvent.JoinCustomLobby, OnJoinCustomLobbyMethod);
            Add(DFromBrowserEvent.JoinCustomLobbyWithPassword, OnJoinCustomLobbyWithPasswordMethod);
            Add(DFromBrowserEvent.JoinedCustomLobbiesMenu, OnJoinedCustomLobbiesMenuMethod);
            Add(DToServerEvent.LeaveLobby, OnLeaveLobbyMethod);
            Add(DFromBrowserEvent.LeftCustomLobbiesMenu, OnLeftCustomLobbiesMenuMethod);
            Add(DToServerEvent.LeftSupportRequest, OnLeftSupportRequestMethod);
            Add(DToServerEvent.LeftSupportRequestsList, OnLeftSupportRequestsListMethod);
            Add(DToServerEvent.LoadApplicationDataForAdmin, LoadApplicationDataForAdminBrowserMethod);
            Add(DToServerEvent.LoadMapNamesToLoadForMapCreator, OnLoadMapNamesToLoadForMapCreatorMethod);
            Add(DToServerEvent.LoadMapForMapCreator, OnLoadMyMapForMapCreatorMethod);
            Add(DToServerEvent.LoadUserpanelData, OnLoadUserpanelDataBrowserMethod);
            Add(DFromBrowserEvent.MapCreatorHighlightPos, OnMapCreatorHighlightPosMethod);
            Add(DFromBrowserEvent.MapCreatorShowObject, OnMapCreatorShowObjectMethod);
            Add(DFromBrowserEvent.MapCreatorShowVehicle, OnMapCreatorShowVehicleMethod);
            Add(DFromBrowserEvent.MapCreatorStartNew, OnMapCreatorStartNewBrowserMethod);
            Add(DFromBrowserEvent.MapCreatorStartObjectChoice, OnMapCreatorStartObjectChoiceMethod);
            Add(DFromBrowserEvent.MapCreatorStopObjectPreview, OnMapCreatorStopObjectPreviewMethod);
            Add(DFromBrowserEvent.MapCreatorStartVehicleChoice, OnMapCreatorStartVehicleChoiceMethod);
            Add(DFromBrowserEvent.MapCreatorStopVehiclePreview, OnMapCreatorStopVehiclePreviewMethod);
            Add(DFromBrowserEvent.OnColorSettingChange, OnColorSettingChangeMethod);
            Add(DFromBrowserEvent.TryLogin, OnTryLoginMethod);
            Add(DFromBrowserEvent.TryRegister, OnTryRegisterMethod);
            Add(DFromBrowserEvent.ChatLoaded, OnChatLoadedMethod);
            Add(DFromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            Add(DToServerEvent.RejectInvitation, OnRejectInvitationMethod);
            Add(DToServerEvent.RemoveMap, OnRemoveMapMethod);
            Add(DFromBrowserEvent.RemoveMapCreatorPosition, OnRemoveMapCreatorPositionMethod);
            Add(DFromBrowserEvent.RemoveMapCreatorTeamNumber, OnRemoveMapCreatorTeamNumberMethod);
            Add(DFromBrowserEvent.SaveMapCreatorData, OnSaveMapCreatorDataMethod);
            Add(DToServerEvent.SaveSettings, OnSaveSettingsMethod);
            Add(DToServerEvent.SendApplication, OnSendApplicationMethod);
            Add(DFromBrowserEvent.SendMapCreatorData, OnSendMapCreatorDataMethod);
            Add(DFromBrowserEvent.SendMapRating, OnBrowserSendMapRatingMethod);
            Add(DToServerEvent.SetSupportRequestClosed, OnSetSupportRequestClosedBrowserMethod);
            Add(DToServerEvent.SendSupportRequest, OnSendSupportRequestMethod);
            Add(DToServerEvent.SendSupportRequestMessage, OnSendSupportRequestMessageMethod);
            Add(DFromBrowserEvent.StartMapCreatorPosPlacing, OnStartMapCreatorPosPlacingMethod);
            Add(DFromBrowserEvent.SyncRegisterLoginLanguageTexts, OnSyncRegisterLoginLanguageTextsMethod);
            Add(DFromBrowserEvent.TeleportToXY, OnTeleportToXYMethod);
            Add(DFromBrowserEvent.TeleportToPositionRotation, OnTeleportToPositionRotationMethod);
            Add(DFromBrowserEvent.ToggleMapFavorite, OnToggleMapFavoriteMethod);

            Add(DFromBrowserEvent.ChatUsed, OnChatUsedMethod);
            Add(DFromBrowserEvent.CommandUsed, OnCommandUsedMethod);
            Add(DFromBrowserEvent.CloseChat, OnCloseChatMethod);

            Add(DToServerEvent.FromBrowserEvent, OnFromBrowserEventMethod);
        }

        private void OnAcceptInvitationMethod(object[] args)
        {
            int invitationId = (int)args[0];
            EventsSender.Send(DToServerEvent.AcceptInvitation, invitationId);
        }

        private void OnAddMapVoteMethod(object[] args)
        {
            int mapId = (int)args[0];
            EventsSender.Send(DToServerEvent.MapVote, mapId);
        }

        private void OnChooseArenaToJoinMethod(object[] args)
        {
            Choice.JoinArena();
        }

        private void OnChooseMapCreatorToJoinMethod(object[] args)
        {
            Choice.JoinMapCreator();
        }

        private void OnChooseTeamMethod(object[] args)
        {
            Browser.Angular.Main.ToggleTeamChoiceMenu(false);
            CursorManager.Visible = false;
            Scoreboard.ReleasedScoreboardKey();
            int index = Convert.ToInt32(args[0]);
            EventsSender.Send(DToServerEvent.ChooseTeam, index);
        }

        private void OnBrowserSendMapRatingMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            int rating = Convert.ToInt32(args[1]);
            EventsSender.Send(DToServerEvent.SendMapRating, mapId, rating);
        }

        private void OnCloseMapVotingMenuMethod(object[] args)
        {
            MapManager.CloseMenu(false);
        }

        private void OnCloseUserpanelMethod(object[] args)
        {
            Userpanel.Close();
        }

        private void OnCreateCustomLobbyMethod(object[] args)
        {
            string dataJson = (string)args[0];
            EventsSender.Send(DToServerEvent.CreateCustomLobby, dataJson);
        }

        private void OnGetSupportRequestDataBrowserMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            EventsSender.Send(DToServerEvent.GetSupportRequestData, requestId);
        }

        private void OnGetVehicleMethod(object[] args)
        {
            // convert because if it fails, there will be an error @clientside, not @serverside
            EFreeroamVehicleType vehType = (EFreeroamVehicleType)(int)args[0];
            EventsSender.Send(DToServerEvent.GetVehicle, (int)vehType);
        }

        private void OnHoldMapCreatorObjectMethod(object[] args)
        {
            int objID = (int)args[0];
            ObjectPlacing.HoldObjectWithID(objID);
        }

        private void OnInputStartedMethod(object[] args)
        {
            Browser.Angular.Shared.InInput = true;
        }

        private void OnInputStoppedMethod(object[] args)
        {
            Browser.Angular.Shared.InInput = false;
        }

        private void OnJoinCustomLobbyMethod(object[] args)
        {
            int lobbyId = (int)args[0];
            EventsSender.Send(DToServerEvent.JoinLobby, lobbyId);
        }

        private void OnJoinCustomLobbyWithPasswordMethod(object[] args)
        {
            int lobbyId = (int)args[0];
            string password = (string)args[1];
            EventsSender.Send(DToServerEvent.JoinLobbyWithPassword, lobbyId, password);
        }

        private void OnJoinedCustomLobbiesMenuMethod(object[] args)
        {
            EventsSender.Send(DToServerEvent.JoinedCustomLobbiesMenu);
        }

        private void OnLeaveLobbyMethod(object[] args)
        {
            Browser.Angular.Main.ToggleTeamChoiceMenu(false);
            Scoreboard.ReleasedScoreboardKey();
            EventsSender.Send(DToServerEvent.LeaveLobby);
        }

        private void OnLeftCustomLobbiesMenuMethod(object[] args)
        {
            EventsSender.Send(DToServerEvent.LeftCustomLobbiesMenu);
        }

        private void OnLeftSupportRequestMethod(object[] args)
        {
            EventsSender.Send(DToServerEvent.LeftSupportRequest);
        }

        private void OnLeftSupportRequestsListMethod(object[] args)
        {
            EventsSender.Send(DToServerEvent.LeftSupportRequestsList);
        }

        private void LoadApplicationDataForAdminBrowserMethod(object[] args)
        {
            int applicationId = Convert.ToInt32(args[0]);
            EventsSender.Send(DToServerEvent.LoadApplicationDataForAdmin, applicationId);
        }

        private void OnLoadMapNamesToLoadForMapCreatorMethod(object[] args)
        {
            EventsSender.Send(DToServerEvent.LoadMapNamesToLoadForMapCreator);
        }

        private void OnLoadMyMapForMapCreatorMethod(object[] args)
        {
            string mapName = (string)args[0];
            EventsSender.Send(DToServerEvent.LoadMapForMapCreator, mapName);
        }

        private void OnLoadUserpanelDataBrowserMethod(object[] args)
        {
            EUserpanelLoadDataType type = (EUserpanelLoadDataType)Convert.ToInt32(args[0]);
            switch (type)
            {
                case EUserpanelLoadDataType.SettingsRest:
                    Browser.Angular.Main.LoadUserpanelData((int)type, Serializer.ToBrowser(Settings.PlayerSettings));
                    break;
                default:
                    EventsSender.Send(DToServerEvent.LoadUserpanelData, (int)type);
                    break;
            }

            Settings.RevertTempSettings();
        }

        private void OnMapCreatorHighlightPosMethod(object[] args)
        {
            ObjectPlacing.HighlightObjectWithId((int)args[0]);
        }

        private void OnMapCreatorShowObjectMethod(object[] args)
        {
            string objName = (string)args[0];
            ObjectPreview.ShowObject(objName);
        }

        private void OnMapCreatorShowVehicleMethod(object[] args)
        {
            string vehicleName = (string)args[0];
            VehiclePreview.ShowVehicle(vehicleName);
        }

        private void OnMapCreatorStartNewBrowserMethod(object[] args)
        {
            Sync.SyncStartNewMap();
        }

        private void OnMapCreatorStartObjectChoiceMethod(object[] args)
        {
            Browser.Angular.MapCreatorObjectChoice.Start();
        }

        private void OnMapCreatorStopObjectPreviewMethod(object[] args)
        {
            ObjectPreview.Stop();
            Browser.Angular.MapCreatorObjectChoice.Stop();
        }

        private void OnMapCreatorStartVehicleChoiceMethod(object[] args)
        {
            Browser.Angular.MapCreatorVehicleChoice.Start();
        }

        private void OnMapCreatorStopVehiclePreviewMethod(object[] args)
        {
            VehiclePreview.Stop();
            Browser.Angular.MapCreatorVehicleChoice.Stop();
        }

        private void OnColorSettingChangeMethod(object[] args)
        {
            string color = (string)args[0];
            EUserpanelSettingKey dataSetting = (EUserpanelSettingKey)(Convert.ToInt32(args[1]));

            switch (dataSetting)
            {
                case EUserpanelSettingKey.MapBorderColor:
                    Settings.MapBorderColor = CommonUtils.GetColorFromHtmlRgba(color);
                    break;
            }
            
        }

        private void OnTryLoginMethod(object[] args)
        {
            string username = (string)args[0];
            string password = (string)args[1];
            RegisterLogin.TryLogin(username, password);
        }

        private void OnTryRegisterMethod(object[] args)
        {
            string username = (string)args[0];
            string password = (string)args[1];
            string email = (string)args[2];
            RegisterLogin.TryRegister(username, password, email);
        }

        private void OnChatLoadedMethod(object[] args)
        {
            ChatManager.Loaded();
        }

        private void OnCommandUsedMethod(object[] args)
        {
            ChatManager.CloseChatInput();
            string msg = (string)args[0];
            if (msg == "checkshoot")
            {
                if (Bomb.BombOnHand || !Round.InFight)
                    Chat.Output("Shooting is blocked. Reason: " + (Round.InFight ? "bomb" : (!Bomb.BombOnHand ? "round" : "both")));
                else
                    Chat.Output("Shooting is not blocked.");
                return;
            } 

            EventsSender.Send(DToServerEvent.CommandUsed, msg);
        }

        private void OnChatUsedMethod(object[] args)
        {
            ChatManager.CloseChatInput();
            string msg = (string)args[0];
            int chatTypeNumber = (int)(args[1]);
            EventsSender.Send(DToServerEvent.LobbyChatMessage, msg, chatTypeNumber);
        }

        private void OnCloseChatMethod(object[] args)
        {
            ChatManager.CloseChatInput();
        }

        private void OnLanguageChangeMethod(object[] args)
        {
            var languageID = Convert.ToInt32(args[0]);
            if (!System.Enum.IsDefined(typeof(ELanguage), languageID))
                return;

            Settings.LanguageEnum = (ELanguage)languageID;
        }

        private void OnRejectInvitationMethod(object[] args)
        {
            int invitationId = (int)args[0];
            EventsSender.Send(DToServerEvent.RejectInvitation, invitationId);
        }

        private void OnRemoveMapMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            if (mapId == 0 || EventsSender.Send(DToServerEvent.RemoveMap, mapId))  
            {
                ObjectsManager.Stop();
                ObjectsManager.Start();
            }
            else
                Browser.Angular.Main.ShowCooldown();

        }
        private void OnRemoveMapCreatorPositionMethod(object[] args)
        {
            int posId = Convert.ToInt32(args[0]);
            ObjectsManager.Delete(posId);
        }

        private void OnRemoveMapCreatorTeamNumberMethod(object[] args)
        {
            int teamNumber = Convert.ToInt32(args[0]);
            ObjectsManager.DeleteTeamObjects(teamNumber);
        }

        private void OnSaveMapCreatorDataMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(DToServerEvent.SaveMapCreatorData, json))
                Browser.Angular.Main.SaveMapCreatorReturn((int)EMapCreateError.Cooldown);
        }

        private void OnSaveSettingsMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(DToServerEvent.SaveSettings, json))
                Browser.Angular.Main.ShowCooldown();
                
        }

        private void OnSendApplicationMethod(object[] args)
        {
            string json = (string)args[0];
            EventsSender.Send(DToServerEvent.SendApplication, json);
        }

        private void OnSendMapCreatorDataMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(DToServerEvent.SendMapCreatorData, json))
                Browser.Angular.Main.SendMapCreatorReturn((int)EMapCreateError.Cooldown);
        }

        private void OnSetSupportRequestClosedBrowserMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            bool closed = Convert.ToBoolean(args[1]);
            EventsSender.Send(DToServerEvent.SetSupportRequestClosed, requestId, closed);
        }

        private void OnSendSupportRequestMethod(object[] args)
        {
            string json = (string)args[0];
            EventsSender.Send(DToServerEvent.SendSupportRequest, json);
        }

        private void OnSendSupportRequestMessageMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            string message = (string)args[1];
            EventsSender.Send(DToServerEvent.SendSupportRequestMessage, requestId, message);
        }

        private void OnStartMapCreatorPosPlacingMethod(object[] args)
        {
            EMapCreatorPositionType type = (EMapCreatorPositionType)(int)args[0];
            object editingTeamIndexOrObjectName = args[1];
            ObjectPlacing.StartNewPlacing(type, editingTeamIndexOrObjectName);
        }

        private void OnSyncRegisterLoginLanguageTextsMethod(object[] args)
        {
            RegisterLogin.SyncLanguage();
        }

        private void OnTeleportToXYMethod(object[] args)
        {
            float x = Convert.ToSingle(args[0]);
            float y = Convert.ToSingle(args[1]);
            float z = 0;
            Misc.GetGroundZFor3dCoord(x, y, 9000, ref z, false);
            Player.LocalPlayer.Position = new Vector3(x, y, z + 0.3f);
        }

        private void OnTeleportToPositionRotationMethod(object[] args)
        {
            float x = Convert.ToSingle(args[0]);
            float y = Convert.ToSingle(args[1]);
            float z = Convert.ToSingle(args[2]);
            float rot = Convert.ToSingle(args[3]);
            if (TDSCamera.ActiveCamera != null)
            {
                TDSCamera.ActiveCamera.Position = new Vector3(x, y, z);
                TDSCamera.ActiveCamera.Rotation = new Vector3(0, 0, rot);
            }
            Player.LocalPlayer.Position = new Vector3(x, y, z);
            Player.LocalPlayer.SetHeading(rot);
        }

        private void OnToggleMapFavoriteMethod(object[] args)
        {
            int mapId = (int)args[0];
            bool isFavorite = Convert.ToBoolean(args[1]);
            EventsSender.Send(DToServerEvent.ToggleMapFavouriteState, mapId, isFavorite);
        }

        private void OnGetHashedPassword(object[] args)
        {
            string pw = Convert.ToString(args[0]);
            Browser.Angular.Main.GetHashedPasswordReturn(CommonUtils.HashPWClient(pw));
        }

        private void OnFromBrowserEventMethod(object[] args)
        {
            /* string eventName = (string) args[0];
            object[] restArgs = new object[args.Length - 1];
            if (args.Length > 1)
            {
                args.CopyTo(restArgs, 1);
            } */
            EventsSender.SendFromBrowser(args);
        }
    }
}
