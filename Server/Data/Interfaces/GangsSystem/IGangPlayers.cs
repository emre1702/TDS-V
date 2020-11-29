using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
    #nullable enable
    public interface IGangPlayers
    {
        int CountOnline { get; }

        void DoForAll(Action<ITDSPlayer> action);
        Task DoForAll(Func<ITDSPlayer, Task> action);
        void DoInMain(Action<ITDSPlayer> action);
        Task DoInMainWait(Action<ITDSPlayer> action);
        ITDSPlayer? GetOnline(int playerId);
        Task RemoveAll();
        void AddOnline(ITDSPlayer player);
        void RemoveOnline(ITDSPlayer player);
    }
}