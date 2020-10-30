using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Commands.System
{
    public class CommandsHandler
    {
        // private delegate void CommandDefaultMethod(TDSPlayer player, TDSCommandInfos commandinfos, object[] args); 
        // private delegate void CommandEmptyDefaultMethod(TDSPlayer player, TDSCommandInfos commandinfos);

        // private const int AmountDefaultParams = 2;

        // With implicit types it's much slower than direct call (Test: 8ms in 10000) - but then you
        // can use Methods with implicit types (e.g. AdminSay([defaultParams], string text, int
        // number)) Without implicit types it's much faster but you can only use Methods with
        // signature Method([defaultParams]) or Method([defaultParams], object[] args) 
        // private const bool UseImplicitTypes = true;

        private readonly ISettingsHandler _settingsHandler;
        private readonly ChatHandler _chatHandler;
        private readonly CommandsLoader _commandsLoader;
        private readonly CommandsUseRightsChecker _commandsUseRightsChecker;
        private readonly CommandsUseParametersConverter _commandsUseParametersConverter;
        private readonly CommandsValidation _commandsValidation;

        public CommandsHandler(MappingHandler mappingHandler, CommandsLoader commandsLoader,
            ISettingsHandler settingsHandler, ChatHandler chatHandler)
        {
            _settingsHandler = settingsHandler;
            _chatHandler = chatHandler;
            _commandsLoader = commandsLoader;
            _commandsUseRightsChecker = new CommandsUseRightsChecker();
            _commandsUseParametersConverter = new CommandsUseParametersConverter(mappingHandler);
            _commandsValidation = new CommandsValidation();

            NAPI.ClientEvent.Register<ITDSPlayer, string>(ToServerEvent.CommandUsed, this, UseCommand);
        }

        public async void UseCommand(ITDSPlayer player, string msg) // here msg is WITHOUT the command char (/) ... (e.g. "kick Pluz Test")
        {
            try
            {
                await Task.Yield();
                await HandleCommand(player, msg).ConfigureAwait(false);
            }
            catch
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.COMMAND_USED_WRONG));
            }
        }

        private async Task HandleCommand(ITDSPlayer player, string msg)
        {
            msg = msg.Trim();
            List<object> usedParameters = _commandsUseParametersConverter.GetUsedParameters(msg, out string cmdOrAlias);

            var commandData = _commandsLoader.GetCommandsData(cmdOrAlias);
            if (commandData is null)
            {
                OutputCommandNotExists(player, msg);
                return;
            }

            var cmdInfos = new TDSCommandInfos(cmdOrAlias);
            var rights = _commandsUseRightsChecker.GetRights(player, commandData.Entity);
            if (rights == CommandUsageRight.NotAllowed)
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.NOT_ALLOWED));
                return;
            }

            if (_commandsValidation.CheckIsInvalidArgsCount(player, commandData, usedParameters))
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.COMMAND_USED_WRONG));
                return;
            }

            var correctMethodAndArgs = await GetCorrectMethodAndArgs(player, commandData, usedParameters).ConfigureAwait(false);
            if (!correctMethodAndArgs.HasValue)
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.COMMAND_USED_WRONG));
                return;
            }

            var finalInvokeArgs = _commandsUseParametersConverter.GetFinalInvokeArgs(correctMethodAndArgs.Value.Method, player, cmdInfos, correctMethodAndArgs.Value.Args);
            //var ret = correctMethodAndArgs.Value.Method.MethodDefault.Invoke(correctMethodAndArgs.Value.Method.Instance, finalInvokeArgs.ToArray());   
            var ret = correctMethodAndArgs.Value.Method.FastMethodInvoker.Invoke(correctMethodAndArgs.Value.Method.Instance, finalInvokeArgs.ToArray());

            if (ret is Task task)
                await task.ConfigureAwait(false);
        }

        private async ValueTask<(CommandMethodDataDto Method, List<object> Args)?> GetCorrectMethodAndArgs(ITDSPlayer player, CommandDataDto commandData, List<object> usedParameters)
        {
            var amountMethods = commandData.MethodDatas.Count;
            for (int methodIndex = 0; methodIndex < amountMethods; ++methodIndex)
            {
                var methodData = commandData.MethodDatas[methodIndex];
                if (_commandsValidation.IsInvalidArgsCount(methodData, usedParameters))
                    continue;

                var args = new List<object>(usedParameters);
                args = _commandsUseParametersConverter.HandleMultipleArgsToOneArg(methodData, args);
                if (args is null)
                    continue;

                args = _commandsUseParametersConverter.HandleDefaultValues(methodData, args);
                args = _commandsUseParametersConverter.HandleRemaingText(methodData, args, out string remainingText);

                var handleArgumentsResult = await _commandsUseParametersConverter.HandleArgumentsTypeConvertings(player, methodData, methodIndex, amountMethods, args)
                    .ConfigureAwait(false);
                if (handleArgumentsResult.IsWrongMethod)
                    continue;

                if (!handleArgumentsResult.Worked)
                    return null;

                if (!_commandsValidation.CheckRemainingTextMinMaxLength(player, methodData.RemainingTextAttribute, remainingText))
                    return null;

                return (methodData, args);
            }
            return null;
        }

        private void OutputCommandNotExists(ITDSPlayer player, string msg)
        {
            if (_settingsHandler.ServerSettings.ErrorToPlayerOnNonExistentCommand)
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.COMMAND_DOESNT_EXIST));
            if (_settingsHandler.ServerSettings.ToChatOnNonExistentCommand)
                _chatHandler.SendLobbyMessage(player, "/" + msg, false);
        }
    }
}
