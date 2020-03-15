using GTANetworkAPI;
using System;
using TDS_Common.Default;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Server.Enums;
using TDS_Server.Instance.GameModes;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Sync;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Core.Manager.EventManager
{
    partial class EventsHandler
    {
        #region Lobby

        [RemoteEvent(DToServerEvent.JoinLobby)]
        public static async void JoinLobbyEvent(Player client, int index)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            if (Lobby.LobbiesByIndex.ContainsKey(index))
            {
                Lobby lobby = Lobby.LobbiesByIndex[index];
                if (lobby is MapCreateLobby mapCreateLobby)
                {
                    if (await lobby.IsPlayerBaned(player))
                        return;
                    MapCreateLobby.Create(player);
                }
                else
                {
                    await lobby.AddPlayer(player, null);
                }
            }
            else
            {
                player.SendMessage(player.Language.LOBBY_DOESNT_EXIST);
                //todo Remove lobby at client view and check, why he saw this lobby
            }
        }

        [RemoteEvent(DToServerEvent.JoinLobbyWithPassword)]
        public static async void JoinLobbyEvent(Player client, int index, string? password = null)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            if (Lobby.LobbiesByIndex.ContainsKey(index))
            {
                Lobby lobby = Lobby.LobbiesByIndex[index];
                if (password != null && lobby.Entity.Password != password)
                {
                    player.SendMessage(player.Language.WRONG_PASSWORD);
                    return;
                }

                await lobby.AddPlayer(player, null);
            }
            else
            {
                player.SendMessage(player.Language.LOBBY_DOESNT_EXIST);
                //todo Remove lobby at client view and check, why he saw this lobby
            }
        }

        [RemoteEvent(DToServerEvent.CreateCustomLobby)]
        public static async void CreateCustomLobbyEvent(Player client, string dataJson)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            await LobbyManager.CreateCustomLobby(player, dataJson);
        }

        [RemoteEvent(DToServerEvent.JoinedCustomLobbiesMenu)]
        public static void JoinedCustomLobbiesMenu(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            CustomLobbyMenuSync.AddPlayer(player);
        }

        [RemoteEvent(DToServerEvent.LeftCustomLobbiesMenu)]
        public static void LeftCustomLobbiesMenu(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            CustomLobbyMenuSync.RemovePlayer(player);
        }

        [RemoteEvent(DToServerEvent.ChooseTeam)]
        public static void ChooseTeamMethod(Player client, int index)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null || !(player.Lobby is Arena arena))
                return;
            arena.ChooseTeam(player, index);
        }

        [RemoteEvent(DToServerEvent.LeaveLobby)]
        public static async void LeaveLobbyMethod(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null)
                return;

            player.Lobby.RemovePlayer(player);
            await LobbyManager.MainMenu.AddPlayer(player, 0);
        }
        #endregion Lobby

        #region Damagesys

        [RemoteEvent(DToServerEvent.GotHit)]
        public void OnPlayerGotHitByOtherPlayer(Player client, int attackerRemoteId, string weaponHashStr, string boneIdxStr)
        {
            /*if (!ushort.TryParse(attackerRemoteIdStr, out ushort attackerRemoteId))
                return;*/
            if (!Enum.TryParse(weaponHashStr, out WeaponHash weaponHash))
                return;
            if (!ulong.TryParse(boneIdxStr, out ulong boneIdx))
                return;

            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
            {
                var attackerClientError = NAPI.Player.GetPlayerFromHandle(new NetHandle((ushort)attackerRemoteId, EntityType.Player));
                ErrorLogsManager.Log(string.Format("Player {0} got hit by {1} with bone {2} - but he is not online.", client.Name, attackerClientError?.Name ?? "id " + attackerRemoteId, boneIdxStr),
                    Environment.StackTrace, client);
                return;
            }

            var attackerClient = NAPI.Player.GetPlayerFromHandle(new NetHandle((ushort)attackerRemoteId, EntityType.Player));
            if (attackerClient is null)
                return;

            TDSPlayer attacker = attackerClient.GetChar();
            if (!attacker.LoggedIn)
            {
                ErrorLogsManager.Log(string.Format("Attacker {0} dealt damage on bone {1} to {2} - but he is not online.", attackerClient.Name, boneIdxStr, client.Name), Environment.StackTrace, attacker);
                return;
            }

            if (!(player.Lobby is FightLobby fightLobby))
            {
                ErrorLogsManager.Log(string.Format("Attacker {0} dealt damage on bone {1} to {2} - but this player isn't in fightlobby.", attackerClient.Name, boneIdxStr, client.Name), Environment.StackTrace, attacker);
                return;
            }

            fightLobby.DamagedPlayer(player, attacker, (WeaponHash)weaponHash, boneIdx);
        }

        #endregion Damagesys

        [RemoteEvent(DToServerEvent.OutsideMapLimit)]
        public void OnOutsideMapLimit(Player player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.Lobby is Arena arena))
                return;
            if (arena.Entity.LobbyMapSettings.MapLimitType == EMapLimitType.KillAfterTime)
                FightLobby.KillPlayer(player, character.Language.TOO_LONG_OUTSIDE_MAP);
        }

        [RemoteEvent(DToServerEvent.SendTeamOrder)]
        public void SendTeamOrder(Player client, int teamOrderInt)
        {
            if (!System.Enum.TryParse(teamOrderInt.ToString(), out ETeamOrder teamOrder))
                return;
            TDSPlayer player = client.GetChar();
            Lobby.SendTeamOrder(player, teamOrder);
        }

        [RemoteEvent(DToServerEvent.SuicideKill)]
        public void SuicideKillMethod(Player player)
        {
            TDSPlayer character = player.GetChar();
            if (!character.LoggedIn)
                return;
            if (character.Lifes == 0)
                return;
            if (!(character.Lobby is FightLobby))
                return;
            FightLobby.KillPlayer(player, character.Language.COMMITED_SUICIDE);
        }

        [RemoteEvent(DToServerEvent.SuicideShoot)]
        public void SuicideShootMethod(Player player)
        {
            TDSPlayer character = player.GetChar();
            if (!character.LoggedIn)
                return;
            if (character.Lifes == 0)
                return;
            if (!(character.Lobby is FightLobby fightLobby))
                return;
            fightLobby.FuncIterateAllPlayers((target, team) =>
            {
                NAPI.Native.SendNativeToPlayer(target.Player, Hash.SET_PED_SHOOTS_AT_COORD, player, 0f, 0f, 0f, false);
            });
        }

        [RemoteEvent(DToServerEvent.ToggleCrouch)]
        public void ToggleCrouchMethod(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            player.IsCrouched = !player.IsCrouched;
            PlayerDataSync.SetData(player, PlayerDataKey.Crouched, PlayerDataSyncMode.Lobby, player.IsCrouched);
        }

        #region Bomb

        [RemoteEvent(DToServerEvent.StartPlanting)]
        public void OnPlayerStartPlantingEvent(Player player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombPlanting(character))
                player.TriggerEvent(ToClientEvent.StopBombPlantDefuse);
        }

        [RemoteEvent(DToServerEvent.StopPlanting)]
        public void OnPlayerStopPlantingEvent(Player player)
        {
            if (!(player.GetChar().Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombPlanting(player);
        }

        [RemoteEvent(DToServerEvent.StartDefusing)]
        public void OnPlayerStartDefusingEvent(Player player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombDefusing(character))
                player.TriggerEvent(ToClientEvent.StopBombPlantDefuse);
        }

        [RemoteEvent(DToServerEvent.StopDefusing)]
        public void OnPlayerStopDefusingEvent(Player player)
        {
            if (!(player.GetChar().Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombDefusing(player);
        }

        #endregion Bomb

        #region Spectate

        [RemoteEvent(DToServerEvent.SpectateNext)]
        public void SpectateNextEvent(Player player, bool forward)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.Lobby is FightLobby lobby))
                return;
            lobby.SpectateNext(character, forward);
        }

        #endregion Spectate

        #region MapVote

        [RemoteEvent(DToServerEvent.MapsListRequest)]
        public void OnMapsListRequestEvent(Player player)
        {
            if (!(player.GetChar().Lobby is Arena arena))
                return;

            arena.SendMapsForVoting(player);
        }

        [RemoteEvent(DToServerEvent.MapVote)]
        public void OnMapVotingRequestEvent(Player client, int mapId)
        {
            TDSPlayer player = client.GetChar();
            if (!(player.Lobby is Arena arena))
                return;

            arena.MapVote(player, mapId);
        }

        #endregion MapVote

        #region Map Rating

        [RemoteEvent(DToServerEvent.SendMapRating)]
        public void SendMapRating(Player client, int mapId, int rating)
        {
            var player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapsRatings.AddPlayerMapRating(player, mapId, (byte)rating);
        }

        #endregion Map Rating

        #region MapCreator
        [RemoteEvent(DToServerEvent.SendMapCreatorData)]
        public async void OnSendMapCreatorData(Player client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            EMapCreateError err = await MapCreator.Create(player, json, false);
            NAPI.ClientEvent.TriggerClientEvent(client, ToClientEvent.SendMapCreatorReturn, (int)err);
        }

        [RemoteEvent(DToServerEvent.SaveMapCreatorData)]
        public async void OnSaveMapCreatorData(Player client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            EMapCreateError err = await MapCreator.Create(player, json, true);
            NAPI.ClientEvent.TriggerClientEvent(client, ToClientEvent.SaveMapCreatorReturn, (int)err);
        }

        [RemoteEvent(DToServerEvent.LoadMapNamesToLoadForMapCreator)]
        public void OnLoadMySavedMapNames(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreator.SendPlayerMapNamesForMapCreator(player);
        }

        [RemoteEvent(DToServerEvent.LoadMapForMapCreator)]
        public void OnLoadMySavedMap(Player client, int mapId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby))
                return;
            MapCreator.SendPlayerMapForMapCreator(player, mapId);
        }

        [RemoteEvent(DToServerEvent.RemoveMap)]
        public void OnRemoveMap(Player client, int mapId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreator.RemoveMap(player, mapId);
        }

        [RemoteEvent(DToServerEvent.GetVehicle)]
        public void OnGetVehicle(Player client, int vehTypeNumber)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null || !(player.Lobby is MapCreateLobby lobby))
                return;
            EFreeroamVehicleType vehType = (EFreeroamVehicleType)vehTypeNumber;
            lobby.GiveVehicle(player, vehType);
        }

        [RemoteEvent(DToServerEvent.MapCreatorSyncLastId)]
        public void OnMapCreatorSyncLastId(Player client, int id)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncLastId(player, id);
        }

        [RemoteEvent(DToServerEvent.MapCreatorSyncNewObject)]
        public void OnMapCreatorSyncNewObject(Player client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncNewObject(player, json);
        }

        [RemoteEvent(DToServerEvent.MapCreatorSyncObjectPosition)]
        public void OnMapCreatorSyncObjectPosition(Player client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncObjectPosition(player, json);
        }

        [RemoteEvent(DToServerEvent.MapCreatorSyncRemoveObject)]
        public void OnMapCreatorSyncRemoveObject(Player client, int id)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncRemoveObject(player, id);
        }

        [RemoteEvent(DToServerEvent.MapCreatorSyncAllObjects)]
        public void OnMapCreatorSyncAllObjects(Player client, int tdsPlayerId, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncAllObjectsToPlayer(tdsPlayerId, json);
        }

        [RemoteEvent(DToServerEvent.MapCreatorStartNewMap)]
        public void OnMapCreatorStartNewMap(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.StartNewMap();
        }
        #endregion MapCreator

        #region Userpanel
        [RemoteEvent(DToServerEvent.LoadUserpanelData)]
        public void PlayerLoadAllCommands(Player client, int dataType)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            EUserpanelLoadDataType type = (EUserpanelLoadDataType)dataType;
            Userpanel.Main.PlayerLoadData(player, type);
        }

        [RemoteEvent(DToServerEvent.SendApplication)]
        public void SendApplicationMethod(Player client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            Userpanel.ApplicationUser.CreateApplication(player, json);
        }

        [RemoteEvent(DToServerEvent.AcceptInvitation)]
        public void AcceptInvitationMethod(Player client, int id)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            Userpanel.ApplicationUser.AcceptInvitation(player, id);
        }

        [RemoteEvent(DToServerEvent.RejectInvitation)]
        public void RejectInvitationMethod(Player client, int id)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            Userpanel.ApplicationUser.RejectInvitation(player, id);
        }

        [RemoteEvent(DToServerEvent.LoadApplicationDataForAdmin)]
        public async void LoadApplicationDataForAdminMethod(Player client, int applicationId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            await Userpanel.ApplicationsAdmin.SendApplicationData(player, applicationId);
        }
        #endregion
    }
}
