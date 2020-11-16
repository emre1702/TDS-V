using System;
using System.Collections.Generic;

namespace TDS.Server.Data.Interfaces.TeamsSystem
{
    public interface ITeamChat
    {
        string Color { get; }

        void Init(ITeam team);

        void InitColor();

        void Send(Dictionary<ILanguage, string> texts);

        void Send(Func<ILanguage, string> langGetter);

        void Send(string msg);

        void Send(string msg, HashSet<int> blockingPlayerIds);
    }
}
