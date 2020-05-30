using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities;

namespace TDS_Server.Handler.GangSystem
{
    public class GangCreateHandler : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly IModAPI _modAPI;

        #endregion Private Fields

        #region Public Constructors

        public GangCreateHandler(IModAPI modAPI, TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : base(dbContext, loggingHandler)
        {
            _modAPI = modAPI;
        }

        #endregion Public Constructors
    }
}
