using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Chat;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Chats
{
    public class BaseLobbyChat : IBaseLobbyChat
    {
        protected IBaseLobby Lobby { get; }
        private readonly LangHelper _langHelper;

        public BaseLobbyChat(IBaseLobby lobby, LangHelper langHelper)
            => (Lobby, _langHelper) = (lobby, langHelper);

        public virtual void Send(string msg)
        {
            Lobby.Players.DoInMain(player =>
            {
                player.SendChatMessage(msg);
            });
        }

        public virtual void Send(string msg, HashSet<int> blockingPlayerIds)
        {
            Lobby.Players.DoInMain(player =>
            {
                if (blockingPlayerIds.Contains(player.Entity?.Id ?? 0))
                    return;
                player.SendChatMessage(msg);
            });
        }

        public virtual void Send(Func<ILanguage, string> langGetter)
        {
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(langGetter);
            Lobby.Players.DoInMain(player =>
            {
                player.SendChatMessage(texts[player.Language]);
            });
        }

        public virtual void Send(Dictionary<ILanguage, string> texts)
        {
            Lobby.Players.DoInMain(player =>
            {
                player.SendChatMessage(texts[player.Language]);
            });
        }
    }
}
