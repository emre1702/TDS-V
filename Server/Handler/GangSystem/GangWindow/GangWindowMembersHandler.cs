using GTANetworkAPI;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models.GangWindow;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.GangSystem.GangWindow
{
    public class GangWindowMembersHandler
    {
        private readonly GangsHandler _gangsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly InvitationsHandler _invitationsHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly OfflineMessagesHandler _offlineMessagesHandler;
        private readonly LangHelper _langHelper;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ILoggingHandler _loggingHandler;

        public GangWindowMembersHandler(GangsHandler gangsHandler, LobbiesHandler lobbiesHandler, ITDSPlayerHandler tdsPlayerHandler,
            InvitationsHandler invitationsHandler, EventsHandler eventsHandler, OfflineMessagesHandler offlineMessagesHandler, LangHelper langHelper,
            DataSyncHandler dataSyncHandler, ILoggingHandler loggingHandler)
        {
            _gangsHandler = gangsHandler;
            _lobbiesHandler = lobbiesHandler;
            _tdsPlayerHandler = tdsPlayerHandler;
            _invitationsHandler = invitationsHandler;
            _eventsHandler = eventsHandler;
            _offlineMessagesHandler = offlineMessagesHandler;
            _langHelper = langHelper;
            _dataSyncHandler = dataSyncHandler;
            _loggingHandler = loggingHandler;
        }

        public string? GetMembers(ITDSPlayer player)
        {
            if (player.Entity is null)
                return null;
            if (player.Gang.Entity.Id <= 0)
                return null;

            var data = player.Gang.Entity.Members.Select(m => new SyncedGangMember(player, m));

            return Serializer.ToBrowser(data);
        }

        public async Task<object?> LeaveGang(ITDSPlayer player, bool sendInfo = true)
        {
            if (player.Gang.Entity.Id <= 0)
                return null;

            var gang = player.Gang;

            gang.Players.RemoveOnline(player);

            var memberInGangEntity = gang.Entity.Members.FirstOrDefault(m => m.PlayerId == player.Id);
            if (memberInGangEntity is null)
                return null;

            player.Gang = _gangsHandler.None;
            player.GangRank = _gangsHandler.NoneRank;
            _dataSyncHandler.SetData(player, PlayerDataKey.GangId, DataSyncMode.Player, player.Gang.Entity.Id);

            if (player.Lobby is IGangLobby || player.Lobby is IGangActionLobby)
                await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0);

            await RemoveMemberFromGang(gang, memberInGangEntity);

            NAPI.Task.RunSafe(() =>
            {
                _eventsHandler.OnGangLeave(player, gang);
                if (sendInfo)
                    player.SendNotification(player.Language.YOUVE_LEFT_THE_GANG);
            });

            return "";
        }

        public object? Invite(ITDSPlayer player, string name)
        {
            var target = _tdsPlayerHandler.FindTDSPlayer(name);
            if (target is null)
                return player.Language.TARGET_NOT_LOGGED_IN;

            if (target.IsInGang)
                return player.Language.TARGET_ALREADY_IN_A_GANG;

            var invitation = _invitationsHandler.FindInvitation(player, target, InvitationType.Gang);
            if (invitation is { })
                return player.Language.YOU_ALREADY_INVITED_TARGET;

            new Invitation(player.Language.GANG_INVITATION_INFO, target, player, _invitationsHandler, AcceptedInvitation, RejectedInvitation, InvitationType.Gang);
            return "";
        }

        public async Task<object?> Kick(ITDSPlayer player, GangMembers gangMember)
        {
            if (player.Entity is null)
                return "?";

            var target = _tdsPlayerHandler.Get(gangMember.PlayerId);
            if (target is { })
            {
                await LeaveGang(target, false);
                target.SendNotification(string.Format(target.Language.YOU_GOT_KICKED_OUT_OF_THE_GANG_BY, player.DisplayName, player.Gang.Entity.Name));
            }
            else
            {
                await RemoveMemberFromGang(player.Gang, gangMember);
                _offlineMessagesHandler.Add(gangMember.Player, player.Entity,
                    string.Format(_langHelper.GetLang(Language.English).YOU_GOT_KICKED_OUT_OF_THE_GANG_BY, player.DisplayName, player.Gang.Entity.Name));
            }

            player.SendNotification(string.Format(player.Language.YOU_KICKED_OUT_OF_GANG, gangMember.Player.Name));
            return "";
        }

        public async Task<object?> RankDown(ITDSPlayer player, GangMembers gangMember)
        {
            var oldRank = gangMember.RankNumber;
            if (oldRank is null || oldRank == 0)
                return "?";

            var msg = await ChangeRank(player, gangMember, (short)(oldRank - 1));
            if (msg is { })
                return msg;

            var target = _tdsPlayerHandler.Get(gangMember.PlayerId);
            if (target is { })
            {
                target.SendNotification(string.Format(target.Language.YOU_GOT_RANK_DOWN_BY, player.DisplayName, oldRank, oldRank - 1));
            }
            else
            {
                _offlineMessagesHandler.Add(gangMember.Player, player.Entity!,
                    string.Format(_langHelper.GetLang(Language.English).YOU_GOT_RANK_DOWN_BY, player.DisplayName, oldRank, oldRank - 1));
            }
            return "";
        }

        public async Task<object?> RankUp(ITDSPlayer player, GangMembers gangMember)
        {
            var oldRank = gangMember.RankNumber;
            if (oldRank is null || oldRank == player.Gang.Entity.Ranks.Max(r => r.Rank))
                return "?";

            var msg = await ChangeRank(player, gangMember, (short)(oldRank + 1));
            if (msg is { })
                return msg;

            var target = _tdsPlayerHandler.Get(gangMember.PlayerId);
            if (target is { })
            {
                target.SendNotification(string.Format(target.Language.YOU_GOT_RANK_UP, player.DisplayName, oldRank, oldRank + 1));
            }
            else
            {
                _offlineMessagesHandler.Add(gangMember.Player, player.Entity!,
                    string.Format(_langHelper.GetLang(Language.English).YOU_GOT_RANK_UP, player.DisplayName, oldRank, oldRank + 1));
            }
            return "";
        }

        private async Task<string?> ChangeRank(ITDSPlayer executer, GangMembers gangMember, short newRank)
        {
            var nextRank = executer.Gang.Entity.Ranks.FirstOrDefault(r => r.Rank == newRank);
            if (nextRank is null)
                return executer.Language.THE_RANK_IS_INVALID_REFRESH_WINDOW;

            gangMember.RankId = nextRank.Id;
            gangMember.RankNumber = nextRank.Rank;

            await executer.Gang.Database.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });
            return null;
        }

        private async void AcceptedInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation invitation)
        {
            try
            {
                if (sender is null || !sender.LoggedIn)
                    return;

                if (player.IsInGang)
                {
                    player.SendNotification(player.Language.YOU_ARE_ALREADY_IN_A_GANG);
                    return;
                }

                var gangRank = sender.Gang.Entity.Ranks.First(r => r.Rank == 0);

                await _eventsHandler.OnGangJoin(player, sender.Gang, gangRank);

                if (player.Lobby is IGangLobby)
                    await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0);

                player.SendNotification(string.Format(player.Language.YOU_JOINED_THE_GANG, player.Gang.Entity.Name));
                player.Gang.Chat.SendNotification(lang => string.Format(lang.PLAYER_JOINED_YOUR_GANG, player.DisplayName));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        private void RejectedInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation invitation)
        {
            if (sender?.LoggedIn == true)
            {
                sender.SendNotification(string.Format(sender.Language.TARGET_REJECTED_INVITATION, player.DisplayName));
                player.SendNotification(string.Format(player.Language.YOU_REJECTED_GANG_INVITATION, sender.Gang.Entity.Name));
            }
            else
            {
                player.SendNotification(player.Language.YOU_REJECTED_INVITATION);
            }
        }

        private async Task RemoveMemberFromGang(IGang gang, GangMembers member)
        {
            if (gang.Entity.Members.Count == 1)
            {
                await gang.Delete();
                return;
            }

            await gang.Database.ExecuteForDBAsync(async dbContext =>
            {
                gang.Entity.Members.Remove(member);

                if (gang.Entity.OwnerId == member.PlayerId)
                    gang.LeaderHandler.AppointNextSuitableLeader();

                await dbContext.SaveChangesAsync();
            });
        }
    }
}
