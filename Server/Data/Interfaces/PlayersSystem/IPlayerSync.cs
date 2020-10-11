using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerSync
    {
        void Init(ITDSPlayer player, IPlayerEvents events);

        void TriggerBrowserEvent(params object[] eventNameAndArgs);
    }
}
