using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities;
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

        Task PlayerLoadData(ITDSPlayer player, UserpanelLoadDataType type);

        #endregion Public Methods
    }
}
