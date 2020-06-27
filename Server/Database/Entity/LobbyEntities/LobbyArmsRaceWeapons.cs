using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.LobbyEntities
{
    public class LobbyArmsRaceWeapons
    {
        #region Public Properties

        public short AtKill { get; set; }
        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public Weapons Weapon { get; set; }
        public WeaponHash? WeaponHash { get; set; }

        #endregion Public Properties
    }
}
