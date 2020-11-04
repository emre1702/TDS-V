using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.GamemodesSystem.DependenciesModels;
using TDS_Server.GamemodesSystem.Gamemodes;
using TDS_Server.GangGamemodesSystem.Players;
using TDS_Server.GangGamemodesSystem.Specials;

namespace TDS_Server.GangGamemodesSystem.Gamemodes
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
