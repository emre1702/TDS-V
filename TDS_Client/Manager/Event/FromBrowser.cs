using Newtonsoft.Json;
using RAGE;
using RAGE.Game;
using System;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Enum.Userpanel;
using static RAGE.Events;
using Player = RAGE.Elements.Player;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        private void AddFromBrowserEvents()
        {
            Add(DFromBrowserEvent.AddMapCreatorPosition, OnAddMapCreatorPositionMethod);
            Add(DFromBrowserEvent.AddMapVote, OnAddMapVoteMethod);
            Add(DFromBrowserEvent.AddRatingToMap, OnAddRatingToMapMethod);
            Add(DFromBrowserEvent.ChooseArenaToJoin, OnChooseArenaToJoinMethod);
            Add(DFromBrowserEvent.ChooseMapCreatorToJoin, OnChooseMapCreatorToJoinMethod);
            Add(DToServerEvent.ChooseTeam, OnChooseTeamMethod);
            Add(DFromBrowserEvent.CloseMapVotingMenu, OnCloseMapVotingMenuMethod);
            Add(DFromBrowserEvent.CloseUserpanel, OnCloseUserpanelMethod);
            Add(DFromBrowserEvent.CreateCustomLobby, OnCreateCustomLobbyMethod);
            Add(DFromBrowserEvent.GetCurrentPositionRotation, OnGetCurrentPositionRotationMethod);
            Add(DFromBrowserEvent.GetVehicle, OnGetVehicleMethod);
            Add(DFromBrowserEvent.JoinCustomLobby, OnJoinCustomLobbyMethod);
            Add(DFromBrowserEvent.JoinCustomLobbyWithPassword, OnJoinCustomLobbyWithPasswordMethod);
            Add(DFromBrowserEvent.JoinedCustomLobbiesMenu, OnJoinedCustomLobbiesMenuMethod);
            Add(DFromBrowserEvent.LeftCustomLobbiesMenu, OnLeftCustomLobbiesMenuMethod);
            Add(DToServerEvent.LoadMapNamesToLoadForMapCreator, OnLoadMapNamesToLoadForMapCreatorMethod);
            Add(DToServerEvent.LoadMapForMapCreator, OnLoadMyMapForMapCreatorMethod);
            Add(DToServerEvent.LoadUserpanelData, OnLoadUserpanelDataBrowserMethod);
            Add(DFromBrowserEvent.TryLogin, OnTryLoginMethod);
            Add(DFromBrowserEvent.TryRegister, OnTryRegisterMethod);
            Add(DFromBrowserEvent.ChatLoaded, OnChatLoadedMethod);
            Add(DFromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            Add(DToServerEvent.RemoveMap, OnRemoveMapMethod);
            Add(DFromBrowserEvent.RemoveMapCreatorPosition, OnRemoveMapCreatorPositionMethod);
            Add(DFromBrowserEvent.SaveMapCreatorData, OnSaveMapCreatorDataMethod);
            Add(DToServerEvent.SaveSettings, OnSaveSettingsMethod);
            Add(DFromBrowserEvent.SendMapCreatorData, OnSendMapCreatorDataMethod);
            Add(DFromBrowserEvent.SendMapRating, OnBrowserSendMapRatingMethod);
            Add(DFromBrowserEvent.SyncRegisterLoginLanguageTexts, OnSyncRegisterLoginLanguageTextsMethod);
            Add(DFromBrowserEvent.TeleportToXY, OnTeleportToXYMethod);
            Add(DFromBrowserEvent.TeleportToPositionRotation, OnTeleportToPositionRotationMethod);
            Add(DFromBrowserEvent.ToggleMapFavorite, OnToggleMapFavoriteMethod);

            Add(DFromBrowserEvent.ChatUsed, OnChatUsedMethod);
            Add(DFromBrowserEvent.CommandUsed, OnCommandUsedMethod);
            Add(DFromBrowserEvent.CloseChat, OnCloseChatMethod);
        }

        private void OnAddMapCreatorPositionMethod(object[] args)
        {
            Lobby.MapCreator.AddedPosition(args);
        }

        private void OnAddMapVoteMethod(object[] args)
        {
            int mapId = (int)args[0];
            EventsSender.Send(DToServerEvent.MapVote, mapId);
        }

        private void OnAddRatingToMapMethod(object[] args)
        {
            string currentmap = (string)args[0];
            int rating = (int)args[1];
            MainBrowser.OnSendMapRating(currentmap, rating);
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

        private void OnGetCurrentPositionRotationMethod(object[] args)
        {
            Angular.SendCurrentPositionRotation();
        }

        private void OnGetVehicleMethod(object[] args)
        {
            // convert because if it fails, there will be an error @clientside, not @serverside
            EFreeroamVehicleType vehType = (EFreeroamVehicleType)(int)args[0];
            EventsSender.Send(DToServerEvent.GetVehicle, (int)vehType);
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

        private void OnLeftCustomLobbiesMenuMethod(object[] args)
        {
            EventsSender.Send(DToServerEvent.LeftCustomLobbiesMenu);
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
                case EUserpanelLoadDataType.Settings:
                    Angular.LoadUserpanelData((int)type, JsonConvert.SerializeObject(Settings.PlayerSettings));
                    break;
                default:
                    EventsSender.Send(DToServerEvent.LoadUserpanelData, (int)type);
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
            }

            EventsSender.Send(DToServerEvent.CommandUsed, msg);
        }

        private void OnChatUsedMethod(object[] args)
        {
            ChatManager.CloseChatInput();
            string msg = (string)args[0];
            bool isDirty = (bool)args[1];
            EventsSender.Send(DToServerEvent.LobbyChatMessage, msg, isDirty);
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

        private void OnRemoveMapMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            if (!EventsSender.Send(DToServerEvent.RemoveMap, mapId))
                Angular.ShowCooldown();
        }
        private void OnRemoveMapCreatorPositionMethod(object[] args)
        {
            EMapCreatorPositionType type = (EMapCreatorPositionType)Convert.ToInt32(args[0]);
            int index = Convert.ToInt32(args[1]);
            int teamNumber = args.Length > 2 ? Convert.ToInt32(args[2]) : 0;
            Lobby.MapCreator.RemovedPosition(type, index, teamNumber);
        }

        private void OnSaveMapCreatorDataMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(DToServerEvent.SaveMapCreatorData, json))
                Angular.SaveMapCreatorReturn((int)EMapCreateError.Cooldown);
        }

        private void OnSaveSettingsMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(DToServerEvent.SaveSettings, json)) 
                Angular.ShowCooldown();
                
        }

        private void OnSendMapCreatorDataMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(DToServerEvent.SendMapCreatorData, json))
                Angular.SendMapCreatorReturn((int)EMapCreateError.Cooldown);
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
            Player.LocalPlayer.Position = new Vector3(x, y, z);
            Player.LocalPlayer.SetHeading(rot);
        }

        private void OnToggleMapFavoriteMethod(object[] args)
        {
            int mapId = (int)args[0];
            bool isFavorite = (bool)args[1];
            EventsSender.Send(DToServerEvent.ToggleMapFavouriteState, mapId, isFavorite);
        }
    }
}
