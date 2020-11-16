using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.GamemodesSystem.DependenciesModels;
using TDS.Server.GamemodesSystem.Gamemodes;
using TDS.Server.GangGamemodesSystem.Players;
using TDS.Server.GangGamemodesSystem.Specials;

namespace TDS.Server.GangGamemodesSystem.Gamemodes
{
    public class GangGangwarGamemode : GangwarGamemode, IGangGangwarGamemode
    {
        protected new IGangActionLobby Lobby => (IGangActionLobby)base.Lobby;

        public GangGangwarGamemode(ISettingsHandler settingsHandler, IServiceProvider serviceProvider) 
            : base(settingsHandler, serviceProvider)
        {
        }

        protected override void InitDependencies(BaseGamemodeDependencies? d = null)
        {
            d ??= new BaseGamemodeDependencies();

            d.Players ??= new GangGangwarPlayers(this);
            d.Specials ??= new GangGangwarSpecials(Lobby, this, SettingsHandler);

            base.InitDependencies(d);
        }
    }
}
