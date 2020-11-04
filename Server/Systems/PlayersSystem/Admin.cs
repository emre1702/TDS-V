using MoreLinq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Data.Models;
using TDS_Server.Handler;

namespace TDS_Server.PlayersSystem
{
    public class Admin : IPlayerAdmin
    {
        public AdminLevelDto Level
        {
            get
            {
                if (_player.IsConsole)
                    return _adminsHandler.GetHighestLevel();
                if (_player.Entity is null)
                    return _adminsHandler.GetLowestLevel();
                return _adminsHandler.GetLevel(_player.Entity.AdminLvl);
            }
        }

        public string LevelName 
        {
            get { lock (Level.Names) return Level.Names[_player.LanguageHandler.Enum]; }
        }

        private readonly AdminsHandler _adminsHandler;
#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public Admin(AdminsHandler adminsHandler)
        {
            _adminsHandler = adminsHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }
    }
}
