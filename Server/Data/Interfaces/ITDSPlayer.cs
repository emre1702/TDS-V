using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.Data.Interfaces
{
    public interface ITDSPlayer
    {
        public IPlayer ModPlayer { get; }
        public string DisplayName { get; }
    }
}
