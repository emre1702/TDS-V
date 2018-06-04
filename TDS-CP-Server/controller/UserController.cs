using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TDSCPServer.controller
{

    [Authorize]
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpGet("names")]
        public async Task<IEnumerable<string>> GetUsernames()
        {
            DataTable result = await Database.ExecResult("SELECT name FROM playeronline");
            return result.Rows.OfType<DataRow>().Select(k => k["name"].ToString());
        }
    }
}