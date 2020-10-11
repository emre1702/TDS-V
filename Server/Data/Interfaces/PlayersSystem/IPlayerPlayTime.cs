using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerPlayTime
    {
        int Hours { get; }
        int Minutes { get; set; }

        void Init(ITDSPlayer player);
    }
}
