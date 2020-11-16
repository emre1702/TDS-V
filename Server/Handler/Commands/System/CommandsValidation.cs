using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Models;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Commands.System
{
    internal class CommandsValidation
    {
        internal bool CheckRemainingTextMinMaxLength(ITDSPlayer player, TDSRemainingTextAttribute? attribute, string remainingText)
        {
            if (attribute is null)
                return true;

            if (remainingText.Length < attribute.MinLength)
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.TEXT_TOO_SHORT));
                return false;
            }
            if (remainingText.Length > attribute.MaxLength)
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.TEXT_TOO_LONG));
                return false;
            }
            return true;
        }

        internal bool CheckIsInvalidArgsCount(ITDSPlayer player, CommandDataDto commandData, List<object> args)
        {
            foreach (var methodData in commandData.MethodDatas)
            {
                var requiredLength = methodData.ParametersWithDefaultValueStartIndex ?? methodData.ParameterInfos.Count;
                if (methodData.MultipleArgsToOneInfos.Count > 0)
                    requiredLength += methodData.MultipleArgsToOneInfos.Sum(m => m.Length - 1);

                if (methodData.ToOneStringAfterParameterCount is { } && args.Count >= methodData.ToOneStringAfterParameterCount)
                    return false;

                if (args.Count == requiredLength || args.Count > requiredLength && args.Count <= methodData.ParameterInfos.Count)
                    return false;

                if (args.Count < requiredLength)
                    NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.COMMAND_TOO_LESS_ARGUMENTS));
                else
                    NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.COMMAND_TOO_MANY_ARGUMENTS));
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
