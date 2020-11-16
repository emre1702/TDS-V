using System;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas
{
#nullable enable

    public interface IRoundEndReason
    {
        Func<ILanguage, string> MessageProvider { get; }
        bool KillLoserTeam { get; }
        ITeam? WinnerTeam { get; }
        bool AddToServerStats { get; }
    }
}
