﻿using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Dto.Map.Creator;
using TDS_Common.Enum.Userpanel;
using TDS_Server.Dto.Map;
using static RAGE.Events;
using Cam = RAGE.Game.Cam;
using Control = RAGE.Game.Control;
using Player = RAGE.Elements.Player;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {

        private void AddFromServerEvents()
        {
            Add(DToClientEvent.AddMapToVoting, OnAddMapToVotingMethod);
            Add(DToClientEvent.AmountInFightSync, OnAmountInFightSyncMethod);
            Add(DToClientEvent.ApplySuicideAnimation, OnApplySuicideAnimationMethod);
            Add(DToClientEvent.BombPlanted, OnBombPlantedMethod);
            Add(DToClientEvent.BombNotOnHand, OnBombNotOnHandMethod);
            Add(DToClientEvent.BombOnHand, OnBombOnHandMethod);
            Add(DToClientEvent.BombDetonated, OnBombDetonatedMethod);
            Add(DToClientEvent.ClearTeamPlayers, OnClearTeamPlayersMethod);
            Add(DToClientEvent.CountdownStart, OnCountdownStartMethod);
            Add(DToClientEvent.CreateCustomLobbyResponse, OnCreateCustomLobbyResponseMethod);
            Add(DToClientEvent.Death, OnDeathMethod);
            Add(DToClientEvent.GetSupportRequestData, OnGetSupportRequestDataMethod);
            Add(DToClientEvent.HitOpponent, OnHitOpponentMethod);
            Add(DToClientEvent.JoinLobby, OnJoinLobbyMethod);
            Add(DToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            Add(DToClientEvent.LeaveCustomLobbyMenu, OnLeaveCustomLobbyMenuMethod);
            Add(DToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            Add(DToClientEvent.LoadApplicationDataForAdmin, OnLoadApplicationDataForAdminMethod);
            Add(DToClientEvent.LoadMapFavourites, OnLoadMapFavouritesMethod);
            Add(DToClientEvent.LoadMapForMapCreator, OnLoadMapForMapCreatorServerMethod);
            Add(DToClientEvent.LoadMapNamesToLoadForMapCreator, OnLoadMapNamesToLoadForMapCreatorServerMethod);
            Add(DToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(DToClientEvent.LoadUserpanelData, OnLoadUserpanelDataMethod);
            Add(DToClientEvent.MapChange, OnMapChangeMethod);
            Add(DToClientEvent.MapClear, OnMapClearMethod);
            Add(DToClientEvent.MapsListRequest, OnMapListRequestMethod);
            Add(DToClientEvent.MapVotingSyncOnPlayerJoin, OnMapVotingSyncOnPlayerJoinMethod);
            Add(DToClientEvent.PlayCustomSound, OnPlayCustomSoundMethod);
            Add(DToClientEvent.PlayerGotBomb, OnPlayerGotBombMethod);
            Add(DToClientEvent.PlayerPlantedBomb, OnPlayerPlantedBombMethod);
            Add(DToClientEvent.PlayerSpectateMode, OnPlayerSpectateModeMethod);
            Add(DToClientEvent.PlayerJoinedTeam, OnPlayerJoinedTeamMethod);
            Add(DToClientEvent.PlayerLeftTeam, OnPlayerLeftTeamMethod);
            Add(DToClientEvent.PlayerRespawned, OnPlayerRespawnedMethod);
            Add(DToClientEvent.PlayerTeamChange, OnPlayerTeamChangeMethod);
            Add(DToClientEvent.PlayerWeaponChange, OnPlayerWeaponChangeMethod);
            Add(DToClientEvent.RegisterLoginSuccessful, OnRegisterLoginSuccessfulMethod);
            Add(DToClientEvent.RemoveCustomLobby, OnRemoveCustomLobbyMethod);
            Add(DToClientEvent.RemoveSyncedPlayerDatas, OnRemoveSyncedPlayerDatasMethod);
            Add(DToClientEvent.RoundStart, OnRoundStartMethod);
            Add(DToClientEvent.RoundEnd, OnRoundEndMethod);
            Add(DToClientEvent.SaveMapCreatorReturn, OnSaveMapCreatorReturnMethod);
            Add(DToClientEvent.SendMapCreatorReturn, OnSendMapCreatorReturnMethod);
            Add(DToClientEvent.SetAssistsForRoundStats, OnSetAssistsForRoundStatsMethod);
            Add(DToClientEvent.SetDamageForRoundStats, OnSetDamageForRoundStatsMethod);
            Add(DToClientEvent.SetKillsForRoundStats, OnSetKillsForRoundStatsMethod);
            Add(DToClientEvent.SetMapVotes, OnSetMapVotesMethod);
            Add(DToClientEvent.SetPlayerData, OnSetPlayerDataMethod);
            Add(DToClientEvent.SetPlayerToSpectatePlayer, OnSetPlayerToSpectatePlayerMethod);
            Add(DToClientEvent.SetSupportRequestClosed, OnSetSupportRequestClosedMethod);
            Add(DToClientEvent.SpectatorReattachCam, OnSpectatorReattachCamMethod);
            Add(DToClientEvent.StartRankingShowAfterRound, OnStartRankingShowAfterRoundMethod);
            Add(DToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            Add(DToClientEvent.StopBombPlantDefuse, OnStopBombPlantDefuseMethod);
            Add(DToClientEvent.StopRoundStats, OnStopRoundStatsMethod);
            Add(DToClientEvent.StopSpectator, OnStopSpectatorMethod);
            Add(DToClientEvent.SyncAllCustomLobbies, OnSyncAllCustomLobbiesMethod);
            Add(DToClientEvent.SyncNewSupportRequestMessage, OnSyncNewSupportRequestMessageMethod);
            Add(DToClientEvent.SyncNewCustomLobby, OnSyncNewCustomLobbyMethod);
            Add(DToClientEvent.SyncPlayerData, OnSyncPlayerDataMethod);
            Add(DToClientEvent.SyncSettings, OnSyncSettingsMethod);
            Add(DToClientEvent.SyncScoreboardData, OnSyncScoreboardDataMethod);
            Add(DToClientEvent.SyncTeamChoiceMenuData, OnSyncTeamChoiceMenuDataMethod);
            Add(DToClientEvent.SyncTeamPlayers, OnSyncTeamPlayersMethod);
            Add(DToClientEvent.ToggleTeamChoiceMenu, OnToggleTeamChoiceMenuMethod);
        }

        private void OnLoadOwnMapRatingsMethod(object[] args)
        {
            string datajson = (string)args[0];
            MainBrowser.OnLoadOwnMapRatings(datajson);
        }

        private void OnLoadUserpanelDataMethod(object[] args)
        {
            int type = (int)args[0];
            string json = (string)args[1];
            Browser.Angular.Main.LoadUserpanelData(type, json);
        }

        private void OnHitOpponentMethod(object[] args)
        {
            ushort remoteId = Convert.ToUInt16(args[0]);
            int damage = (int)args[1];
            DeathmatchInfo.HittedOpponent(ClientUtils.GetPlayerByHandleValue(remoteId), damage);
        }

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
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
            Players.Load(player);
            VoiceManager.SetForPlayer(player);
        }

        private void OnLeaveCustomLobbyMenuMethod(object[] args)
        {
            Browser.Angular.Main.LeaveCustomLobbyMenu();
        }

        private void OnLeaveSameLobbyMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
            string name = (string)args[1];
            Players.Remove(player, name);
        }

        private void OnLoadApplicationDataForAdminMethod(object[] args)
        {
            string json = (string)args[0];
            Browser.Angular.Main.LoadApplicationDataForAdmin(json);
        }

        private void OnLoadMapFavouritesMethod(object[] args)
        {
            string mapFavoritesJson = (string)args[0];
            Browser.Angular.Main.LoadFavoriteMaps(mapFavoritesJson);
        }

        private void OnLoadMapForMapCreatorServerMethod(object[] args)
        {
            string json = (string)args[0];
            Browser.Angular.Main.LoadMapForMapCreator(json);

            var mapCreatorData = JsonConvert.DeserializeObject<MapCreateDataDto>(json);
            ObjectsManager.LoadMap(mapCreatorData);
        }

        private void OnLoadMapNamesToLoadForMapCreatorServerMethod(object[] args)
        {
            string json = (string)args[0];
            Browser.Angular.Main.LoadMapNamesForMapCreator(json);
        }

        private void OnMapChangeMethod(object[] args)
        {
            Graphics.StopScreenEffect(DEffectName.DEATHFAILMPIN);
            Cam.SetCamEffect(0);
            Cam.DoScreenFadeIn(Settings.MapChooseTime);
            MapInfo.SetMapInfo((string)args[0]);
            MainBrowser.HideRoundEndReason();
            var maplimit = JsonConvert.DeserializeObject<List<Position4DDto>>((string)args[1]);
            LobbyCam.SetToMapCenter(JsonConvert.DeserializeObject<Vector3>((string)args[2]));
            Round.InFight = false;
            if (maplimit.Count > 0)
                MapLimitManager.Load(maplimit);
        }

        private void OnMapClearMethod(object[] args)
        {
            Round.Reset(false);
        }

        private void OnCountdownStartMethod(object[] args)
        {
            Ranking.Stop();
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
            Browser.Angular.Main.CreateCustomLobbyReturn(errorOrEmpty);
        }

        private void OnRoundStartMethod(object[] args)
        {
            Round.IsSpectator = (bool)args[0];
            Round.InFight = !Round.IsSpectator;
            Cam.DoScreenFadeIn(50);
            LobbyCam.StopCountdown();
            Countdown.End(args.Length < 2 || Convert.ToUInt64(args[1]) != 0);
            RoundInfo.Start(args.Length >= 2 ? Convert.ToUInt64(args[1]) : 0);
        }

        private void OnRoundEndMethod(object[] args)
        {
            Cam.DoScreenFadeOut(Settings.RoundEndTime / 2);
            Bomb.Stop();
            Round.StopFight();
            Countdown.Stop();
            LobbyCam.StopCountdown();
            RoundInfo.Stop();
            Browser.Angular.Main.ResetMapVoting();
            string reason = (string)args[0];
            int mapId = (int)args[1];
            MainBrowser.ShowRoundEndReason(reason, mapId);
        }

        private void OnSaveMapCreatorReturnMethod(object[] args)
        {
            int err = (int)args[0];
            Browser.Angular.Main.SaveMapCreatorReturn(err);
        }

        private void OnSendMapCreatorReturnMethod(object[] args)
        {
            int err = (int)args[0];
            Browser.Angular.Main.SendMapCreatorReturn(err);
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
            Player player = ClientUtils.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            bool willRespawn = (bool)args[3];
            if (player == Player.LocalPlayer)
            {
                Round.InFight = false;
                Bomb.RestartRound();
            }

            if (!willRespawn)
                RoundInfo.OnePlayerDied((int)args[1]);
            string killinfoStr = (string)args[2];
            MainBrowser.AddKillMessage(killinfoStr);
        }

        private void OnGetSupportRequestDataMethod(object[] args)
        {
            string json = (string)args[0];
            Browser.Angular.Main.GetSupportRequestData(json);
        }

        private void OnPlayerGotBombMethod(object[] args)
        {
            Bomb.LocalPlayerGotBomb(JsonConvert.DeserializeObject<Position4DDto[]>((string)args[0]));
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

        private void OnPlayerRespawnedMethod(object[] args)
        {
            if (!Round.IsSpectator)
                Round.InFight = true;
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
                if (!team.IsSpectator)
                    team.AmountPlayers = list[team.Index - 1];
            }
            RoundInfo.RefreshAllTeamTexts();
        }

        private void OnApplySuicideAnimationMethod(object[] args)
        {
            ushort playerHandle = Convert.ToUInt16(args[0]);
            string animName = (string)args[1];
            float animTime = Convert.ToSingle(args[2]);

            Player player = ClientUtils.GetPlayerByHandleValue(playerHandle);
            if (player == null)
                return;

            SuicideAnim.ApplyAnimation(player, animName, animTime);
        }

        private void OnSyncAllCustomLobbiesMethod(object[] args)
        {
            string json = (string)args[0];
            Browser.Angular.Main.SyncAllCustomLobbies(json);
        }

        private void OnSyncNewCustomLobbyMethod(object[] args)
        {
            string json = (string)args[0];
            Browser.Angular.Main.AddCustomLobby(json);
        }

        private void OnSyncNewSupportRequestMessageMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            string messageJson = (string)args[1];

            Browser.Angular.Main.SyncNewSupportRequestMessage(requestId, messageJson);
        }

        private void OnSyncPlayerDataMethod(object[] args)
        {
            PlayerDataSync.AppendDictionaryFromServer((string)args[0]);
        }

        private void OnSyncSettingsMethod(object[] args)
        {
            string json = (string)args[0];
            var settings = JsonConvert.DeserializeObject<SyncedPlayerSettingsDto>(json);
            Settings.LoadUserSettings(settings);
            Browser.Angular.Main.LoadUserpanelData((int)EUserpanelLoadDataType.Settings, json);
        }

        private void OnRemoveCustomLobbyMethod(object[] args)
        {
            int lobbyId = (int)args[0];
            Browser.Angular.Main.RemoveCustomLobby(lobbyId);
        }

        private void OnRemoveSyncedPlayerDatasMethod(object[] args)
        {
            ushort playerHandle = Convert.ToUInt16(args[0]);
            PlayerDataSync.RemovePlayerData(playerHandle);
        }

        private void OnPlayCustomSoundMethod(object[] args)
        {
            string soundName = (string)args[0];
            MainBrowser.PlaySound(soundName);
        }

        private void OnMapVotingSyncOnPlayerJoinMethod(object[] args)
        {
            string mapVotesJson = (string)args[0];
            Browser.Angular.Main.LoadMapVoting(mapVotesJson);
        }

        private void OnMapListRequestMethod(object[] args)
        {
            //List<SyncedMapDataDto> maps = JsonConvert.DeserializeObject<List<SyncedMapDataDto>>((string)args[0]);
            MapManager.LoadMapList((string)args[0]);
        }

        private void OnAddMapToVotingMethod(object[] args)
        {
            string mapVoteJson = (string)args[0];
            Browser.Angular.Main.AddMapToVoting(mapVoteJson);
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

        private void OnStopSpectatorMethod(object[] args)
        {
            Spectate.SpectatingEntity = null;
        }

        private void OnRegisterLoginSuccessfulMethod(object[] args)
        {
            Settings.LoadSyncedSettings(JsonConvert.DeserializeObject<SyncedServerSettingsDto>(args[0].ToString()));
            Settings.LoadUserSettings(JsonConvert.DeserializeObject<SyncedPlayerSettingsDto>(args[1].ToString()));
            RegisterLogin.Stop();
            MainBrowser.Load();
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.PressedScoreboardKey, EKeyPressState.Down);
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.ReleasedScoreboardKey, EKeyPressState.Up);
            Settings.Load();
            VoiceManager.Init();
            Team.Init();
            Crouching.Init();

            BindManager.Add(EKey.F3, MapManager.ToggleMenu);
            BindManager.Add(EKey.U, Userpanel.Toggle);
        }

        private void OnSetMapVotesMethod(object[] args)
        {
            int mapId = (int)args[0];
            int amountVotes = (int)args[1];
            Browser.Angular.Main.SetMapVotes(mapId, amountVotes);
        }

        private void OnSetPlayerDataMethod(object[] args)
        {
            PlayerDataSync.HandleDataFromServer(args);
        }

        private void OnSetPlayerToSpectatePlayerMethod(object[] args)
        {
            Player target = ClientUtils.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            if (target != null)
                Spectate.SpectatingEntity = target;
        }

        private void OnSetSupportRequestClosedMethod(object[] args)
        {
            int requestId = Convert.ToInt32(args[0]);
            bool closed = Convert.ToBoolean(args[1]);
            Browser.Angular.Main.SetSupportRequestClosed(requestId, closed);
        }

        private void OnSpectatorReattachCamMethod(object[] args)
        {
            if (Spectate.SpectatingEntity != null)
            {
                CameraManager.SpectateCam.Spectate(Spectate.SpectatingEntity);
            }
        }

        private void OnStartRankingShowAfterRoundMethod(object[] args)
        {
            Ranking.Start((string)args[0], Convert.ToUInt16(args[1]), Convert.ToUInt16(args[2]), Convert.ToUInt16(args[3]));
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

        private void OnSyncTeamChoiceMenuDataMethod(object[] args)
        {
            string teamsJson = (string)args[0];
            bool mixTeamsAfterRound = Convert.ToBoolean(args[1]);
            Browser.Angular.Main.SyncTeamChoiceMenuData(teamsJson, mixTeamsAfterRound);
            Scoreboard.PressedScoreboardKey();
            CursorManager.Visible = true;
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

        private void OnToggleTeamChoiceMenuMethod(object[] args)
        {
            bool boolean = Convert.ToBoolean(args[0]);
            CursorManager.Visible = boolean;
            if (boolean)
                Scoreboard.PressedScoreboardKey();
            else 
                Scoreboard.ReleasedScoreboardKey();
            Browser.Angular.Main.ToggleTeamChoiceMenu(boolean);
        }

    }
}
