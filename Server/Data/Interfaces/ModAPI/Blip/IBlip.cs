namespace TDS_Server.Data.Interfaces.ModAPI.Blip
{
    public interface IBlip : IEntity
    {
        #region Public Properties

        int Color { get; set; }
        string Name { get; set; }
        int RouteColor { get; set; }
        bool RouteVisible { get; set; }
        float Scale { get; set; }
        bool ShortRange { get; set; }
        uint Sprite { get; set; }
        int Transparency { get; set; }

        #endregion Public Properties
    }
}
