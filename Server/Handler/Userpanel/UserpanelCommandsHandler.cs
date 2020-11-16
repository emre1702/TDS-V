using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Userpanel.Command;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelCommandsHandler
    {
        public string Data => _commandDatasJson;

        private string _commandDatasJson = "[]";

        public UserpanelCommandsHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
        }

        public List<UserpanelCommandDataDto> CommandDatas { get; } = new List<UserpanelCommandDataDto>();

        public void LoadCommandData(Dictionary<string, CommandDataDto> commandDatas)
        {
            foreach (var entry in commandDatas)
            {
                string command = entry.Key;
                var entity = entry.Value.Entity;

                var userpanelCommandData = new UserpanelCommandDataDto
                {
                    Id = entity.Id,
                    Command = entity.Command,
                    Aliases = entity.CommandAlias.Select(a => a.Alias).ToList(),
                    Description = entity.CommandInfos.ToDictionary(i => (int)i.Language, i => i.Info),
                    LobbyOwnerCanUse = entity.LobbyOwnerCanUse,
                    MinAdminLevel = entity.NeededAdminLevel,
                    MinDonation = entity.NeededDonation,
                    VIPCanUse = entity.VipCanUse
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

            _commandDatasJson = Serializer.ToBrowser(CommandDatas);
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            var data = Data;
            NAPI.Task.RunSafe(() => 
                player.TriggerBrowserEvent(ToBrowserEvent.SyncCommandsData, data));
        }
    }
}
