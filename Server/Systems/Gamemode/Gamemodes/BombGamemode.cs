using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS_Server.Data.Interfaces.GamemodesSystem.Players;
using TDS_Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS_Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS_Server.GamemodesSystem.Deathmatch;
using TDS_Server.GamemodesSystem.DependenciesModels;
using TDS_Server.GamemodesSystem.MapHandlers;
using TDS_Server.GamemodesSystem.Players;
using TDS_Server.GamemodesSystem.Rounds;
using TDS_Server.GamemodesSystem.Specials;
using TDS_Server.GamemodesSystem.Teams;
using TDS_Server.GamemodesSystem.Weapons;
using TDS_Server.Handler.FakePickups;

namespace TDS_Server.GamemodesSystem.Gamemodes
{
    public class BombGamemode : BaseGamemode, IBombGamemode
    {
        public new IBombGamemodeMapHandler MapHandler => (IBombGamemodeMapHandler)base.MapHandler;
        public new IBombGamemodePlayers Players => (IBombGamemodePlayers)base.Players;
        public new IBombGamemodeSpecials Specials => (IBombGamemodeSpecials)base.Specials;
        public new IBombGamemodeTeams Teams => (IBombGamemodeTeams)base.Teams;

        public BombGamemode(ISettingsHandler settingsHandler, IServiceProvider serviceProvider) : base(settingsHandler, serviceProvider)
        {
        }

        protected override void InitDependencies(BaseGamemodeDependencies? d = null)
        {
            var fakePickupsHandler = ServiceProvider.GetRequiredService<FakePickupsHandler>();

            d ??= new BaseGamemodeDependencies();

            d.Deathmatch ??= new BombDeathmatch(this);
            d.MapHandler ??= new BombMapHandler(Lobby, this, fakePickupsHandler);
            d.Players ??= new BombPlayers(this);
            d.Rounds ??= new BombRounds(Lobby, this);
            d.Specials ??= new BombSpecials(Lobby, this, SettingsHandler);
            d.Teams ??= new BombTeams(Lobby);
            d.Weapons ??= new BombWeapons(this);

            base.InitDependencies(d);
        }
    }
}
