using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS_Server.Data.Interfaces.GamemodesSystem.Players;
using TDS_Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS_Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS_Server.GamemodesSystem.DependenciesModels;
using TDS_Server.GamemodesSystem.MapHandlers;
using TDS_Server.GamemodesSystem.Players;
using TDS_Server.GamemodesSystem.Specials;
using TDS_Server.GamemodesSystem.Teams;

namespace TDS_Server.GamemodesSystem.Gamemodes
{
    public class GangwarGamemode : BaseGamemode, IGangwarGamemode
    {
        public new IGangwarGamemodeMapHandler MapHandler => (IGangwarGamemodeMapHandler)base.MapHandler;
        public new IGangwarGamemodePlayers Players => (IGangwarGamemodePlayers)base.Players;
        public new IGangwarGamemodeSpecials Specials => (IGangwarGamemodeSpecials)base.Specials;
        public new IGangwarGamemodeTeams Teams => (IGangwarGamemodeTeams)base.Teams;

        public GangwarGamemode(ISettingsHandler settingsHandler) : base(settingsHandler)
        {
        }

        protected override void InitDependencies(BaseGamemodeDependencies? d = null)
        {
            d ??= new BaseGamemodeDependencies();

            d.MapHandler ??= new GangwarMapHandler(Lobby, this, SettingsHandler);
            d.Players ??= new GangwarPlayers();
            d.Specials ??= new GangwarSpecials(Lobby, this, SettingsHandler);
            d.Teams ??= new GangwarTeams(Lobby);

            base.InitDependencies(d);
        }
    }
}
