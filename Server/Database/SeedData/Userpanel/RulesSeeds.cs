using Microsoft.EntityFrameworkCore;
using TDS.Server.Database.Entity.Userpanel;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Database.SeedData.Userpanel
{
    public static class RulesSeeds
    {
        public static ModelBuilder HasRules(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rules>().HasData(
                new Rules { Id = 1, Target = RuleTarget.User, Category = RuleCategory.General },
                new Rules { Id = 2, Target = RuleTarget.User, Category = RuleCategory.Chat },
                new Rules { Id = 3, Target = RuleTarget.Admin, Category = RuleCategory.General },
                new Rules { Id = 4, Target = RuleTarget.Admin, Category = RuleCategory.General },
                new Rules { Id = 5, Target = RuleTarget.Admin, Category = RuleCategory.General },
                new Rules { Id = 6, Target = RuleTarget.VIP, Category = RuleCategory.General },
                new Rules { Id = 7, Target = RuleTarget.VIP, Category = RuleCategory.General }
            );
            return modelBuilder;
        }
    }
}
