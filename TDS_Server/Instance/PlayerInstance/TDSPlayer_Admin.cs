using MoreLinq;
using System.Linq;
using TDS_Server.Dto;
using TDS_Server.Manager.Admin;

namespace TDS_Server.Instance.PlayerInstance
{
    partial class TDSPlayer
    {
        public AdminLevelDto AdminLevel
        {
            get
            {
                if (IsConsole)
                    return AdminsManager.AdminLevels.Values.MaxBy(a => a.Level).First();
                if (Entity is null)
                    return AdminsManager.AdminLevels[0];
                return AdminsManager.AdminLevels[Entity.AdminLvl];
            }
        }

        public string AdminLevelName => AdminLevel.Names[LanguageEnum];
    }
}
