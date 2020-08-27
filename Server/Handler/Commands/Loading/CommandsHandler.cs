using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Command;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Userpanel;
using TDS_Shared.Data.Attributes;
using TDS_Shared.Default;

using DB = TDS_Server.Database.Entity.Command;

namespace TDS_Server.Handler.Commands.Loading
{
    public partial class CommandsHandler
    {

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
        private readonly ArgsHandler _argsHandler;

        public CommandsHandler(TDSDbContext dbContext, UserpanelCommandsHandler userpanelCommandsHandler, MappingHandler mappingHandler,
            ISettingsHandler settingsHandler, ChatHandler chatHandler, BaseCommands baseCommands)
        {
            _mappingHandler = mappingHandler;
            _settingsHandler = settingsHandler;
            _chatHandler = chatHandler;
            _baseCommands = baseCommands;
            _argsHandler = new ArgsHandler(mappingHandler);

            LoadCommands(dbContext, userpanelCommandsHandler);

            AltAsync.OnClient<ITDSPlayer, string, Task>(ToServerEvent.CommandUsed, UseCommand);
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
                    methodDefault: method,
                    isAsync: IsAsyncMethod(method)
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

                    var argLength = parameter.ParameterType.GetCustomAttribute<TDSCommandArgLength>();
                    if (argLength is { })
                    {
                        methoddata.MultipleArgsToOneInfos.Add(new CommandMultipleArgsToOneInfo { Index = i, Length = argLength.ArgLength });
                    }

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

        public async Task UseCommand(ITDSPlayer player, string msg) // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            try
            {
                msg = msg.Trim();
                List<object> origArgs = GetArgs(msg, out string cmd);
                TDSCommandInfos cmdinfos = new TDSCommandInfos(command: cmd);
                if (_commandByAlias.ContainsKey(cmd))
                    cmd = _commandByAlias[cmd];

                if (!CheckCommandExists(player, cmd, origArgs))
                    return;

                DB.Commands entity = _commandsDict[cmd];

                if (!CheckRights(player, entity, cmdinfos))
                    return;

                CommandDataDto commanddata = _commandDataByCommand[cmd];

                if (_argsHandler.IsInvalidArgsCount(player, commanddata, origArgs))
                    return;

                int amountmethods = commanddata.MethodDatas.Count;
                for (int methodindex = 0; methodindex < amountmethods; ++methodindex)
                {
                    var methoddata = commanddata.MethodDatas[methodindex];
                    if (_argsHandler.IsInvalidArgsCount(methoddata, origArgs))
                        continue;

                    var args = new List<object>(origArgs);
                    args = _argsHandler.HandleMultipleArgsToOneArg(methoddata, args);
                    if (args is null)
                        continue;

                    args = _argsHandler.HandleDefaultValues(methoddata, args);
                    args = _argsHandler.HandleRemaingText(methoddata, args, out string remainingText);

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

                    var handleArgumentsResult = await _argsHandler.HandleArgumentsTypeConvertings(player, methoddata, methodindex, amountmethods, args);
                    if (handleArgumentsResult.IsWrongMethod)
                        continue;

                    if (!handleArgumentsResult.Worked)
                        return;

                    //if (UseImplicitTypes)
                    //{
                    var finalInvokeArgs = _argsHandler.GetFinalInvokeArgs(methoddata, player, cmdinfos, args);
                    if (methoddata.IsAsync)
                        await (Task)(methoddata.MethodDefault.Invoke(_baseCommands, finalInvokeArgs.ToArray()))!;
                    else
                        await AltAsync.Do(() =>
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

        

        private bool IsAsyncMethod(MethodInfo method)
        {
            return method.GetCustomAttribute<AsyncStateMachineAttribute>() is { };
        }
    }
}
