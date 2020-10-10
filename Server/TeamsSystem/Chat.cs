using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler.Helper;

namespace TDS_Server.TeamsSystem
{
    public class Chat : ITeamChat
    {
        public string Color { get; private set; } = string.Empty;

        private readonly ITeam _team;
        private readonly LangHelper _langHelper;

        public Chat(ITeam team, LangHelper langHelper)
        {
            _team = team;
            _langHelper = langHelper;

            InitColor();
        }

        public void InitColor()
        {
            Color = $"!${_team.Entity.ColorR}|{_team.Entity.ColorG}|{_team.Entity.ColorB}$";
        }

        public void Send(string msg)
        {
            _team.Players.DoInMain(player =>
            {
                player.SendChatMessage(msg);
            });
        }

        public void Send(string msg, HashSet<int> blockingPlayerIds)
        {
            _team.Players.DoInMain(player =>
            {
                if (blockingPlayerIds.Contains(player.Entity?.Id ?? 0))
                    return;
                player.SendChatMessage(msg);
            });
        }

        public void Send(Func<ILanguage, string> langGetter)
        {
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(langGetter);
            _team.Players.DoInMain(player =>
            {
                player.SendChatMessage(texts[player.Language]);
            });
        }

        public void Send(Dictionary<ILanguage, string> texts)
        {
            _team.Players.DoInMain(player =>
            {
                player.SendChatMessage(texts[player.Language]);
            });
        }
    }
}
