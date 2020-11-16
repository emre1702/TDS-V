using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.Userpanel;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelSupportUserHandler : IUserpanelSupportUserHandler
    {
        private readonly IUserpanelSupportRequestHandler _userpanelSupportRequestHandler;

        public UserpanelSupportUserHandler(IUserpanelSupportRequestHandler userpanelSupportRequestHandler)
            => _userpanelSupportRequestHandler = userpanelSupportRequestHandler;

        public Task<string?> GetData(ITDSPlayer player)
        {
            if (player.Entity is null)
                return Task.FromResult((string?)null);

            return _userpanelSupportRequestHandler.GetSupportRequests(player);
        }
    }
}
