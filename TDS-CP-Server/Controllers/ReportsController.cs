using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TDSCPServer.Controllers
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
            uint uid = Utils.User.GetUID(User);
            if (uid == 0)
                return null;
            
            DataTable result = await Database.ExecResult(
                $@"SELECT reports.*, player.name, Count(reporttexts.id) as amountanswers 
                FROM reports
                LEFT JOIN reporttexts ON reports.id = reporttexts.reportid
                LEFT JOIN player ON reports.authoruid = player.uid
                WHERE reports.authoruid = {uid}");
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

            return list;
        }

        [HttpPost("user/toggle_open")]
        public void ToggleOwnReportOpen(int reportid)
        {
            uint uid = Utils.User.GetUID(User);
            if (uid == 0)
                return;

            Database.Exec($"UPDATE reports SET open = !open WHERE id = {reportid} AND authoruid = {uid}");
        }
    }
}