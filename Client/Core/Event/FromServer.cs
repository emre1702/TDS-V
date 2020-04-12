using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using TDS_Shared.Core;
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
            Add(ToClientEvent.AddMapToVoting, OnAddMapToVotingMethod);
            Add(ToClientEvent.AmountInFightSync, OnAmountInFightSyncMethod);
            Add(ToClientEvent.ApplySuicideAnimation, OnApplySuicideAnimationMethod);
            Add(ToClientEvent.BombPlanted, OnBombPlantedMethod);
            Add(ToClientEvent.BombNotOnHand, OnBombNotOnHandMethod);
            Add(ToClientEvent.BombOnHand, OnBombOnHandMethod);
            Add(ToClientEvent.BombDetonated, OnBombDetonatedMethod);
            Add(ToClientEvent.ClearTeamPlayers, OnClearTeamPlayersMethod);
            Add(ToClientEvent.CountdownStart, OnCountdownStartMethod);
            Add(ToClientEvent.CreateCustomLobbyResponse, OnCreateCustomLobbyResponseMethod);
            Add(ToClientEvent.Death, OnDeathMethod);
            Add(ToClientEvent.ExplodeHead, OnExplodeHeadMethod);
            Add(ToClientEvent.GetSupportRequestData, OnGetSupportRequestDataMethod);
            Add(ToClientEvent.HitOpponent, OnHitOpponentMethod);
            Add(ToClientEvent.JoinLobby, OnJoinLobbyMethod);
            Add(ToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            Add(ToClientEvent.LeaveCustomLobbyMenu, OnLeaveCustomLobbyMenuMethod);
            Add(ToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            Add(ToClientEvent.LoadApplicationDataForAdmin, OnLoadApplicationDataForAdminMethod);
            Add(ToClientEvent.LoadMapFavourites, OnLoadMapFavouritesMethod);
            Add(ToClientEvent.LoadMapForMapCreator, OnLoadMapForMapCreatorServerMethod);
            Add(ToClientEvent.LoadMapNamesToLoadForMapCreator, OnLoadMapNamesToLoadForMapCreatorServerMethod);
            Add(ToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(ToClientEvent.LoadUserpanelData, OnLoadUserpanelDataMethod);
            Add(ToClientEvent.MapChange, OnMapChangeMethod);
            Add(ToClientEvent.MapClear, OnMapClearMethod);
            Add(ToClientEvent.MapCreatorRequestAllObjectsForPlayer, OnMapCreatorRequestAllObjectsForPlayerMethod);
            Add(ToClientEvent.MapCreatorStartNewMap, OnMapCreatorStartNewMapMethod);
            Add(ToClientEvent.MapCreatorSyncAllObjects, OnMapCreatorSyncAllObjectsMethod);
            Add(ToClientEvent.MapCreatorSyncData, OnMapCreatorSyncDataMethod);
            Add(ToClientEvent.MapCreatorSyncFixLastId, OnMapCreatorSyncFixLastIdMethod);
            Add(ToClientEvent.MapCreatorSyncNewObject, OnMapCreatorSyncNewObjectMethod);
            Add(ToClientEvent.MapCreatorSyncObjectPosition, OnMapCreatorSyncObjectPositionMethod);
            Add(ToClientEvent.MapCreatorSyncObjectRemove, MapCreatorSyncObjectRemoveMethod);
            Add(ToClientEvent.MapsListRequest, OnMapListRequestMethod);
            Add(ToClientEvent.MapVotingSyncOnPlayerJoin, OnMapVotingSyncOnPlayerJoinMethod);
            Add(ToClientEvent.PlayCustomSound, OnPlayCustomSoundMethod);
            Add(ToClientEvent.PlayerGotBomb, OnPlayerGotBombMethod);
            Add(ToClientEvent.PlayerPlantedBomb, OnPlayerPlantedBombMethod);
            Add(ToClientEvent.PlayerSpectateMode, OnPlayerSpectateModeMethod);
            Add(ToClientEvent.PlayerJoinedTeam, OnPlayerJoinedTeamMethod);
            Add(ToClientEvent.PlayerLeftTeam, OnPlayerLeftTeamMethod);
            Add(ToClientEvent.PlayerRespawned, OnPlayerRespawnedMethod);
            Add(ToClientEvent.PlayerTeamChange, OnPlayerTeamChangeMethod);
            //Add(ToClientEvent.PlayerWeaponChange, OnPlayerWeaponChangeMethod);
            Add(ToClientEvent.RegisterLoginSuccessful, OnRegisterLoginSuccessfulMethod);
            Add(ToClientEvent.RemoveCustomLobby, OnRemoveCustomLobbyMethod);
            Add(ToClientEvent.RemoveForceStayAtPosition, OnRemoveForceStayAtPositionMethod);
            Add(ToClientEvent.RemoveSyncedPlayerDatas, OnRemoveSyncedPlayerDatasMethod);
            Add(ToClientEvent.RoundStart, OnRoundStartMethod);
            Add(ToClientEvent.RoundEnd, OnRoundEndMethod);
            Add(ToClientEvent.SaveMapCreatorReturn, OnSaveMapCreatorReturnMethod);
            Add(ToClientEvent.SendMapCreatorReturn, OnSendMapCreatorReturnMethod);
            //Add(ToClientEvent.SetAssistsForRoundStats, OnSetAssistsForRoundStatsMethod);
            //Add(ToClientEvent.SetDamageForRoundStats, OnSetDamageForRoundStatsMethod);
            //Add(ToClientEvent.SetKillsForRoundStats, OnSetKillsForRoundStatsMethod);
            Add(ToClientEvent.SetForceStayAtPosition, OnSetForceStayAtPositionMethod);
            Add(ToClientEvent.SetMapVotes, OnSetMapVotesMethod);
            Add(ToClientEvent.SetPlayerData, OnSetPlayerDataMethod);
            Add(ToClientEvent.SetPlayerToSpectatePlayer, OnSetPlayerToSpectatePlayerMethod);
            Add(ToClientEvent.SetSupportRequestClosed, OnSetSupportRequestClosedMethod);
            Add(ToClientEvent.SpectatorReattachCam, OnSpectatorReattachCamMethod);
            Add(ToClientEvent.StartRankingShowAfterRound, OnStartRankingShowAfterRoundMethod);
            Add(ToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            Add(ToClientEvent.StopBombPlantDefuse, OnStopBombPlantDefuseMethod);
            Add(ToClientEvent.StopMapVoting, OnStopMapVotingMethod);
            Add(ToClientEvent.StopRoundStats, OnStopRoundStatsMethod);
            Add(ToClientEvent.StopSpectator, OnStopSpectatorMethod);
            Add(ToClientEvent.SyncAllCustomLobbies, OnSyncAllCustomLobbiesMethod);
            Add(ToClientEvent.SyncNewSupportRequestMessage, OnSyncNewSupportRequestMessageMethod);
            Add(ToClientEvent.SyncNewCustomLobby, OnSyncNewCustomLobbyMethod);
            Add(ToClientEvent.SyncPlayerData, OnSyncPlayerDataMethod);
            Add(ToClientEvent.SyncSettings, OnSyncSettingsMethod);
            Add(ToClientEvent.SyncScoreboardData, OnSyncScoreboardDataMethod);
            Add(ToClientEvent.SyncTeamChoiceMenuData, OnSyncTeamChoiceMenuDataMethod);
            Add(ToClientEvent.SyncTeamPlayers, OnSyncTeamPlayersMethod);
            Add(ToClientEvent.ToggleTeamChoiceMenu, OnToggleTeamChoiceMenuMethod);

            Add(ToClientEvent.ToBrowserEvent, OnToBrowserEventMethod);
            Add(ToClientEvent.FromBrowserEventReturn, OnFromBrowserEventReturnMethod);
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
            ushort targetHandle = Convert.ToUInt16(args[0]);
            int damage = (int)args[1];
            Player target = ClientUtils.GetPlayerByHandleValue(targetHandle);

            FightInfo.HittedOpponent(target, damage);

        }

        // Join always means we also left another lobby (except on login)
        private void OnJoinLobbyMethod(object[] args)
        {
            var oldSettings = Settings.GetSyncedLobbySettings();
            SyncedLobbySettingsDto settings = Serializer.FromServer<SyncedLobbySettingsDto>((string)args[0]);
            Settings.LoadSyncedLobbySettings(settings);
            Players.Load(ClientUtils.GetTriggeredPlayersList((string)args[1]));
            TeamsHandler.LobbyTeams = Serializer.FromServer<List<SyncedTeamDataDto>>((string)args[2]);
            Lobby.Lobby.Joined(oldSettings, settings);
            DiscordManager.Update();
            MainBrowser.HidRoundEndReason();
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

            var mapCreatorData = Serializer.FromServer<MapCreateDataDto>(json);
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
            MainBrowser.HidRoundEndReason();
            Round.InFight = false;

            var mapData = Serializer.FromServer<ClientSyncedDataDto>((string)args[0]);
            MapDataManager.SetMapData(mapData);
        }

        private void OnMapCreatorRequestAllObjectsForPlayerMethod(object[] args)
        {
            int tdsPlayerId = Convert.ToInt32(args[0]);
            Sync.SyncAllObjectsToPlayer(tdsPlayerId);
        }

        private void OnMapCreatorStartNewMapMethod(object[] args)
        {
            MapCreator.Main.StartNewMap();
        }

        private void OnMapCreatorSyncAllObjectsMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var data = Serializer.FromServer<MapCreateDataDto>(json);
            Sync.SyncAllObjectsFromLobbyOwner(data);
            Browser.Angular.Main.LoadMapForMapCreator(json);
        }

        private void OnMapCreatorSyncDataMethod(object[] args)
        {
            Browser.Angular.Main.MapCreatorSyncData(Convert.ToInt32(args[0]), args[1]);
        }

        private void OnMapCreatorSyncFixLastIdMethod(object[] args)
        {
            int oldId = Convert.ToInt32(args[0]);
            int newId = Convert.ToInt32(args[1]);
            Sync.SyncLatestIdFromServer(oldId, newId);
        }

        private void OnMapClearMethod(object[] args)
        {
            Round.Reset(false);
            CustomEventManager.SetMapClear();
        }

        private void OnMapCreatorSyncNewObjectMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var dto = Serializer.FromServer<MapCreatorPosition>(json);
            Sync.SyncNewObjectFromLobby(dto);
        }

        private void OnMapCreatorSyncObjectPositionMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var dto = Serializer.FromServer<MapCreatorPosData>(json);
            Sync.SyncObjectPositionFromLobby(dto);
        }

        private void MapCreatorSyncObjectRemoveMethod(object[] args)
        {
            int objId = Convert.ToInt32(args[0]);
            Sync.SyncObjectRemoveFromLobby(objId);
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
                MainBrowser.HidRoundEndReason();
            }
        }

        private void OnCreateCustomLobbyResponseMethod(object[] args)
        {
            string errorOrEmpty = (string)args[0];
            Browser.Angular.Main.CreateCustomLobbyReturn(errorOrEmpty);
        }

        private void OnRoundStartMethod(object[] args)
        {
            Round.IsSpectator = Convert.ToBoolean(args[0]);
            Round.InFight = !Round.IsSpectator;
            Cam.DoScreenFadeIn(50);
            LobbyCam.StopCountdown();
            Countdown.End(args.Length < 2);
            RoundInfo.Start(args.Length >= 2 ? Convert.ToUInt32(args[1]) : 0);
            CustomEventManager.SetRoundStart(Round.IsSpectator);
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
            CustomEventManager.SetRoundEnd();
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

        /*private void OnSetAssistsForRoundStatsMethod(object[] args)
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
        }*/

        private void OnPlayerSpectateModeMethod(object[] args)
        {
            Round.IsSpectator = true;
            Spectate.Start();
        }

        private void OnDeathMethod(object[] args)
        {
            Player player = ClientUtils.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            bool willRespawn = Convert.ToBoolean(args[3]);
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

        private void OnExplodeHeadMethod(object[] args)
        {
            var weaponHash = Convert.ToUInt32(args[0]);
            Player.LocalPlayer.ExplodeHead(weaponHash);
        }

        private void OnGetSupportRequestDataMethod(object[] args)
        {
            string json = (string)args[0];
            Browser.Angular.Main.GetSupportRequestData(json);
        }



        private void OnPlayerGotBombMethod(object[] args)
        {
            Bomb.LocalPlayerGotBomb();
        }

        private void OnPlayerPlantedBombMethod(object[] args)
        {
            Bomb.LocalPlayerPlantedBomb();
        }

        private void OnBombPlantedMethod(object[] args)
        {
            Bomb.BombPlanted(Serializer.FromServer<Vector3>((string)args[0]), Convert.ToBoolean(args[1]), args.Length > 2 ? (int?)args[2] : null);
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
            TeamsHandler.ClearSameTeam();
        }

        private void OnPlayerJoinedTeamMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            Player player = ClientUtils.GetPlayerByHandleValue(handleValue);
            TeamsHandler.AddSameTeam(player);
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
            TeamsHandler.RemoveSameTeam(player);
        }

        private void OnPlayerRespawnedMethod(object[] args)
        {
            if (!Round.IsSpectator)
                Round.InFight = true;
        }

        private void OnPlayerTeamChangeMethod(object[] args)
        {
            TeamsHandler.CurrentTeamName = (string)args[0];
            DiscordManager.Update();
        }

        /*private void OnPlayerWeaponChangeMethod(object[] args)
        {
            uint weaponHash = (uint)(int)args[0];
            //int damage = Convert.ToInt32(args[1]);
            CustomEventManager.SetNewWeapon(weaponHash);
            //Damagesys.CurrentWeaponDamage = damage;

        }*/

        private void OnAmountInFightSyncMethod(object[] args)
        {
            SyncedTeamPlayerAmountDto[] list = Serializer.FromServer<SyncedTeamPlayerAmountDto[]>((string)args[0]);
            foreach (var team in TeamsHandler.LobbyTeams)
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
            var settings = Serializer.FromServer<SyncedPlayerSettingsDto>(json);
            Settings.LoadUserSettings(settings);
            Browser.Angular.Main.LoadUserpanelData((int)UserpanelLoadDataType.SettingsRest, json);
        }

        private void OnRemoveCustomLobbyMethod(object[] args)
        {
            int lobbyId = (int)args[0];
            Browser.Angular.Main.RemoveCustomLobby(lobbyId);
        }

        private void OnRemoveForceStayAtPositionMethod(object[] args)
        {
            ForceStayAtPosHandler.Stop();
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
            //List<SyncedMapDataDto> maps = Serializer.FromServer<List<SyncedMapDataDto>>((string)args[0]);
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
            bool isregistered = Convert.ToBoolean(args[1]);
            RegisterLoginHandler.Start(scname, isregistered);
        }

        private void OnStopBombPlantDefuseMethod(object[] args)
        {
            Bomb.StopRequestByServer();
        }

        private void OnStopMapVotingMethod(object[] args)
        {
            Browser.Angular.Main.ResetMapVoting();
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
            Settings.LoadSyncedSettings(Serializer.FromServer<SyncedServerSettingsDto>(args[0].ToString()));
            Settings.LoadUserSettings(Serializer.FromServer<SyncedPlayerSettingsDto>(args[1].ToString()));
            RegisterLoginHandler.Stop();
            Settings.LoggedIn = true;
            MainBrowser.Load();
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.PressedScoreboardKey, EKeyPressState.Down);
            BindManager.Add(Control.MultiplayerInfo, Scoreboard.ReleasedScoreboardKey, EKeyPressState.Up);
            Settings.Load();
            VoiceManager.Init();
            TeamsHandler.Init();
            CrouchingHandler.Init();
            AFKCheckManager.Init();

            BindManager.Add(EKey.F3, MapManager.ToggleMenu);
            BindManager.Add(EKey.U, Userpanel.Toggle);

            TickManager.Add(ClientUtils.HideHUDOriginalComponents);

            Browser.Angular.Main.Start(Convert.ToString(args[2]));
        }

        private void OnSetForceStayAtPositionMethod(object[] args)
        {
            var pos = Serializer.FromServer<Position3DDto>(Convert.ToString(args[0]));
            var radius = Convert.ToSingle(args[1]);
            var type = args.Length >= 3 ? (MapLimitType)Convert.ToInt32(args[2]) : MapLimitType.Block;
            var allowedTimeOut = args.Length >= 4 ? Convert.ToInt32(args[3]) : 0;

            ForceStayAtPosHandler.Start(pos, radius, type, allowedTimeOut);
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
                var list = Serializer.FromServer<List<SyncedScoreboardMainmenuLobbyDataDto>>((string)args[0]);
                Scoreboard.ClearRows();
                Scoreboard.AddMainmenuData(list);
            }
            else
            {
                var playerlist = Serializer.FromServer<List<SyncedScoreboardLobbyDataDto>>((string)args[0]);
                var lobbylist = Serializer.FromServer<List<SyncedScoreboardMainmenuLobbyDataDto>>((string)args[1]);
                Scoreboard.ClearRows();
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
            TeamsHandler.ClearSameTeam();
            IEnumerable<int> listOfPlayerHandles = Serializer.FromServer<IEnumerable<int>>(args[0].ToString());
            foreach (var handle in listOfPlayerHandles)
            {
                Player player = Entities.Players.GetAtHandle(handle);
                if (player != null)
                    TeamsHandler.AddSameTeam(player);
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



        private void OnToBrowserEventMethod(object[] args)
        {
            string eventName = (string)args[0];
            Browser.Angular.Main.FromServerToBrowser(eventName, args.Skip(1).ToArray());
        }

        private void OnFromBrowserEventReturnMethod(object[] args)
        {
            string eventName = (string)args[0];
            object ret = args[1];
            Browser.Angular.Main.FromBrowserEventReturn(eventName, ret);
        }
    }
}
