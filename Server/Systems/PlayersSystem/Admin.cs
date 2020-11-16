using MoreLinq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Data.Models;
using TDS.Server.Handler;

namespace TDS.Server.PlayersSystem
{
    public class Admin : IPlayerAdmin
    {
        public AdminLevelDto Level
        {
            get
            {
                if (_player.IsConsole)
                    return _adminsHandler.HighestLevel;
                if (_player.Entity is null)
                    return _adminsHandler.LowestLevel;
                return _adminsHandler.GetLevel(_player.Entity.AdminLvl);
            }
        }

        public string LevelName 
        {
            get { lock (Level.Names) return Level.Names[_player.LanguageHandler.EnumValue]; }
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
