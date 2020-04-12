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
using TDS_Shared.Default;
using TDS_Shared.Enum;
using TDS_Shared.Enum.Userpanel;
using TDS_Shared.Core;
using static RAGE.Events;
using Player = RAGE.Elements.Player;
using Script = RAGE.Events.Script;
using TDS_Client.Data.Defaults;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        private void AdFromBrowserEvents()
        {
            Add(ToServerEvent.AcceptInvitation, OnAcceptInvitationMethod);
            Add(FromBrowserEvent.AddMapVote, OnAddMapVoteMethod);

            
            Add(FromBrowserEvent.CreateCustomLobby, OnCreateCustomLobbyMethod);
            Add(FromBrowserEvent.GetHashedPassword, OnGetHashedPassword);
            Add(ToServerEvent.GetSupportRequestData, OnGetSupportRequestDataBrowserMethod);
            Add(FromBrowserEvent.GetVehicle, OnGetVehicleMethod);
           
            Add(FromBrowserEvent.JoinCustomLobby, OnJoinCustomLobbyMethod);
            Add(FromBrowserEvent.JoinCustomLobbyWithPassword, OnJoinCustomLobbyWithPasswordMethod);
            Add(FromBrowserEvent.JoinedCustomLobbiesMenu, OnJoinedCustomLobbiesMenuMethod);
            Add(FromBrowserEvent.LeftCustomLobbiesMenu, OnLeftCustomLobbiesMenuMethod);
            Add(ToServerEvent.LeftSupportRequest, OnLeftSupportRequestMethod);
            Add(ToServerEvent.LeftSupportRequestsList, OnLeftSupportRequestsListMethod);
            Add(ToServerEvent.LoadApplicationDataForAdmin, LoadApplicationDataForAdminBrowserMethod);
            Add(ToServerEvent.LoadMapNamesToLoadForMapCreator, OnLoadMapNamesToLoadForMapCreatorMethod);
            Add(ToServerEvent.LoadMapForMapCreator, OnLoadMyMapForMapCreatorMethod);
            Add(ToServerEvent.LoadUserpanelData, OnLoadUserpanelDataBrowserMethod);
            Add(FromBrowserEvent.MapCreatorShowVehicle, OnMapCreatorShowVehicleMethod);
            Add(FromBrowserEvent.MapCreatorStopObjectPreview, OnMapCreatorStopObjectPreviewMethod);
            
            Add(FromBrowserEvent.MapCreatorStopVehiclePreview, OnMapCreatorStopVehiclePreviewMethod);
            Add(FromBrowserEvent.OnColorSettingChange, OnColorSettingChangeMethod);
            Add(FromBrowserEvent.TryLogin, OnTryLoginMethod);
            Add(FromBrowserEvent.TryRegister, OnTryRegisterMethod);
            Add(FromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            Add(ToServerEvent.RejectInvitation, OnRejectInvitationMethod);
            Add(ToServerEvent.RemoveMap, OnRemoveMapMethod);
            Add(FromBrowserEvent.RemoveMapCreatorPosition, OnRemoveMapCreatorPositionMethod);
            Add(FromBrowserEvent.RemoveMapCreatorTeamNumber, OnRemoveMapCreatorTeamNumberMethod);
            Add(FromBrowserEvent.SaveMapCreatorData, OnSaveMapCreatorDataMethod);
            Add(ToServerEvent.SaveSettings, OnSaveSettingsMethod);
            Add(ToServerEvent.SendApplication, OnSendApplicationMethod);
            Add(FromBrowserEvent.SendMapCreatorData, OnSendMapCreatorDataMethod);
            Add(FromBrowserEvent.SendMapRating, OnBrowserSendMapRatingMethod);
            Add(ToServerEvent.SetSupportRequestClosed, OnSetSupportRequestClosedBrowserMethod);
            Add(ToServerEvent.SendSupportRequest, OnSendSupportRequestMethod);
            Add(ToServerEvent.SendSupportRequestMessage, OnSendSupportRequestMessageMethod);
            Add(FromBrowserEvent.StartMapCreatorPosPlacing, OnStartMapCreatorPosPlacingMethod);
            Add(FromBrowserEvent.SyncRegisterLoginLanguageTexts, OnSyncRegisterLoginLanguageTextsMethod);
            Add(FromBrowserEvent.TeleportToXY, OnTeleportToXYMethod);
            Add(FromBrowserEvent.TeleportToPositionRotation, OnTeleportToPositionRotationMethod);
            Add(FromBrowserEvent.ToggleMapFavorite, OnToggleMapFavoriteMethod);

            Add(FromBrowserEvent.ChatUsed, OnChatUsedMethod);
            Add(FromBrowserEvent.CommandUsed, OnCommandUsedMethod);

            Add(ToServerEvent.FromBrowserEvent, OnFromBrowserEventMethod);
        }

        private void OnAcceptInvitationMethod(object[] args)
        {
            int invitationId = (int)args[0];
            EventsSender.Send(ToServerEvent.AcceptInvitation, invitationId);
        }

        private void OnAddMapVoteMethod(object[] args)
        {
            int mapId = (int)args[0];
            EventsSender.Send(ToServerEvent.MapVote, mapId);
        }

        private void OnBrowserSendMapRatingMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            int rating = Convert.ToInt32(args[1]);
            EventsSender.Send(ToServerEvent.SendMapRating, mapId, rating);
        }

        private void OnCreateCustomLobbyMethod(object[] args)
        {
            string dataJson = (string)args[0];
            EventsSender.Send(ToServerEvent.CreateCustomLobby, dataJson);
        }

        private void OnGetSupportRequestDataBrowserMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            EventsSender.Send(ToServerEvent.GetSupportRequestData, requestId);
        }

        private void OnGetVehicleMethod(object[] args)
        {
            // convert because if it fails, there will be an error @clientside, not @serverside
            FreeroamVehicleType vehType = (FreeroamVehicleType)(int)args[0];
            EventsSender.Send(ToServerEvent.GetVehicle, (int)vehType);
        }

       
        private void OnJoinCustomLobbyMethod(object[] args)
        {
            int lobbyId = (int)args[0];
            EventsSender.Send(ToServerEvent.JoinLobby, lobbyId);
        }

        private void OnJoinCustomLobbyWithPasswordMethod(object[] args)
        {
            int lobbyId = (int)args[0];
            string password = (string)args[1];
            EventsSender.Send(ToServerEvent.JoinLobbyWithPassword, lobbyId, password);
        }

        private void OnJoinedCustomLobbiesMenuMethod(object[] args)
        {
            EventsSender.Send(ToServerEvent.JoinedCustomLobbiesMenu);
        }

        private void OnLeftCustomLobbiesMenuMethod(object[] args)
        {
            EventsSender.Send(ToServerEvent.LeftCustomLobbiesMenu);
        }

        private void OnLeftSupportRequestMethod(object[] args)
        {
            EventsSender.Send(ToServerEvent.LeftSupportRequest);
        }

        private void OnLeftSupportRequestsListMethod(object[] args)
        {
            EventsSender.Send(ToServerEvent.LeftSupportRequestsList);
        }

        private void LoadApplicationDataForAdminBrowserMethod(object[] args)
        {
            int applicationId = Convert.ToInt32(args[0]);
            EventsSender.Send(ToServerEvent.LoadApplicationDataForAdmin, applicationId);
        }

        private void OnLoadMapNamesToLoadForMapCreatorMethod(object[] args)
        {
            EventsSender.Send(ToServerEvent.LoadMapNamesToLoadForMapCreator);
        }

        private void OnLoadMyMapForMapCreatorMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            EventsSender.Send(ToServerEvent.LoadMapForMapCreator, mapId);
        }

        private void OnLoadUserpanelDataBrowserMethod(object[] args)
        {
            UserpanelLoadDataType type = (UserpanelLoadDataType)Convert.ToInt32(args[0]);
            switch (type)
            {
                case UserpanelLoadDataType.SettingsRest:
                    Browser.Angular.Main.LoadUserpanelData((int)type, Serializer.ToBrowser(Settings.PlayerSettings));
                    break;
                default:
                    EventsSender.Send(ToServerEvent.LoadUserpanelData, (int)type);
                    break;
            }

            Settings.RevertTempSettings();
        }

        private void OnColorSettingChangeMethod(object[] args)
        {
            string color = (string)args[0];
            EUserpanelSettingKey dataSetting = (EUserpanelSettingKey)(Convert.ToInt32(args[1]));

            switch (dataSetting)
            {
                case EUserpanelSettingKey.MapBorderColor:
                    Settings.MapBorderColor = SharedUtils.GetColorFromHtmlRgba(color) ?? Settings.MapBorderColor;
                    break;
                case EUserpanelSettingKey.NametagDeadColor:
                    Settings.NametagDeadColor = SharedUtils.GetColorFromHtmlRgba(color);
                    break;
                case EUserpanelSettingKey.NametagHealthEmptyColor:
                    Settings.NametagHealthEmptyColor = SharedUtils.GetColorFromHtmlRgba(color) ?? Settings.NametagHealthEmptyColor;
                    break;
                case EUserpanelSettingKey.NametagHealthFullColor:
                    Settings.NametagHealthFullColor = SharedUtils.GetColorFromHtmlRgba(color) ?? Settings.NametagHealthFullColor;
                    break;
                case EUserpanelSettingKey.NametagArmorEmptyColor:
                    Settings.NametagArmorEmptyColor = SharedUtils.GetColorFromHtmlRgba(color);
                    break;
                case EUserpanelSettingKey.NametagArmorFullColor:
                    Settings.NametagArmorFullColor = SharedUtils.GetColorFromHtmlRgba(color) ?? Settings.NametagArmorFullColor;
                    break;
            }
            
        }


        

        private void OnRejectInvitationMethod(object[] args)
        {
            int invitationId = (int)args[0];
            EventsSender.Send(ToServerEvent.RejectInvitation, invitationId);
        }

        private void OnRemoveMapMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            if (mapId == 0 || EventsSender.Send(ToServerEvent.RemoveMap, mapId))  
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
            if (!EventsSender.Send(ToServerEvent.SaveMapCreatorData, json))
                Browser.Angular.Main.SaveMapCreatorReturn((int)MapCreateError.Cooldown);
        }

        private void OnSaveSettingsMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(ToServerEvent.SaveSettings, json))
                Browser.Angular.Main.ShowCooldown();
                
        }

        private void OnSendApplicationMethod(object[] args)
        {
            string json = (string)args[0];
            EventsSender.Send(ToServerEvent.SendApplication, json);
        }

        private void OnSendMapCreatorDataMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(ToServerEvent.SendMapCreatorData, json))
                Browser.Angular.Main.SendMapCreatorReturn((int)MapCreateError.Cooldown);
        }

        private void OnSetSupportRequestClosedBrowserMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            bool closed = Convert.ToBoolean(args[1]);
            EventsSender.Send(ToServerEvent.SetSupportRequestClosed, requestId, closed);
        }

        private void OnSendSupportRequestMethod(object[] args)
        {
            string json = (string)args[0];
            EventsSender.Send(ToServerEvent.SendSupportRequest, json);
        }

        private void OnSendSupportRequestMessageMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            string message = (string)args[1];
            EventsSender.Send(ToServerEvent.SendSupportRequestMessage, requestId, message);
        }

        private void OnStartMapCreatorPosPlacingMethod(object[] args)
        {
            MapCreatorPositionType type = (MapCreatorPositionType)(int)args[0];
            object editingTeamIndexOrObjectName = args[1];
            ObjectPlacing.StartNewPlacing(type, editingTeamIndexOrObjectName);
        }

        private void OnSyncRegisterLoginLanguageTextsMethod(object[] args)
        {
            RegisterLoginHandler.SyncLanguage();
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
            EventsSender.Send(ToServerEvent.ToggleMapFavouriteState, mapId, isFavorite);
        }

        private void OnGetHashedPassword(object[] args)
        {
            string pw = Convert.ToString(args[0]);
            Browser.Angular.Main.GetHashedPasswordReturn(SharedUtils.HashPWClient(pw));
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
