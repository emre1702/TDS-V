﻿using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Server.CustomAttribute;
using TDS_Server.Enum;
using TDS_Server.Instance.Dto;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Mapping;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Command;
using TDS_Server_DB.Entity.Player;
using DB = TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Commands
{
    internal class CommandsManager : Script
    {
        // private delegate void CommandDefaultMethod(TDSPlayer character, TDSCommandInfos commandinfos, object[] args);
        // private delegate void CommandEmptyDefaultMethod(TDSPlayer character, TDSCommandInfos commandinfos);

        private class CommandData
        {
            public List<CommandMethodData> MethodDatas = new List<CommandMethodData>();
        }

        private class HandleArgumentsResult
        {
            public bool Worked;
            public bool IsWrongMethod;
        }

        private class CommandMethodData
        {
            public MethodInfo MethodDefault;  // only used when UseImplicitTypes == true

            // public CommandDefaultMethod? Method;    // only used when UseImplicitTypes == false
            // public CommandEmptyDefaultMethod? MethodEmpty;   // only used when UseImplicitTypes == false
            public Type[] ParameterTypes = new Type[0];

            public int Priority;
            public int? ToOneStringAfterParameterCount = null;
            public bool HasCommandInfos = false;

            public int AmountDefaultParams => 1 + (HasCommandInfos ? 1 : 0);

            public CommandMethodData(MethodInfo methodDefault, int priority)
            {
                MethodDefault = methodDefault;
                Priority = priority;
            }
        }

        private static readonly Dictionary<string, CommandData> _commandDataByCommand = new Dictionary<string, CommandData>();  // this is the primary Dictionary for commands!
        private static readonly Dictionary<string, DB.Command.Commands> _commandsDict = new Dictionary<string, DB.Command.Commands>();
        private static readonly Dictionary<string, string> _commandByAlias = new Dictionary<string, string>();
        private static readonly Dictionary<Type, Func<string, Task<object?>>> _typeConverter = new Dictionary<Type, Func<string, Task<object?>>>();

        // private const int AmountDefaultParams = 2;

        // With implicit types it's much slower than direct call (Test: 8ms in 10000) - but then you can use Methods with implicit types (e.g. AdminSay([defaultParams], string text, int number))
        // Without implicit types it's much faster but you can only use Methods with signature Method([defaultParams]) or Method([defaultParams], object[] args)
        // private const bool UseImplicitTypes = true;

        public static async Task LoadCommands(TDSNewContext dbcontext)
        {
            LoadConverters();

            foreach (DB.Command.Commands command in await dbcontext.Commands.Include(c => c.CommandAlias).ToListAsync())
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
                string cmd = attribute.Command.ToLower();
                if (!_commandsDict.ContainsKey(cmd))  // Only add the command if we got an entry in DB
                    continue;

                CommandData commanddata;
                if (_commandDataByCommand.ContainsKey(cmd))
                    commanddata = _commandDataByCommand[cmd];
                else
                    commanddata = new CommandData();
                CommandMethodData methoddata = new CommandMethodData
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

                var parameters = method.GetParameters().Skip(methoddata.AmountDefaultParams);
                Type[] parametertypes = parameters.Select(p => p.ParameterType).ToArray();

                foreach (var parameter in parameters)
                {
                    #region TDSRemainingText attribute

                    if (!methoddata.ToOneStringAfterParameterCount.HasValue)
                        if (parameter.CustomAttributes.Any(d => d.AttributeType == typeof(TDSRemainingText)))
                            methoddata.ToOneStringAfterParameterCount = parameter.Position;

                    #endregion TDSRemainingText attribute

                    #region Save parameter types

                    // Don't need Type for parameters beginning at ToOneStringAfterParameterCount (because they are always strings)
                    if (!methoddata.ToOneStringAfterParameterCount.HasValue || parameter.Position <= methoddata.ToOneStringAfterParameterCount.Value)
                        parametertypes[parameter.Position - methoddata.AmountDefaultParams] = parameter.ParameterType;

                    #endregion Save parameter types
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
        }

        [RemoteEvent(DToServerEvent.CommandUsed)]
        public static async void UseCommand(Client player, string msg)   // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            TDSPlayer character = player.GetChar();
            try
            {
                if (character.Entity == null || !character.Entity.PlayerStats.LoggedIn)
                    return;

                object[]? args = GetArgs(msg, out string cmd);
                TDSCommandInfos cmdinfos = new TDSCommandInfos(command: cmd);
                if (_commandByAlias.ContainsKey(cmd))
                    cmd = _commandByAlias[cmd];

                if (!CheckCommandExists(character, cmd, args))
                    return;

                DB.Command.Commands entity = _commandsDict[cmd];

                if (!CheckRights(character, entity, cmdinfos))
                    return;

                CommandData commanddata = _commandDataByCommand[cmd];

                if (IsInvalidArgsCount(character, commanddata, args))
                    return;

                int amountmethods = commanddata.MethodDatas.Count;
                for (int methodindex = 0; methodindex < amountmethods; ++methodindex)
                {
                    var methoddata = commanddata.MethodDatas[methodindex];

                    args = HandleRemaingText(methoddata, args);
                    var handleArgumentsResult = await HandleArgumentsTypeConvertings(character, methoddata, methodindex, amountmethods, args);
                    if (handleArgumentsResult.IsWrongMethod)
                        continue;

                    if (!handleArgumentsResult.Worked)
                        return;

                    //if (UseImplicitTypes)
                    //{
                    object[] finalInvokeArgs = GetFinalInvokeArgs(methoddata, character, cmdinfos, args);
                    methoddata.MethodDefault.Invoke(null, finalInvokeArgs);
                    /*}
                    else
                    {
#pragma warning disable
                        if (args != null)
                            methoddata.Method.Invoke(character, cmdinfos, args);
                        else
                            methoddata.MethodEmpty.Invoke(character, cmdinfos);
#pragma warning enable
                    }*/
                    break;
                }
            }
            catch
            {
                NAPI.Chat.SendChatMessageToPlayer(player, character.Language.COMMAND_USED_WRONG);
            }
        }

        private static async Task<HandleArgumentsResult> HandleArgumentsTypeConvertings(TDSPlayer player, CommandMethodData methoddata, int methodindex, int amountmethodsavailable, object[]? args)
        {
            if (args == null)
                return new HandleArgumentsResult { Worked = true };

            try
            {
                for (int i = 0; i < Math.Min((args?.Length ?? 0), methoddata.ParameterTypes.Length); ++i)
                {
                    if (args == null || args[i] == null)
                        continue;

                    var parameterType = methoddata.ParameterTypes[i];
                    object? arg = await GetConvertedArg(args[i], parameterType);

                    #region Check for null

                    if (arg == null)
                    {
                        #region Check if player exists

                        if (parameterType == typeof(TDSPlayer) || parameterType == typeof(Client) || parameterType == typeof(Players))
                        {
                            // if it's the last method (there can be an alternative method with string etc. instead of TDSPlayer/Client)
                            if (methodindex + 1 == amountmethodsavailable)
                            {
                                NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.PLAYER_DOESNT_EXIST);
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

        private static object[]? HandleRemaingText(CommandMethodData methodData, object[]? args)
        {
            if (args != null && methodData.ToOneStringAfterParameterCount.HasValue)
            {
                int index = methodData.ToOneStringAfterParameterCount.Value - methodData.AmountDefaultParams;
                args[index] = string.Join(' ', args.Skip(index));
                return args.Take(index + 1).ToArray();
            }
            return args;
        }

        private static bool IsInvalidArgsCount(TDSPlayer player, CommandData commanddata, object[]? args)
        {
            if ((args?.Length ?? 0) < commanddata.MethodDatas[0].ParameterTypes.Length)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.COMMAND_TOO_LESS_ARGUMENTS);
                return true;
            }
            return false;
        }

        private static bool CheckCommandExists(TDSPlayer player, string cmd, object[]? args)
        {
            if (!_commandDataByCommand.ContainsKey(cmd))
            {
                if (SettingsManager.ErrorToPlayerOnNonExistentCommand)
                    NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.COMMAND_DOESNT_EXIST);
                if (SettingsManager.ToChatOnNonExistentCommand)
                    ChatManager.SendLobbyMessage(player, "/" + cmd + " " + string.Join(' ', args), false);
                return false;
            }
            return true;
        }

        private static bool CheckRights(TDSPlayer character, DB.Command.Commands entity, TDSCommandInfos cmdinfos)
        {
            bool canuse = false;
            bool needright = false;

            #region Lobby-Owner Check

            if (!canuse && entity.LobbyOwnerCanUse)
            {
                needright = true;
                canuse = character.IsLobbyOwner;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.LobbyOwner;
            }

            #endregion Lobby-Owner Check

            #region Admin Check

            if (!canuse && entity.NeededAdminLevel.HasValue)
            {
                needright = true;
                canuse = (character.Entity?.AdminLvl ?? 0) >= entity.NeededAdminLevel.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.Admin;
            }

            #endregion Admin Check

            #region Donator Check

            if (!canuse && entity.NeededDonation.HasValue)
            {
                needright = true;
                canuse = (character.Entity?.Donation ?? 0) >= entity.NeededDonation.Value;
                if (canuse)
                    cmdinfos.WithRight = ECommandUsageRight.VIP;
            }

            #endregion Donator Check

            #region VIP Check

            if (!canuse && entity.VipCanUse)
            {
                needright = true;
                canuse = character.Entity?.IsVip ?? false;
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
                NAPI.Chat.SendChatMessageToPlayer(character.Client, character.Language.NOT_ALLOWED);

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

        private static object[] GetFinalInvokeArgs(CommandMethodData methodData, TDSPlayer cmdUser, TDSCommandInfos cmdInfos, object[]? args)
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

        private static void LoadConverters()
        {
            _typeConverter[typeof(Players)] = GetDatabasePlayerByName;
        }

        private static async Task<object?> GetDatabasePlayerByName(string name)
        {
            object? result = null;
            result = await Player.Player.DbContext.Players
                .Where(p => p.Name == name)
                .Select(p => (object?)p)
                .FirstOrDefaultAsync();
            return result;
        }
    }
}