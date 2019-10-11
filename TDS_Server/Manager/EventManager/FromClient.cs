using GTANetworkAPI;
using System;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Enum.Userpanel;
using TDS_Server.Instance.GameModes;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Sync;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.EventManager
{
    partial class EventsHandler
    {
        #region Lobby

        [RemoteEvent(DToServerEvent.JoinLobby)]
        public static async void JoinLobbyEvent(Client client, int index)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            if (Lobby.LobbiesByIndex.ContainsKey(index))
            {
                Lobby lobby = Lobby.LobbiesByIndex[index];
                await lobby.AddPlayer(player, null);
            }
            else
            {
                NAPI.Chat.SendChatMessageToPlayer(client, player.Language.LOBBY_DOESNT_EXIST);
                //todo Remove lobby at client view and check, why he saw this lobby
            }
        }

        [RemoteEvent(DToServerEvent.JoinLobbyWithPassword)]
        public static async void JoinLobbyEvent(Client client, int index, string? password = null)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            if (Lobby.LobbiesByIndex.ContainsKey(index))
            {
                Lobby lobby = Lobby.LobbiesByIndex[index];
                if (password != null && lobby.LobbyEntity.Password != password)
                {
                    NAPI.Chat.SendChatMessageToPlayer(client, player.Language.WRONG_PASSWORD);
                    return;
                }

                await lobby.AddPlayer(player, null);
            }
            else
            {
                NAPI.Chat.SendChatMessageToPlayer(client, player.Language.LOBBY_DOESNT_EXIST);
                //todo Remove lobby at client view and check, why he saw this lobby
            }
        }

        [RemoteEvent(DToServerEvent.JoinArena)]
        public static async void JoinArenaEvent(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            await LobbyManager.Arena.AddPlayer(player, null);
        }

        [RemoteEvent(DToServerEvent.JoinMapCreator)]
        public static void JoinMapCreatorEvent(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreateLobby.Create(player);
        }

        [RemoteEvent(DToServerEvent.CreateCustomLobby)]
        public static async void CreateCustomLobbyEvent(Client client, string dataJson)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            await LobbyManager.CreateCustomLobby(player, dataJson);
        }

        [RemoteEvent(DToServerEvent.JoinedCustomLobbiesMenu)]
        public static void JoinedCustomLobbiesMenu(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            CustomLobbyMenuSync.AddPlayer(player);
        }

        [RemoteEvent(DToServerEvent.LeftCustomLobbiesMenu)]
        public static void LeftCustomLobbiesMenu(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            CustomLobbyMenuSync.RemovePlayer(player);
        }

        [RemoteEvent(DToServerEvent.ChooseTeam)]
        public static void ChooseTeamMethod(Client client, int index)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (player.CurrentLobby is null || !(player.CurrentLobby is Arena arena))
                return;
            arena.ChooseTeam(player, index);
        }

        [RemoteEvent(DToServerEvent.LeaveLobby)]
        public static async void LeaveLobbyMethod(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (player.CurrentLobby is null)
                return;

            player.CurrentLobby.RemovePlayer(player);
            await LobbyManager.MainMenu.AddPlayer(player, 0);
        }
        #endregion Lobby

        #region Damagesys

        [RemoteEvent(DToServerEvent.GotHit)]
        public void OnPlayerGotHitByOtherPlayer(Client client, ushort attackerRemoteId, int boneOrZero, int damage)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
            {
                var attackerClientError = NAPI.Player.GetPlayerFromHandle(new NetHandle(attackerRemoteId, EntityType.Player));
                ErrorLogsManager.Log(string.Format("Player {0} got hit by {1} with {2} damage - but he is not online.", client.Name, attackerClientError?.Name ?? "id " + attackerRemoteId, damage),
                    Environment.StackTrace, client);
                return;
            }

            var attackerClient = NAPI.Player.GetPlayerFromHandle(new NetHandle(attackerRemoteId, EntityType.Player));
            if (attackerClient is null)
                return;

            TDSPlayer attacker = attackerClient.GetChar();
            if (!attacker.LoggedIn)
            {
                ErrorLogsManager.Log(string.Format("Attacker {0} dealt {1} damage to {2} - but he is not online.", attackerClient.Name, damage, client.Name), Environment.StackTrace, attacker);
                return;
            }

            if (!(player.CurrentLobby is FightLobby fightLobby))
            {
                ErrorLogsManager.Log(string.Format("Attacker {0} dealt {1} damage to {2} - but this player isn't in fightlobby.", attackerClient.Name, damage, client.Name), Environment.StackTrace, attacker);
                return;
            }

            WeaponHash currentweapon = client.CurrentWeapon;
            fightLobby.DamagedPlayer(player, attacker, currentweapon, boneOrZero == 0 ? (int?)null : boneOrZero, damage);
        }

        #endregion Damagesys

        [RemoteEvent(DToServerEvent.OutsideMapLimit)]
        public void OnOutsideMapLimit(Client player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.CurrentLobby is Arena arena))
                return;
            if (arena.LobbyEntity.LobbyMapSettings.MapLimitType == EMapLimitType.KillAfterTime)
                arena.KillPlayer(player, character.Language.TOO_LONG_OUTSIDE_MAP);
        }

        [RemoteEvent(DToServerEvent.SendTeamOrder)]
        public void SendTeamOrder(Client client, int teamOrderInt)
        {
            if (!System.Enum.TryParse(teamOrderInt.ToString(), out ETeamOrder teamOrder))
                return;
            TDSPlayer player = client.GetChar();
            player.CurrentLobby?.SendTeamOrder(player, teamOrder);
        }

        #region Bomb

        [RemoteEvent(DToServerEvent.StartPlanting)]
        public void OnPlayerStartPlantingEvent(Client player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.CurrentLobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombPlanting(character))
                player.TriggerEvent(DToClientEvent.StopBombPlantDefuse);
        }

        [RemoteEvent(DToServerEvent.StopPlanting)]
        public void OnPlayerStopPlantingEvent(Client player)
        {
            if (!(player.GetChar().CurrentLobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombPlanting(player);
        }

        [RemoteEvent(DToServerEvent.StartDefusing)]
        public void OnPlayerStartDefusingEvent(Client player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.CurrentLobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombDefusing(character))
                player.TriggerEvent(DToClientEvent.StopBombPlantDefuse);
        }

        [RemoteEvent(DToServerEvent.StopDefusing)]
        public void OnPlayerStopDefusingEvent(Client player)
        {
            if (!(player.GetChar().CurrentLobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombDefusing(player);
        }

        #endregion Bomb

        #region Spectate

        [RemoteEvent(DToServerEvent.SpectateNext)]
        public void SpectateNextEvent(Client player, bool forward)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.CurrentLobby is FightLobby lobby))
                return;
            lobby.SpectateNext(character, forward);
        }

        #endregion Spectate

        #region MapVote

        [RemoteEvent(DToServerEvent.MapsListRequest)]
        public void OnMapsListRequestEvent(Client player)
        {
            if (!(player.GetChar().CurrentLobby is Arena arena))
                return;

            arena.SendMapsForVoting(player);
        }

        [RemoteEvent(DToServerEvent.MapVote)]
        public void OnMapVotingRequestEvent(Client client, int mapId)
        {
            TDSPlayer player = client.GetChar();
            if (!(player.CurrentLobby is Arena arena))
                return;

            arena.MapVote(player, mapId);
        }

        #endregion MapVote

        #region Map Rating

        [RemoteEvent(DToServerEvent.SendMapRating)]
        public void SendMapRating(Client client, int mapId, int rating)
        {
            MapsRatings.AddPlayerMapRating(client, mapId, (byte)rating);
        }

        #endregion Map Rating

        #region MapCreator
        [RemoteEvent(DToServerEvent.SendMapCreatorData)]
        public async void OnSendMapCreatorData(Client client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            EMapCreateError err = await MapCreator.Create(player, json, false);
            NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SendMapCreatorReturn, (int)err);
        }

        [RemoteEvent(DToServerEvent.SaveMapCreatorData)]
        public async void OnSaveMapCreatorData(Client client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            EMapCreateError err = await MapCreator.Create(player, json, true);
            NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SaveMapCreatorReturn, (int)err);
        }

        [RemoteEvent(DToServerEvent.LoadMapNamesToLoadForMapCreator)]
        public void OnLoadMySavedMapNames(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreator.SendPlayerMapNamesForMapCreator(player);
        }

        [RemoteEvent(DToServerEvent.LoadMapForMapCreator)]
        public void OnLoadMySavedMap(Client client, string mapName)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreator.SendPlayerMapForMapCreator(player, mapName);
        }

        [RemoteEvent(DToServerEvent.RemoveMap)]
        public void OnRemoveMap(Client client, int mapId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreator.RemoveMap(player, mapId);
        }

        [RemoteEvent(DToServerEvent.GetVehicle)]
        public void OnGetVehicle(Client client, int vehTypeNumber)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            if (player.CurrentLobby is null || !(player.CurrentLobby is MapCreateLobby lobby))
                return;
            EFreeroamVehicleType vehType = (EFreeroamVehicleType)vehTypeNumber;
            lobby.GiveVehicle(player, vehType);
        }
        #endregion MapCreator

        #region Userpanel
        [RemoteEvent(DToServerEvent.LoadUserpanelData)]
        public void PlayerLoadAllCommands(Client client, int dataType)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            EUserpanelLoadDataType type = (EUserpanelLoadDataType)dataType;
            Manager.Userpanel.Main.PlayerLoadData(player, type);
        }
        #endregion
    }
}
