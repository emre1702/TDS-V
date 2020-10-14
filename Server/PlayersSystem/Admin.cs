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
                    return _adminsHandler.AdminLevels.Values.MaxBy(a => a.Level).First();
                if (_player.Entity is null)
                    return _adminsHandler.AdminLevels[0];
                return _adminsHandler.AdminLevels[_player.Entity.AdminLvl];
            }
        }

        public string LevelName => Level.Names[_player.LanguageHandler.Enum];

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
