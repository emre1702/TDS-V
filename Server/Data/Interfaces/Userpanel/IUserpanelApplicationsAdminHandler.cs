using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelApplicationsAdminHandler
    {
        Task<string> GetData(ITDSPlayer player);
    }
}