using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Instance.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Log;

namespace TDS_Server.Core.Manager.Logs
{
    class LogsManager : EntityWrapperClass
    {
        private static LogsManager? _instance;

        public static void Init()
        {
            _instance = new LogsManager();
        }

        public static async Task Save()
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });
        }

        public static async void AddLog(LogRests log)
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDB(dbContext =>
            {
                dbContext.LogRests.Add(log);
            });
        }

        public static async void AddLog(LogErrors log)
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDB(dbContext =>
            {
                dbContext.LogErrors.Add(log);
            });
        }

        public static async void AddLog(LogChats log)
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDB(dbContext =>
            {
                dbContext.LogChats.Add(log);
            });
        }

        public static async void AddLog(LogAdmins log)
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDB(dbContext =>
            {
                dbContext.LogAdmins.Add(log);
            });
        }

        public static async void AddLog(LogKills log)
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDB(dbContext =>
            {
                dbContext.LogKills.Add(log);
            });
        }
    }
}
