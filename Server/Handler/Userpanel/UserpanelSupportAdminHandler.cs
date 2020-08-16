using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Userpanel;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSupportAdminHandler : IUserpanelSupportAdminHandler
    {
        #region Private Fields

        private readonly IUserpanelSupportRequestHandler _userpanelSupportRequestHandler;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelSupportAdminHandler(IUserpanelSupportRequestHandler userpanelSupportRequestHandler)
            => _userpanelSupportRequestHandler = userpanelSupportRequestHandler;

        #endregion Public Constructors

        #region Public Methods

        public Task<string?> GetData(ITDSPlayer player)
        {
            if (player.Entity is null)
                return Task.FromResult((string?)null);

            return _userpanelSupportRequestHandler.GetSupportRequests(player);
        }

        #endregion Public Methods
    }
}
