using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSCPServer.Controllers;
using TDSCPServer.Models;

namespace TDSCPServer
{
    public class NotifyHub : Hub
    { 
        public Task SendChatMessage(ChatMessage message)
        {
            ChatController.AddChatMessage(message);
            return Clients.Others.SendAsync("SendChatMessage", message);
        }

        public Task AddToGroup(EGroupID id)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
        }

        public Task RemoveFromGroup(EGroupID id)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, id.ToString());
        }
    }
}
