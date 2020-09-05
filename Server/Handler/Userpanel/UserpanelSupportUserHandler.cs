using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Userpanel;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSupportUserHandler : IUserpanelSupportUserHandler
    {
        #region Private Fields

        private readonly IUserpanelSupportRequestHandler _userpanelSupportRequestHandler;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelSupportUserHandler(IUserpanelSupportRequestHandler userpanelSupportRequestHandler)
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
