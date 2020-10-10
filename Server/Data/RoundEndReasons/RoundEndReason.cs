using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.RoundEndReasons
{
#nullable enable

    public abstract class RoundEndReason : IRoundEndReason
    {
        public abstract Func<ILanguage, string> MessageProvider { get; }

        public abstract bool KillLoserTeam { get; }

        public ITeam? WinnerTeam { get; private set; }

        public abstract bool AddToServerStats { get; }

        public RoundEndReason(ITeam? winnerTeam)
        {
            WinnerTeam = winnerTeam;
        }
    }
}
