using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using RAGE.NUI;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Draw.Scaleform;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Dto.Map;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;
using TDS_Common.Manager.Utility;
using static RAGE.Events;
using Cam = RAGE.Game.Cam;
using Control = RAGE.Game.Control;

namespace TDS_Client.Manager
{
    internal class EventsHandler : Script
    {
        public EventsHandler()
        {
            LoadOnStart();
            AddRAGEEvents();
            AddToClientEvents();
            AddFromBrowserEvents();
            Workaround.AddEvents();
        }

        #region Load on start

        private void LoadOnStart()
        {
            Settings.Load();
            Dx.RefreshResolution();
            VoiceManager.Init();
            Team.Init();
        }

        #endregion Load on start

        #region RAGE events

        private void AddRAGEEvents()
        {
            Tick += OnTickMethod;
            OnPlayerWeaponShot += OnPlayerWeaponShotMethod;
            OnPlayerSpawn += OnPlayerSpawnMethod;
            OnPlayerDeath += OnPlayerDeathMethod;
            OnPlayerQuit += OnPlayerQuitMethod;
        }

        private void OnTickMethod(List<TickNametagData> nametags)
        {
            ScaleformMessage.Render();
            Dx.RenderAll();
            if (Bomb.DataChanged)
            {
                if (Bomb.CheckPlantDefuseOnTick)
                    Bomb.CheckPlantDefuse();
                Bomb.UpdatePlantDefuseProgress();
                if (Bomb.BombOnHand)
                    ClientUtils.DisableAttack();
            }
            if (RoundInfo.RefreshOnTick)
                RoundInfo.RefreshTime();
            if (Round.InFight)
            {
                Damagesys.ShowBloodscreenIfNecessary();
                FloatingDamageInfo.UpdateAllPositions();
            }
            else
                ClientUtils.DisableAttack();
            ChatManager.OnUpdate();
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

        private void OnPlayerQuitMethod(Player player)
        {
        }

        #endregion RAGE events

        #region From Server events

        private void AddToClientEvents()
        {
            Add(DToClientEvent.AddVoteToMap, OnAddVoteToMapServerMethod);
            Add(DToClientEvent.AmountInFightSync, OnAmountInFightSyncMethod);
            Add(DToClientEvent.BombPlanted, OnBombPlantedMethod);
            Add(DToClientEvent.BombNotOnHand, OnBombNotOnHandMethod);
            Add(DToClientEvent.BombOnHand, OnBombOnHandMethod);
            Add(DToClientEvent.BombDetonated, OnBombDetonatedMethod);
            Add(DToClientEvent.ClearTeamPlayers, OnClearTeamPlayersMethod);
            Add(DToClientEvent.CountdownStart, OnCountdownStartMethod);
            Add(DToClientEvent.Death, OnDeathMethod);
            //Add(DToClientEvent.HitOpponent, OnHitOpponentMethod);
            Add(DToClientEvent.JoinLobby, OnJoinLobbyMethod);
            Add(DToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            Add(DToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            Add(DToClientEvent.LoadMapFavourites, OnLoadMapFavouritesMethod);
            Add(DToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(DToClientEvent.MapChange, OnMapChangeMethod);
            Add(DToClientEvent.MapClear, OnMapClearMethod);
            Add(DToClientEvent.MapsListRequest, OnMapListRequestMethod);
            Add(DToClientEvent.MapVotingSyncOnPlayerJoin, OnMapVotingSyncOnPlayerJoinMethod);
            Add(DToClientEvent.PlayerAdminLevelChange, OnPlayerAdminLevelChangeMethod);
            Add(DToClientEvent.PlayerGotBomb, OnPlayerGotBombMethod);
            Add(DToClientEvent.PlayerMoneyChange, OnPlayerMoneyChangeMethod);
            Add(DToClientEvent.PlayerPlantedBomb, OnPlayerPlantedBombMethod);
            Add(DToClientEvent.PlayerSpectateMode, OnPlayerSpectateModeMethod);
            Add(DToClientEvent.PlayerJoinedTeam, OnPlayerJoinedTeamMethod);
            Add(DToClientEvent.PlayerLeftTeam, OnPlayerLeftTeamMethod);
            Add(DToClientEvent.PlayerTeamChange, OnPlayerTeamChangeMethod);
            Add(DToClientEvent.PlayerWeaponChange, OnPlayerWeaponChangeMethod);
            Add(DToClientEvent.RegisterLoginSuccessful, OnRegisterLoginSuccessfulMethod);
            Add(DToClientEvent.RoundStart, OnRoundStartMethod);
            Add(DToClientEvent.RoundEnd, OnRoundEndMethod);
            Add(DToClientEvent.SetAssistsForRoundStats, OnSetAssistsForRoundStatsMethod);
            Add(DToClientEvent.SetDamageForRoundStats, OnSetDamageForRoundStatsMethod);
            Add(DToClientEvent.SetKillsForRoundStats, OnSetKillsForRoundStatsMethod);
            Add(DToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            Add(DToClientEvent.StopRoundStats, OnStopRoundStatsMethod);
            Add(DToClientEvent.SyncCurrentMapName, OnSyncCurrentMapNameMethod);
            Add(DToClientEvent.SyncScoreboardData, OnSyncScoreboardDataMethod);
            Add(DToClientEvent.SyncTeamPlayers, OnSyncTeamPlayersMethod);
        }

        private void OnLoadOwnMapRatingsMethod(object[] args)
        {
            string datajson = (string)args[0];
            MainBrowser.OnLoadOwnMapRatings(datajson);
        }

        /*private void OnHitOpponentMethod(object[] args)
        {
            DeathmatchInfo.HittedOpponent();
        }*/

        // Join always means we also left another lobby (except on login)
        private void OnJoinLobbyMethod(object[] args)
        {
            SyncedLobbySettingsDto settings = JsonConvert.DeserializeObject<SyncedLobbySettingsDto>((string)args[0]);
            Settings.LoadSyncedLobbySettings(settings);
            Players.Load(ClientUtils.GetTriggeredPlayersList(args[1]));
            Team.CurrentLobbyTeams = JsonConvert.DeserializeObject<SyncedTeamDataDto[]>((string)args[2]);
            Lobby.Lobby.Joined(settings);
            DiscordManager.Update();
            MainBrowser.HideRoundEndReason();
        }

        private void OnJoinSameLobbyMethod(object[] args)
        {
            Players.Load((Player)args[0]);
        }

        private void OnLeaveSameLobbyMethod(object[] args)
        {
            Player player = (Player)args[0];
            Players.Remove(player);
            VoiceManager.RemovePlayer(player);
        }

        private void OnLoadMapFavouritesMethod(object[] args)
        {
            string mapFavouritesJson = (string)args[0];
            MapManager.LoadedMapFavourites(mapFavouritesJson);
        }

        private void OnMapChangeMethod(object[] args)
        {
            Cam.DoScreenFadeIn(Settings.MapChooseTime);
            MapInfo.SetMapInfo((string)args[0]);
            MainBrowser.HideRoundEndReason();
            var maplimit = JsonConvert.DeserializeObject<MapPositionDto[]>((string)args[1]);
            if (maplimit.Length > 0)
                MapLimitManager.Load(maplimit);
            LobbyCam.SetToMapCenter(JsonConvert.DeserializeObject<Vector3>((string)args[2]));
            Round.InFight = false;
        }

        private void OnMapClearMethod(object[] args)
        {
            Round.Reset(false);
        }

        private void OnCountdownStartMethod(object[] args)
        {
            LobbyCam.Stop();
            int mstimetoplayer = (int)Math.Ceiling(Settings.CountdownTime * 1000 * 0.9);
            if (args == null)
            {
                Countdown.Start();
                LobbyCam.SetGoTowardsPlayer(mstimetoplayer);
            }
            else
            {
                int remainingms = (int)args[0];
                Countdown.StartAfterwards(remainingms);
                int timeofcountdowncameraisatplayer = Settings.CountdownTime * 1000 - mstimetoplayer;
                if (remainingms < timeofcountdowncameraisatplayer)
                    LobbyCam.SetGoTowardsPlayer(remainingms);
                else
                    LobbyCam.SetGoTowardsPlayer(remainingms - timeofcountdowncameraisatplayer);
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
            LobbyCam.StopCountdown();
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
            Round.StopFight();
            Countdown.Stop();
            LobbyCam.StopCountdown();
            RoundInfo.Stop();
            MainBrowser.ClearMapVotingsInBrowser();
            MainBrowser.ShowRoundEndReason((string)args[0], MapInfo.CurrentMap);
        }

        private void OnSetAssistsForRoundStatsMethod(object[] args)
        {
            RoundInfo.CurrentAssists = (int)args[0];
        }

        private void OnSetDamageForRoundStatsMethod(object[] args)
        {
            RoundInfo.CurrentDamage = (int)args[0];
        }

        private void OnSetKillsForRoundStatsMethod(object[] args)
        {
            RoundInfo.CurrentKills = (int)args[0];
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
            Bomb.LocalPlayerGotBomb(JsonConvert.DeserializeObject<MapPositionDto[]>((string)args[0]));
        }

        private void OnPlayerMoneyChangeMethod(object[] args)
        {
            AccountData.Money = (int)args[0];
            MoneyDisplay.Refresh();
        }

        private void OnPlayerPlantedBombMethod(object[] args)
        {
            Bomb.LocalPlayerPlantedBomb();
        }

        private void OnBombPlantedMethod(object[] args)
        {
            Bomb.BombPlanted(JsonConvert.DeserializeObject<Vector3>((string)args[0]), (bool)args[1], args.Length > 2 ? (int?)args[2] : null);
        }

        private void OnBombNotOnHandMethod(object[] args)
        {
            Bomb.BombOnHand = false;
        }

        private void OnBombOnHandMethod(object[] args)
        {
            Bomb.BombOnHand = true;
        }

        private void OnBombDetonatedMethod(object[] args)
        {
            Bomb.Detonate();
        }

        private void OnClearTeamPlayersMethod(object[] args)
        {
            Team.SameTeamPlayers.Clear();
        }

        private void OnPlayerJoinedTeamMethod(object[] args)
        {
            Player player = (Player)args[0];
            Team.SameTeamPlayers.Add(player);
        }

        private void OnPlayerLeftTeamMethod(object[] args)
        {
            Player player = (Player)args[0];
            Team.SameTeamPlayers.Remove(player);
        }

        private void OnPlayerTeamChangeMethod(object[] args)
        {
            Team.CurrentTeamName = (string)args[0];
            DiscordManager.Update();
        }

        private void OnPlayerWeaponChangeMethod(object[] args)
        {
            //int weaponHash = (int)args[0];
            int damage = (int)args[1];
            Damagesys.CurrentWeaponDamage = damage;
        }

        private void OnAmountInFightSyncMethod(object[] args)
        {
            SyncedTeamPlayerAmountDto[] list = JsonConvert.DeserializeObject<SyncedTeamPlayerAmountDto[]>((string)args[0]);
            foreach (var team in Team.CurrentLobbyTeams)
            {
                if (team.Index != 0)
                    team.AmountPlayers = list[team.Index - 1];
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
            MapManager.LoadMapList((string)args[0]);
        }

        private void OnAddVoteToMapServerMethod(object[] args)
        {
            string newmapname = (string)args[0];
            string oldmapname = args.Length > 1 ? (string)args[1] : null;
            MapManager.AddVote(newmapname, oldmapname);
        }

        private void OnStartRegisterLoginMethod(object[] args)
        {
            string scname = (string)args[0];
            bool isregistered = (bool)args[1];
            RegisterLogin.Start(scname, isregistered);
        }

        private void OnStopRoundStatsMethod(object[] args)
        {
            RoundInfo.StopDeathmatchInfo();
        }

        private void OnRegisterLoginSuccessfulMethod(object[] args)
        {
            int adminlvl = (int)args[0];
            AccountData.AdminLevel = adminlvl;
            Settings.LoadSyncedSettings(JsonConvert.DeserializeObject<SyncedServerSettingsDto>(args[1].ToString()));
            Settings.LoadUserSettings(JsonConvert.DeserializeObject<SyncedPlayerSettingsDto>(args[2].ToString()));
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

        private void OnSyncTeamPlayersMethod(object[] args)
        {
            Team.SameTeamPlayers.Clear();
            IEnumerable<int> listOfPlayerHandles = JsonConvert.DeserializeObject<IEnumerable<int>>(args[0].ToString());
            foreach (var handle in listOfPlayerHandles)
            {
                Player player = Entities.Players.GetAtHandle(handle);
                if (player != null)
                    Team.SameTeamPlayers.Add(player);
            }
        }

        #endregion From Server events

        #region From Browser events

        private void AddFromBrowserEvents()
        {
            Add(DFromBrowserEvent.AddRatingToMap, OnAddRatingToMapMethod);
            Add(DFromBrowserEvent.ChooseLobbyToJoin, OnChooseLobbyToJoinMethod);
            Add(DFromBrowserEvent.CloseMapVotingMenu, OnCloseMapVotingMenuMethod);
            Add(DFromBrowserEvent.MapVote, OnMapVoteMethod);
            Add(DFromBrowserEvent.TryLogin, OnTryLoginMethod);
            Add(DFromBrowserEvent.TryRegister, OnTryRegisterMethod);
            Add(DFromBrowserEvent.ChatLoaded, OnChatLoadedMethod);
            Add(DFromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            Add(DFromBrowserEvent.SendMapRating, OnBrowserSendMapRatingMethod);
            Add(DFromBrowserEvent.SyncChoiceLanguageTexts, OnSyncChoiceLanguageTextsMethod);
            Add(DFromBrowserEvent.SyncRegisterLoginLanguageTexts, OnSyncRegisterLoginLanguageTextsMethod);
            Add(DFromBrowserEvent.ToggleMapFavouriteState, OnToggleMapFavouriteStateMethod);

            Add(DFromBrowserEvent.ChatUsed, OnChatUsedMethod);
            Add(DFromBrowserEvent.CommandUsed, OnCommandUsedMethod);
            Add(DFromBrowserEvent.CloseChat, OnCloseChatMethod);
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

        private void OnBrowserSendMapRatingMethod(object[] args)
        {
            string mapName = (string)args[0];
            int rating = (int)args[1];
            EventsSender.Send(DToServerEvent.SendMapRating, mapName, rating);
        }

        private void OnSyncChoiceLanguageTextsMethod(object[] args)
        {
            Choice.SyncLanguageTexts();
        }

        private void OnCloseMapVotingMenuMethod(object[] args)
        {
            MapManager.CloseMenu();
        }

        private void OnMapVoteMethod(object[] args)
        {
            string mapname = (string)args[0];
            EventsSender.Send(DToServerEvent.MapVote, mapname);
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
            ChatManager.CloseChatInput();
            string msg = (string)args[0];
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
            var languageID = Convert.ToByte(args[0]);
            if (!System.Enum.IsDefined(typeof(ELanguage), languageID))
                return;
            
            Settings.LanguageEnum = (ELanguage)languageID;
        }

        private void OnSyncRegisterLoginLanguageTextsMethod(object[] args)
        {
            RegisterLogin.SyncLanguage();
        }

        private void OnToggleMapFavouriteStateMethod(object[] args)
        {
            string mapname = (string)args[0];
            bool isfavourite = (bool)args[1];
            EventsSender.Send(DToServerEvent.ToggleMapFavouriteState, mapname, isfavourite);
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

        #endregion From Browser events
    }
}