using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.GangsSystem;

namespace TDS.Server.Data.Interfaces
{
#nullable enable

    public interface IGangGamemode
    {
        #region Public Properties

        string AreaName { get; }
        IGang? AttackerGang { get; }
        IGang? OwnerGang { get; }
        GangActionType Type { get; }

        #endregion Public Properties
    }
}
