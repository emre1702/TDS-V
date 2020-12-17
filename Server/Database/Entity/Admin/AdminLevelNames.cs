using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Admin
{
    public class AdminLevelNames
    {
        public Language Language { get; set; }
        public short Level { get; set; }
        public virtual AdminLevels LevelNavigation { get; set; }
        public string Name { get; set; }
    }
}
