using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminMuteCommands
    {
        private readonly DatabasePlayerHelper _databasePlayerHelper;

        public AdminMuteCommands(DatabasePlayerHelper databasePlayerHelper)
            => _databasePlayerHelper = databasePlayerHelper;

        [TDSCommand(AdminCommand.Mute, 1)]
        public void MutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, int minutes, [RemainingText(MinLength = 4)] string reason)
        {
            if (!CheckIsMuteTimeValid(minutes, player))
                return;

            target.MuteHandler.ChangeMuteTime(target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Mute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Mute, 0)]
        public async Task MutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [RemainingText(MinLength = 4)] string reason)
        {
            if (!CheckIsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerMuteTime(player, dbTarget, minutes, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.Mute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.VoiceMute, 0)]
        public async Task VoiceMutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [RemainingText(MinLength = 4)] string reason)
        {
            if (!CheckIsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerVoiceMuteTime(player, dbTarget, minutes, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.VoiceMute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.VoiceMute, 1)]
        public void VoiceMutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, int minutes, [RemainingText(MinLength = 4)] string reason)
        {
            if (!CheckIsMuteTimeValid(minutes, player))
                return;

            target.MuteHandler.ChangeVoiceMuteTime(player, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                LoggingHandler.Instance.LogAdmin(LogType.VoiceMute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        private bool CheckIsMuteTimeValid(int muteTime, ITDSPlayer outputTo)
        {
            if (muteTime < -1)
            {
                NAPI.Task.RunSafe(() => outputTo.SendChatMessage(outputTo.Language.MUTETIME_INVALID));
                return false;
            }
            return true;
        }
    }
}
