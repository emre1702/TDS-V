using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.GangSystem.GangWindow;
using TDS.Shared.Default;

namespace TDS.Server.Handler.GangSystem
{
    public class GangWindowHandler
    {
        private readonly ILoggingHandler _loggingHandler;

        private readonly GangWindowMainMenuHandler _mainMenu;
        private readonly GangWindowCreateHandler _create;
        private readonly GangWindowMembersHandler _member;
        private readonly GangWindowRanksLevelsHandler _ranksLevels;
        private readonly GangWindowRanksPermissionsHandler _ranksPermissions;
        private readonly GangWindowSpecialPageHandler _specialPage;

        public GangWindowHandler(ILoggingHandler loggingHandler, IServiceProvider serviceProvider, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            _loggingHandler = loggingHandler;

            _mainMenu = ActivatorUtilities.CreateInstance<GangWindowMainMenuHandler>(serviceProvider);
            _create = ActivatorUtilities.CreateInstance<GangWindowCreateHandler>(serviceProvider);
            _member = ActivatorUtilities.CreateInstance<GangWindowMembersHandler>(serviceProvider);
            _ranksLevels = ActivatorUtilities.CreateInstance<GangWindowRanksLevelsHandler>(serviceProvider);
            _ranksPermissions = ActivatorUtilities.CreateInstance<GangWindowRanksPermissionsHandler>(serviceProvider);
            _specialPage = ActivatorUtilities.CreateInstance<GangWindowSpecialPageHandler>(serviceProvider);

            remoteBrowserEventsHandler.Add(ToServerEvent.GangCommand, ExecuteCommand);
            remoteBrowserEventsHandler.Add(ToServerEvent.LoadGangWindowData, OnLoadGangWindowData);
        }

        private object? OnLoadGangWindowData(RemoteBrowserEventArgs args)
        {
            try
            {
                var type = (GangWindowLoadDataType)(int)args.Args[0];

                string? json = null;

                switch (type)
                {
                    case GangWindowLoadDataType.MainMenu:
                        json = _mainMenu.GetMainData(args.Player);
                        break;

                    case GangWindowLoadDataType.AllGangs:
                        break;

                    case GangWindowLoadDataType.GangInfo:
                        break;

                    case GangWindowLoadDataType.Members:
                        json = _member.GetMembers(args.Player);
                        break;

                    case GangWindowLoadDataType.RanksLevels:
                        json = _ranksLevels.GetRanksJson(args.Player);
                        break;

                    case GangWindowLoadDataType.RanksPermissions:
                        json = _ranksPermissions.GetPermissions(args.Player, _ranksLevels);
                        break;

                    case GangWindowLoadDataType.Vehicles:
                        break;
                }

                if (json is { })
                    NAPI.Task.RunSafe(() =>
                        args.Player.TriggerBrowserEvent(ToBrowserEvent.LoadedGangWindowData, type, json));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex, args.Player);
            }

            return null;
        }

        private async Task<object?> ExecuteCommand(RemoteBrowserEventArgs args)
        {
            var player = args.Player;
            var type = (GangCommand)args.Args[0];

            var errorMsg = CheckRequirementsForCommand(player, type, ref args.Args, out GangMembers? target);
            if (errorMsg is { })
                return errorMsg;

            var ret = type switch
            {
                GangCommand.Create => await _create.CreateGang(player, args.Args[1].ToString()!).ConfigureAwait(false),
                GangCommand.Invite => _member.Invite(player, args.Args[1].ToString()!),
                GangCommand.Kick => await _member.Kick(player, target!).ConfigureAwait(false),
                GangCommand.Leave => await _member.LeaveGang(player).ConfigureAwait(false),
                GangCommand.ModifyPermissions => await _ranksPermissions.Modify(player, args.Args[1].ToString()!).ConfigureAwait(false),
                GangCommand.ModifyRanks => await _ranksLevels.Modify(player, args.Args[1].ToString()!).ConfigureAwait(false),
                GangCommand.RankDown => await _member.RankDown(player, target!).ConfigureAwait(false),
                GangCommand.RankUp => await _member.RankUp(player, target!).ConfigureAwait(false),
                GangCommand.OpenOnlyOneEditorPage => _specialPage.OpenOnlyOneEditorPage(player, (GangWindowOnlyOneEditorPage)(int)args.Args[1]),
                GangCommand.CloseOnlyOneEditorPage => _specialPage.CloseOnlyOneEditorPage(player, (GangWindowOnlyOneEditorPage)(int)args.Args[1]),
                _ => null
            };

            return ret ?? "";
        }

        private string? CheckRequirementsForCommand(ITDSPlayer player, GangCommand type, ref ArraySegment<object> args, out GangMembers? target)
        {
            target = null;

            // Is he in a gang? Or does he want to create one?
            if (type == GangCommand.Create && player.IsInGang || type != GangCommand.Create && !player.IsInGang)
                return player.IsInGang ? player.Language.YOU_ARE_ALREADY_IN_A_GANG : player.Language.YOU_ARE_NOT_IN_A_GANG;

            if (!player.Gang.PermissionsHandler.IsAllowedTo(player, type))
                return player.Language.NOT_ALLOWED;

            // Is target rank lower? Or is he the owner?
            switch (type)
            {
                case GangCommand.Kick:
                case GangCommand.RankDown:
                case GangCommand.RankUp:
                    var errorMsg = CheckRequirementsForCommandWithTarget(player, (int)args[1], out target);
                    if (errorMsg is { })
                        return errorMsg;
                    break;
            }

            return null;
        }

        private string? CheckRequirementsForCommandWithTarget(ITDSPlayer player, int targetId, out GangMembers? target)
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