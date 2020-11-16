using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Players
{
#nullable enable

    public interface IBaseLobbyPlayers
    {
        int Count { get; }

        Task<bool> AddPlayer(ITDSPlayer player, int teamIndex = 0);

        Task<bool> Any();

        Task<bool> Any(Func<ITDSPlayer, bool> func);

        Task DoForAll(Action<ITDSPlayer> func);

        Task DoInMain(Action<ITDSPlayer> func);

        void DoWithoutLock(Action<ITDSPlayer> func);

        Task<ITDSPlayer?> GetById(int playerId);

        Task<IEnumerable<ITDSPlayer>> GetExcept(ITDSPlayer player);

        Task<ITDSPlayer?> GetFirst(ITDSPlayer exceptPlayer);

        Task<ITDSPlayer?> GetLobbyOwner();

        List<ITDSPlayer> GetPlayers();

        bool IsLobbyOwner(ITDSPlayer player);

        Task OnPlayerLoggedOut(ITDSPlayer player);

        Task<bool> RemovePlayer(ITDSPlayer player);

        Task<IOrderedEnumerable<string>> GetOrderedNames();
    }
}
