using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.RoundEndReasons
{
#nullable enable

    public abstract class RoundEndReason : IRoundEndReason
    {
        public abstract Func<ILanguage, string> MessageProvider { get; }

        public abstract bool KillLoserTeam { get; }

        public ITeam? WinnerTeam { get; private set; }

        public abstract bool AddToServerStats { get; }

        protected RoundEndReason(ITeam? winnerTeam)
        {
            WinnerTeam = winnerTeam;
        }
    }
}
