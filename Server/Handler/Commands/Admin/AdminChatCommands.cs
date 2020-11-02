using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;

namespace TDS_Server.Handler.Commands.Admin
{
    public class AdminChatCommands
    {
        private readonly ChatHandler _chatHandler;

        public AdminChatCommands(ChatHandler chatHandler)
            => _chatHandler = chatHandler;

        [TDSCommand(AdminCommand.AdminChat)]
        public void AdminChat(ITDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminChat(player, text);
        }

        [TDSCommand(AdminCommand.AdminSay)]
        public void AdminSay(ITDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminMessage(player, text);
        }
    }
}
