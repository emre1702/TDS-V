using TDS.Server.Data.Interfaces.GangsSystem;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerGangHandler
    {
        IGang Gang { get; set; }
    }
}
