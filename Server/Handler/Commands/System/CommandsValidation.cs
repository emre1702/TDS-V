using GTANetworkAPI;
using System;
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
        private readonly AdminsHandler _adminsHandler;

        internal CommandsValidation(AdminsHandler adminsHandler)
            => _adminsHandler = adminsHandler;

        internal bool CheckIsValid(ITDSPlayer player, CommandMethodAndArgs commandMethodAndArgs)
        {
            if (!CheckRemainingTextMinMaxLength(player, commandMethodAndArgs.Method.RemainingTextAttribute, commandMethodAndArgs.RemainingText))
                return false;
            if (!CheckAdminLevelProperties(player, commandMethodAndArgs))
                return false;
            if (!CheckLimitedNumberProperties(player, commandMethodAndArgs))
                return false;

            return true;
        }

        private bool CheckRemainingTextMinMaxLength(ITDSPlayer player, RemainingTextAttribute? attribute, string remainingText)
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

        private bool CheckAdminLevelProperties(ITDSPlayer player, CommandMethodAndArgs commandMethodAndArgs)
        {
            foreach (var parameterInfo in commandMethodAndArgs.Method.ParameterInfos)
            {
                if (!Attribute.IsDefined(parameterInfo, typeof(AdminLevelParameterAttribute)))
                    continue;

                var index = commandMethodAndArgs.Method.ParameterInfos.IndexOf(parameterInfo);
                var arg = commandMethodAndArgs.Args[index];

                if (!int.TryParse(arg.ToString(), out int adminLevel))
                {
                    player.SendNotification(player.Language.ERROR_INFO);
                    LoggingHandler.Instance.LogError($"Expect AdminLevel to be convertable to integer but it's not. Maybe wrong AdminLevelParameter attribute usage? Value: {arg} | Cmd: {commandMethodAndArgs.Method.MethodDefault.Name} | Index: {index}", Environment.StackTrace);
                    return false;
                }

                var lowestLevel = _adminsHandler.LowestLevel.Level;
                if (adminLevel < lowestLevel)
                {
                    player.SendNotification(string.Format(player.Language.ADMIN_LEVEL_MIN_NUMBER, lowestLevel));
                    return false;
                }

                var highestLevel = _adminsHandler.HighestLevel.Level;
                if (adminLevel > highestLevel)
                {
                    player.SendNotification(string.Format(player.Language.ADMIN_LEVEL_MAX_NUMBER, highestLevel));
                    return false;
                }

            }
            return true;
        }

        private bool CheckLimitedNumberProperties(ITDSPlayer player, CommandMethodAndArgs commandMethodAndArgs)
        {
            foreach (var parameterInfo in commandMethodAndArgs.Method.ParameterInfos)
            {
                if (!Attribute.IsDefined(parameterInfo, typeof(LimitedNumberAttribute)))
                    continue;

                var index = commandMethodAndArgs.Method.ParameterInfos.IndexOf(parameterInfo);
                var arg = commandMethodAndArgs.Args[index];
                var attribute = (LimitedNumberAttribute)parameterInfo.GetCustomAttributes(typeof(LimitedNumberAttribute), false)[0];

                if (!float.TryParse(arg.ToString(), out float number))
                {
                    player.SendNotification(player.Language.ERROR_INFO);
                    LoggingHandler.Instance.LogError($"Expect number to be convertable to float but it's not. Maybe wrong LimitedNumberAttribute attribute usage? Value: {arg} | Cmd: {commandMethodAndArgs.Method.MethodDefault.Name} | Index: {index}", Environment.StackTrace);
                    return false;
                }

                if (number < attribute.MinValue)
                {
                    player.SendNotification(string.Format(player.Language.MIN_NUMBER, attribute.MinValue));
                    return false;
                }

                if (number > attribute.MaxValue)
                {
                    player.SendNotification(string.Format(player.Language.MAX_NUMBER, attribute.MaxValue));
                    return false;
                }

            }
            return true;
        }

        internal bool CheckIsInvalidArgsCount(ITDSPlayer player, CommandDataDto commandData, List<object> args)
        {
            int? possibleMethodRequiredLength = null;
            foreach (var methodData in commandData.MethodDatas)
            {
                var requiredLength = methodData.ParametersWithDefaultValueStartIndex ?? methodData.ParameterInfos.Count;
                if (methodData.MultipleArgsToOneInfos.Count > 0)
                    requiredLength += methodData.MultipleArgsToOneInfos.Sum(m => m.Length - 1);

                if (methodData.ToOneStringAfterParameterCount is { } && args.Count >= methodData.ToOneStringAfterParameterCount)
                    return false;

                if (args.Count == requiredLength || args.Count > requiredLength && args.Count <= methodData.ParameterInfos.Count)
                    return false;

                possibleMethodRequiredLength = requiredLength;
            }

            if (possibleMethodRequiredLength.HasValue)
            {
                if (args.Count < possibleMethodRequiredLength.Value)
                    NAPI.Task.RunSafe(() => player.SendChatMessage(string.Format(player.Language.COMMAND_TOO_LESS_ARGUMENTS, possibleMethodRequiredLength, args.Count)));
                else
                    NAPI.Task.RunSafe(() => player.SendChatMessage(string.Format(player.Language.COMMAND_TOO_MANY_ARGUMENTS, possibleMethodRequiredLength, args.Count)));
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
