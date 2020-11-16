using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminChatCommands
    {
        private readonly ChatHandler _chatHandler;

        public AdminChatCommands(ChatHandler chatHandler)
            => _chatHandler = chatHandler;

        [TDSCommandAttribute(AdminCommand.AdminChat)]
        public void AdminChat(ITDSPlayer player, [TDSRemainingTextAttribute] string text)
        {
            _chatHandler.SendAdminChat(player, text);
        }

        [TDSCommandAttribute(AdminCommand.AdminSay)]
        public void AdminSay(ITDSPlayer player, [TDSRemainingTextAttribute] string text)
        {
            _chatHandler.SendAdminMessage(player, text);
        }
    }
}
