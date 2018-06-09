using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TDSCPServer.controller
{

    public class LogEntry
    {
        public int id;
        public string name;
        public string target;
        public int type;
        public string info;
        public string lobby;
        public string date;

        public LogEntry(DataRow row, int type)
        {
            id = Convert.ToInt32(row["id"]);
            name = Convert.ToString(row["name"]);
            target = Convert.ToString(row["target"]);
            this.type = type;
            info = Convert.ToString(row["info"]);
            lobby = Convert.ToString(row["lobby"]);
            date = Convert.ToString(row["date"]);
        }
    }

    [Authorize]
    [Route("[controller]")]
    public class LogsController : Controller
    {
        private const int showEntriesPerPage = 25;

        [HttpGet("logs/amountrows")]
        public async Task<int> GetLogEntriesAmount(int type, string onlyname = "", string onlytarget = "", string onlylobby = "")
        {
            string rowsamountsql;
            if (onlyname == "" && onlytarget == "" && onlylobby == "") 
            {
                rowsamountsql = $"SELECT Count(id) as count FROM log WHERE type = {type}";
            } else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"SELECT Count(DISTINCT log.id) as count FROM log, player, player as target WHERE log.type = {type}");
                if (onlyname != "")
                {
                    if (onlyname == "0")
                    {
                        builder.Append($" AND log.uid = 0");
                    }
                    else
                    {
                        builder.Append($" AND log.uid = player.uid AND player.name = '{onlyname}'");
                    }
                }
                if (onlytarget != "")
                {
                    if (onlyname == "0")
                    {
                        builder.Append($" AND log.targetuid = 0");
                    }
                    else
                    {
                        builder.Append($" AND log.targetuid = target.uid AND target.name = '{onlytarget}'");
                    }
                }
                if (onlylobby != "")
                {
                    builder.Append($" AND log.lobby = '{onlylobby}'");
                }
                rowsamountsql = builder.ToString();
            }
            Console.WriteLine(rowsamountsql);
            DataTable rowsamounttable = await Database.ExecResult(rowsamountsql);
            if (rowsamounttable.Rows.Count > 0)
            {   
                return Convert.ToInt32(rowsamounttable.Rows[0]["count"]);
            }
            return 0;
        }

        [HttpGet("logs")]
        public async Task<IEnumerable<LogEntry>> GetLogEntries(int type, int page, string onlyname = "", string onlytarget = "", string onlylobby = "")
        {
            page = page * showEntriesPerPage;
            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT log.id, IF(log.uid = 0, log.uid, player.name) AS name, IF(log.targetuid = 0, log.targetuid, targetplayer.name) AS target, log.info, log.lobby, log.date FROM log, player, player as targetplayer WHERE log.type = {type} AND (log.uid = 0 OR log.uid = player.uid) AND (log.targetuid = 0 OR log.targetuid = targetplayer.uid)");
            if (onlyname != "")
            {
                builder.Append($" AND IF(log.uid = 0, log.uid, player.name) = '{onlyname}'");
            }
            if (onlytarget != "")
            {
                builder.Append($" AND IF(log.targetuid = 0, log.targetuid, targetplayer.name) = '{onlytarget}'");
            }
            if (onlylobby != "")
            {
                builder.Append($" AND log.lobby = '{onlylobby}'");
            }
            builder.Append($" GROUP BY log.id ORDER BY id DESC LIMIT {page}, {showEntriesPerPage}");
            string logsql = builder.ToString();
            Console.WriteLine(logsql);
            DataTable table = await Database.ExecResult(logsql);
            List <LogEntry> entries = new List<LogEntry>();
            foreach (DataRow row in table.Rows)
            {
                entries.Add(new LogEntry(row, type));
            }
            return entries;
        }
    }
}