using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TDSCPServer.controller
{

    public class Player
    {
        public String Name;
        public uint AdminLvl;
    }

    [Authorize]
    [Route("[controller]")]
    public class PlayerController : Controller
    {
        [HttpGet("names")]
        public async Task<IEnumerable<Player>> GetUsernames()
        {
            DataTable result = await Database.ExecResult("SELECT playeronline.uid, player.name, player.adminlvl FROM playeronline, player WHERE playeronline.uid = player.uid");
            List<Player> players = new List<Player>();
            if (result != null && result.Rows != null && result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Player player = new Player { Name = Convert.ToString(row["name"]), AdminLvl = Convert.ToUInt32(row["adminlvl"]) };
                    players.Add(player);
                }
            }

            return players;
        }
    }
}