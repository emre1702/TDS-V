using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TDS_Server.Core.Init.Services.Creators;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Server;

namespace TDS_Server.Core.Init
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TDSDbContext>
    {
        #region Public Methods

        public TDSDbContext CreateDbContext(string[] args)
        {
            var appConfigHandler = new AppConfigHandler();

            var optionsBuilder = new DbContextOptionsBuilder<TDSDbContext>();

            DatabaseCreator.CreateDbContextOptionsBuilder(optionsBuilder, appConfigHandler, null);

            return new TDSDbContext(optionsBuilder.Options);
        }

        #endregion Public Methods
    }
}
