namespace TDS_Server.Entity
{
    public class AdminLevelNames
    {
        public byte Level { get; set; }
        public byte Language { get; set; }
        public string Name { get; set; }

        public virtual Languages LanguageNavigation { get; set; }
        public virtual AdminLevels LevelNavigation { get; set; }
    }
}
