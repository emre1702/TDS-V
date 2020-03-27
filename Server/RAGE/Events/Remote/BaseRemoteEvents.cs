﻿using GTANetworkAPI;
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
            var tdsPlayer = Init.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            if (!System.Enum.IsDefined(typeof(Language), language))
                return;

            Init.TDSCore.RemoteEventsHandler.OnLanguageChange(tdsPlayer, (Language)language);
        }

        [RemoteEvent(ToServerEvent.RequestPlayersForScoreboard)]
        public static void RequestPlayersForScoreboard(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteEventsHandler.OnRequestPlayersForScoreboard(tdsPlayer);
        }
    }
}
