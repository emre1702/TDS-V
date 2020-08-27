using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Commands.Loading
{
    internal class ArgsHandler
    {
        private readonly MappingHandler _mappingHandler;

        internal ArgsHandler(MappingHandler mappingHandler)
        {
            _mappingHandler = mappingHandler;
        }

        internal async Task<HandleArgumentsResult> HandleArgumentsTypeConvertings(ITDSPlayer player, CommandMethodDataDto methoddata, int methodindex,
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

                        if (parameterInfo.ParameterType == typeof(ITDSPlayer)
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

        internal List<object>? HandleMultipleArgsToOneArg(CommandMethodDataDto methodData, List<object> args)
        {
            foreach (var info in methodData.MultipleArgsToOneInfos)
            {
                if (args.Count < info.Index + info.Length)
                    return null;

                var entries = args.GetRange(info.Index, info.Length);
                args.RemoveRange(info.Index + 1, info.Length - 1);
                args[info.Index] = string.Join('|', entries);
            }
            return args;
        }

        internal List<object> HandleDefaultValues(CommandMethodDataDto methodData, List<object> args)
        {
            if (methodData.ParametersWithDefaultValueStartIndex.HasValue)
                for (int i = methodData.ParametersWithDefaultValueStartIndex.Value; i < methodData.ParameterInfos.Count; ++i)
                    if (args.Count < methodData.AmountDefaultParams + i)
                        args.Add(methodData.ParameterInfos[i].DefaultValue!);
            return args;
        }

        internal List<object> GetFinalInvokeArgs(CommandMethodDataDto methodData, ITDSPlayer cmdUser, TDSCommandInfos cmdInfos, List<object> args)
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

        internal List<object> HandleRemaingText(CommandMethodDataDto methodData, List<object> args, out string remainingText)
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

        internal bool IsInvalidArgsCount(ITDSPlayer player, CommandDataDto commanddata, List<object> args)
        {
            foreach (var methodData in commanddata.MethodDatas)
            {
                var requiredLength = methodData.ParametersWithDefaultValueStartIndex ?? methodData.ParameterInfos.Count;
                if (methodData.MultipleArgsToOneInfos.Count > 0)
                    requiredLength += methodData.MultipleArgsToOneInfos.Sum(m => m.Length - 1);

                if (methodData.ToOneStringAfterParameterCount is { } && args.Count >= methodData.ToOneStringAfterParameterCount)
                    return false;

                if (args.Count == requiredLength || args.Count > requiredLength && args.Count <= methodData.ParameterInfos.Count)
                    return false;

                if (args.Count < requiredLength)
                    player.SendMessage(player.Language.COMMAND_TOO_LESS_ARGUMENTS);
                else
                    player.SendMessage(player.Language.COMMAND_TOO_MANY_ARGUMENTS);
            }

            return true;
        }

        internal bool IsInvalidArgsCount(CommandMethodDataDto methodData, List<object> args)
        {
            var requiredLength = methodData.ParametersWithDefaultValueStartIndex ?? methodData.ParameterInfos.Count;
            if (methodData.MultipleArgsToOneInfos.Count > 0)
                requiredLength += methodData.MultipleArgsToOneInfos.Sum(m => m.Length - 1);

            if (methodData.ToOneStringAfterParameterCount is { } && args.Count >= methodData.ToOneStringAfterParameterCount)
                return false;

            if (args.Count == requiredLength || args.Count > requiredLength && args.Count <= methodData.ParameterInfos.Count)
                return false;

            return true;
        }
    }
}
