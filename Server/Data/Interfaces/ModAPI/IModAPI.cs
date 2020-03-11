using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        IPlayerAPI Player { get; }
    }
}
