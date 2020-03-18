using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        public void SendAllPlayerLangMessage(Func<ILanguage, string> langGetter, bool includePlayersInActions)
        {
            SendAllPlayerLangMessage(langGetter);
            if (includePlayersInActions)
            {
                var lobbiesToSendTheMsg = GetAllDerivedLobbies();
                foreach (var lobby in lobbiesToSendTheMsg)
                {
                    lobby.SendAllPlayerLangMessage(langGetter);
                }
            }
        }

        public void SendAllPlayerChatMessage(string message, bool includePlayersInActions)
        {
            SendAllPlayerChatMessage(message);
            if (includePlayersInActions)
            {
                var lobbiesToSendTheMsg = GetAllDerivedLobbies();
                foreach (var lobby in lobbiesToSendTheMsg)
                {
                    lobby.SendAllPlayerChatMessage(message);
                }
            }
        }

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langGetter, bool includePlayersInActions)
        {
            SendAllPlayerLangNotification(langGetter);
            if (includePlayersInActions)
            {
                var lobbiesToSendTheMsg = GetAllDerivedLobbies();
                foreach (var lobby in lobbiesToSendTheMsg)
                {
                    lobby.SendAllPlayerLangNotification(langGetter);
                }
            }
        }
    }
}
