using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.Data.Interfaces.ModAPI.Blip
{
    public interface IBlip : IEntity
    {
        #region Public Properties

        int Rotation { set; }

        #endregion Public Properties
    }
}
