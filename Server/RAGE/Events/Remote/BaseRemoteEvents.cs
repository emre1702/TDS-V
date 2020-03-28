using GTANetworkAPI;
using System;
using TDS_Server.RAGE.Player;
using TDS_Server.RAGE.Startup;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.RAGE.Events.Remote
{
    partial class BaseRemoteEvents : BaseEvents
    {
        [RemoteEvent(ToServerEvent.LobbyChatMessage)]
        public void LobbyChatMessage(GTANetworkAPI.Player player, string message, int chatTypeNumber)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.LobbyChatMessage(tdsPlayer, message, chatTypeNumber);
        }

        [RemoteEvent(ToServerEvent.TryLogin)]
        public void TryLogin(GTANetworkAPI.Player player, string username, string password)
        {
            var tdsPlayer = Init.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.TryLogin(tdsPlayer, username, password);
        }

        [RemoteEvent(ToServerEvent.TryRegister)]
        public void TryRegister(GTANetworkAPI.Player player, string username, string password, string email)
        {
            var tdsPlayer = Init.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.TryRegister(tdsPlayer, username, password, email);
        }

        [RemoteEvent(ToServerEvent.ToggleMapFavouriteState)]
        public void ToggleMapFavouriteState(GTANetworkAPI.Player player, int mapId, bool isFavorite)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.ToggleMapFavouriteState(tdsPlayer, mapId, isFavorite);
        }

        [RemoteEvent(ToServerEvent.CommandUsed)]
        public void UseCommand(GTANetworkAPI.Player player, string msg)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.UseCommand(tdsPlayer, msg);
        }

        [RemoteEvent(ToServerEvent.LanguageChange)]
        public void LanguageChange(GTANetworkAPI.Player player, int language)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            if (!System.Enum.IsDefined(typeof(Language), language))
                return;

            Init.TDSCore.RemoteEventsHandler.OnLanguageChange(tdsPlayer, (Language)language);
        }

        [RemoteEvent(ToServerEvent.RequestPlayersForScoreboard)]
        public static void RequestPlayersForScoreboard(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnRequestPlayersForScoreboard(tdsPlayer);
        }

        #region Lobby

        [RemoteEvent(ToServerEvent.JoinLobby)]
        public void OnJoinLobby(GTANetworkAPI.Player player, int index)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnJoinLobby(tdsPlayer, index);
        }

        [RemoteEvent(ToServerEvent.JoinLobbyWithPassword)]
        public void OnJoinLobbyWithPassword(GTANetworkAPI.Player player, int index, string? password = null)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnJoinLobbyWithPassword(tdsPlayer, index, password);
        }

        [RemoteEvent(ToServerEvent.CreateCustomLobby)]
        public void OnCreateCustomLobby(GTANetworkAPI.Player player, string dataJson)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnCreateCustomLobby(tdsPlayer, dataJson);
        }

        [RemoteEvent(ToServerEvent.JoinedCustomLobbiesMenu)]
        public void OnJoinedCustomLobbiesMenu(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnJoinedCustomLobbiesMenu(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.LeftCustomLobbiesMenu)]
        public void OnLeftCustomLobbiesMenu(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLeftCustomLobbiesMenu(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.ChooseTeam)]
        public void OnChooseTeam(GTANetworkAPI.Player player, int index)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnChooseTeam(tdsPlayer, index);
        }

        [RemoteEvent(ToServerEvent.LeaveLobby)]
        public void OnLeaveLobby(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLeaveLobby(tdsPlayer);
        }
        #endregion Lobby

        #region Damagesys

        [RemoteEvent(ToServerEvent.GotHit)]
        public void OnGotHit(GTANetworkAPI.Player player, int attackerRemoteId, string weaponHashStr, string boneIdxStr)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            var attacker = Init.GetTDSPlayerIfLoggedIn((ushort)attackerRemoteId);
            if (attacker is null)
                return;

            if (!Enum.TryParse(weaponHashStr, out TDS_Shared.Data.Enums.WeaponHash weaponHash))
                return;

            if (!ulong.TryParse(boneIdxStr, out ulong boneIdx))
                return;

            Init.TDSCore.RemoteEventsHandler.OnGotHit(tdsPlayer, attacker, weaponHash, boneIdx);
        }

        #endregion Damagesys

        [RemoteEvent(ToServerEvent.OutsideMapLimit)]
        public void OnOutsideMapLimit(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnOutsideMapLimit(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.SendTeamOrder)]
        public void OnSendTeamOrder(GTANetworkAPI.Player player, int teamOrderInt)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            if (!Enum.IsDefined(typeof(TeamOrder), teamOrderInt))
                return;
            Init.TDSCore.RemoteEventsHandler.OnSendTeamOrder(tdsPlayer, (TeamOrder)teamOrderInt);
        }

        [RemoteEvent(ToServerEvent.SuicideKill)]
        public void OnSuicideKill(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSuicideKill(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.SuicideShoot)]
        public void OnSuicideShoot(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSuicideShoot(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.ToggleCrouch)]
        public void OnToggleCrouch(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnToggleCrouch(tdsPlayer);
        }

        #region Bomb

        [RemoteEvent(ToServerEvent.StartPlanting)]
        public void OnStartPlanting(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStartPlanting(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.StopPlanting)]
        public void OnStopPlanting(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStopPlanting(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.StartDefusing)]
        public void OnStartDefusing(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStartDefusing(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.StopDefusing)]
        public void OnStopDefusing(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStopDefusing(tdsPlayer);
        }

        #endregion Bomb

        #region Spectate

        [RemoteEvent(ToServerEvent.SpectateNext)]
        public void OnSpectateNext(GTANetworkAPI.Player player, bool forward)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSpectateNext(tdsPlayer, forward);
        }

        #endregion Spectate

        #region MapVote

        [RemoteEvent(ToServerEvent.MapsListRequest)]
        public void OnMapsListRequest(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapsListRequest(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.MapVote)]
        public void OnMapVote(GTANetworkAPI.Player player, int mapId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapVote(tdsPlayer, mapId);
        }

        #endregion MapVote

        #region Map Rating

        [RemoteEvent(ToServerEvent.SendMapRating)]
        public void OnSendMapRating(GTANetworkAPI.Player player, int mapId, int rating)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSendMapRating(tdsPlayer, mapId, rating);
        }

        #endregion Map Rating

        #region MapCreator
        [RemoteEvent(ToServerEvent.SendMapCreatorData)]
        public void OnSendMapCreatorData(GTANetworkAPI.Player player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSendMapCreatorData(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.SaveMapCreatorData)]
        public void OnSaveMapCreatorData(GTANetworkAPI.Player player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSaveMapCreatorData(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.LoadMapNamesToLoadForMapCreator)]
        public void OnLoadMapNamesToLoadForMapCreator(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLoadMapNamesToLoadForMapCreator(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.LoadMapForMapCreator)]
        public void OnLoadMapForMapCreator(GTANetworkAPI.Player player, int mapId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLoadMapForMapCreator(tdsPlayer, mapId);
        }

        [RemoteEvent(ToServerEvent.RemoveMap)]
        public void OnRemoveMap(GTANetworkAPI.Player player, int mapId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnRemoveMap(tdsPlayer, mapId);
        }

        [RemoteEvent(ToServerEvent.GetVehicle)]
        public void OnGetVehicle(GTANetworkAPI.Player player, int vehTypeNumber)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnGetVehicle(tdsPlayer, vehTypeNumber);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncLastId)]
        public void OnMapCreatorSyncLastId(GTANetworkAPI.Player player, int id)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncLastId(tdsPlayer, id);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncNewObject)]
        public void OnMapCreatorSyncNewObject(GTANetworkAPI.Player player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncNewObject(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncObjectPosition)]
        public void OnMapCreatorSyncObjectPosition(GTANetworkAPI.Player player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncObjectPosition(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncRemoveObject)]
        public void OnMapCreatorSyncRemoveObject(GTANetworkAPI.Player player, int id)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncRemoveObject(tdsPlayer, id);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncAllObjects)]
        public void OnMapCreatorSyncAllObjects(GTANetworkAPI.Player player, int tdsPlayerId, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncAllObjects(tdsPlayer, tdsPlayerId, json);
        }

        [RemoteEvent(ToServerEvent.MapCreatorStartNewMap)]
        public void OnMapCreatorStartNewMap(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorStartNewMap(tdsPlayer);
        }
        #endregion MapCreator

        #region Userpanel
        [RemoteEvent(ToServerEvent.LoadUserpanelData)]
        public void OnLoadUserpanelData(GTANetworkAPI.Player player, int dataType)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLoadUserpanelData(tdsPlayer, dataType);
        }

        [RemoteEvent(ToServerEvent.SendApplication)]
        public void OnSendApplication(GTANetworkAPI.Player player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSendApplication(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.AcceptInvitation)]
        public void OnAcceptInvitation(GTANetworkAPI.Player player, int id)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnAcceptInvitation(tdsPlayer, id);
        }

        [RemoteEvent(ToServerEvent.RejectInvitation)]
        public void OnRejectInvitation(GTANetworkAPI.Player player, int id)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnRejectInvitation(tdsPlayer, id);
        }

        [RemoteEvent(ToServerEvent.LoadApplicationDataForAdmin)]
        public void OnLoadApplicationDataForAdmin(GTANetworkAPI.Player player, int applicationId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLoadApplicationDataForAdmin(tdsPlayer, applicationId);
        }
        #endregion
    }
}
