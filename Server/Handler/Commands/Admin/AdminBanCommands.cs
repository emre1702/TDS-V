using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminBanCommands
    {
        private readonly LobbiesHandler _lobbiesHandler;

        public AdminBanCommands(LobbiesHandler lobbiesHandler) 
            => (_lobbiesHandler) = (lobbiesHandler);

        [TDSCommand(AdminCommand.Ban, 1)]
        public async Task BanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, TimeSpan length, [TDSRemainingTextAttribute(MinLength = 4)] string reason)
        {
            PlayerBans? ban = null;
            if (length == TimeSpan.MinValue)
                await _lobbiesHandler.MainMenu.Bans.Unban(player, target, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                ban = await _lobbiesHandler.MainMenu.Bans.Ban(player, target, null, reason).ConfigureAwait(false);
            else
                ban = await _lobbiesHandler.MainMenu.Bans.Ban(player, target, length, reason).ConfigureAwait(false);

            if (ban is { })
                NAPI.Task.RunSafe(() => Utils.HandleBan(target, ban));

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Ban, 0)]
        public async Task BanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, TimeSpan length, [TDSRemainingTextAttribute(MinLength = 4)] string reason)
        {
            if (length == TimeSpan.MinValue)
                await _lobbiesHandler.MainMenu.Bans.Unban(player, dbTarget, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                await _lobbiesHandler.MainMenu.Bans.Ban(player, dbTarget, null, reason).ConfigureAwait(false);
            else
                await _lobbiesHandler.MainMenu.Bans.Ban(player, dbTarget, length, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.LobbyBan, 1)]
        public async Task LobbyBanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, TimeSpan length, [TDSRemainingTextAttribute(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby is IMainMenu)
                return;
            var lobby = player.Lobby;

            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            else if (lobby.Type == LobbyType.DamageTestLobby)
                lobby = _lobbiesHandler.DamageTestLobbyDummy;

            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == TimeSpan.MinValue)
                await lobby.Bans.Unban(player, target, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                await lobby.Bans.Ban(player, target, null, reason).ConfigureAwait(false);
            else
                await lobby.Bans.Ban(player, target, length, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Lobby_Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.LobbyBan, 0)]
        public async Task LobbyBanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, TimeSpan length, [TDSRemainingTextAttribute(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby.Type == LobbyType.MainMenu)
                return;
            var lobby = player.Lobby;

            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            else if (lobby.Type == LobbyType.DamageTestLobby)
                lobby = _lobbiesHandler.DamageTestLobbyDummy;

            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == TimeSpan.MinValue)
                await lobby.Bans.Unban(player, dbTarget, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                await lobby.Bans.Ban(player, dbTarget, null, reason).ConfigureAwait(false);
            else
                await lobby.Bans.Ban(player, dbTarget, length, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Lobby_Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }
    }
}
