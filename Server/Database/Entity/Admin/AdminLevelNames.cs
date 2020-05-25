﻿using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Admin
{
    public partial class AdminLevelNames
    {
        #region Public Properties

        public Language Language { get; set; }
        public short Level { get; set; }
        public virtual AdminLevels LevelNavigation { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }
}
