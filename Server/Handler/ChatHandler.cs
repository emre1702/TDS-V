using AltV.Net;
using System;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Interfaces.Handlers;
using TDS_Server.Handler.Helper;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class ChatHandler
    {
        #region Private Fields

        private readonly AdminsHandler _adminsHandler;
        private readonly LangHelper _langHelper;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public ChatHandler(ILoggingHandler loggingHandler, ITDSPlayerHandler tdsPlayerHandler, AdminsHandler adminsHandler, LangHelper langHelper)
        {
            (_loggingHandler, _tdsPlayerHandler, _adminsHandler, _langHelper) = (loggingHandler, tdsPlayerHandler, adminsHandler, langHelper);

            Alt.OnClient<ITDSPlayer, string, int>(ToServerEvent.LobbyChatMessage, SendLobbyMessage);
        }

        #endregion Public Constructors

        #region Public Methods

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
            string changedMessage = "[ADMINCHAT] " + player.AdminLevel.FontColor + player.DisplayName + ": !$220|220|220$" + message;
            _adminsHandler.SendMessage(changedMessage);
            _loggingHandler.LogChat(message, player, isGlobal: true, isAdminChat: true);
        }

        public void SendAdminMessage(ITDSPlayer player, string message)
        {
            string changedMessage = player.AdminLevel.FontColor + "[" + player.AdminLevelName + "] !$255|255|255$" + player.DisplayName + ": !$220|220|220$" + message;
            SendMessage(changedMessage);
            _loggingHandler.LogChat(message, player, isGlobal: true, isAdminChat: true);
        }

        public void SendGlobalMessage(ITDSPlayer player, string message)
        {
            string changedmessage = "[GLOBAL] " + (player.Team?.ChatColor ?? string.Empty) + player.DisplayName + "!$220|220|220$: " + message + "$Global$";
            foreach (var target in _tdsPlayerHandler.LoggedInPlayers)
            {
                if (target.HasRelationTo(player, TDS_Shared.Data.Enums.PlayerRelation.Block))
                    continue;
                SendMessage(target, changedmessage);
            }
            _loggingHandler.LogChat(message, player, isGlobal: true);
        }

        public void SendLobbyMessage(ITDSPlayer player, string message, int chatTypeNumber)
        {
            if (!player.LoggedIn)
                return;

            if (player.IsPermamuted)
            {
                player.SendNotification(player.Language.STILL_PERMAMUTED);
                return;
            }
            if (player.IsMuted)
            {
                player.SendNotification(player.Language.STILL_MUTED.Replace("{0}", player.MuteTime?.ToString() ?? "?"));
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

        public void SendLobbyMessage(ITDSPlayer player, string message, bool isDirty)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null)
                return;

            //if (!character.MuteTime.HasValue)
            string changedmessage = (player.Team?.ChatColor ?? string.Empty) + player.DisplayName + "!$220|220|220$: " + message;
            if (isDirty)
                changedmessage = "!$160|50|0$[DIRTY] " + changedmessage + "$Dirty$";
            SendMessage(player.Lobby, changedmessage);

            if (player.Lobby?.IsOfficial == true && !isDirty)
                _loggingHandler.LogChat(message, player);
            //else if (character.IsPermamuted)
            //    player.SendNotification(character.Language.STILL_PERMAMUTED);
            //else
            //    player.SendNotification(string.Format(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public void SendPrivateMessage(ITDSPlayer player, ITDSPlayer target, string message)
        {
            string changedMessage = "[PM] !$253|132|85$" + player.DisplayName + ": !$220|220|220$" + message;
            SendMessage(target, changedMessage);
            _loggingHandler.LogChat(message, player, target: target);
        }

        public void SendTeamChat(ITDSPlayer player, string message)
        {
            if (player.Team is null)
                return;
            string changedMessage = "[TEAM] " + player.Team.ChatColor + player.DisplayName + ": !$220|220|220$" + message + "$Team$";
            SendMessage(player.Team, changedMessage);
            _loggingHandler.LogChat(message, player, isTeamChat: true);
        }

        public void SendMessage(string message)
        {
            Alt.EmitAllClients(ToClientEvent.ToBrowserEvent, ToBrowserEvent.ChatOutput, message);
        }

        public void SendMessage(ITDSPlayer player, string message)
        {
            player.Emit(ToClientEvent.ToBrowserEvent, ToBrowserEvent.ChatOutput, message);
        }

        public void SendMessage(ILobby lobby, string message)
        {
            lobby.FuncIterateAllPlayers((player, team) =>
            {
                player.Emit(ToClientEvent.ToBrowserEvent, ToBrowserEvent.ChatOutput, message);
            });
        }

        public void SendMessage(ITeam team, string message)
        {
            foreach (var player in team.Players)
            {
                player.Emit(ToClientEvent.ToBrowserEvent, ToBrowserEvent.ChatOutput, message);
            }
        }

        #endregion Public Methods
    }
}
