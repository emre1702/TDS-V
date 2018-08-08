using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDSCPServer.Enums;
using TDSCPServer.Utils;

namespace TDSCPServer.Controllers
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

        
        private static T GetValue<T>(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) ? (T) row[column] : default(T);
        }

        public LogEntry(DataRow row, int type)
        {
            id = Convert.ToInt32(row["id"]);
            name = Convert.ToString(row["name"]);
            target = Convert.ToString(row["target"]);
            this.type = type;
            info = Convert.ToString(row["info"]);
            lobby = GetValue<string>(row, "lobby");
            date = Convert.ToString(row["date"]);
        }
    }

    [Authorize]
    [Route("[controller]")]
    public class LogsController : Controller
    {
        private const int showEntriesPerPage = 25;
        private static readonly Dictionary<int, Actions> adminActionByType = new Dictionary<int, Actions>
        {
            { -1, Actions.AdminLogAll },
            { 0, Actions.AdminLogPermaban },
            { 1, Actions.AdminLogTimeban },
            { 2, Actions.AdminLogUnban },
            { 3, Actions.AdminLogPermamute },
            { 4, Actions.AdminLogTimemute },
            { 5, Actions.AdminLogUnmute },
            { 6, Actions.AdminLogNext },
            { 7, Actions.AdminLogKick },
            { 8, Actions.AdminLogLobbyKick },
            { 9, Actions.AdminLogPermabanLobby },
            { 10, Actions.AdminLogTimebanLobby },
            { 11, Actions.AdminLogUnbanLobby }
        };
        private static readonly Dictionary<int, Actions> restActionByType = new Dictionary<int, Actions>
        {
            { 0, Actions.RestLogLogin },
            { 1, Actions.RestLogRegister },
            { 2, Actions.RestLogChat },
            { 3, Actions.RestLogError },
            { 4, Actions.RestLogLobbyOwner },
            { 5, Actions.RestLogLobbyJoin },
            { 6, Actions.RestLogVIP }
        };


        [HttpGet("admin/amountrows")]
        public async Task<int> GetLogEntriesAmountAdmin(int type, string onlyname = "", string onlytarget = "")
        {
            if (!Admin.IsAllowed(User, adminActionByType[type]))
                return 0;

            string rowsamountsql;
            if (onlyname == "" && onlytarget == "")
            {
                rowsamountsql = "SELECT Count(id) as count FROM adminlog";
                if (type != -1)
                {
                    rowsamountsql += $" WHERE type = {type}";
                }
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"SELECT Count(DISTINCT log.id) as count FROM adminlog as log, player, player as target WHERE log.type = " + (type == -1 ? "log.type" : type.ToString()) );
                if (type != -1)
                {

                }
                if (onlyname != "")
                {
                    if (onlyname == "0")
                    {
                        builder.Append($" AND log.adminuid = 0");
                    }
                    else
                    {
                        builder.Append($" AND log.adminuid = player.uid AND player.name = '{onlyname}'");
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
                rowsamountsql = builder.ToString();
            }
            DataTable rowsamounttable = await Database.ExecResult(rowsamountsql);
            if (rowsamounttable.Rows.Count > 0)
            {
                return Convert.ToInt32(rowsamounttable.Rows[0]["count"]);
            }
            return 0;
        }

        [HttpGet("rest/amountrows")]
        public async Task<int> GetLogEntriesAmountRest(int type, string onlyname = "", string onlytarget = "", string onlylobby = "")
        {
            if (!Admin.IsAllowed(User, restActionByType[type]))
                return 0;

            string rowsamountsql;
            if (onlyname == "" && onlytarget == "" && onlylobby == "") 
            {
                rowsamountsql = $"SELECT Count(id) as count FROM log";
                if (type != -1)
                {
                    rowsamountsql += $" WHERE type = {type}";
                }
            } else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"SELECT Count(DISTINCT log.id) as count FROM log, player, player as target WHERE log.type = " + (type == -1 ? "log.type" : type.ToString()));
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
            DataTable rowsamounttable = await Database.ExecResult(rowsamountsql);
            if (rowsamounttable.Rows.Count > 0)
            {   
                return Convert.ToInt32(rowsamounttable.Rows[0]["count"]);
            }
            return 0;
        }

        [HttpGet("admin")]
        public async Task<IEnumerable<LogEntry>> GetLogEntriesAdmin(int type, int page, string onlyname = "", string onlytarget = "")
        {

            if (!Admin.IsAllowed(User, adminActionByType[type]))
                return null;

            page = page * showEntriesPerPage;
            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT log.id, IF(log.adminuid = 0, log.adminuid, player.name) AS name, IF(log.targetuid = 0, log.targetuid, targetplayer.name) AS target, log.info, log.date FROM adminlog as log, player, player as targetplayer WHERE log.type = " + (type == -1 ? "log.type" : type.ToString()) + " AND (log.adminuid = 0 OR log.adminuid = player.uid) AND (log.targetuid = 0 OR log.targetuid = targetplayer.uid)");
            if (onlyname != "")
            {
                builder.Append($" AND IF(log.adminuid = 0, log.adminuid, player.name) = '{onlyname}'");
            }
            if (onlytarget != "")
            {
                builder.Append($" AND IF(log.targetuid = 0, log.targetuid, targetplayer.name) = '{onlytarget}'");
            }
            builder.Append($" GROUP BY log.id ORDER BY id DESC LIMIT {page}, {showEntriesPerPage}");
            string logsql = builder.ToString();
            DataTable table = await Database.ExecResult(logsql);

            List<LogEntry> entries = new List<LogEntry>();
            foreach (DataRow row in table.Rows)
            {
                entries.Add(new LogEntry(row, type));
            }
            return entries;
        }

        [HttpGet("rest")]
        public async Task<IEnumerable<LogEntry>> GetLogEntriesRest(int type, int page, string onlyname = "", string onlytarget = "", string onlylobby = "")
        {
            if (!Admin.IsAllowed(User, restActionByType[type]))
                return null;

            page = page * showEntriesPerPage;
            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT log.id, IF(log.uid = 0, log.uid, player.name) AS name, IF(log.targetuid = 0, log.targetuid, targetplayer.name) AS target, log.info, log.lobby, log.date FROM log, player, player as targetplayer WHERE log.type = "+(type == -1 ? "log.type" : type.ToString())+" AND (log.uid = 0 OR log.uid = player.uid) AND (log.targetuid = 0 OR log.targetuid = targetplayer.uid)");
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