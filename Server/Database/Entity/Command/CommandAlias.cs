namespace TDS.Server.Database.Entity.Command
{
    public partial class CommandAlias
    {
        #region Public Properties

        public string Alias { get; set; }
        public short Command { get; set; }

        public virtual Commands CommandNavigation { get; set; }

        #endregion Public Properties
    }
}
