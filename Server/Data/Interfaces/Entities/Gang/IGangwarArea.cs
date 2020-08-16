using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces.Entities.Gang
{
    #nullable enable
    public interface IGangwarArea
    {
        GangwarAreas? Entity { get; }
        IArena? InLobby { get; set; }
        IGang? Owner { get; }
        IGang? Attacker { get; }
        MapDto Map { get; }

        Task? SetConqueredWithoutAttack(IGang newOwner);
        Task SetAttackEnded(bool conquered);
        void SetInPreparation(IGang gang);
    }
}
