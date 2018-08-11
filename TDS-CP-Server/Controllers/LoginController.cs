using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TDSCPServer.Models;

namespace TDSCPServer.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody]UserLoginData data)
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
                    new Claim(ClaimTypes.Name, data.Username),
                    new Claim("UID", data.UID.ToString()),
                    new Claim("AdminLvl", data.AdminLvl.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenhandler.CreateToken(descriptor);
            string tokenstring = tokenhandler.WriteToken(token);

            return Ok(new
            {
                uid = data.UID,
                token = tokenstring,
                username = data.Username,
                adminlvl = data.AdminLvl
            });
        }
    }
}