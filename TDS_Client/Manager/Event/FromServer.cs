using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Dto.Map;
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
            Add(DToClientEvent.LoadAllCommands, OnLoadAllCommandsServerMethod);
            Add(DToClientEvent.LoadAllRules, OnLoadAllRulesServerMethod);
            Add(DToClientEvent.LoadMapFavourites, OnLoadMapFavouritesMethod);
            Add(DToClientEvent.LoadMapForMapCreator, OnLoadMapForMapCreatorServerMethod);
            Add(DToClientEvent.LoadMapNamesToLoadForMapCreator, OnLoadMapNamesToLoadForMapCreatorServerMethod);
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
            Add(DToClientEvent.PlayerRespawned, OnPlayerRespawnedMethod);
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
            Add(DToClientEvent.SetPlayerToSpectatePlayer, OnSetPlayerToSpectatePlayerMethod);
            Add(DToClientEvent.SpectatorReattachCam, OnSpectatorReattachCamMethod);
            Add(DToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            Add(DToClientEvent.StopBombPlantDefuse, OnStopBombPlantDefuseMethod);
            Add(DToClientEvent.StopRoundStats, OnStopRoundStatsMethod);
            Add(DToClientEvent.StopSpectator, OnStopSpectatorMethod);
            Add(DToClientEvent.SyncAllCustomLobbies, OnSyncAllCustomLobbiesMethod);
            Add(DToClientEvent.SyncNewCustomLobby, OnSyncNewCustomLobbyMethod);
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
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
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

        private void OnLoadAllCommandsServerMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.LoadAllCommands(json);
        }

        private void OnLoadAllRulesServerMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.LoadAllRules(json);
        }

        private void OnLoadMapFavouritesMethod(object[] args)
        {
            string mapFavoritesJson = (string)args[0];
            Angular.LoadFavoriteMaps(mapFavoritesJson);
        }

        private void OnLoadMapForMapCreatorServerMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.LoadMapForMapCreator(json);
        }

        private void OnLoadMapNamesToLoadForMapCreatorServerMethod(object[] args)
        {
            string json = (string)args[0];
            Angular.LoadMapNamesForMapCreator(json);
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
            Round.IsSpectator = (bool)args[0];
            Round.InFight = !Round.IsSpectator;
            Cam.DoScreenFadeIn(50);
            LobbyCam.StopCountdown();
            Countdown.End(args.Length < 2 || (ulong)args[1] != 0);
            RoundInfo.Start(args.Length >= 2 ? (ulong)args[1] : 0);
        }

        private void OnRoundEndMethod(object[] args)
        {
            Cam.DoScreenFadeOut(Settings.RoundEndTime / 2);
            Bomb.Stop();
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

        private void OnSyncNewCustomLobbyMethod(object[] args)
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

        private void OnStopSpectatorMethod(object[] args)
        {
            Spectate.SpectatingEntity = null;
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

        private void OnSetPlayerToSpectatePlayerMethod(object[] args)
        {
            Player target = ClientUtils.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            if (target != null)
                Spectate.SpectatingEntity = target;
        }

        private void OnSpectatorReattachCamMethod(object[] args)
        {
            if (Spectate.SpectatingEntity != null)
            {
                CameraManager.SpectateCam.Spectate(Spectate.SpectatingEntity);
            }

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
            Angular.SyncTeamChoiceMenuData(teamsJson, Settings.MixTeamsAfterRound);
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
            Angular.ToggleTeamChoiceMenu(boolean);
        }

    }
}
