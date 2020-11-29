using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;

namespace TDS.Server.Handler.Commands.User
{
    public class UserChatCommands
    {
        private readonly ChatHandler _chatHandler;

        public UserChatCommands(ChatHandler chatHandler)
            => _chatHandler = chatHandler;

        [TDSCommand(UserCommand.GlobalChat)]
        public void GlobalChat(ITDSPlayer player, [RemainingText] string message)
        {
            _chatHandler.SendGlobalMessage(player, message);
        }

        [TDSCommand(UserCommand.TeamChat)]
        public void TeamChat(ITDSPlayer player, [RemainingText] string message)
        {
            _chatHandler.SendTeamChat(player, message);
        }
    }
}
