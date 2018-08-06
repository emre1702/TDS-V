using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TDSCPServer.Models;

namespace TDSCPServer.Controllers
{

    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {

        private static Queue<ChatMessage> lastChatMessages = new Queue<ChatMessage>();

        public static void AddChatMessage(ChatMessage message)
        {
            lastChatMessages.Enqueue(message);
            while (lastChatMessages.Count > 25)
                lastChatMessages.Dequeue();
        }

        [HttpGet]
        public IEnumerable<ChatMessage> GetLastChatMessages()
        {
            return lastChatMessages.Take(25);
        }
    }
}