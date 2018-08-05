using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSCPServer.Models;

namespace TDSCPServer
{
    public class NotifyHub : Hub
    {
        private static Queue<ChatMessage> lastChatMessages = new Queue<ChatMessage>();

        public Task SendChatMessage(ChatMessage message)
        {
            lastChatMessages.Enqueue(message);
            while (lastChatMessages.Count > 25)
                lastChatMessages.Dequeue();
            return Clients.All.SendAsync("SendChatMessage", message);
        }

        public Task SendLastChatMessages()
        {
            return Clients.Caller.SendAsync("SendLastChatMessages", lastChatMessages.Take(25));
        }
    }
}
