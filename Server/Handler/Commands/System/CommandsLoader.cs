using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Userpanel;
using TDS_Shared.Data.Attributes;
using DB = TDS_Server.Database.Entity.Command;

namespace TDS_Server.Handler.Commands.System
{
    public class CommandsLoader
    {
        private readonly Dictionary<string, CommandDataDto> _commandsDatas = new Dictionary<string, CommandDataDto>();
        private readonly Dictionary<string, string> _commandByAlias = new Dictionary<string, string>();

        private readonly ILoggingHandler _logger;
        private readonly CustomServiceProvider _serviceProvider;

        public CommandsLoader(TDSDbContext dbContext, CustomServiceProvider serviceProvider, UserpanelCommandsHandler userpanelCommandsHandler, ILoggingHandler logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            LoadCommandsFromAllSingletons(dbContext);
            userpanelCommandsHandler.LoadCommandData(_commandsDatas);

            dbContext.Dispose();
        }

        public CommandDataDto? GetCommandsData(string cmdOrAlias)
        {
            cmdOrAlias = cmdOrAlias.ToLower();
            lock (_commandByAlias)
            {
                if (_commandByAlias.TryGetValue(cmdOrAlias, out var realCmd))
                    cmdOrAlias = realCmd;
            }
            lock (_commandsDatas)
            {
                if (_commandsDatas.TryGetValue(cmdOrAlias, out var data))
                    return data;
            }
            return null;
        }

        private void LoadCommandsFromAllSingletons(TDSDbContext dbContext)
        {
            try
            {
                var commands = GetCommandsFromDatabase(dbContext);
                AddCommandsAndAliases(commands);
                AddAllMethods();
                SortMethodDatasByPriority();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        private void AddCommandsAndAliases(List<DB.Commands> commands)
        {
            foreach (var command in commands)
            {
                var cmd = command.Command.ToLower();
                var commandData = new CommandDataDto { Entity = command };
                _commandsDatas[cmd] = commandData;
                foreach (var alias in command.CommandAlias)
                    _commandByAlias[alias.Alias.ToLower()] = cmd;
            }
        }

        private void AddAllMethods()
        {
            var allMethods = GetAllCommandMethodsFromSingletons();
            foreach (var method in allMethods)
                AddMethod(method);
        }

        private IEnumerable<MethodInfo> GetAllCommandMethodsFromSingletons()
        {
            var allTypes = _serviceProvider.GetAllSingletonTypes();
            return allTypes.SelectMany(type => 
                type.GetType()
                    .GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(TDSCommand), false).Length > 0));
        }

        private bool HasCommandInfos(MethodInfo method)
        {
            var methodParams = method.GetParameters();
            if (methodParams.Length >= 2)
                if (methodParams[1].ParameterType == typeof(TDSCommandInfos))
                    return true;
            return false;
        }

        private void AddMethod(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<TDSCommand>()!;
            var cmd = attribute.Command.ToLower();
            if (!_commandsDatas.TryGetValue(cmd, out var commandData))
            {
                _logger.LogError($"TDSCommand '{attribute.Command}' exists but has no entry in the database. " +
                    $"Either add the command in the database or remove/comment out the method.", Environment.StackTrace, "CommandsLoader");
                return;
            }

            var methodData = new CommandMethodDataDto(method, attribute.Priority);
            commandData.MethodDatas.Add(methodData);
            methodData.HasCommandInfos = HasCommandInfos(method);

            var commandParameters = GetCommandParameters(method, methodData);
            HandleCommandParameters(commandParameters, methodData);
            methodData.ParameterInfos = commandParameters;
        }

        private void HandleCommandParameters(List<ParameterInfo> commandParameters, CommandMethodDataDto methodData)
        {
            for (int i = 0; i < commandParameters.Count; ++i)
            {
                if (methodData.ToOneStringAfterParameterCount.HasValue)
                    return;
                var parameter = commandParameters[i];
                HandleMultipleCommandParametersToOne(parameter, i, methodData);
                HandleParamsWithDefaultValuesStart(parameter, i, methodData);
                HandleRemainingText(parameter, methodData);
            }
        }

        private void HandleMultipleCommandParametersToOne(ParameterInfo parameter, int index, CommandMethodDataDto methodData)
        {
            var argLength = parameter.ParameterType.GetCustomAttribute<TDSCommandArgLength>();
            if (argLength is { })
                methodData.MultipleArgsToOneInfos.Add(new CommandMultipleArgsToOneInfo { Index = index, Length = argLength.ArgLength });

            else if (parameter.ParameterType == typeof(Vector3))
                methodData.MultipleArgsToOneInfos.Add(new CommandMultipleArgsToOneInfo { Index = index, Length = 3 });
        }

        private void HandleParamsWithDefaultValuesStart(ParameterInfo parameter, int index, CommandMethodDataDto methodData)
        {
            if (methodData.ParametersWithDefaultValueStartIndex is null && parameter.HasDefaultValue)
                methodData.ParametersWithDefaultValueStartIndex = index;
        }

        private void HandleRemainingText(ParameterInfo parameter, CommandMethodDataDto methodData)
        {
            var remainingTextAttribute = parameter.GetCustomAttribute(typeof(TDSRemainingText), false);
            if (remainingTextAttribute is TDSRemainingText attr)
            {
                methodData.ToOneStringAfterParameterCount = parameter.Position;
                methodData.RemainingTextAttribute = attr;
            }
        }

        private void SortMethodDatasByPriority()
        {
            foreach (var entry in _commandsDatas)
                entry.Value.MethodDatas.Sort((a, b) => -1 * a.Priority.CompareTo(b.Priority));
        }

        private List<ParameterInfo> GetCommandParameters(MethodInfo method, CommandMethodDataDto methodData)
        {
            var parameters = method.GetParameters();
            var skipAmount = methodData.AmountDefaultParams;
            return parameters.Skip(skipAmount).ToList();
        }

        private List<DB.Commands> GetCommandsFromDatabase(TDSDbContext dbContext)
            => dbContext.Commands.Include(c => c.CommandAlias).Include(c => c.CommandInfos).AsNoTracking().ToList();
    }
}
