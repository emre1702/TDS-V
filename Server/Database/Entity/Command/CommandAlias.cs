namespace TDS.Server.Database.Entity.Command
{
    public class CommandAlias
    {
        public string Alias { get; set; }
        public short Command { get; set; }

        public virtual Commands CommandNavigation { get; set; }
    }
}
