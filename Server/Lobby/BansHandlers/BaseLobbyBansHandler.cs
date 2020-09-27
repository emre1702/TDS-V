using System;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.BansHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Helper;
using PlayerDb = TDS_Server.Database.Entity.Player.Players;

namespace TDS_Server.LobbySystem.BansHandlers
{
    public class BaseLobbyBansHandler : IBaseLobbyBansHandler
    {
        protected readonly IBaseLobby Lobby;
        protected readonly LangHelper LangHelper;

        public BaseLobbyBansHandler(IBaseLobby lobby, LangHelper langHelper)
            => (Lobby, LangHelper) = (lobby, langHelper);

        public virtual async ValueTask<bool> CheckIsBanned(ITDSPlayer player)
        {
            var ban = await Lobby.Database.GetBan(player.Entity?.Id);
            if (ban is null)
                return false;
            if (await CheckHasExpired(ban))
                return false;

            OutputBanInfos(player, ban);

            return false;
        }

        private async ValueTask<bool> CheckHasExpired(PlayerBans ban)
        {
            if (!ban.EndTimestamp.HasValue)
                return false;
            if (ban.EndTimestamp.Value > DateTime.UtcNow)
                return false;

            await Lobby.Database.Remove<PlayerBans>(ban);

            return true;
        }

        private void OutputBanInfos(ITDSPlayer player, PlayerBans ban)
        {
            string duration = "-";
            if (ban.EndTimestamp.HasValue)
            {
                duration = DateTime.UtcNow.DurationTo(ban.EndTimestamp.Value);
            }
            string msg = string.Format(player.Language.GOT_LOBBY_BAN, duration, ban.Reason);
            NAPI.Task.Run(() => player.SendChatMessage(msg));
        }

        public async Task<PlayerBans?> Ban(ITDSPlayer admin, PlayerDb target, TimeSpan? length, string reason, string? serial = null)
        {
            int targetId = target.Id;
            if (serial is null)
                serial = await Lobby.Database.GetLastUsedSerial(targetId);

            var ban = await Lobby.Database.GetBan(targetId);
            if (ban is { })
            {
                UpdateBanEntity(ban, admin.Entity?.Id, length, reason, serial);
                await Lobby.Database.Save();
            }
            else
            {
                ban = CreateBanEntity(admin.Entity?.Id, target.Id, length, reason, serial);
                await Lobby.Database.AddBanEntity(ban);
            }
            OutputNewBanInfo(ban, admin, target.Name);
            Lobby.Events.TriggerNewBan(ban, target.PlayerSettings?.DiscordUserId);

            return ban;
        }

        public async Task<PlayerBans?> Ban(ITDSPlayer admin, ITDSPlayer target, TimeSpan? length, string reason)
        {
            var ban = await Ban(admin, target.Entity!, length, reason, target.Serial);
            if (ban is null)
                return null;

            if (length.HasValue)
                OutputTempBanInfoToTarget(target, ban, admin);
            else
                OutputPermBanInfoToTarget(target, ban, admin);

            return ban;
        }

        protected virtual void OutputNewBanInfo(PlayerBans ban, ITDSPlayer admin, string targetName)
        {
            if (ban.EndTimestamp.HasValue)
                OutputNewTempBanInfo(ban, admin, targetName);
            else
                OutputNewPermBanInfo(ban, admin, targetName);
        }

        protected virtual void OutputNewTempBanInfo(PlayerBans ban, ITDSPlayer admin, string targetName)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            string msgProvider(ILanguage lang) => string.Format(lang.TIMEBAN_LOBBY_INFO, targetName, lengthHours, Lobby.Entity.Name, admin.DisplayName, ban.Reason);
            if (Lobby.Entity.IsOfficial)
                LangHelper.SendAllChatMessage(msgProvider);
            else
                Lobby.Chat.Send(msgProvider);
        }

        protected virtual void OutputTempBanInfoToTarget(ITDSPlayer target, PlayerBans ban, ITDSPlayer admin)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            NAPI.Task.Run(() =>
                target.SendChatMessage(string.Format(target.Language.TIMEBAN_LOBBY_YOU_INFO, lengthHours, Lobby.Entity.Name, admin.DisplayName, ban.Reason)));
        }

        protected virtual void OutputPermBanInfoToTarget(ITDSPlayer target, PlayerBans ban, ITDSPlayer admin)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            NAPI.Task.Run(() =>
                target.SendChatMessage(string.Format(target.Language.PERMABAN_LOBBY_YOU_INFO, Lobby.Entity.Name, admin.DisplayName, ban.Reason)));
        }

        protected virtual void OutputNewPermBanInfo(PlayerBans ban, ITDSPlayer admin, string targetName)
        {
            string msgProvider(ILanguage lang) => string.Format(lang.PERMABAN_LOBBY_INFO, targetName, Lobby.Entity.Name, admin.DisplayName, ban.Reason);
            if (Lobby.Entity.IsOfficial)
                LangHelper.SendAllChatMessage(msgProvider);
            else
                Lobby.Chat.Send(msgProvider);
        }

        private void UpdateBanEntity(PlayerBans ban, int? adminId, TimeSpan? length, string reason, string? serial)
        {
            ban.AdminId = adminId ?? -1;
            ban.Serial = serial;
            ban.StartTimestamp = DateTime.UtcNow;
            ban.EndTimestamp = length.HasValue ? DateTime.UtcNow + length : null;
            ban.Reason = reason;
        }

        private PlayerBans CreateBanEntity(int? adminId, int targetId, TimeSpan? length, string reason, string? serial)
        {
            return new PlayerBans()
            {
                PlayerId = targetId,
                LobbyId = Lobby.Entity.Id,
                Serial = serial,
                AdminId = adminId ?? -1,
                EndTimestamp = DateTime.UtcNow + length,
                Reason = reason
            };
        }

        public virtual async Task Unban(ITDSPlayer admin, ITDSPlayer target, string reason)
        {
            await Unban(admin, target.Entity!, reason);
            target.SendChatMessage(string.Format(target.Language.UNBAN_YOU_LOBBY_INFO, Lobby.Entity.Name, admin.DisplayName, reason));
        }

        public virtual async Task<PlayerBans?> Unban(ITDSPlayer admin, PlayerDb target, string reason)
        {
            var ban = await Lobby.Database.GetBan(target.Id);
            if (ban is null)
            {
                NAPI.Task.Run(() => admin.SendChatMessage(admin.Language.PLAYER_ISNT_BANED));
                return null;
            }
            await Lobby.Database.Remove<PlayerBans>(ban);
            OutputUnbanMessage(admin, target.Name, reason);

            return ban;
        }

        protected virtual void OutputUnbanMessage(ITDSPlayer admin, string targetName, string reason)
        {
            if (Lobby.Entity.IsOfficial)
                LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_LOBBY_INFO, targetName, Lobby.Entity.Name, admin.AdminLevelName, reason));
            else
                Lobby.Chat.Send(lang => string.Format(lang.UNBAN_LOBBY_INFO, targetName, Lobby.Entity.Name, admin.AdminLevelName, reason));
        }
    }
}
