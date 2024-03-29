﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Userpanel;
using TDS.Shared.Data.Attributes;
using DB = TDS.Server.Database.Entity.Command;

namespace TDS.Server.Handler.Commands.System
{
    public class CommandsLoader
    {
        private readonly Dictionary<string, CommandDataDto> _commandsDatas = new Dictionary<string, CommandDataDto>();
        private readonly Dictionary<string, string> _commandByAlias = new Dictionary<string, string>();

        private readonly ILoggingHandler _logger;
        private readonly CustomServiceProvider _serviceProvider;
        private readonly FastMethodInvoker _fastMethodInvoker;

        public CommandsLoader(TDSDbContext dbContext, CustomServiceProvider serviceProvider, UserpanelCommandsHandler userpanelCommandsHandler, ILoggingHandler logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _fastMethodInvoker = new FastMethodInvoker();

            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
                RemoveInvalidCommandDatas();
                SortMethodDatasByPriority();
                OutputLoadedCommandsInfo();
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
            foreach (var (Method, Instance) in allMethods)
                AddMethod(Method, Instance);
        }

        private IEnumerable<(MethodInfo Method, object Instance)> GetAllCommandMethodsFromSingletons()
        {
            var allTypes = _serviceProvider.GetAllSingletonTypes();
            return allTypes.SelectMany(type =>
                type.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(TDSCommandAttribute), false).Length > 0)
                    .Select(m => (m, _serviceProvider.GetRequiredService(type))));
        }

        private bool HasCommandInfos(MethodInfo method)
        {
            var methodParams = method.GetParameters();
            if (methodParams.Length >= 2)
                if (methodParams[1].ParameterType == typeof(TDSCommandInfos))
                    return true;
            return false;
        }

        private void AddMethod(MethodInfo method, object instance)
        {
            var attribute = method.GetCustomAttribute<TDSCommandAttribute>()!;
            var cmd = attribute.Command.ToLower();
            if (!_commandsDatas.TryGetValue(cmd, out var commandData))
            {
                _logger.LogError($"TDSCommand '{attribute.Command}' exists but has no entry in the database. " +
                    $"Either add the command in the database or remove/comment out the method.", Environment.StackTrace, "CommandsLoader");
                return;
            }

            var methodData = new CommandMethodDataDto(method, _fastMethodInvoker.GetMethodInvoker(method), instance, attribute.Priority);
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
            var remainingTextAttribute = parameter.GetCustomAttribute(typeof(RemainingTextAttribute), false);
            if (remainingTextAttribute is RemainingTextAttribute attr)
            {
                methodData.ToOneStringAfterParameterCount = parameter.Position;
                methodData.RemainingTextAttribute = attr;
            }
        }

        private void RemoveInvalidCommandDatas()
        {
            foreach (var entry in _commandsDatas)
                if (entry.Value.MethodDatas.Count == 0)
                    _commandsDatas.Remove(entry.Key);
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

        private void OutputLoadedCommandsInfo()
        {
            var classAndAmountCommands = _commandsDatas.Values
                .Select(entry => entry.MethodDatas[0].Instance.GetType().Name)
                .Distinct()
                .Select(className => (ClassName: className, AmountCommands: GetCountOfCommandsInClassName(className)))
                .OrderBy(entry => entry.ClassName)
                .ToList();

            var previousForegroundColor = Console.ForegroundColor;
            foreach (var (ClassName, AmountCommands) in classAndAmountCommands)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Loaded ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(ClassName);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" with ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(AmountCommands);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" command(s).");
            }
            Console.ForegroundColor = previousForegroundColor;
        }

        private int GetCountOfCommandsInClassName(string className)
        {
            return _commandsDatas.Values.Count(v => v.MethodDatas[0].Instance.GetType().Name == className);
        }
    }
}