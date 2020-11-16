using System.Collections.Generic;
using TDS.Server.DamageSystem.Damages;
using TDS.Server.Data.Interfaces.DamageSystem.Damages;
using TDS.Server.Data.Interfaces.DamageSystem.Deaths;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Data.Interfaces.DamageSystem
{
    public interface IDamageHandler
    {
        IDamageDealer DamageDealer { get; }
        IDamageProvider DamageProvider { get; }
        IDeathHandler DeathHandler { get; }
        IHitterHandler HitterHandler { get; }

        bool DamageDealtThisRound { get; }

        void Init(IFightLobby lobby, IEnumerable<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards);
    }
}