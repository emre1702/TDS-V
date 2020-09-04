using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.Entities.Gamemodes
{
#nullable enable
    public interface IGangwar : IGamemode
    {
        ITeam AttackerTeam { get; }
        ITeam OwnerTeam { get; }
        ITDSObject? TargetObject { get; }
    }
}
