using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;

namespace TDS.Server.GamemodesSystem.Gamemodes
{
    public class DeathmatchGamemode : BaseGamemode, IDeathmatchGamemode
    {
        public DeathmatchGamemode(ISettingsHandler settingsHandler, IServiceProvider serviceProvider) : base(settingsHandler, serviceProvider)
        {
        }
    }
}
