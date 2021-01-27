using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Shared.Default;

namespace TDS.Server.Handler
{
    public class ChatHandler
    {
        private readonly IAdminsHandler _adminsHandler;
        private readonly LangHelper _langHelper;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public ChatHandler(ILoggingHandler loggingHandler, ITDSPlayerHandler tdsPlayerHandler, IAdminsHandler adminsHandler, LangHelper langHelper)
        {
            (_loggingHandler, _tdsPlayerHandler, _adminsHandler, _langHelper) = (loggingHandler, tdsPlayerHandler, adminsHandler, langHelper);

            NAPI.ClientEvent.Register<ITDSPlayer, string, int>(ToServerEvent.LobbyChatMessage, this, SendLobbyMessage);
        }

        public void OutputMuteInfo(string adminName, string targetName, float minutes, string reason)
        {
            switch (minutes)
            {
                case -1:
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.PERMAMUTE_INFO, targetName, adminName, reason));
                    break;

                case 0:
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.UNMUTE_INFO, targetName, adminName, reason));
                    break;

                default:
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.TIMEMUTE_INFO, targetName, adminName, minutes, reason));
                    break;
            }
        }

        public void OutputVoiceMuteInfo(string adminName, string targetName, float minutes, string reason)
        {
            switch (minutes)
            {
                case -1:
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.PERMAVOICEMUTE_INFO, targetName, adminName, reason));
                    break;

                case 0:
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.UNVOICEMUTE_INFO, targetName, adminName, reason));
                    break;

                default:
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.TIMEVOICEMUTE_INFO, targetName, adminName, minutes, reason));
                    break;
            }
        }

        public void SendAdminChat(ITDSPlayer player, string message)
        {
            var changedMessage = "[ADMINCHAT] " + player.Admin.Level.FontColor + player.DisplayName + ": !$220|220|220$" + message;
            NAPI.Task.RunSafe(() => _adminsHandler.SendMessage(changedMessage));
            _loggingHandler.LogChat(message, player, isGlobal: true, isAdminChat: true);
        }

        public void SendAdminMessage(ITDSPlayer player, string message)
        {
            var changedMessage = player.Admin.Level.FontColor + "[" + player.Admin.LevelName + "] !$255|255|255$" + player.DisplayName + ": !$220|220|220$" + message;
            NAPI.Task.RunSafe(() => NAPI.Chat.SendChatMessageToAll(changedMessage));
            _loggingHandler.LogChat(message, player, isGlobal: true, isAdminChat: true);
        }

        public void SendGlobalMessage(ITDSPlayer player, string message)
        {
            var changedMessage = "[GLOBAL] " + (player.Team?.Chat.Color ?? string.Empty) + player.DisplayName + "!$220|220|220$: " + message + "$Global$";
            NAPI.Task.RunSafe(() =>
            {
                foreach (var target in _tdsPlayerHandler.LoggedInPlayers)
                {
                    if (target.Relations.HasRelationTo(player, TDS.Shared.Data.Enums.PlayerRelation.Block))
                        continue;
                    target.SendChatMessage(changedMessage);
                }
            });

            _loggingHandler.LogChat(message, player, isGlobal: true);
        }

        public void SendLobbyMessage(ITDSPlayer player, string message, int chatTypeNumber)
        {
            try
            {
                if (player.MuteHandler.IsPermamuted)
                {
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.STILL_PERMAMUTED));
                    return;
                }
                if (player.MuteHandler.IsMuted)
                {
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.STILL_MUTED.Replace("{0}", player.MuteHandler.MuteTime?.ToString() ?? "?")));
                    return;
                }

                var chatType = (ChatType)chatTypeNumber;
                switch (chatType)
                {
                    case ChatType.Normal:
                        SendLobbyMessage(player, message, false);
                        break;

                    case ChatType.Dirty:
                        SendLobbyMessage(player, message, true);
                        break;

                    case ChatType.Team:
                        SendTeamChat(player, message);
                        break;

                    case ChatType.Global:
                        SendGlobalMessage(player, message);
                        break;
                }
            }
            catch (Exception ex) { LoggingHandler.Instance?.LogError(ex); }
        }

        public void SendLobbyMessage(ITDSPlayer player, string message, bool isDirty)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null)
                return;

            //if (!character.MuteTime.HasValue)
            var changedMessage = (player.Team?.Chat.Color ?? string.Empty) + player.DisplayName + "!$220|220|220$: " + message;
            if (isDirty)
                changedMessage = "!$160|50|0$[DIRTY] " + changedMessage + "$Dirty$";
            player.Lobby.Chat.Send(changedMessage);

            if (player.Lobby?.IsOfficial == true && !isDirty)
                _loggingHandler.LogChat(message, player);
            //else if (character.IsPermamuted)
            //    player.SendNotification(character.Language.STILL_PERMAMUTED);
            //else
            //    player.SendNotification(string.Format(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public void SendPrivateMessage(ITDSPlayer player, ITDSPlayer target, string message)
        {
            var changedMessage = "[PM] !$253|132|85$" + player.DisplayName + ": !$220|220|220$" + message;
            NAPI.Task.RunSafe(() =>
                target.SendChatMessage(changedMessage));
            _loggingHandler.LogChat(message, player, target: target);
        }

        public void SendTeamChat(ITDSPlayer player, string message)
        {
            if (player.Team is null)
                return;
            var changedMessage = "[TEAM] " + player.Team.Chat.Color + player.DisplayName + ": !$220|220|220$" + message + "$Team$";
            player.Team.Chat.Send(changedMessage);
            _loggingHandler.LogChat(message, player, isTeamChat: true);
        }
    }
}