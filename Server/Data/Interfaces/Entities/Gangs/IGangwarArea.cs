using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models.Map;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Data.Interfaces.Entities.Gangs
{
    public interface IGangwarArea
    {
        IGang Attacker { get; }
        GangwarAreas Entity { get; }
        bool HasCooldown { get; set; }
        IGangActionLobby InLobby { get; set; }
        MapDto Map { get; }
        IGang Owner { get; }

        void CreateGangLobbyMapInfo();

        Task SetAttackEnded(bool conquered);

        Task SetConqueredWithoutAttack(IGang newOwner);

        void SetInAttack();

        void SetInPreparation(IGang attackerGang);
    }
}
