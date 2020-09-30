using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces.GangSystem.GangGamemodes
{
    public interface IGangArea
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
