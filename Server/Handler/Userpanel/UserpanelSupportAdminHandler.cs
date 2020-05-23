using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSupportAdminHandler : IUserpanelSupportAdminHandler
    {
        private readonly IUserpanelSupportRequestHandler _userpanelSupportRequestHandler;

        public UserpanelSupportAdminHandler(IUserpanelSupportRequestHandler userpanelSupportRequestHandler)
            => _userpanelSupportRequestHandler = userpanelSupportRequestHandler;

        public Task<string?> GetData(ITDSPlayer player)
        {
            if (player.Entity is null)
                return Task.FromResult((string?)null);

            return _userpanelSupportRequestHandler.GetSupportRequests(player);
        }
    }
}
