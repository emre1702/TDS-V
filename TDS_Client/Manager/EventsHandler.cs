﻿using RAGE;
using RAGE.Elements;
using Cam = RAGE.Game.Cam;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw.Scaleform;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using static RAGE.Events;
using Newtonsoft.Json;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Draw;
using Pad = RAGE.Game.Pad;
using Control = RAGE.Game.Control;
using RAGE.Ui;
using TDS_Common.Enum;
using System.Linq;
using System;
using Newtonsoft.Json.Linq;
using TDS_Client.Instance.Draw.Dx;

namespace TDS_Client.Manager
{
    class EventsHandler : Script
    {
        public EventsHandler()
        {
            LoadOnStart();
            AddRAGEEvents();
            AddToClientEvents();
            AddFromBrowserEvents();
        }

        #region Load on start
        private void LoadOnStart()
        {
            Settings.Load();
            Dx.RefreshResolution();
        }
        #endregion Load on start

        #region RAGE events
        private void AddRAGEEvents()
        {
            Tick += OnTickMethod;
            OnPlayerWeaponShot += OnPlayerWeaponShotMethod;
            OnPlayerSpawn += OnPlayerSpawnMethod;
            OnPlayerDeath += OnPlayerDeathMethod;
            OnBrowserDomReady += OnBrowserDomReadyMethod;
        }

        private void OnTickMethod(List<TickNametagData> nametags)
        {
            ScaleformMessage.Render();
            Dx.RenderAll();
            if (Bomb.CheckPlantDefuseOnTick)
                Bomb.CheckPlantDefuse();
            if (RoundInfo.RefreshOnTick)
                RoundInfo.RefreshTime();
            Damagesys.ShowBloodscreenIfNecessary();
            if (!Round.InFight)
            {
                Pad.DisableControlAction(0, (int)Control.Attack, true);
                Pad.DisableControlAction(0, (int)Control.Attack2, true);
            }
        }

        private void OnPlayerWeaponShotMethod(Vector3 targetPos, Player target, CancelEventArgs cancel)
        {
            Damagesys.OnWeaponShot(targetPos, target, cancel);
        }

        private void OnPlayerSpawnMethod(CancelEventArgs cancel)
        {
            Death.PlayerSpawn();
        }

        private void OnPlayerDeathMethod(Player player, uint reason, Player killer, CancelEventArgs cancel)
        {
            Death.PlayerDeath(player);
        }

        private void OnBrowserDomReadyMethod(HtmlWindow browser)
        {
            if (browser == RegisterLogin.Browser)
                RegisterLogin.SendDataToBrowser();
            else if (browser == Choice.Browser)
                Choice.SyncLanguageTexts();

            /*
             * else if ( browser === mainbrowserdata.browser )
                loadOrderNamesInBrowser( JSON.stringify( getLang( "orders" ) ) );

            else if ( browser === mapcreatordata.browser )
                mapcreatordata.browser.execute( "loadLanguage (`" + JSON.stringify( getLang( "mapcreator_menu" ) ) + "`);" );*/
        }
        #endregion RAGE events

        #region From Server events 
        private void AddToClientEvents()
        {
            Add(DToClientEvent.AddVoteToMap, OnAddVoteToMapServerMethod);
            Add(DToClientEvent.AmountInFightSync, OnAmountInFightSyncMethod);
            Add(DToClientEvent.BombPlanted, OnBombPlantedMethod);
            Add(DToClientEvent.BombDetonated, OnBombDetonatedMethod);
            Add(DToClientEvent.CountdownStart, OnCountdownStartMethod);
            Add(DToClientEvent.Death, OnDeathMethod);
            Add(DToClientEvent.HitOpponent, OnHitOpponentMethod);
            Add(DToClientEvent.JoinLobby, OnJoinLobbyMethod);
            Add(DToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            Add(DToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            Add(DToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(DToClientEvent.MapChange, OnMapChangeMethod);
            Add(DToClientEvent.MapsListRequest, OnMapListRequestMethod);
            Add(DToClientEvent.MapVotingSyncOnPlayerJoin, OnMapVotingSyncOnPlayerJoinMethod);
            Add(DToClientEvent.PlayerAdminLevelChange, OnPlayerAdminLevelChangeMethod);
            Add(DToClientEvent.PlayerGotBomb, OnPlayerGotBombMethod);
            Add(DToClientEvent.PlayerMoneyChange, OnPlayerMoneyChangeMethod);
            Add(DToClientEvent.PlayerPlantedBomb, OnPlayerPlantedBombMethod);
            Add(DToClientEvent.PlayerSpectateMode, OnPlayerSpectateModeMethod);
            Add(DToClientEvent.PlayerTeamChange, OnPlayerTeamChangeMethod);
            Add(DToClientEvent.RegisterLoginSuccessful, OnRegisterLoginSuccessfulMethod);
            Add(DToClientEvent.RoundStart, OnRoundStartMethod);
            Add(DToClientEvent.RoundEnd, OnRoundEndMethod);
            Add(DToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            Add(DToClientEvent.SyncCurrentMapName, OnSyncCurrentMapNameMethod);
            Add(DToClientEvent.SyncScoreboardData, OnSyncScoreboardDataMethod);
        }

        private void OnLoadOwnMapRatingsMethod(object[] args)
        {
            string datajson = (string) args[0];
            MainBrowser.OnLoadOwnMapRatings(datajson);
        }

        private void OnHitOpponentMethod(object[] args)
        {
            DeathmatchInfo.HittedOpponent();
        }

        private void OnJoinLobbyMethod(object[] args)
        {
            SyncedLobbySettingsDto settings = JsonConvert.DeserializeObject<SyncedLobbySettingsDto>((string)args[0]);
            Settings.LoadSyncedLobbySettings(settings);
            Players.Load(ClientUtils.GetTriggeredPlayersList(args[1]));
            Team.CurrentLobbyTeams = JsonConvert.DeserializeObject<SyncedTeamDataDto[]>((string)args[2]);
            Lobby.Lobby.Joined(settings);
        }

        private void OnJoinSameLobbyMethod(object[] args)
        {
            Players.Load((Player)args[0]);
        }

        private void OnLeaveSameLobbyMethod(object[] args)
        {
            Players.Remove((Player)args[0]);
        }

        private void OnMapChangeMethod(object[] args)
        {
            Cam.DoScreenFadeIn(Settings.MapChooseTime);
            MapInfo.SetMapInfo((string)args[0]);
            MainBrowser.HideRoundEndReason();
            var maplimit = JsonConvert.DeserializeObject<List<Vector3>>((string)args[1]);
            if (maplimit.Count > 0)
                MapLimitManager.Load(maplimit);
            CameraManager.SetToMapCenter(JsonConvert.DeserializeObject<Vector3>((string)args[2]));
            Round.InFight = false;
        }

        private void OnCountdownStartMethod(object[] args)
        {
            CameraManager.Stop();
            uint mstimetoplayer = (uint)(Settings.CountdownTime * 1000 * 0.9);
            if (args == null)
            {
                Countdown.Start();
                CameraManager.SetGoTowardsPlayer(mstimetoplayer);
            }
            else
            {
                uint remainingms = (uint)args[0];
                Countdown.StartAfterwards(remainingms);
                uint timeofcountdowncameraisatplayer = Settings.CountdownTime * 1000 - mstimetoplayer;
                if (remainingms < timeofcountdowncameraisatplayer)
                    CameraManager.SetGoTowardsPlayer(remainingms);
                else
                    CameraManager.SetGoTowardsPlayer(remainingms - timeofcountdowncameraisatplayer);
            }
            if (Round.IsSpectator)
            {
                Spectate.Start();
                MainBrowser.HideRoundEndReason();
            }

        }

        private void OnRoundStartMethod(object[] args)
        {
            Cam.DoScreenFadeIn(50);
            CameraManager.StopCountdown();
            Countdown.End();
            Round.IsSpectator = (bool)args[0];
            if (!Round.IsSpectator)
            {
                MapLimitManager.Start();
                Round.InFight = true;
            }
            RoundInfo.Start(args.Length >= 2 ? (ulong)args[1] : 0);
        }

        private void OnRoundEndMethod(object[] args)
        {
            Cam.DoScreenFadeOut(Settings.RoundEndTime / 2);
            Bomb.Reset();
            Round.Reset(false);
            Countdown.Stop();
            CameraManager.StopCountdown();
            RoundInfo.Stop();
            MainBrowser.ClearMapVotingsInBrowser();
            MainBrowser.ShowRoundEndReason((string)args[0], MapInfo.CurrentMap);
        }

        private void OnPlayerSpectateModeMethod(object[] args)
        {
            Round.IsSpectator = true;
            Spectate.Start();
        }

        private void OnDeathMethod(object[] args)
        {
            Player player = (Player)args[0];
            if (player == Player.LocalPlayer)
            {
                Round.InFight = false;
                Bomb.Reset();
            }
            RoundInfo.OnePlayerDied((int)args[1], (string)args[2]);
        }

        private void OnPlayerGotBombMethod(object[] args)
        {
            Bomb.LocalPlayerGotBomb(JsonConvert.DeserializeObject<List<Vector3>>((string)args[0]));
        }

        private void OnPlayerMoneyChangeMethod(object[] args)
        {
            AccountData.Money = (int)args[0];
            MainBrowser.LoadMoney();
        }

        private void OnPlayerPlantedBombMethod(object[] args)
        {
            Bomb.LocalPlayerPlantedBomb();
        }

        private void OnBombPlantedMethod(object[] args)
        {
            Bomb.BombPlanted(JsonConvert.DeserializeObject<Vector3>((string)args[0]), (bool)args[1]);
        }

        private void OnBombDetonatedMethod(object[] args)
        {
            Bomb.Detonate();
        }

        private void OnPlayerTeamChangeMethod(object[] args)
        {
            // setVoiceChatRoom( teamUID );
        }

        private void OnAmountInFightSyncMethod(object[] args)
        {
            SyncedTeamPlayerAmountDto[] list = JsonConvert.DeserializeObject<SyncedTeamPlayerAmountDto[]>((string)args[0]);
            foreach (var team in Team.CurrentLobbyTeams)
            {
                if (team.Index != 0)
                    team.AmountPlayers = list[team.Index-1];
            }
            RoundInfo.RefreshAllTeamTexts();
        }

        private void OnSyncCurrentMapNameMethod(object[] args)
        {
            MapInfo.SetMapInfo((string)args[0]);
        }

        private void OnMapVotingSyncOnPlayerJoinMethod(object[] args)
        {
            MainBrowser.LoadMapVotingsForMapBrowser((string)args[0]);
        }

        private void OnPlayerAdminLevelChangeMethod(object[] args)
        {
            AccountData.AdminLevel = (int)args[0];
        }

        private void OnMapListRequestMethod(object[] args)
        {
            //List<SyncedMapDataDto> maps = JsonConvert.DeserializeObject<List<SyncedMapDataDto>>((string)args[0]);
            MapVoting.LoadMapList((string)args[0]);
        }

        private void OnAddVoteToMapServerMethod(object[] args)
        {
            string newmapname = (string)args[0];
            string oldmapname = (string)args[1];
            MapVoting.AddVote(newmapname, oldmapname);
        }

        private void OnStartRegisterLoginMethod(object[] args)
        {
            string scname = (string)args[0];
            bool isregistered = (bool)args[1];
            RegisterLogin.Start(scname, isregistered);
        }

        private void OnRegisterLoginSuccessfulMethod(object[] args)
        {
            int adminlvl = (int)args[0];
            AccountData.AdminLevel = adminlvl;
            Settings.LoadSyncedSettings(JsonConvert.DeserializeObject<SyncedSettingsDto>(args[1].ToString()));
            RegisterLogin.Stop();
            MainBrowser.Load();
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.PressedScoreboardKey, Enum.EKeyPressState.Down);
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.ReleasedScoreboardKey, Enum.EKeyPressState.Up);
        }

        private void OnSyncScoreboardDataMethod(object[] args)
        {
            bool inmainmenu = args.Length == 1;
            if (inmainmenu)
            {
                var list = JsonConvert.DeserializeObject<List<SyncedScoreboardMainmenuLobbyDataDto>>((string)args[0]);
                Scoreboard.AddMainmenuData(list);
            }
            else
            {
                var playerlist = JsonConvert.DeserializeObject<List<SyncedScoreboardLobbyDataDto>>((string)args[0]);
                var lobbylist = JsonConvert.DeserializeObject<List<SyncedScoreboardMainmenuLobbyDataDto>>((string)args[1]);
                Scoreboard.AddLobbyData(playerlist, lobbylist);
            }
            
        }
        #endregion From Server events

        #region From Browser events 
        private void AddFromBrowserEvents()
        {
            Add(DFromBrowserEvent.AddRatingToMap, OnAddRatingToMapMethod);
            Add(DFromBrowserEvent.ChooseLobbyToJoin, OnChooseLobbyToJoinMethod);
            Add(DFromBrowserEvent.SyncChoiceLanguageTexts, OnSyncChoiceLanguageTextsMethod);
            Add(DFromBrowserEvent.CloseMapVotingMenu, OnCloseMapVotingMenuMethod);
            Add(DFromBrowserEvent.AddMapToVoting, OnAddMapToVotingMethod);
            Add(DFromBrowserEvent.AddVoteToMap, OnAddVoteToMapMethod);
            Add(DFromBrowserEvent.TryLogin, OnTryLoginMethod);
            Add(DFromBrowserEvent.TryRegister, OnTryRegisterMethod);
            Add(DFromBrowserEvent.ChatLoaded, OnChatLoadedMethod);
            Add(DFromBrowserEvent.CommandUsed, OnCommandUsedMethod);
            Add(DFromBrowserEvent.ChatInputToggled, OnChatInputToggledMethod);
            Add(DFromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            Add(DFromBrowserEvent.SyncRegisterLoginLanguageTexts, OnSyncRegisterLoginLanguageTextsMethod);
        }

        private void OnAddRatingToMapMethod(object[] args)
        {
            string currentmap = (string)args[0];
            int rating = (int)args[1];
            MainBrowser.OnSendMapRating(currentmap, rating);
        }

        private void OnChooseLobbyToJoinMethod(object[] args)
        {
            Choice.JoinLobby((int)args[0], (int)args[1]);
        }

        private void OnSyncChoiceLanguageTextsMethod(object[] args)
        {
            Choice.SyncLanguageTexts();
        }

        private void OnCloseMapVotingMenuMethod(object[] args)
        {
            MapVoting.CloseMenu();
        }

        private void OnAddMapToVotingMethod(object[] args)
        {
            string mapname = (string)args[0];
            CallRemote(DToServerEvent.AddMapToVoting, mapname);
        }

        private void OnAddVoteToMapMethod(object[] args)
        {
            string mapname = (string)args[0];
            CallRemote(DToServerEvent.AddVoteToMap, mapname);
        }

        private void OnTryLoginMethod(object[] args)
        {
            string password = (string)args[0];
            RegisterLogin.TryLogin(password);
        }

        private void OnTryRegisterMethod(object[] args)
        {
            string password = (string)args[0];
            string email = (string)args[1];
            RegisterLogin.TryRegister(password, email);
        }

        private void OnChatLoadedMethod(object[] args)
        {
            ChatManager.Loaded();
        }

        private void OnCommandUsedMethod(object[] args)
        {
            string msg = (string)args[0];
            ChatManager.CommandUsed(msg);
        }

        private void OnChatInputToggledMethod(object[] args)
        {
            ChatManager.IsOpen = (bool)args[0];
        }

        private void OnLanguageChangeMethod(object[] args)
        {
            if (!System.Enum.IsDefined(typeof(ELanguage), (int)args[0]))
                return;
            Settings.LanguageEnum = (ELanguage)args[0];
        }

        private void OnSyncRegisterLoginLanguageTextsMethod(object[] args)
        {
            RegisterLogin.SyncLanguage();
        }

        /*// triggered by browser //
        mp.events.add( "onClientToggleMapFavourite", ( mapname, bool ) => {
            if ( bool )
                mapvotingdata.favourites.push( mapname )
            else {
                for ( let i = 0; i < mapvotingdata.favourites.length; ++i ) {
                    if ( mapvotingdata.favourites[i] == mapname )
                        mapvotingdata.favourites.splice( i, 1 );
                }
            }
            mp.storage.data.mapfavourites = JSON.stringify( mapvotingdata.favourites );
        } );*/
        #endregion Browser events 
    }
}