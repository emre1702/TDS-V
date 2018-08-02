using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TDSCPServer.controller
{

    public class ReportUserEntry
    {
        public int id;
        public string author;
        public string title;
        public int amountanswers;
        public ushort foradminlvl;
        public bool open;
    }


    [Authorize]
    [Route("[controller]")]
    public class ReportsController : Controller
    {

        [HttpGet("user")] 
        public async Task<List<ReportUserEntry>> GetOwnReports()
        {
            string uidstr = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UID").Value;
            if (uidstr == null)
                return null;
            int uid = Convert.ToInt32(uidstr);
            
            DataTable result = await Database.ExecPreparedResult(
                @"SELECT reports.*, player.name, Count(reporttexts.id) as amountanswers 
                FROM reports
                LEFT JOIN reporttexts ON reports.id = reporttexts.reportid
                LEFT JOIN player ON reports.authoruid = player.uid
                WHERE reports.authoruid = 1", 
                new Dictionary<string, string> { { "uid", uid.ToString() } } 
            );
            List<ReportUserEntry> list = new List<ReportUserEntry>();
            foreach (DataRow row in result.Rows)
            {
                ReportUserEntry entry = new ReportUserEntry
                {
                    id = Convert.ToInt32(row["id"]),
                    author = Convert.ToString(row["name"]),
                    title = Convert.ToString(row["title"]),
                    amountanswers = Convert.ToInt32(row["amountanswers"]),
                    foradminlvl = Convert.ToUInt16(row["foradminlvl"]),
                    open = Convert.ToBoolean(row["open"])
                };
                list.Add(entry);
            }

            Console.WriteLine(list.Count);

            return list;
        }
    }
}