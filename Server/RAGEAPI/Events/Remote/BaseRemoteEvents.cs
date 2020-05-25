using GTANetworkAPI;
using System;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.RAGEAPI.Events.Remote
{
    partial class BaseRemoteEvents : BaseEvents
    {
        #region Public Methods

        [RemoteEvent(ToServerEvent.RequestPlayersForScoreboard)]
        public static void RequestPlayersForScoreboard(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnRequestPlayersForScoreboard(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.LanguageChange)]
        public void LanguageChange(IPlayer player, int language)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            if (!System.Enum.IsDefined(typeof(Language), language))
                return;

            Init.TDSCore.RemoteEventsHandler.OnLanguageChange(tdsPlayer, (Language)language);
        }

        [RemoteEvent(ToServerEvent.LobbyChatMessage)]
        public void LobbyChatMessage(IPlayer player, string message, int chatTypeNumber)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.LobbyChatMessage(tdsPlayer, message, chatTypeNumber);
        }

        [RemoteEvent(ToServerEvent.ChooseTeam)]
        public void OnChooseTeam(IPlayer player, int index)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnChooseTeam(tdsPlayer, index);
        }

        [RemoteEvent(ToServerEvent.GotHit)]
        public void OnGotHit(IPlayer player, int attackerRemoteId, string weaponHashStr, string boneIdxStr)
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

        [RemoteEvent(ToServerEvent.LeaveLobby)]
        public void OnLeaveLobby(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLeaveLobby(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.LoadUserpanelData)]
        public void OnLoadUserpanelData(IPlayer player, int dataType)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnLoadUserpanelData(tdsPlayer, dataType);
        }

        [RemoteEvent(ToServerEvent.MapCreatorStartNewMap)]
        public void OnMapCreatorStartNewMap(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorStartNewMap(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncLastId)]
        public void OnMapCreatorSyncLastId(IPlayer player, int id)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncLastId(tdsPlayer, id);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncNewObject)]
        public void OnMapCreatorSyncNewObject(IPlayer player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncNewObject(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncObjectPosition)]
        public void OnMapCreatorSyncObjectPosition(IPlayer player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncObjectPosition(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncRemoveObject)]
        public void OnMapCreatorSyncRemoveObject(IPlayer player, int id)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncRemoveObject(tdsPlayer, id);
        }

        [RemoteEvent(ToServerEvent.MapCreatorSyncRemoveTeamObjects)]
        public void OnMapCreatorSyncRemoveTeamObjects(IPlayer player, int teamNumber)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSyncRemoveTeamObjects(tdsPlayer, teamNumber);
        }

        [RemoteEvent(ToServerEvent.MapsListRequest)]
        public void OnMapsListRequest(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapsListRequest(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.OutsideMapLimit)]
        public void OnOutsideMapLimit(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnOutsideMapLimit(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.RemoveMap)]
        public void OnRemoveMap(IPlayer player, int mapId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnRemoveMap(tdsPlayer, mapId);
        }

        [RemoteEvent(ToServerEvent.SendMapRating)]
        public void OnSendMapRating(IPlayer player, int mapId, int rating)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSendMapRating(tdsPlayer, mapId, rating);
        }

        [RemoteEvent(ToServerEvent.SendTeamOrder)]
        public void OnSendTeamOrder(IPlayer player, int teamOrderInt)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            if (!Enum.IsDefined(typeof(TeamOrder), teamOrderInt))
                return;
            Init.TDSCore.RemoteEventsHandler.OnSendTeamOrder(tdsPlayer, (TeamOrder)teamOrderInt);
        }

        [RemoteEvent(ToServerEvent.SetInFreecam)]
        public void OnSetInFreecam(IPlayer player, bool inFreeCam)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnMapCreatorSetInFreeCam(tdsPlayer, inFreeCam);
        }

        [RemoteEvent(ToServerEvent.SpectateNext)]
        public void OnSpectateNext(IPlayer player, bool forward)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSpectateNext(tdsPlayer, forward);
        }

        [RemoteEvent(ToServerEvent.StartDefusing)]
        public void OnStartDefusing(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStartDefusing(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.StartPlanting)]
        public void OnStartPlanting(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStartPlanting(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.StopDefusing)]
        public void OnStopDefusing(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStopDefusing(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.StopPlanting)]
        public void OnStopPlanting(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnStopPlanting(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.SuicideKill)]
        public void OnSuicideKill(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSuicideKill(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.SuicideShoot)]
        public void OnSuicideShoot(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnSuicideShoot(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.ToggleCrouch)]
        public void OnToggleCrouch(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnToggleCrouch(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.TryLogin)]
        public void TryLogin(IPlayer player, string username, string password)
        {
            var tdsPlayer = Init.GetNotLoggedInTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.TryLogin(tdsPlayer, username, password);
        }

        [RemoteEvent(ToServerEvent.TryRegister)]
        public void TryRegister(IPlayer player, string username, string password, string email)
        {
            var tdsPlayer = Init.GetNotLoggedInTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.TryRegister(tdsPlayer, username, password, email);
        }

        [RemoteEvent(ToServerEvent.CommandUsed)]
        public void UseCommand(IPlayer player, string msg)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.UseCommand(tdsPlayer, msg);
        }

        #endregion Public Methods
    }
}
