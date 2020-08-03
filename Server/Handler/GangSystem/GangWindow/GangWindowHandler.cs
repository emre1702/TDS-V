using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem.GangWindow;
using TDS_Server.Handler.Player;
using TDS_Shared.Core;

namespace TDS_Server.Handler.GangSystem
{
    public class GangWindowHandler
    {
        private readonly IModAPI _modAPI;
        private readonly ILoggingHandler _loggingHandler;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        private readonly GangWindowMainMenuHandler _mainMenu;
        private readonly GangWindowCreateHandler _create;
        private readonly GangWindowMembersHandler _member;
        private readonly GangWindowRanksLevelsHandler _ranksLevels;
        private readonly GangWindowRanksPermissionsHandler _ranksPermissions;
        private readonly GangWindowSpecialPageHandler _specialPage;


        public GangWindowHandler(IModAPI modAPI, ILoggingHandler loggingHandler, TDSPlayerHandler tdsPlayerHandler, TDSDbContext dbContext,
            Serializer serializer, IServiceProvider serviceProvider)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            _mainMenu = ActivatorUtilities.CreateInstance<GangWindowMainMenuHandler>(serviceProvider);
            _create = ActivatorUtilities.CreateInstance<GangWindowCreateHandler>(serviceProvider);
            _member = ActivatorUtilities.CreateInstance<GangWindowMembersHandler>(serviceProvider);
            _ranksLevels = ActivatorUtilities.CreateInstance<GangWindowRanksLevelsHandler>(serviceProvider);
            _ranksPermissions = ActivatorUtilities.CreateInstance<GangWindowRanksPermissionsHandler>(serviceProvider);
            _specialPage = ActivatorUtilities.CreateInstance<GangWindowSpecialPageHandler>(serviceProvider);
        }

        public object? OnLoadGangWindowData(ITDSPlayer player, ref ArraySegment<object> args)
        {
            try
            {
                var type = (GangWindowLoadDataType)(int)args[0];

                string? json = null;

                switch (type)
                {
                    case GangWindowLoadDataType.MainMenu:
                        json = _mainMenu.GetMainData(player);
                        break;
                    case GangWindowLoadDataType.AllGangs:
                        break;
                    case GangWindowLoadDataType.GangInfo:
                        break;
                    case GangWindowLoadDataType.Members:
                        json = _member.GetMembers(player);
                        break;
                    case GangWindowLoadDataType.RanksLevels:
                        json = _ranksLevels.GetRanks(player);
                        break;
                    case GangWindowLoadDataType.RanksPermissions:
                        json = _ranksPermissions.GetPermissions(player);
                        break;
                    case GangWindowLoadDataType.Vehicles:
                        break;
                }

                if (json is { })
                    player.SendBrowserEvent(ToBrowserEvent.LoadedGangWindowData, type, json);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex, player);
            }

            return null;
        }

        public async Task<object?> ExecuteCommand(ITDSPlayer player, ArraySegment<object> args)
        {
            var type = (GangCommand)args[0];

            var errorMsg = CheckRequirementsForCommand(player, type, args, out GangMembers? target);
            if (errorMsg is { })
                return errorMsg;

            var ret = type switch
            {
                GangCommand.Create => await _create.CreateGang(player, args[1].ToString()!),
                GangCommand.Invite => _member.Invite(player, args[1].ToString()!),
                GangCommand.Kick => await _member.Kick(player, target!),
                GangCommand.Leave => await _member.LeaveGang(player),
                GangCommand.ModifyPermissions => _ranksPermissions.Modify(player, args[1].ToString()!),
                GangCommand.ModifyRanks => await _ranksLevels.Modify(player, args[1].ToString()!),
                GangCommand.RankDown => await _member.RankDown(player, target!),
                GangCommand.RankUp => await _member.RankUp(player, target!),
                GangCommand.OpenOnlyOneEditorPage => _specialPage.OpenOnlyOneEditorPage(player, (GangWindowOnlyOneEditorPage)(int)args[1]),
                GangCommand.CloseOnlyOneEditorPage => _specialPage.CloseOnlyOneEditorPage(player, (GangWindowOnlyOneEditorPage)(int)args[1]),
                _ => null
            };

            return ret ?? "";
        }

        private string? CheckRequirementsForCommand(ITDSPlayer player, GangCommand type, ArraySegment<object> args, out GangMembers? target)
        {
            target = null;

            // Is he in a gang? Or does he want to create one?
            if (type == GangCommand.Create && player.IsInGang || !player.IsInGang)
                return player.IsInGang ? player.Language.YOU_ARE_ALREADY_IN_A_GANG : player.Language.YOU_ARE_NOT_IN_A_GANG;

            if (!player.Gang.IsAllowedTo(player, type))
                return player.Language.NOT_ALLOWED;

            // Is target rank lower? Or is he the owner?
            switch (type)
            {
                case GangCommand.Kick:
                case GangCommand.RankDown:
                case GangCommand.RankUp:
                    var errorMsg = CheckRequirementsForCommandWithTarget(player, type, (int)args[1], out target);
                    if (errorMsg is { })
                        return errorMsg;
                    break;
            }

            return null;
        }

        private string? CheckRequirementsForCommandWithTarget(ITDSPlayer player, GangCommand type, int targetId, out GangMembers? target)
        {
            target = player.Gang.Entity.Members.FirstOrDefault(m => m.PlayerId == targetId);
            if (target is null)
                return player.Language.PLAYER_NOT_IN_YOUR_GANG;

            if (!player.IsGangOwner && target.Rank.Rank >= (player.GangRank?.Rank ?? 0))
                return player.Language.TARGET_RANK_IS_HIGHER_OR_EQUAL;


            return null;
        }
    }
}
