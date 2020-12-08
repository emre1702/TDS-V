using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS.Server.Data.Interfaces.GamemodesSystem.Players;
using TDS.Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS.Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS.Server.GamemodesSystem.DependenciesModels;
using TDS.Server.GamemodesSystem.MapHandlers;
using TDS.Server.GamemodesSystem.Players;
using TDS.Server.GamemodesSystem.Specials;
using TDS.Server.GamemodesSystem.Teams;

namespace TDS.Server.GamemodesSystem.Gamemodes
{
    public class GangwarGamemode : BaseGamemode, IGangwarGamemode
    {
        public new IGangwarGamemodeMapHandler MapHandler => (IGangwarGamemodeMapHandler)base.MapHandler;
        public new IGangwarGamemodePlayers Players => (IGangwarGamemodePlayers)base.Players;
        public new IGangwarGamemodeSpecials Specials => (IGangwarGamemodeSpecials)base.Specials;
        public new IGangwarGamemodeTeams Teams => (IGangwarGamemodeTeams)base.Teams;

        public GangwarGamemode(ISettingsHandler settingsHandler, IServiceProvider serviceProvider) : base(settingsHandler, serviceProvider)
        {
        }

        protected override void InitDependencies(BaseGamemodeDependencies? d = null)
        {
            d ??= new BaseGamemodeDependencies();

            d.MapHandler ??= new GangwarMapHandler(Lobby, SettingsHandler);
            d.Players ??= new GangwarPlayers();
            d.Specials ??= new GangwarSpecials(Lobby, this, SettingsHandler);
            d.Teams ??= new GangwarTeams(Lobby);

            base.InitDependencies(d);
        }
    }
}
