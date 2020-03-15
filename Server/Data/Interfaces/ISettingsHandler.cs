using TDS_Server.Database.Entity.Server;

namespace TDS_Server.Data.Interfaces
{
    public interface ISettingsHandler
    {
        public ServerSettings ServerSettings { get; }
    }
}
