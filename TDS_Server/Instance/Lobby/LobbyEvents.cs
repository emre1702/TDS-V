using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    internal class LobbyEvents : Script
    {
        #region Server

        [ServerEvent(Event.PlayerSpawn)]
        public static void OnPlayerSpawn(Client player)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerSpawn(character);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerDisconnected(character);
        }

        //[DisableDefaultOnDeathRespawn]
        [ServerEvent(Event.PlayerDeath)]
        public static void OnPlayerDeath(Client player, Client killer, uint reason)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerDeath(character, killer, reason);
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public static void OnPlayerEnterColShape(ColShape shape, Client player)
        {
            TDSPlayer character = player.GetChar();
            character.CurrentLobby?.OnPlayerEnterColShape(shape, character);
        }

        [ServerEvent(Event.PlayerWeaponSwitch)]
        public static void OnPlayerWeaponSwitch(Client player, WeaponHash oldweapon, WeaponHash newweapon)
        {
            TDSPlayer character = player.GetChar();
            if (character.CurrentLobby is FightLobby fightlobby)
                fightlobby.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
        }

        #endregion Server

        #region Remote

        #region Lobby

        [RemoteEvent(DToServerEvent.JoinLobby)]
        public static async void JoinLobbyEvent(Client player, int index, uint teamindex)
        {
            if (Lobby.LobbiesByIndex.ContainsKey(index))
            {
                Lobby lobby = Lobby.LobbiesByIndex[index];
                await lobby.AddPlayer(player.GetChar(), teamindex);
            }
            else
            {
                NAPI.Chat.SendChatMessageToPlayer(player, player.GetChar().Language.LOBBY_DOESNT_EXIST);
                //todo Remove lobby at client view and check, why he saw this lobby
            }
        }

        [RemoteEvent(DToServerEvent.JoinMapCreator)]
        public static void JoinMapCreatorEvent(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
           MapCreateLobby.Create(player);
        }

        #endregion Lobby

        #region Damagesys

        [RemoteEvent(DToServerEvent.HitOtherPlayer)]
        public void OnPlayerHitOtherPlayer(Client player, string hittedName, bool headshot, int clientHasSentThisDamage)
        {
            TDSPlayer character = player.GetChar();
            Client hitted = NAPI.Player.GetPlayerFromName(hittedName);
            if (hitted == null)
                return;
            if (character.CurrentLobby is FightLobby fightlobby)
            {
                WeaponHash currentweapon = player.CurrentWeapon;
                fightlobby.DamagedPlayer(hitted.GetChar(), character, currentweapon, headshot, clientHasSentThisDamage);
            }
        }

        #endregion Damagesys

        [RemoteEvent(DToServerEvent.OutsideMapLimit)]
        public void OnOutsideMapLimit(Client player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.CurrentLobby is Arena arena))
                return;

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
            if (!arena.StartBombPlanting(character))
                player.TriggerEvent(DToClientEvent.StopBombPlantDefuse);
        }

        [RemoteEvent(DToServerEvent.StopPlanting)]
        public void OnPlayerStopPlantingEvent(Client player)
        {
            if (!(player.GetChar().CurrentLobby is Arena arena))
                return;
            arena.StopBombPlanting(player);
        }

        [RemoteEvent(DToServerEvent.StartDefusing)]
        public void OnPlayerStartDefusingEvent(Client player)
        {
            TDSPlayer character = player.GetChar();
            if (!(character.CurrentLobby is Arena arena))
                return;
            if (!arena.StartBombDefusing(character))
                player.TriggerEvent(DToClientEvent.StopBombPlantDefuse);
        }

        [RemoteEvent(DToServerEvent.StopDefusing)]
        public void OnPlayerStopDefusingEvent(Client player)
        {
            if (!(player.GetChar().CurrentLobby is Arena arena))
                return;
            arena.StopBombDefusing(player);
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
        public void SendMapRating(Client client, string mapName, int rating)
        {
            MapsRatings.AddPlayerMapRating(client, mapName, (byte)rating);
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

        [RemoteEvent(DToServerEvent.LoadMySavedMap)]
        public void OnLoadMySavedMap(Client client, string mapName)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreator.SendPlayerHisSavedMap(player, mapName);
        }

        [RemoteEvent(DToServerEvent.LoadMySavedMapNames)]
        public void OnLoadMySavedMapNames(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            MapCreator.SendPlayerHisSavedMapNames(player);
        }
        #endregion MapCreator

        #endregion Remote

        /*[RemoteEvent("joinMapCreatorLobby")]
        public void JoinMapCreatorLobbyEvent(Client player)
        {
            manager.lobby.MapCreatorLobby.Join(player.GetChar());
        }

        #region MapCreate

        [RemoteEvent("checkMapName")]
        public void OnCheckMapNameEvent(Client player, string mapname)
        {
            player.TriggerEvent("sendMapNameCheckResult", Map.DoesMapNameExist(mapname));
        }

        [RemoteEvent("sendMapFromCreator")]
        public void SendMapFromCreatorEvent(Client player, string map)
        {
            Character character = player.GetChar();
            Map.CreateNewMap(map, character.UID);
            character.Lobby.RemovePlayerDerived(character);
        }

        [RemoteEvent("requestNewMapsList")]
        public void RequestNewMapsListEvent(Client player, bool requestall)
        {
            Map.RequestNewMapsList(player, requestall);
        }

        #endregion MapCreate

        #region MapRanking

        [RemoteEvent("addRatingToMap")]
        public void AddRatingToMapEvent(Client player, string mapname, uint rating)
        {
            Map.AddPlayerMapRating(player, mapname, rating);
        }

        #endregion MapRanking

        #region Freecam

        //case "setFreecamObjectPositionTo":
        //player.GetChar ().Lobby.SetPlayerFreecamPos ( player, args[0] );    //TODO
        //break;

        #endregion Freecam

        #region Order

        [RemoteEvent("onPlayerGiveOrder")]
        public void OnPlayerGiveOrderEvent(Client player, string ordershort)
        {
            Character character = player.GetChar();
            character.Lobby.SendTeamOrder(character, ordershort);
        }

        #endregion Order

        */
    }
}