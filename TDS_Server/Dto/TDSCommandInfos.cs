using TDS_Server.Enum;

namespace TDS_Server.Instance.Dto
{
    internal class TDSCommandInfos
    {
        public string Command;
        public ECommandUsageRight WithRight = ECommandUsageRight.User;

        public bool AsAdmin => WithRight == ECommandUsageRight.Admin;
        public bool AsDonator => WithRight == ECommandUsageRight.Donator;
        public bool AsLobbyOwner => WithRight == ECommandUsageRight.LobbyOwner;
        public bool AsUser => WithRight == ECommandUsageRight.User;
        public bool AsVIP => WithRight == ECommandUsageRight.VIP;

        public TDSCommandInfos(string command) => Command = command;
    }
}