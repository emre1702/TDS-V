using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;

namespace TDS_Server.GamemodesSystem.Gamemodes
{
    public class DeathmatchGamemode : BaseGamemode, IDeathmatchGamemode
    {
        public DeathmatchGamemode(ISettingsHandler settingsHandler) : base(settingsHandler)
        {
        }
    }
}
