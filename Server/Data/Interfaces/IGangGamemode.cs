using TDS_Server.Data.Enums;

namespace TDS_Server.Data.Interfaces
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
