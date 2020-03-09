using System.Collections.Generic;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Dto.Userpanel.Command;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Core.Manager.Userpanel
{
    class Commands
    {
        private static readonly List<UserpanelCommandDataDto> _commandDatas = new List<UserpanelCommandDataDto>();
        private static string _commandDatasJson = "[]";

        public static void LoadCommandData(
            Dictionary<string, CommandDataDto> commandDataByCommand, 
            Dictionary<string, TDS_Server_DB.Entity.Command.Commands> commandsDict)
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

            _commandDatasJson = Serializer.ToBrowser(_commandDatas);
        }

        public static string GetData()
        {
            return _commandDatasJson;
        }
    }
}
