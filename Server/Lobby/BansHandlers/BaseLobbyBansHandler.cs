using System;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using PlayerDb = TDS_Server.Database.Entity.Player.Players;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.BansHandlers
{
    public class BaseLobbyBansHandler
    {
        private readonly BaseLobbyDatabase _database;
        private readonly IBaseLobbyEventsHandler _events;
        protected readonly LobbyDb Entity;
        private readonly BaseLobbyChat _chat;
        protected readonly LangHelper LangHelper;

        public BaseLobbyBansHandler(BaseLobbyDatabase database, IBaseLobbyEventsHandler events, LangHelper langHelper, BaseLobbyChat chat, LobbyDb entity)
            => (_database, _events, LangHelper, _chat, Entity) = (database, events, langHelper, chat, entity);

        public virtual async ValueTask<bool> CheckIsBanned(ITDSPlayer player)
        {
            var ban = await _database.GetBan(player.Entity?.Id);
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

            await _database.Remove<PlayerBans>(ban);

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
                serial = await _database.GetLastUsedSerial(targetId);

            var ban = await _database.GetBan(targetId);
            if (ban is { })
            {
                UpdateBanEntity(ban, admin.Entity?.Id, length, reason, serial);
                await _database.Save();
            }
            else
            {
                ban = CreateBanEntity(admin.Entity?.Id, target.Id, length, reason, serial);
                await _database.AddBanEntity(ban);
            }
            OutputNewBanInfo(ban, admin, target.Name);
            _events.TriggerNewBan(ban, target.PlayerSettings?.DiscordUserId);

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
            string msgProvider(ILanguage lang) => string.Format(lang.TIMEBAN_LOBBY_INFO, targetName, lengthHours, Entity.Name, admin.DisplayName, ban.Reason);
            if (Entity.IsOfficial)
                LangHelper.SendAllChatMessage(msgProvider);
            else
                _chat.Send(msgProvider);
        }

        protected virtual void OutputTempBanInfoToTarget(ITDSPlayer target, PlayerBans ban, ITDSPlayer admin)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            NAPI.Task.Run(() =>
                target.SendChatMessage(string.Format(target.Language.TIMEBAN_LOBBY_YOU_INFO, lengthHours, Entity.Name, admin.DisplayName, ban.Reason)));
        }

        protected virtual void OutputPermBanInfoToTarget(ITDSPlayer target, PlayerBans ban, ITDSPlayer admin)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            NAPI.Task.Run(() =>
                target.SendChatMessage(string.Format(target.Language.PERMABAN_LOBBY_YOU_INFO, Entity.Name, admin.DisplayName, ban.Reason)));
        }

        protected virtual void OutputNewPermBanInfo(PlayerBans ban, ITDSPlayer admin, string targetName)
        {
            string msgProvider(ILanguage lang) => string.Format(lang.PERMABAN_LOBBY_INFO, targetName, Entity.Name, admin.DisplayName, ban.Reason);
            if (Entity.IsOfficial)
                LangHelper.SendAllChatMessage(msgProvider);
            else
                _chat.Send(msgProvider);
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
                LobbyId = Entity.Id,
                Serial = serial,
                AdminId = adminId ?? -1,
                EndTimestamp = DateTime.UtcNow + length,
                Reason = reason
            };
        }

        public virtual async Task Unban(ITDSPlayer admin, ITDSPlayer target, string reason)
        {
            await Unban(admin, target.Entity!, reason);
            target.SendChatMessage(string.Format(target.Language.UNBAN_YOU_LOBBY_INFO, Entity.Name, admin.DisplayName, reason));
        }

        public virtual async Task<PlayerBans?> Unban(ITDSPlayer admin, PlayerDb target, string reason)
        {
            var ban = await _database.GetBan(target.Id);
            if (ban is null)
            {
                NAPI.Task.Run(() => admin.SendChatMessage(admin.Language.PLAYER_ISNT_BANED));
                return null;
            }
            await _database.Remove<PlayerBans>(ban);
            OutputUnbanMessage(admin, target.Name, reason);

            return ban;
        }

        protected virtual void OutputUnbanMessage(ITDSPlayer admin, string targetName, string reason)
        {
            if (Entity.IsOfficial)
                LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_LOBBY_INFO, targetName, Entity.Name, admin.AdminLevelName, reason));
            else
                _chat.Send(lang => string.Format(lang.UNBAN_LOBBY_INFO, targetName, Entity.Name, admin.AdminLevelName, reason));
        }
    }
}
