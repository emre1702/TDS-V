using TDS_Server.Data.Enums;

namespace TDS_Server.Data.Models
{
    public class TDSCommandInfos
    {
        #region Public Fields

        public string Command;
        public CommandUsageRight WithRight = CommandUsageRight.User;

        #endregion Public Fields

        #region Public Constructors

        public TDSCommandInfos(string command) => Command = command;

        #endregion Public Constructors

        #region Public Properties

        public bool AsAdmin => WithRight == CommandUsageRight.Admin;
        public bool AsDonator => WithRight == CommandUsageRight.Donator;
        public bool AsLobbyOwner => WithRight == CommandUsageRight.LobbyOwner;
        public bool AsUser => WithRight == CommandUsageRight.User;
        public bool AsVIP => WithRight == CommandUsageRight.VIP;

        #endregion Public Properties
    }
}
