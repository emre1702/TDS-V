namespace TDS_Server.Data.Interfaces.Entities.Gamemodes
{
    public interface IGangwar : IGamemode
    {
        ITeam AttackerTeam { get; }
        ITeam OwnerTeam { get; }
        object TargetObject { get; }
    }
}
