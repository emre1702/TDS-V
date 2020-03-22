using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Threading.Tasks;
using TDS_Server.Database.Entity;

namespace TDS_Server.Core.Manager.Utility
{
    internal class ResourceStart
    {

        private void LoadAll()
        {
            try
            {
                if (!CodeMistakesChecker.EverythingsAlright())
                {
                    NAPI.Resource.StopResource("tds");
                    return;
                }

                Workaround.Init();

            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }

        

        
    }
}
