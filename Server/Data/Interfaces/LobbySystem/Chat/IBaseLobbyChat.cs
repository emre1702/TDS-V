using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.Interfaces.LobbySystem.Chat
{
    public interface IBaseLobbyChat
    {
        void Send(Dictionary<ILanguage, string> texts, ITeam? targetTeam = null);
        void Send(Func<ILanguage, string> langGetter, ITeam? targetTeam = null);
        void Send(string msg, ITeam? targetTeam = null);
        void Send(string msg, HashSet<int> blockingPlayerIds, ITeam? targetTeam = null);
    }
}