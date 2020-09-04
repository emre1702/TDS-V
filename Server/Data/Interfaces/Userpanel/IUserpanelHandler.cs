using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelHandler
    {
        #region Public Properties

        IUserpanelApplicationsAdminHandler ApplicationsAdminHandler { get; }
        IUserpanelApplicationUserHandler ApplicationUserHandler { get; }
        IUserpanelOfflineMessagesHandler OfflineMessagesHandler { get; }
        IUserpanelPlayerWeaponStatsHandler PlayerWeaponStatsHandler { get; }
        IUserpanelPlayerCommandsHandler SettingsCommandsHandler { get; }
        IUserpanelSettingsNormalHandler SettingsNormalHandler { get; }
        IUserpanelSettingsSpecialHandler SettingsSpecialHandler { get; }
        IUserpanelSupportAdminHandler SupportAdminHandler { get; }
        IUserpanelSupportRequestHandler SupportRequestHandler { get; }
        IUserpanelSupportUserHandler SupportUserHandler { get; }

        #endregion Public Properties

        #region Public Methods

        void PlayerLoadData(ITDSPlayer player, UserpanelLoadDataType type);

        #endregion Public Methods
    }
}
