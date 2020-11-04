using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Chats
{
    public class GangLobbyChat : BaseLobbyChat
    {
        protected new IGangLobby Lobby => (IGangLobby)base.Lobby;

        public GangLobbyChat(IGangLobby lobby, LangHelper langHelper)
            : base(lobby, langHelper)
        {
        }

        public override void Send(string msg)
        {
            base.Send(msg);
            Lobby.DoForGangActionLobbies(lobby => lobby.Chat.Send(msg));
        }

        public override void Send(string msg, HashSet<int> blockingPlayerIds)
        {
            base.Send(msg, blockingPlayerIds);
            Lobby.DoForGangActionLobbies(lobby => lobby.Chat.Send(msg, blockingPlayerIds));
        }

        public override void Send(Dictionary<ILanguage, string> texts)
        {
            base.Send(texts);
            Lobby.DoForGangActionLobbies(lobby => lobby.Chat.Send(texts));
        }

        public override void Send(Func<ILanguage, string> langGetter)
        {
            base.Send(langGetter);
            Lobby.DoForGangActionLobbies(lobby => lobby.Chat.Send(langGetter));
        }
    }
}
