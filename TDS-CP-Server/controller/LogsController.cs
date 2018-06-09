using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public LogEntry(DataRow row)
        {
            id = Convert.ToInt32(row["id"]);
            name = Convert.ToString(row["name"]);
            target = Convert.ToString(row["target"]);
            type = Convert.ToInt32(row["type"]);
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
        public async Task<int> GetLogEntriesAmount(int type)
        {
            string rowsamountsql = $"SELECT Count(id) as count FROM log WHERE type = {type}";
            DataTable rowsamounttable = await Database.ExecResult(rowsamountsql);
            if (rowsamounttable.Rows.Count > 0)
            {
                return Convert.ToInt32(rowsamounttable.Rows[0]["count"]);
            }
            return 0;
        }

        [HttpGet("logs")]
        public async Task<IEnumerable<LogEntry>> GetLogEntries(int type, int page)
        {
            page = page * showEntriesPerPage;
            string logsql = $"SELECT log.id, IF(log.uid = 0, log.uid, player.name) AS name, IF(log.targetuid = 0, log.targetuid, targetplayer.name) AS target, log.type, log.info, log.lobby, log.date FROM log, player, player as targetplayer WHERE log.type = {type} AND (log.uid = 0 OR log.uid = player.uid) AND (log.targetuid = 0 OR log.targetuid = targetplayer.uid) GROUP BY log.id ORDER BY id DESC LIMIT {page}, {showEntriesPerPage}";
            DataTable table = await Database.ExecResult(logsql);
            List <LogEntry> entries = new List<LogEntry>();
            foreach (DataRow row in table.Rows)
            {
                entries.Add(new LogEntry(row));
            }
            return entries;
        }
    }
}