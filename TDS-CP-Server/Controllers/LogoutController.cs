using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace TDSCPServer.Controllers
{

    [Authorize]
    [Route("[controller]")]
    public class LogoutController : Controller
    {
        [HttpPut]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}