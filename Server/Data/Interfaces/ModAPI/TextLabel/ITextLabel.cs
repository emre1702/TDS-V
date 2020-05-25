namespace TDS_Server.Data.Interfaces.ModAPI.TextLabel
{
    public interface ITextLabel : IEntity
    {
        #region Public Properties

        System.Drawing.Color Color { get; set; }
        float Range { get; set; }
        bool Seethrough { get; set; }
        string Text { get; set; }

        #endregion Public Properties
    }
}
