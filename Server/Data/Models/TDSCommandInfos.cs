using TDS_Server.Data.Enums;

namespace TDS_Server.Data.Models
{
    public class TDSCommandInfos
    {

        public string Command;
        public CommandUsageRight WithRight = CommandUsageRight.User;

        public TDSCommandInfos(string command) => Command = command;

        public bool AsAdmin => WithRight == CommandUsageRight.Admin;
        public bool AsDonator => WithRight == CommandUsageRight.Donator;
        public bool AsLobbyOwner => WithRight == CommandUsageRight.LobbyOwner;
        public bool AsUser => WithRight == CommandUsageRight.User;
        public bool AsVIP => WithRight == CommandUsageRight.VIP;

    }
}
