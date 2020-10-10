using System;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas
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
