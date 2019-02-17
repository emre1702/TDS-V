using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Enum;

namespace TDS_Server.Instance.Dto
{
    class TDSCommandInfos
    {
        public string Command;
        public ECommandUsageRight WithRight;

        public bool AsAdmin => WithRight == ECommandUsageRight.Admin;
        public bool AsDonator => WithRight == ECommandUsageRight.Donator;
        public bool AsLobbyOwner => WithRight == ECommandUsageRight.LobbyOwner;
        public bool AsUser => WithRight == ECommandUsageRight.User;
        public bool AsVIP => WithRight == ECommandUsageRight.VIP;
    }
}
