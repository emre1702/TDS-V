using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto
{
    class CustomLobbyData
    {
        public int? LobbyId;
        public string Name = "";
        public string? OwnerName;
        public string Password = "";
        public short StartHealth;
        public short StartArmor;
        public short AmountLifes;
        public int SpawnAgainAfterDeathMs;
        public int DieAfterOutsideMapLimitTime;
    }
}
