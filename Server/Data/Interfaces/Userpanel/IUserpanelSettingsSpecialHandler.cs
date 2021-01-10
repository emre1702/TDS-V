using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSettingsSpecialHandler
    {
        string GetData(ITDSPlayer player);
    }
}