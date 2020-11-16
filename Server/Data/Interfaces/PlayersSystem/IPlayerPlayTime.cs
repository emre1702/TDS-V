using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerPlayTime
    {
        int Hours { get; }
        int Minutes { get; set; }

        void Init(ITDSPlayer player);
    }
}
