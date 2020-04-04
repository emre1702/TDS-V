using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IEntity
    {
        string Name { get; }
    }
}
