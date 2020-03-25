using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;

namespace TDS_Server.Core.Init
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TDSDbContext>
    {
        public TDSDbContext CreateDbContext(string[] args)
        {
            var appConfigHandler = new AppConfigHandler();

            var optionsBuilder = new DbContextOptionsBuilder<TDSDbContext>();

            Services.InitDbContextOptionsBuilder(optionsBuilder, appConfigHandler, null);

            return new TDSDbContext(optionsBuilder.Options);
        }
    }
}
