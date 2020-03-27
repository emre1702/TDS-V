﻿using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Userpanel.Command;
using TDS_Server.Database.Entity.Command;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelCommandsHandler
    {
        private readonly List<UserpanelCommandDataDto> _commandDatas = new List<UserpanelCommandDataDto>();
        private string _commandDatasJson = "[]";

        private readonly Serializer _serializer;

        public UserpanelCommandsHandler(Serializer serializer)
            => _serializer = serializer;

        public void LoadCommandData(
            Dictionary<string, CommandDataDto> commandDataByCommand,
            Dictionary<string, Database.Entity.Command.Commands> commandsDict)
        {
            foreach (var entry in commandDataByCommand)
            {
                string command = entry.Key;
                var dbCommand = commandsDict[command];

                var userpanelCommandData = new UserpanelCommandDataDto
                {
                    Command = dbCommand.Command,
                    Aliases = dbCommand.CommandAlias.Select(a => a.Alias).ToList(),
                    Description = dbCommand.CommandInfos.ToDictionary(i => (int)i.Language, i => i.Info),
                    LobbyOwnerCanUse = dbCommand.LobbyOwnerCanUse,
                    MinAdminLevel = dbCommand.NeededAdminLevel,
                    MinDonation = dbCommand.NeededDonation,
                    VIPCanUse = dbCommand.VipCanUse
                };

                var commandData = entry.Value;
                foreach (var methodData in commandData.MethodDatas)
                {
                    var syntax = new UserpanelCommandSyntaxDto();

                    var parameters = methodData.MethodDefault.GetParameters().Skip(methodData.AmountDefaultParams);
                    foreach (var parameter in parameters)
                    {
                        var parameterData = new UserpanelCommandParameterDto
                        {
                            Name = parameter.Name ?? "?",
                            Type = parameter.ParameterType.Name,
                            DefaultValue = parameter.DefaultValue?.ToString()
                        };
                        syntax.Parameters.Add(parameterData);
                    }

                    userpanelCommandData.Syntaxes.Add(syntax);
                }

                _commandDatas.Add(userpanelCommandData);
            }

            _commandDatasJson = _serializer.ToBrowser(_commandDatas);
        }

        public string GetData()
        {
            return _commandDatasJson;
        }
    }
}