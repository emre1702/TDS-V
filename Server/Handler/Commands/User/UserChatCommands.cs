using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;

namespace TDS_Server.Handler.Commands.User
{
    public class UserChatCommands
    {
        private readonly ChatHandler _chatHandler;

        public UserChatCommands(ChatHandler chatHandler)
            => _chatHandler = chatHandler;

        [TDSCommand(UserCommand.GlobalChat)]
        public void GlobalChat(ITDSPlayer player, [TDSRemainingText] string message)
        {
            _chatHandler.SendGlobalMessage(player, message);
        }

        [TDSCommand(UserCommand.TeamChat)]
        public void TeamChat(ITDSPlayer player, [TDSRemainingText] string message)
        {
            _chatHandler.SendTeamChat(player, message);
        }
    }
}
