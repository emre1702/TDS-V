using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerSync
    {
        void Init(ITDSPlayer player, IPlayerEvents events);

        void TriggerBrowserEvent(params object[] eventNameAndArgs);
    }
}
