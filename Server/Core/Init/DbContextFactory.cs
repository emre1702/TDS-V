using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TDS.Server.Core.Init.Services.Creators;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Server;

namespace TDS.Server.Core.Init
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TDSDbContext>
    {
        public TDSDbContext CreateDbContext(string[] args)
        {
            var appConfigHandler = new EnvironmentConfigHandler();

            var optionsBuilder = new DbContextOptionsBuilder<TDSDbContext>();

            DatabaseCreator.CreateDbContextOptionsBuilder(optionsBuilder, appConfigHandler, null);

            return new TDSDbContext(optionsBuilder.Options);
        }
    }
}