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

        [TDSCommand(AdminCommand.AdminChat)]
        public void AdminChat(ITDSPlayer player, [RemainingText] string text)
        {
            _chatHandler.SendAdminChat(player, text);
        }

        [TDSCommand(AdminCommand.AdminSay)]
        public void AdminSay(ITDSPlayer player, [RemainingText] string text)
        {
            _chatHandler.SendAdminMessage(player, text);
        }
    }
}
