using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Models;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminKickCommands
    {
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;

        public AdminKickCommands(LangHelper langHelper, LobbiesHandler lobbiesHandler)
            => (_langHelper, _lobbiesHandler) = (langHelper, lobbiesHandler);

        [TDSCommand(AdminCommand.Kick)]
        public void KickPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingTextAttribute(MinLength = 4)] string reason)
        {
            _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_INFO, target.DisplayName, player.DisplayName, reason));
            NAPI.Task.RunSafe(() =>
            {
                target.SendNotification(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));
                target.SendChatMessage(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));
            });

            var _ = new TDSTimer(() =>
            {
                NAPI.Task.RunSafe(() =>
                {
                    if (target is { } && !target.IsNull)
                        target?.Kick("Kick");
                });

            }, 2000);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.LobbyKick)]
        public async Task LobbyKick(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingTextAttribute(MinLength = 4)] string reason)
        {
            if (player == target)
                return;
            if (target.Lobby is null)
                return;
            if (!cmdinfos.AsLobbyOwner)
            {
                LoggingHandler.Instance.LogAdmin(LogType.Lobby_Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
                _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            else
            {
                if (player.Lobby != target.Lobby)
                {
                    NAPI.Task.RunSafe(() =>
                    {
                        player.SendChatMessage(player.Language.TARGET_NOT_IN_SAME_LOBBY);
                    });
                    return;
                }
                target.Lobby.Chat.Send(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            await target.Lobby.Players.RemovePlayer(target).ConfigureAwait(false);
            await _lobbiesHandler.MainMenu.Players.AddPlayer(target, 0).ConfigureAwait(false);
        }
    }
}
