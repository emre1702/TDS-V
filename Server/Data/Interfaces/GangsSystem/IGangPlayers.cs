using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.GangsSystem
{
    #nullable enable
    public interface IGangPlayers
    {
        void Do(Action<ITDSPlayer> action);
        Task Do(Func<ITDSPlayer, Task> action);
        void DoInMain(Action<ITDSPlayer> action);
        Task DoInMainWait(Action<ITDSPlayer> action);
        ITDSPlayer? GetOnline(int playerId);
        Task RemoveAll();
        void AddOnline(ITDSPlayer player);
        void RemoveOnline(ITDSPlayer player);
    }
}