using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TDSCPServer.controller
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