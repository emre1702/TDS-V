using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelHandler
    {
        IUserpanelApplicationsAdminHandler ApplicationsAdminHandler { get; }
        IUserpanelApplicationUserHandler ApplicationUserHandler { get; }
        IUserpanelSupportUserHandler SupportUserHandler { get; }
        IUserpanelSupportAdminHandler SupportAdminHandler { get; }
        IUserpanelSupportRequestHandler SupportRequestHandler { get; }
        IUserpanelSettingsNormalHandler SettingsNormalHandler { get; }
        IUserpanelSettingsSpecialHandler SettingsSpecialHandler { get; }
        IUserpanelOfflineMessagesHandler OfflineMessagesHandler { get; }

        void PlayerLoadData(ITDSPlayer player, UserpanelLoadDataType type);
    }
}
