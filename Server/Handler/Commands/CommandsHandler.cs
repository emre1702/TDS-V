using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Command;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Userpanel;
using TDS_Shared.Default;

using DB = TDS_Server.Database.Entity.Command;

namespace TDS_Server.Handler.Commands
{
    public class CommandsHandler
    {
        // private delegate void CommandDefaultMethod(TDSPlayer player, TDSCommandInfos
        // commandinfos, object[] args); private delegate void CommandEmptyDefaultMethod(TDSPlayer
        // player, TDSCommandInfos commandinfos);

        private class HandleArgumentsResult
        {
            public bool Worked;
            public bool IsWrongMethod;
        }

        private readonly Dictionary<string, CommandDataDto> _commandDataByCommand = new Dictionary<string, CommandDataDto>();  // this is the primary Dictionary for commands!
        private readonly Dictionary<string, DB.Commands> _commandsDict = new Dictionary<string, DB.Commands>();
        private readonly Dictionary<string, string> _commandByAlias = new Dictionary<string, string>();

        // private const int AmountDefaultParams = 2;

        // With implicit types it's much slower than direct call (Test: 8ms in 10000) - but then you
        // can use Methods with implicit types (e.g. AdminSay([defaultParams], string text, int
        // number)) Without implicit types it's much faster but you can only use Methods with
        // signature Method([defaultParams]) or Method([defaultParams], object[] args) private const
        // bool UseImplicitTypes = true;

        private readonly MappingHandler _mappingHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly ChatHandler _chatHandler;
        private readonly BaseCommands _baseCommands;
        private readonly IModAPI _modAPI;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public CommandsHandler(IModAPI modAPI, TDSDbContext dbContext, UserpanelCommandsHandler userpanelCommandsHandler, MappingHandler mappingHandler,
            ISettingsHandler settingsHandler, ChatHandler chatHandler, BaseCommands baseCommands, TDSPlayerHandler tdsPlayerHandler)
        {
            _modAPI = modAPI;
            _mappingHandler = mappingHandler;
            _settingsHandler = settingsHandler;
            _chatHandler = chatHandler;
            _baseCommands = baseCommands;
            _tdsPlayerHandler = tdsPlayerHandler;

            LoadCommands(dbContext, userpanelCommandsHandler);

            modAPI.ClientEvent.Add<IPlayer, string>(ToServerEvent.CommandUsed, this, UseCommand);
        }

        public void LoadCommands(TDSDbContext dbcontext, UserpanelCommandsHandler userpanelCommandsHandler)
        {
            foreach (DB.Commands command in dbcontext.Commands.Include(c => c.CommandAlias).Include(c => c.CommandInfos).ToList())
            {
                _commandsDict[command.Command.ToLower()] = command;

                foreach (CommandAlias alias in command.CommandAlias)
                {
                    _commandByAlias[alias.Alias.ToLower()] = command.Command.ToLower();
                }
            }

            List<MethodInfo> methods = _baseCommands
                    .GetType()
                    .GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(TDSCommand), false).Length > 0)
                    .ToList();
            foreach (MethodInfo method in methods)
            {
                var attribute = method.GetCustomAttribute<TDSCommand>();
                if (attribute is null)
                    continue;
                string cmd = attribute.Command.ToLower();
                if (!_commandsDict.ContainsKey(cmd))  // Only add the command if we got an entry in DB
                    continue;

                CommandDataDto commanddata;
                if (_commandDataByCommand.ContainsKey(cmd))
                    commanddata = _commandDataByCommand[cmd];
                else
                    commanddata = new CommandDataDto();
                CommandMethodDataDto methoddata = new CommandMethodDataDto
                (
                    priority: attribute.Priority,
                    methodDefault: method
                );
                commanddata.MethodDatas.Add(methoddata);

                var methodParams = method.GetParameters();
                if (methodParams.Length >= 2)
                {
                    if (methodParams[1].ParameterType == typeof(TDSCommandInfos))
                        methoddata.HasCommandInfos = true;
                }

                var parameters = method.GetParameters().Skip(methoddata.AmountDefaultParams).ToList();

                for (int i = 0; i < parameters.Count; ++i)
                {
                    var parameter = parameters[i];

                    #region Save parameters start index with default value

                    if (methoddata.ParametersWithDefaultValueStartIndex is null && parameter.HasDefaultValue)
                    {
                        methoddata.ParametersWithDefaultValueStartIndex = i;
                    }

                    #endregion Save parameters start index with default value

                    #region TDSRemainingText attribute

                    if (!methoddata.ToOneStringAfterParameterCount.HasValue)
                    {
                        var remainingTextAttribute = parameter.GetCustomAttribute(typeof(TDSRemainingText), false);
                        if (remainingTextAttribute != null)
                        {
                            methoddata.ToOneStringAfterParameterCount = parameter.Position;
                            methoddata.RemainingTextAttribute = (TDSRemainingText)remainingTextAttribute;
                            break;
                        }
                    }

                    #endregion TDSRemainingText attribute
                }
                methoddata.ParameterInfos = parameters;

                /*if (!UseImplicitTypes)
                {
#pragma warning disable
                    if (parametertypes.Length == 0)
                        methoddata.MethodEmpty = (CommandEmptyDefaultMethod)method.CreateDelegate(typeof(CommandEmptyDefaultMethod));
                    else
                        methoddata.Method = (CommandDefaultMethod)method.CreateDelegate(typeof(CommandDefaultMethod));
                }*/

                _commandDataByCommand[cmd] = commanddata;
            }

            foreach (var commanddata in _commandDataByCommand.Values)
            {
                commanddata.MethodDatas.Sort((a, b) => -1 * a.Priority.CompareTo(b.Priority));
            }

            userpanelCommandsHandler.LoadCommandData(_commandDataByCommand, _commandsDict);
        }

        public void UseCommand(IPlayer modPlayer, string msg) // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            UseCommand(player, msg);
        }

        public async void UseCommand(ITDSPlayer player, string msg) // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            try
            {
                List<object> args = GetArgs(msg, out string cmd);
                TDSCommandInfos cmdinfos = new TDSCommandInfos(command: cmd);
                if (_commandByAlias.ContainsKey(cmd))
                    cmd = _commandByAlias[cmd];

                if (!CheckCommandExists(player, cmd, args))
                    return;

                DB.Commands entity = _commandsDict[cmd];

                if (!CheckRights(player, entity, cmdinfos))
                    return;

                CommandDataDto commanddata = _commandDataByCommand[cmd];

                if (IsInvalidArgsCount(player, commanddata, args))
                    return;

                int amountmethods = commanddata.MethodDatas.Count;
                for (int methodindex = 0; methodindex < amountmethods; ++methodindex)
                {
                    var methoddata = commanddata.MethodDatas[methodindex];

                    args = HandleDefaultValues(methoddata, args);
                    args = HandleRemaingText(methoddata, args, out string remainingText);

                    #region Check if remaining text is correct (length)

                    if (methoddata.RemainingTextAttribute != null)
                    {
                        if (remainingText.Length < methoddata.RemainingTextAttribute.MinLength)
                        {
                            player.SendMessage(player.Language.TEXT_TOO_SHORT);
                            return;
                        }
                        if (remainingText.Length > methoddata.RemainingTextAttribute.MaxLength)
                        {
                            player.SendMessage(player.Language.TEXT_TOO_LONG);
                            return;
                        }
                    }

                    #endregion Check if remaining text is correct (length)

                    var handleArgumentsResult = await HandleArgumentsTypeConvertings(player, methoddata, methodindex, amountmethods, args);
                    if (handleArgumentsResult.IsWrongMethod)
                        continue;

                    if (!handleArgumentsResult.Worked)
                        return;

                    //if (UseImplicitTypes)
                    //{
                    var finalInvokeArgs = GetFinalInvokeArgs(methoddata, player, cmdinfos, args);
                    _modAPI.Thread.RunInMainThread(() =>
                        methoddata.MethodDefault.Invoke(_baseCommands, finalInvokeArgs.ToArray()));
                    /*}
                    else
                    {
#pragma warning disable
                        if (args != null)
                            methoddata.Method.Invoke(player, cmdinfos, args);
                        else
                            methoddata.MethodEmpty.Invoke(player, cmdinfos);
#pragma warning enable
                    }*/
                    break;
                }
            }
            catch
            {
                player.SendMessage(player.Language.COMMAND_USED_WRONG);
            }
        }

        private async Task<HandleArgumentsResult> HandleArgumentsTypeConvertings(ITDSPlayer player, CommandMethodDataDto methoddata, int methodindex,
            int amountmethodsavailable, List<object> args)
        {
            if (args.Count == 0)
                return new HandleArgumentsResult { Worked = true };

            try
            {
                for (int i = 0; i < Math.Min(args.Count, methoddata.ParameterInfos.Count); ++i)
                {
                    if (args[i] is null)
                        continue;

                    var parameterInfo = methoddata.ParameterInfos[i];
                    object? arg = await GetConvertedArg(args[i], parameterInfo.ParameterType);

                    #region Check for null

                    if (arg is null)
                    {
                        #region Check if player exists

                        if (parameterInfo.ParameterType == typeof(TDSPlayer)
                            || parameterInfo.ParameterType == typeof(ITDSPlayer)
                            || parameterInfo.ParameterType == typeof(Players))
                        {
                            // if it's the last method (there can be an alternative method with
                            // string etc. instead of TDSPlayer/Player)
                            if (methodindex + 1 == amountmethodsavailable)
                            {
                                player.SendMessage(player.Language.PLAYER_DOESNT_EXIST);
                                return new HandleArgumentsResult();
                            }
                            return new HandleArgumentsResult { IsWrongMethod = true };
                        }

                        #endregion Check if player exists
                    }

                    #endregion Check for null

                    object theArg = arg ?? string.Empty;
                    args[i] = theArg;  // arg shouldn't be able to be null
                }
                return new HandleArgumentsResult { Worked = true };
            }
            catch
            {
                if (methodindex + 1 == amountmethodsavailable)
                    return new HandleArgumentsResult();
                else
                    return new HandleArgumentsResult { IsWrongMethod = true };
            }
        }

        private List<object> HandleDefaultValues(CommandMethodDataDto methodData, List<object> args)
        {
            if (methodData.ParametersWithDefaultValueStartIndex.HasValue)
                for (int i = methodData.ParametersWithDefaultValueStartIndex.Value; i < methodData.ParameterInfos.Count; ++i)
                    if (args.Count < methodData.AmountDefaultParams + i)
                        args.Add(methodData.ParameterInfos[i].DefaultValue!);
            return args;
        }

        private List<object> HandleRemaingText(CommandMethodDataDto methodData, List<object> args, out string remainingText)
        {
            if (args.Count > 0 && methodData.ToOneStringAfterParameterCount.HasValue)
            {
                int index = methodData.ToOneStringAfterParameterCount.Value - methodData.AmountDefaultParams;
                remainingText = string.Join(' ', args.Skip(index)).Trim();
                args[index] = remainingText;
                return args.Take(index + 1).ToList();
            }
            remainingText = string.Empty;
            return args;
        }

        private bool IsInvalidArgsCount(ITDSPlayer player, CommandDataDto commanddata, List<object> args)
        {
            foreach (var methodData in commanddata.MethodDatas)
            {
                var requiredLength = methodData.ParametersWithDefaultValueStartIndex ?? methodData.ParameterInfos.Count;
                if (methodData.ToOneStringAfterParameterCount is { } && args.Count >= methodData.ToOneStringAfterParameterCount)
                    return false;

                if (args.Count == requiredLength || args.Count > requiredLength && args.Count <= methodData.ParameterInfos.Count)
                    return false;
            }

            player.SendMessage(player.Language.COMMAND_TOO_LESS_ARGUMENTS);
            return true;
        }

        private bool CheckCommandExists(ITDSPlayer player, string cmd, List<object>? args)
        {
            if (!_commandDataByCommand.ContainsKey(cmd))
            {
                if (_settingsHandler.ServerSettings.ErrorToPlayerOnNonExistentCommand)
                    player.SendMessage(player.Language.COMMAND_DOESNT_EXIST);
                if (_settingsHandler.ServerSettings.ToChatOnNonExistentCommand)
                    _chatHandler.SendLobbyMessage(player, "/" + cmd + (args != null ? " " + string.Join(' ', args) : ""), false);
                return false;
            }
            return true;
        }

        private bool CheckRights(ITDSPlayer player, DB.Commands entity, TDSCommandInfos cmdinfos)
        {
            bool canuse = false;
            bool needright = false;

            #region Lobby-Owner Check

            if (!canuse && entity.LobbyOwnerCanUse)
            {
                needright = true;
                canuse = player.IsLobbyOwner;
                if (canuse)
                    cmdinfos.WithRight = CommandUsageRight.LobbyOwner;
            }

            #endregion Lobby-Owner Check

            #region Admin Check

            if (!canuse && entity.NeededAdminLevel.HasValue)
            {
                needright = true;
                canuse = player.AdminLevel.Level >= entity.NeededAdminLevel.Value;
                if (canuse)
                    cmdinfos.WithRight = CommandUsageRight.Admin;
            }

            #endregion Admin Check

            #region Donator Check

            if (!canuse && entity.NeededDonation.HasValue)
            {
                needright = true;
                canuse = (player.Entity?.Donation ?? 0) >= entity.NeededDonation.Value;
                if (canuse)
                    cmdinfos.WithRight = CommandUsageRight.VIP;
            }

            #endregion Donator Check

            #region VIP Check

            if (!canuse && entity.VipCanUse)
            {
                needright = true;
                canuse = player.Entity?.IsVip ?? false;
                if (canuse)
                    cmdinfos.WithRight = CommandUsageRight.Donator;
            }

            #endregion VIP Check

            #region User Check

            if (!needright)
            {
                canuse = true;
                cmdinfos.WithRight = CommandUsageRight.User;
            }

            #endregion User Check

            if (!canuse)
                player.SendMessage(player.Language.NOT_ALLOWED);

            return canuse;
        }

        private List<object> GetArgs(string msg, out string cmd)
        {
            int cmdendindex = msg.IndexOf(' ');
            if (cmdendindex == -1)
            {
                cmd = msg.ToLower();
                return new List<object>();
            }
            cmd = msg.Substring(0, cmdendindex).ToLower();
            return msg.Substring(cmdendindex + 1).Split(' ').Cast<object>().ToList();
        }

        private List<object> GetFinalInvokeArgs(CommandMethodDataDto methodData, ITDSPlayer cmdUser, TDSCommandInfos cmdInfos, List<object> args)
        {
            args.Insert(0, cmdUser);
            if (methodData.HasCommandInfos)
                args.Insert(1, cmdInfos);
            return args;
        }

        private async Task<object?> GetConvertedArg(object notConvertedArg, Type theType)
        {
            theType = _mappingHandler.GetCorrectDestType(theType);
            object? converterReturn = _mappingHandler.Mapper.Map(notConvertedArg, typeof(string), theType);
            if (converterReturn != null && converterReturn is Task<Players?> task)
                return await task;
            return converterReturn;
        }
    }
}
