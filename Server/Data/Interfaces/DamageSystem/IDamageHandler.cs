using System.Collections.Generic;
using TDS_Server.DamageSystem.Damages;
using TDS_Server.Data.Interfaces.DamageSystem.Damages;
using TDS_Server.Data.Interfaces.DamageSystem.Deaths;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Data.Interfaces.DamageSystem
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