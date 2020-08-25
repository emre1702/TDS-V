﻿using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Entity.LobbySystem.GangLobbySystem
{
    partial class GangLobby
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}