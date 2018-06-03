using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TDSCPServer.controller
{
    [Authorize]
    [Route("[controller]")]
    public class LoginController : Controller
    {

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody]UserData data)
        {
            string error = await Login.AuthenticateAsync(data);
            if (error != null)
                return Ok(new { error });

            JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
            byte[] key = Startup.Secret;
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, data.UID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenhandler.CreateToken(descriptor);
            string tokenstring = tokenhandler.WriteToken(token);

            return Ok(new
            {
                uid = data.UID,
                token = tokenstring
            });
        }

        public IActionResult Index()
        {
            Database context = HttpContext.RequestServices.GetService(typeof(Database)) as Database;

            return View();
        }
    }
}