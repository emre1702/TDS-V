﻿using GTANetworkAPI;
using Newtonsoft.Json;

namespace TDS.Server.Data.Models.CustomLobby
{
    public class CustomLobbyArmsRaceWeaponData
    {
        [JsonProperty("0")]
        public WeaponHash? WeaponHash { get; set; }

        [JsonProperty("1")]
        public short AtKill { get; set; }
    }
}
