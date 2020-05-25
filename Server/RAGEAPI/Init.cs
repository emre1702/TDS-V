using GTANetworkAPI;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI
{
    internal class Init : Script
    {
#nullable disable warnings

        #region Public Fields

        public static BaseAPI BaseAPI;
        public static Core.Init.Program TDSCore;
        public static WorkaroundsHandler WorkaroundsHandler;

        #endregion Public Fields

#nullable restore warnings

        #region Public Constructors

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

        #endregion Public Constructors

        #region Internal Methods

        internal static ITDSPlayer GetNewTDSPlayer(IPlayer player)
        {
            return TDSCore.GetNotLoggedInTDSPlayer(player);
        }

        internal static ITDSPlayer? GetNotLoggedInTDSPlayer(IPlayer player)
            => GetNewTDSPlayer(player);

        internal static ITDSPlayer? GetTDSPlayer(IPlayer player)
            => GetTDSPlayer(player);

        internal static ITDSPlayer? GetTDSPlayerIfLoggedIn(IPlayer? player)
            => player is { } ? TDSCore.GetTDSPlayerIfLoggedIn(player) : null;

        internal static ITDSPlayer? GetTDSPlayerIfLoggedIn(ushort remoteId)
            => TDSCore.GetTDSPlayerIfLoggedIn(remoteId);

        #endregion Internal Methods
    }
}
