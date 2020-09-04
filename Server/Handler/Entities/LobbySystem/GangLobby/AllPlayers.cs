using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        #region Public Methods

        public void SendAllPlayerChatMessage(string message, bool includePlayersInActions)
        {
            SendMessage(message);
            if (includePlayersInActions)
            {
                var lobbiesToSendTheMsg = GetAllDerivedLobbies();
                foreach (var lobby in lobbiesToSendTheMsg)
                {
                    lobby.SendChatMessage(message);
                }
            }
        }

        public void SendAllPlayerLangMessage(Func<ILanguage, string> langGetter, bool includePlayersInActions)
        {
            SendMessage(langGetter);
            if (includePlayersInActions)
            {
                var lobbiesToSendTheMsg = GetAllDerivedLobbies();
                foreach (var lobby in lobbiesToSendTheMsg)
                {
                    lobby.SendChatMessage(langGetter);
                }
            }
        }

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langGetter, bool includePlayersInActions)
        {
            SendNotification(langGetter);
            if (includePlayersInActions)
            {
                var lobbiesToSendTheMsg = GetAllDerivedLobbies();
                foreach (var lobby in lobbiesToSendTheMsg)
                {
                    lobby.SendNotification(langGetter);
                }
            }
        }

        #endregion Public Methods
    }
}