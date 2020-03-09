using TDS_Common.Enum;

namespace TDS_Server.Database.Entity.Admin
{
    public partial class AdminLevelNames
    {
        public short Level { get; set; }
        public ELanguage Language { get; set; }
        public string Name { get; set; }

        public virtual AdminLevels LevelNavigation { get; set; }
    }
}
