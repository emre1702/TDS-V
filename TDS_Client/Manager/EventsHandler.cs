using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using TDS_Client.Default;
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
using static RAGE.Events;
using Cam = RAGE.Game.Cam;
using Control = RAGE.Game.Control;
using Player = RAGE.Elements.Player;
using Script = RAGE.Events.Script;

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
            FreeCam.Render();
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
            Add(DToClientEvent.AddCustomLobby, OnAddCustomLobbyMethod);
            Add(DToClientEvent.AddMapToVoting, OnAddMapToVotingMethod);
            Add(DToClientEvent.AmountInFightSync, OnAmountInFightSyncMethod);
            Add(DToClientEvent.BombPlanted, OnBombPlantedMethod);
            Add(DToClientEvent.BombNotOnHand, OnBombNotOnHandMethod);
            Add(DToClientEvent.BombOnHand, OnBombOnHandMethod);
            Add(DToClientEvent.BombDetonated, OnBombDetonatedMethod);
            Add(DToClientEvent.ClearTeamPlayers, OnClearTeamPlayersMethod);
            Add(DToClientEvent.CountdownStart, OnCountdownStartMethod);
            Add(DToClientEvent.CreateCustomLobbyResponse, OnCreateCustomLobbyResponseMethod);
            Add(DToClientEvent.Death, OnDeathMethod);
            //Add(DToClientEvent.HitOpponent, OnHitOpponentMethod);
            Add(DToClientEvent.JoinLobby, OnJoinLobbyMethod);
            Add(DToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            Add(DToClientEvent.LeaveCustomLobbyMenu, OnLeaveCustomLobbyMenuMethod);
            Add(DToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            Add(DToClientEvent.LoadMapFavourites, OnLoadMapFavouritesMethod);
            Add(DToClientEvent.LoadMySavedMap, OnLoadMySavedMapFromServerMethod);
            Add(DToClientEvent.LoadMySavedMapNames, OnLoadMySavedMapNamesFromServerMethod);
            Add(DToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(DToClientEvent.MapChange, OnMapChangeMethod);
            Add(DToClientEvent.MapClear, OnMapClearMethod);
            Add(DToClientEvent.MapsListRequest, OnMapListRequestMethod);
            Add(DToClientEvent.MapVotingSyncOnPlayerJoin, OnMapVotingSyncOnPlayerJoinMethod);
            Add(DToClientEvent.PlayCustomSound, OnPlayCustomSoundMethod);
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
            Add(DToClientEvent.RemoveCustomLobby, OnRemoveCustomLobbyMethod);
            Add(DToClientEvent.RoundStart, OnRoundStartMethod);
            Add(DToClientEvent.RoundEnd, OnRoundEndMethod);
            Add(DToClientEvent.SaveMapCreatorReturn, OnSaveMapCreatorReturnMethod);
            Add(DToClientEvent.SendMapCreatorReturn, OnSendMapCreatorReturnMethod);
            Add(DToClientEvent.SetAssistsForRoundStats, OnSetAssistsForRoundStatsMethod);
            Add(DToClientEvent.SetDamageForRoundStats, OnSetDamageForRoundStatsMethod);
            Add(DToClientEvent.SetKillsForRoundStats, OnSetKillsForRoundStatsMethod);
            Add(DToClientEvent.SetMapVotes, OnSetMapVotesMethod);
            Add(DToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            Add(DToClientEvent.StopBombPlantDefuse, OnStopBombPlantDefuseMethod);
            Add(DToClientEvent.StopRoundStats, OnStopRoundStatsMethod);
            Add(DToClientEvent.SyncAllCustomLobbies, OnSyncAllCustomLobbiesMethod);
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
            Players.Load(ClientUtils.GetTriggeredPlayersList((string)args[1]));
            Team.CurrentLobbyTeams = JsonConvert.DeserializeObject<SyncedTeamDataDto[]>((string)args[2]);
            Lobby.Lobby.Joined(settings);
            DiscordManager.Update();
            MainBrowser.HideRoundEndReason();
        }

        private void OnJoinSameLobbyMethod(object[] args)
        {
            Chat.Output($"JoinSameLobby: {args.Length}");
            if (args.Length > 0)
            {
                Chat.Output(args[0] == null ? "is null" : "not null");
                Chat.Output(args[0].ToString());
                Chat.Output(args[0] is Player ? "is Player" : "is not Player");
                Chat.Output(args[0].GetType().Name);
            }
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
            Chat.Output(player.Name + " has been added");
            Players.Load(player);
            VoiceManager.AddPlayer(player);
        }

        private void OnLeaveCustomLobbyMenuMethod(object[] args)
        {
            Angular.LeaveCustomLobbyMenu();
        }

        private void OnLeaveSameLobbyMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
            Players.Remove(player);
            VoiceManager.RemovePlayer(player);
        }

        private void OnLoadMapFavouritesMethod(object[] args)
        {
            string mapFavoritesJson = (string)args[0];
            Angular.LoadFavoriteMaps(mapFavoritesJson);
        }

        private void OnLoadMySavedMapFromServerMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.LoadMySavedMap(json);
        }

        private void OnLoadMySavedMapNamesFromServerMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.LoadMySavedMapNames(json);
        }

        private void OnMapChangeMethod(object[] args)
        {
            Graphics.StopScreenEffect(DEffectName.DEATHFAILMPIN);
            Cam.SetCamEffect(0);
            Cam.DoScreenFadeIn(Settings.MapChooseTime);
            MapInfo.SetMapInfo((string)args[0]);
            MainBrowser.HideRoundEndReason();
            var maplimit = JsonConvert.DeserializeObject<Position4DDto[]>((string)args[1]);
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

        private void OnCreateCustomLobbyResponseMethod(object[] args)
        {
            string errorOrEmpty = (string)args[0];
            Angular.CreateCustomLobbyReturn(errorOrEmpty);
        }

        private void OnRoundStartMethod(object[] args)
        {
            Cam.DoScreenFadeIn(50);
            LobbyCam.StopCountdown();
            Countdown.End();
            Round.IsSpectator = (bool)args[0];
            Round.InFight = !Round.IsSpectator;
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
            Angular.ResetMapVoting();
            string reason = (string)args[0];
            int mapId = (int)args[1];
            MainBrowser.ShowRoundEndReason(reason, mapId);
        }

        private void OnSaveMapCreatorReturnMethod(object[] args)
        {
            int err = (int)args[0];
            Angular.SaveMapCreatorReturn(err);
        }

        private void OnSendMapCreatorReturnMethod(object[] args)
        {
            int err = (int)args[0];
            Angular.SendMapCreatorReturn(err);
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
            bool willSpawn = (bool)args[3];
            if (player == Player.LocalPlayer)
            {
                if (!willSpawn)
                {
                    Round.InFight = false;
                    Bomb.Reset();
                }
                else
                {
                    if (!Round.IsSpectator)
                        MapLimitManager.Start();
                    Bomb.BombOnHand = false;
                }
            }

            if (!willSpawn)
                RoundInfo.OnePlayerDied((int)args[1]);
            string killinfoStr = (string)args[2];
            MainBrowser.AddKillMessage(killinfoStr);
        }

        private void OnPlayerGotBombMethod(object[] args)
        {
            Bomb.LocalPlayerGotBomb(JsonConvert.DeserializeObject<Position4DDto[]>((string)args[0]));
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
            Team.ClearSameTeam();
        }

        private void OnPlayerJoinedTeamMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
            Team.AddSameTeam(player);
        }

        private void OnPlayerLeftTeamMethod(object[] args)
        {
            if (args[0] is string)
            {
                MainBrowser.Browser.ExecuteJs($"alert(`Fehler in OnPlayerLeftTeamMethod: {args[0]}`)");
                return;
            }
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
            Team.RemoveSameTeam(player);
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

        private void OnSyncAllCustomLobbiesMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.SyncAllCustomLobbies(json);
        }

        private void OnSyncCurrentMapNameMethod(object[] args)
        {
            MapInfo.SetMapInfo((string)args[0]);
        }

        private void OnAddCustomLobbyMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.AddCustomLobby(json);
        }

        private void OnRemoveCustomLobbyMethod(object[] args)
        {
            int lobbyId = (int)args[0];
            Angular.RemoveCustomLobby(lobbyId);
        }

        private void OnPlayCustomSoundMethod(object[] args)
        {
            string soundName = (string)args[0];
            MainBrowser.PlaySound(soundName);
        }

        private void OnMapVotingSyncOnPlayerJoinMethod(object[] args)
        {
            string mapVotesJson = (string)args[0];
            Angular.LoadMapVoting(mapVotesJson);
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

        private void OnAddMapToVotingMethod(object[] args)
        {
            string mapVoteJson = (string)args[0];
            Angular.AddMapToVoting(mapVoteJson);
        }

        private void OnStartRegisterLoginMethod(object[] args)
        {
            string scname = (string)args[0];
            bool isregistered = (bool)args[1];
            RegisterLogin.Start(scname, isregistered);
        }

        private void OnStopBombPlantDefuseMethod(object[] args)
        {
            Bomb.StopRequestByServer();
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
            Angular.Load();
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.PressedScoreboardKey, Enum.EKeyPressState.Down);
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.ReleasedScoreboardKey, Enum.EKeyPressState.Up);
            Settings.Load();
            VoiceManager.Init();
            Team.Init();
        }

        private void OnSetMapVotesMethod(object[] args)
        {
            int mapId = (int)args[0];
            int amountVotes = (int)args[1];
            Angular.SetMapVotes(mapId, amountVotes);
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
            Team.ClearSameTeam();
            IEnumerable<int> listOfPlayerHandles = JsonConvert.DeserializeObject<IEnumerable<int>>(args[0].ToString());
            foreach (var handle in listOfPlayerHandles)
            {
                Player player = Entities.Players.GetAtHandle(handle);
                if (player != null)
                    Team.AddSameTeam(player);
            }
        }

        #endregion From Server events

        #region From Browser events

        private void AddFromBrowserEvents()
        {
            Add(DFromBrowserEvent.AddMapVote, OnAddMapVoteMethod);
            Add(DFromBrowserEvent.AddRatingToMap, OnAddRatingToMapMethod);
            Add(DFromBrowserEvent.ChooseArenaToJoin, OnChooseArenaToJoinMethod);
            Add(DFromBrowserEvent.ChooseMapCreatorToJoin, OnChooseMapCreatorToJoinMethod);
            Add(DFromBrowserEvent.CloseMapVotingMenu, OnCloseMapVotingMenuMethod);
            Add(DFromBrowserEvent.CreateCustomLobby, OnCreateCustomLobbyMethod);
            Add(DFromBrowserEvent.GetCurrentPositionRotation, OnGetCurrentPositionRotationMethod);
            Add(DFromBrowserEvent.GetVehicle, OnGetVehicleMethod);
            Add(DFromBrowserEvent.JoinCustomLobby, OnJoinCustomLobbyMethod);
            Add(DFromBrowserEvent.JoinCustomLobbyWithPassword, OnJoinCustomLobbyWithPasswordMethod);
            Add(DFromBrowserEvent.JoinedCustomLobbiesMenu, OnJoinedCustomLobbiesMenuMethod);
            Add(DFromBrowserEvent.LeftCustomLobbiesMenu, OnLeftCustomLobbiesMenuMethod);
            Add(DFromBrowserEvent.LoadMySavedMap, OnLoadMySavedMapFromBrowserMethod);
            Add(DFromBrowserEvent.LoadMySavedMapNames, OnLoadMySavedMapsFromBrowserMethod);
            Add(DFromBrowserEvent.TryLogin, OnTryLoginMethod);
            Add(DFromBrowserEvent.TryRegister, OnTryRegisterMethod);
            Add(DFromBrowserEvent.ChatLoaded, OnChatLoadedMethod);
            Add(DFromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
            Add(DFromBrowserEvent.SaveMapCreatorData, OnSaveMapCreatorDataMethod);
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
            bool isSpectator = (bool)args[0];
            Choice.JoinArena(isSpectator);
        }

        private void OnChooseMapCreatorToJoinMethod(object[] args)
        {
            Choice.JoinMapCreator();
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

        private void OnLoadMySavedMapFromBrowserMethod(object[] args)
        {
            string mapName = (string)args[0];
            EventsSender.Send(DToServerEvent.LoadMySavedMap, mapName);
        }

        private void OnLoadMySavedMapsFromBrowserMethod(object[] args)
        {
            EventsSender.Send(DToServerEvent.LoadMySavedMapNames);
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
            var languageID = Convert.ToInt32(args[0]);
            if (!System.Enum.IsDefined(typeof(ELanguage), languageID))
                return;

            Settings.LanguageEnum = (ELanguage)languageID;
        }

        private void OnSaveMapCreatorDataMethod(object[] args)
        {
            string json = (string)args[0];
            if (!EventsSender.Send(DToServerEvent.SaveMapCreatorData, json))
                Angular.SaveMapCreatorReturn((int)EMapCreateError.Cooldown);
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
            EventsSender.Send(DToServerEvent.TeleportToPositionRotation, x, y, z + 0.3, 0);
        }

        private void OnTeleportToPositionRotationMethod(object[] args)
        {
            float x = Convert.ToSingle(args[0]);
            float y = Convert.ToSingle(args[1]);
            float z = Convert.ToSingle(args[2]);
            float rot = Convert.ToSingle(args[3]);
            EventsSender.Send(DToServerEvent.TeleportToPositionRotation, x, y, z, rot);
        }

        private void OnToggleMapFavoriteMethod(object[] args)
        {
            int mapId = (int)args[0];
            bool isFavorite = (bool)args[1];
            EventsSender.Send(DToServerEvent.ToggleMapFavouriteState, mapId, isFavorite);
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