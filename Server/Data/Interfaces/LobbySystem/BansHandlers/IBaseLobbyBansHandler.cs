using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Interfaces.LobbySystem.BansHandlers
{
#nullable enable

    public interface IBaseLobbyBansHandler
    {
        Task<PlayerBans?> Ban(ITDSPlayer admin, ITDSPlayer target, TimeSpan? length, string reason);

        Task<PlayerBans?> Ban(ITDSPlayer admin, TDS.Server.Database.Entity.Player.Players target, TimeSpan? length, string reason, string? serial = null);

        ValueTask<bool> CheckIsBanned(ITDSPlayer player);

        Task Unban(ITDSPlayer admin, ITDSPlayer target, string reason);

        Task<PlayerBans?> Unban(ITDSPlayer admin, TDS.Server.Database.Entity.Player.Players target, string reason);
    }
}
