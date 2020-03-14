using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Admin
{
    public partial class AdminLevelNames
    {
        public short Level { get; set; }
        public Language Language { get; set; }
        public string Name { get; set; }

        public virtual AdminLevels LevelNavigation { get; set; }
    }
}
