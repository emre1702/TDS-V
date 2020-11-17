using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Commands.System
{
    internal class CommandsUseParametersConverter
    {
        private readonly MappingHandler _mappingHandler;

        internal CommandsUseParametersConverter(MappingHandler mappingHandler) 
            => _mappingHandler = mappingHandler;

        internal async Task<CommandsHandleArgumentsResult> HandleArgumentsTypeConvertings(ITDSPlayer player, CommandMethodDataDto methoddata, int methodindex,
            int amountmethodsavailable, List<object> args)
        {
            if (args.Count == 0)
                return new CommandsHandleArgumentsResult(Worked: true);

            try
            {
                for (int i = 0; i < Math.Min(args.Count, methoddata.ParameterInfos.Count); ++i)
                {
                    if (args[i] is null)
                        continue;

                    var parameterInfo = methoddata.ParameterInfos[i];
                    object? arg = await GetConvertedArg(args[i], parameterInfo.ParameterType).ConfigureAwait(false);

                    if (arg is null)
                    {

                        if (parameterInfo.ParameterType == typeof(ITDSPlayer)
                            || parameterInfo.ParameterType == typeof(Players))
                        {
                            // if it's the last method (there can be an alternative method with
                            // string etc. instead of TDSPlayer/Player)
                            if (methodindex + 1 == amountmethodsavailable)
                            {
                                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.PLAYER_DOESNT_EXIST));
                                return new CommandsHandleArgumentsResult();
                            }
                            return new CommandsHandleArgumentsResult(IsWrongMethod: true);
                        }

                    }

                    object theArg = arg ?? string.Empty;
                    args[i] = theArg;  // arg shouldn't be able to be null
                }
                return new CommandsHandleArgumentsResult(Worked: true);
            }
            catch
            {
                if (methodindex + 1 == amountmethodsavailable)
                    return new CommandsHandleArgumentsResult();
                else
                    return new CommandsHandleArgumentsResult(IsWrongMethod: true);
            }
        }

        private async ValueTask<object?> GetConvertedArg(object notConvertedArg, Type theType)
        {
            theType = _mappingHandler.GetCorrectDestType(theType);
            object? converterReturn = _mappingHandler.Mapper.Map(notConvertedArg, typeof(string), theType);
            if (converterReturn != null && converterReturn is Task<Players?> task)
                return await task.ConfigureAwait(false);
            return converterReturn;
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

        internal List<object> GetUsedParameters(string msg, out string cmd)
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

        internal List<object> GetFinalInvokeArgs(CommandMethodDataDto methodData, ITDSPlayer cmdUser, TDSCommandInfos cmdInfos, List<object> args)
        {
            args.Insert(0, cmdUser);
            if (methodData.HasCommandInfos)
                args.Insert(1, cmdInfos);
            return args;
        }
    }
}
