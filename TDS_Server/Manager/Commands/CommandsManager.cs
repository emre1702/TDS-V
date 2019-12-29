using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Server.CustomAttribute;
using TDS_Server.Dto;
using TDS_Server.Enums;
using TDS_Server.Instance.Dto;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Mapping;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Command;
using TDS_Server_DB.Entity.Player;
using DB = TDS_Server_DB.Entity.Command;

namespace TDS_Server.Manager.Commands
{
    internal class CommandsManager : Script
    {
        // private delegate void CommandDefaultMethod(TDSPlayer player, TDSCommandInfos commandinfos, object[] args);
        // private delegate void CommandEmptyDefaultMethod(TDSPlayer player, TDSCommandInfos commandinfos);


        private class HandleArgumentsResult
        {
            public bool Worked;
            public bool IsWrongMethod;
        }

        private static readonly Dictionary<string, CommandDataDto> _commandDataByCommand = new Dictionary<string, CommandDataDto>();  // this is the primary Dictionary for commands!
        private static readonly Dictionary<string, DB.Commands> _commandsDict = new Dictionary<string, DB.Commands>();
        private static readonly Dictionary<string, string> _commandByAlias = new Dictionary<string, string>();

        // private const int AmountDefaultParams = 2;

        // With implicit types it's much slower than direct call (Test: 8ms in 10000) - but then you can use Methods with implicit types (e.g. AdminSay([defaultParams], string text, int number))
        // Without implicit types it's much faster but you can only use Methods with signature Method([defaultParams]) or Method([defaultParams], object[] args)
        // private const bool UseImplicitTypes = true;

        public static async Task LoadCommands(TDSDbContext dbcontext)
        {
            foreach (DB.Commands command in await dbcontext.Commands.Include(c => c.CommandAlias).Include(c => c.CommandInfos).ToListAsync())
            {
                _commandsDict[command.Command.ToLower()] = command;

                foreach (CommandAlias alias in command.CommandAlias)
                {
                    _commandByAlias[alias.Alias.ToLower()] = command.Command.ToLower();
                }
            }

            List<MethodInfo> methods = Assembly.GetExecutingAssembly().GetTypes()
                   .SelectMany(t => t.GetMethods())
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
                Type[] parametertypes = parameters.Select(p => p.ParameterType).ToArray();

                for (int i = 0; i < parameters.Count; ++i)
                {
                    var parameter = parameters[i];

                    #region Save parameter types
                    parametertypes[parameter.Position - methoddata.AmountDefaultParams] = parameter.ParameterType;
                    #endregion Save parameter types

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
                methoddata.ParameterTypes = parametertypes;

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

            Userpanel.Commands.LoadCommandData(_commandDataByCommand, _commandsDict);
        }


        [RemoteEvent(DToServerEvent.CommandUsed)]
        public static void UseCommand(Client client, string msg)   // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;
            UseCommand(player, msg);
        }

        public static async void UseCommand(TDSPlayer player, string msg) // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        { 
            
            try
            {
                object[]? args = GetArgs(msg, out string cmd);
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
                    #endregion

                    var handleArgumentsResult = await HandleArgumentsTypeConvertings(player, methoddata, methodindex, amountmethods, args);
                    if (handleArgumentsResult.IsWrongMethod)
                        continue;

                    if (!handleArgumentsResult.Worked)
                        return;

                    //if (UseImplicitTypes)
                    //{
                    object[] finalInvokeArgs = GetFinalInvokeArgs(methoddata, player, cmdinfos, args);
                    methoddata.MethodDefault.Invoke(null, finalInvokeArgs);
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

        private static async Task<HandleArgumentsResult> HandleArgumentsTypeConvertings(TDSPlayer player, CommandMethodDataDto methoddata, int methodindex, int amountmethodsavailable, object[]? args)
        {
            if (args is null)
                return new HandleArgumentsResult { Worked = true };

            try
            {
                for (int i = 0; i < Math.Min((args?.Length ?? 0), methoddata.ParameterTypes.Length); ++i)
                {
                    if (args is null || args[i] is null)
                        continue;

                    var parameterType = methoddata.ParameterTypes[i];
                    object? arg = await GetConvertedArg(args[i], parameterType);

                    #region Check for null

                    if (arg is null)
                    {
                        #region Check if player exists

                        if (parameterType == typeof(TDSPlayer) || parameterType == typeof(Client) || parameterType == typeof(Players))
                        {
                            // if it's the last method (there can be an alternative method with string etc. instead of TDSPlayer/Client)
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

        private static object[]? HandleRemaingText(CommandMethodDataDto methodData, object[]? args, out string remainingText)
        {
            if (args != null && methodData.ToOneStringAfterParameterCount.HasValue)
            {
                int index = methodData.ToOneStringAfterParameterCount.Value - methodData.AmountDefaultParams;
                remainingText = string.Join(' ', args.Skip(index));
                args[index] = remainingText;
                return args.Take(index + 1).ToArray();
            }
            remainingText = string.Empty;
            return args;
        }

        private static bool IsInvalidArgsCount(TDSPlayer player, CommandDataDto commanddata, object[]? args)
        {
            if ((args?.Length ?? 0) < commanddata.MethodDatas[0].ParameterTypes.Length)
            {
                player.SendMessage(player.Language.COMMAND_TOO_LESS_ARGUMENTS);
                return true;
            }
            return false;
        }

        private static bool CheckCommandExists(TDSPlayer player, string cmd, object[]? args)
        {
            if (!_commandDataByCommand.ContainsKey(cmd))
            {
                if (SettingsManager.ErrorToPlayerOnNonExistentCommand)
                    player.SendMessage(player.Language.COMMAND_DOESNT_EXIST);
                if (SettingsManager.ToChatOnNonExistentCommand)
                    ChatManager.SendLobbyMessage(player, "/" + cmd + (args != null ?  " " + string.Join(' ', args) : ""), false);
                return false;
            }
            return true;
        }

        private static bool CheckRights(TDSPlayer player, DB.Commands entity, TDSCommandInfos cmdinfos)
        {
            bool canuse = false;
            bool needright = false;

            #region Lobby-Owner Check

            if (!canuse && entity.LobbyOwnerCanUse)
            {
                needright = true;
                canuse = player.IsLobbyOwner;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.LobbyOwner;
            }

            #endregion Lobby-Owner Check

            #region Admin Check

            if (!canuse && entity.NeededAdminLevel.HasValue)
            {
                needright = true;
                canuse = player.AdminLevel.Level >= entity.NeededAdminLevel.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.Admin;
            }

            #endregion Admin Check

            #region Donator Check

            if (!canuse && entity.NeededDonation.HasValue)
            {
                needright = true;
                canuse = (player.Entity?.Donation ?? 0) >= entity.NeededDonation.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.VIP;
            }

            #endregion Donator Check

            #region VIP Check

            if (!canuse && entity.VipCanUse)
            {
                needright = true;
                canuse = player.Entity?.IsVip ?? false;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.Donator;
            }

            #endregion VIP Check

            #region User Check

            if (!needright)
            {
                canuse = true;
                cmdinfos.WithRight = ECommandUsageRight.User;
            }

            #endregion User Check

            if (!canuse)
                player.SendMessage(player.Language.NOT_ALLOWED);

            return canuse;
        }

        private static object[]? GetArgs(string msg, out string cmd)
        {
            int cmdendindex = msg.IndexOf(' ');
            if (cmdendindex == -1)
            {
                cmd = msg.ToLower();
                return null;
            }
            cmd = msg.Substring(0, cmdendindex).ToLower();
            return msg.Substring(cmdendindex + 1).Split(' ').Cast<object>().ToArray();
        }

        private static object[] GetFinalInvokeArgs(CommandMethodDataDto methodData, TDSPlayer cmdUser, TDSCommandInfos cmdInfos, object[]? args)
        {
            object[] newargs = new object[(args?.Length ?? 0) + methodData.AmountDefaultParams];
            newargs[0] = cmdUser;
            if (methodData.HasCommandInfos)
                newargs[1] = cmdInfos;
            if (args != null)
                args.CopyTo(newargs, methodData.AmountDefaultParams);
            return newargs;
        }

        private static async Task<object?> GetConvertedArg(object notConvertedArg, Type theType)
        {
            theType = MappingManager.GetCorrectDestType(theType);
            object? converterReturn = MappingManager.Mapper.Map(notConvertedArg, typeof(string), theType);
            if (converterReturn != null && converterReturn is Task<Players?> task)
                return await task;
            return converterReturn;
        }
    }
}