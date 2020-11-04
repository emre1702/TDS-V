using System.Collections.Generic;
using TDS_Server.DamageSystem.Damages;
using TDS_Server.DamageSystem.Deaths;
using TDS_Server.DamageSystem.KillingSprees;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.DamageSystem;
using TDS_Server.Data.Interfaces.DamageSystem.Damages;
using TDS_Server.Data.Interfaces.DamageSystem.Deaths;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler;

namespace TDS_Server.DamageSystem
{
    public class DamageHandler : IDamageHandler
    {
        public bool DamageDealtThisRound => HitterHandler.HasAnyHitters();

        public IDamageDealer DamageDealer { get; }
        public IDamageProvider DamageProvider { get; }
        public IDeathHandler DeathHandler { get; }
        public IHitterHandler HitterHandler { get; }

        private readonly KillingSpreeHandler _killingSpreeHandler;

        public DamageHandler(WeaponDatasLoadingHandler weaponDatasLoadingHandler, ILoggingHandler logger)
        {
            HitterHandler = new HitterHandler();

            DamageProvider = new DamageProvider(weaponDatasLoadingHandler);
            DamageDealer = new DamageDealer(DamageProvider, HitterHandler);

            _killingSpreeHandler = new KillingSpreeHandler();

            DeathHandler = new DeathHandler(HitterHandler, _killingSpreeHandler, logger);
        }

        public void Init(IFightLobby lobby, IEnumerable<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            HitterHandler.Init(lobby);
            DamageProvider.Init(weapons);
            _killingSpreeHandler.Init(killingspreeRewards);
            DeathHandler.Init(lobby);
        }
    }
}
