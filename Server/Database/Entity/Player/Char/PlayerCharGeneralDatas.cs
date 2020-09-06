﻿using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharGeneralDatas
    {
        #region Public Properties
        public int PlayerId { get; set; }
        public byte Slot { get; set; }

        public CharCreateGeneralData SyncedData { get; set; }

        public virtual PlayerCharDatas CharDatas { get; set; }

        #endregion Public Properties
    }
}
