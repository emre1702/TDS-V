using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.Userpanel;

namespace TDS.Server.Handler.Userpanel
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
