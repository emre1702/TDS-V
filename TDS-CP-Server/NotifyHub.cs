using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSCPServer.Controllers;
using TDSCPServer.Enums;
using TDSCPServer.Models;

namespace TDSCPServer
{
    public class NotifyHub : Hub
    {
        private static IHubContext<NotifyHub> _hubContext;

        public NotifyHub(IHubContext<NotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendChatMessage(ChatMessage message)
        {
            ChatController.AddChatMessage(message);
            return Clients.OthersInGroup(EGroups.Loggedin.ToString()).SendAsync("SendChatMessage", message);
        }

        public Task AddToGroup(EGroups group, string optional = "")
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group.ToString() + optional);
        }

        public Task RemoveFromGroup(EGroups group, string optional = "")
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group.ToString() + optional);
        }

        public static void LogoutAll()
        {
            _hubContext.Clients.All.SendAsync("Logout");
        }

        
    }
}
