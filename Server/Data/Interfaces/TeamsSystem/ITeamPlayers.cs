using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.TeamsSystem
{
#nullable enable

    public interface ITeamPlayers
    {
        int Amount { get; }
        int AmountAlive { get; }
        int AmountSpectatable { get; }
        bool HasAnySpectatable { get; }
        bool HasAnyAlive { get; }

        void Init(ITeam team);

        void Add(ITDSPlayer player);

        void DoForAll(Action<ITDSPlayer> action);

        void DoInMain(Action<ITDSPlayer> action);

        void DoList(Action<List<ITDSPlayer>> action);
        Task<T> DoListInMain<T>(Func<List<ITDSPlayer>, T> action);

        ITDSPlayer? GetNearestPlayer(Vector3 position);

        ITDSPlayer[] GetAllArray();

        ITDSPlayer[] GetAllArrayExcept(ITDSPlayer player);

        IEnumerable<ushort> GetRemoteIds();

        bool Remove(ITDSPlayer player);

        bool RemoveAlive(ITDSPlayer player);

        bool RemoveSpectatable(ITDSPlayer player);

        void AddToSpectatable(ITDSPlayer player);

        void ClearAlive();

        void ClearRound();

        void ClearLists();

        ITDSPlayer GetRandom();

        void AddAlive(ITDSPlayer player);

        int GetSpectatableIndex(ITDSPlayer player);

        ITDSPlayer? GetSpectatableAtIndex(int index);

        ITDSPlayer? Last();

        int GetAlivesHealth(int armorPerLife, int hpPerLife);
    }
}
