using GTANetworkAPI;
using System;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.RAGEAPI.Events.Remote
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
            var tdsPlayer = Init.GetNotLoggedInTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.TryLogin(tdsPlayer, username, password);
        }

        [RemoteEvent(ToServerEvent.TryRegister)]
        public void TryRegister(GTANetworkAPI.Player player, string username, string password, string email)
        {
            var tdsPlayer = Init.GetNotLoggedInTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.TryRegister(tdsPlayer, username, password, email);
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

        [RemoteEvent(ToServerEvent.RemoveMap)]
        public void OnRemoveMap(GTANetworkAPI.Player player, int mapId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnRemoveMap(tdsPlayer, mapId);
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

        [RemoteEvent(ToServerEvent.MapCreatorSyncRemoveTeamObjects)]
        public void OnMapCreatorSyncRemoveTeamObjects(GTANetworkAPI.Player player, int teamNumber)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncRemoveTeamObjects(tdsPlayer, teamNumber);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncAllObjects)]
        public void OnMapCreatorSyncAllObjects(GTANetworkAPI.Player player, int tdsPlayerId, string json, int lastId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncAllObjects(tdsPlayer, tdsPlayerId, json, lastId);
        }

        [RemoteEvent(ToServerEvent.MapCreatorStartNewMap)]
        public void OnMapCreatorStartNewMap(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorStartNewMap(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.SetInFreecam)]
        public void OnSetInFreecam(GTANetworkAPI.Player player, bool inFreeCam)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSetInFreeCam(tdsPlayer, inFreeCam);
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
        #endregion
    }
}
