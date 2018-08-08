using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace TDSCPServer.Controllers
{

    [Authorize]
    [Route("[controller]")]
    public class LogoutController : Controller
    {

        private static IHubContext<NotifyHub> _hubContext;

        public LogoutController(IHubContext<NotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public static void LogoutAll()
        {
            _hubContext.Clients.All.SendAsync("Logout");
        }

        [HttpPut]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}