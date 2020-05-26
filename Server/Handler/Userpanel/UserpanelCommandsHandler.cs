using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Userpanel.Command;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Userpanel
{
  public class UserpanelCommandsHandler
  {
    #region Private Fields

    public List<UserpanelCommandDataDto> CommandDatas { get; } = new List<UserpanelCommandDataDto>();
    private readonly Serializer _serializer;
    private string _commandDatasJson = "[]";

    #endregion Private Fields

    #region Public Constructors

    public UserpanelCommandsHandler(Serializer serializer)
        => _serializer = serializer;

    #endregion Public Constructors

    #region Public Methods

    public string GetData()
    {
      return _commandDatasJson;
    }

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
          Id = dbCommand.Id,
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

        CommandDatas.Add(userpanelCommandData);
      }

      _commandDatasJson = _serializer.ToBrowser(CommandDatas);
    }

    #endregion Public Methods
  }
}
