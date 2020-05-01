using GTANetworkAPI;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI
{
    class Init : Script
    {
#nullable disable warnings
        public static BaseAPI BaseAPI;
        public static Core.Init.Program TDSCore;
        public static WorkaroundsHandler WorkaroundsHandler;
#nullable restore warnings

        public Init()
        {
            BaseAPI = new BaseAPI();

            TDSCore = new Core.Init.Program(BaseAPI);
            WorkaroundsHandler = new WorkaroundsHandler(TDSCore.EventsHandler, BaseAPI, TDSCore.LobbiesHandler);

            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);
            var date = DateTime.UtcNow;
            NAPI.World.SetTime(date.Hour, date.Minute, date.Second);

            TDSCore.EventsHandler.Minute += (_) =>
            {
                date = DateTime.UtcNow;
                NAPI.World.SetTime(date.Hour, date.Minute, date.Second);
            };
        }

        internal static ITDSPlayer? GetTDSPlayerIfLoggedIn(GTANetworkAPI.Player player)
        {
            if (player is null)
                return null;

            var modPlayer = GetModPlayer(player);
            if (modPlayer is null)
                return null;

            return GetTDSPlayerIfLoggedIn(modPlayer);
        }

        internal static ITDSPlayer? GetTDSPlayerIfLoggedIn(IPlayer? player)
            => player is { } ? TDSCore.GetTDSPlayerIfLoggedIn(player) : null;

        internal static ITDSPlayer? GetTDSPlayerIfLoggedIn(ushort remoteId)
            => TDSCore.GetTDSPlayerIfLoggedIn(remoteId);

        internal static ITDSPlayer? GetTDSPlayer(GTANetworkAPI.Player player)
        {
            var modPlayer = GetModPlayer(player);
            if (modPlayer is null)
                return null;

            return GetTDSPlayer(modPlayer);
        }

        internal static ITDSPlayer GetTDSPlayer(IPlayer player)
        {
            return TDSCore.GetTDSPlayer(player);
        }

        internal static ITDSVehicle? GetTDSVehicle(IVehicle? vehicle) 
            => vehicle is null ? null : TDSCore.GetTDSVehicle(vehicle);

        internal static ITDSPlayer? GetNotLoggedInTDSPlayer(GTANetworkAPI.Player player)
        {
            var modPlayer = GetModPlayer(player);
            if (modPlayer is null)
                return null;

            return GetNewTDSPlayer(modPlayer);
        }

        internal static ITDSPlayer GetNewTDSPlayer(IPlayer player)
        {
            return TDSCore.GetNotLoggedInTDSPlayer(player);
        }

        internal static IPlayer? GetModPlayer(GTANetworkAPI.Player player)
        {
            return BaseAPI.EntityConvertingHandler.GetEntity(player);
        }

    }
}
