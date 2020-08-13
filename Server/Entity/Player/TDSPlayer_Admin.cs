using MoreLinq;
using System.Linq;
using TDS_Server.Data.Models;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Properties

        public AdminLevelDto AdminLevel
        {
            get
            {
                if (IsConsole)
                    return _adminsHandler.AdminLevels.Values.MaxBy(a => a.Level).First();
                if (Entity is null)
                    return _adminsHandler.AdminLevels[0];
                return _adminsHandler.AdminLevels[Entity.AdminLvl];
            }
        }

        public string AdminLevelName => AdminLevel.Names[LanguageEnum];

        #endregion Public Properties
    }
}
