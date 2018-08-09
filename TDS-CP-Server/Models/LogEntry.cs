using System;
using System.Data;

namespace TDSCPServer.Models
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
            return row.Table.Columns.Contains(column) ? (T)row[column] : default(T);
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
}
