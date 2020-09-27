using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Data.Interfaces.LobbySystem.BansHandlers
{
    public interface IBaseLobbyBansHandler
    {
        Task<PlayerBans?> Ban(ITDSPlayer admin, ITDSPlayer target, TimeSpan? length, string reason);
        Task<PlayerBans?> Ban(ITDSPlayer admin, TDS_Server.Database.Entity.Player.Players target, TimeSpan? length, string reason, string? serial = null);
        ValueTask<bool> CheckIsBanned(ITDSPlayer player);
        Task Unban(ITDSPlayer admin, ITDSPlayer target, string reason);
        Task<PlayerBans?> Unban(ITDSPlayer admin, TDS_Server.Database.Entity.Player.Players target, string reason);
    }
}