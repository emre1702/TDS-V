using System.Threading.Tasks;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Logs
{
    static class LogsManager
    {
        public static TDSNewContext DbContext { get; private set; }

        static LogsManager()
        {
            DbContext = new TDSNewContext();
        }

        public static Task Save()
        {
            //return DbContext.SaveChangesAsync();
            return Task.FromResult(true);
        }
    }
}
