﻿using TDS_Common.Enum;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server_DB.Entity.LobbyEntities
{
    public partial class LobbyWeapons
    {
        public EWeaponHash Hash { get; set; }
        public int Lobby { get; set; }
        public int Ammo { get; set; }
        public short? Damage { get; set; }
        public float? HeadMultiplicator { get; set; }

        public virtual Weapons HashNavigation { get; set; }
        public virtual Lobbies LobbyNavigation { get; set; }
    }
}