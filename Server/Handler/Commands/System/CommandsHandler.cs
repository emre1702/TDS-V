using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Commands.System
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
        private readonly CommandsUsedParametersConverter _commandsUsedParametersConverter;
        private readonly CommandsValidation _commandsValidation;

        public CommandsHandler(MappingHandler mappingHandler, CommandsLoader commandsLoader,
            ISettingsHandler settingsHandler, ChatHandler chatHandler, AdminsHandler adminsHandler)
        {
            _settingsHandler = settingsHandler;
            _chatHandler = chatHandler;
            _commandsLoader = commandsLoader;
            _commandsUseRightsChecker = new CommandsUseRightsChecker();
            _commandsUsedParametersConverter = new CommandsUsedParametersConverter(mappingHandler);
            _commandsValidation = new CommandsValidation(adminsHandler);

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
            msg = msg.TrimAndRemoveDuplicateSpaces();
            List<object> usedParameters = _commandsUsedParametersConverter.GetUsedParameters(msg, out string cmdOrAlias);

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
                return;

            var correctMethodAndArgs = await GetCorrectMethodAndArgs(player, commandData, usedParameters).ConfigureAwait(false);
            if (correctMethodAndArgs is null)
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.COMMAND_USED_WRONG));
                return;
            }

            if (!_commandsValidation.CheckIsValid(player, correctMethodAndArgs))
                return;

            var finalInvokeArgs = _commandsUsedParametersConverter.GetFinalInvokeArgs(correctMethodAndArgs.Method, player, cmdInfos, correctMethodAndArgs.Args);
            //var ret = correctMethodAndArgs.Value.Method.MethodDefault.Invoke(correctMethodAndArgs.Value.Method.Instance, finalInvokeArgs.ToArray());   
            var ret = correctMethodAndArgs.Method.FastMethodInvoker.Invoke(correctMethodAndArgs.Method.Instance, finalInvokeArgs.ToArray());

            if (ret is Task task)
                await task.ConfigureAwait(false);
        }

        private async ValueTask<CommandMethodAndArgs?> GetCorrectMethodAndArgs(ITDSPlayer player, CommandDataDto commandData, List<object> usedParameters)
        {
            var amountMethods = commandData.MethodDatas.Count;
            for (int methodIndex = 0; methodIndex < amountMethods; ++methodIndex)
            {
                var methodData = commandData.MethodDatas[methodIndex];
                if (_commandsValidation.IsInvalidArgsCount(methodData, usedParameters))
                    continue;

                var args = new List<object>(usedParameters);
                args = _commandsUsedParametersConverter.HandleMultipleArgsToOneArg(methodData, args);
                if (args is null)
                    continue;

                args = _commandsUsedParametersConverter.HandleDefaultValues(methodData, args);
                args = _commandsUsedParametersConverter.HandleRemaingText(methodData, args, out string remainingText);

                var handleArgumentsResult = await _commandsUsedParametersConverter
                    .HandleArgumentsTypeConvertings(player, methodData, methodIndex, amountMethods, args)
                    .ConfigureAwait(false);
                if (handleArgumentsResult.IsWrongMethod)
                    continue;

                if (!handleArgumentsResult.Worked)
                    return null;

                return new() { Method = methodData, Args = args, RemainingText = remainingText };
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
