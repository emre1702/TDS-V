﻿using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.LobbyEntities
{
    public class LobbyArmsRaceWeapons
    {
        public int LobbyId { get; set; }
        public WeaponHash? WeaponHash { get; set; }

        public short AtKill { get; set; }
       
        public virtual Lobbies Lobby { get; set; }
        public virtual Weapons Weapon { get; set; }
    }
}
