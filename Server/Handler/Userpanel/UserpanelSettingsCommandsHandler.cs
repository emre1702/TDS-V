using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.PlayerCommands;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelSettingsCommandsHandler : DatabaseEntityWrapper, IUserpanelPlayerCommandsHandler
    {
        private readonly UserpanelCommandsHandler _userpanelCommandsHandler;

        public UserpanelSettingsCommandsHandler(TDSDbContext dbContext, UserpanelCommandsHandler userpanelCommandsHandler,
            EventsHandler eventsHandler)
            : base(dbContext)
        {
            _userpanelCommandsHandler = userpanelCommandsHandler;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
        }

        public async Task<UserpanelPlayerCommandData?> GetData(ITDSPlayer player)
        {
            try
            {
                var addedCommandsList = await ExecuteForDBAsync(async dbContext =>
                {
                    return await dbContext.PlayerCommands
                        .Where(c => c.PlayerId == player.Id)
                        .Include(c => c.Command)
                        .Select(c => new UserpanelPlayerConfiguredCommandData
                        {
                            CommandId = c.CommandId,
                            CustomCommand = c.CommandText
                        })
                        .ToListAsync()
                        .ConfigureAwait(false);
                }).ConfigureAwait(false);

                var data = new UserpanelPlayerCommandData
                {
                    InitialCommands = _userpanelCommandsHandler.CommandDatas
                        .Select(c => new UserpanelPlayerCommandCommandData
                        {
                            Id = c.Id,
                            Command = c.Command
                        })
                        .ToList(),
                    AddedCommands = addedCommandsList
                };

                return data;
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, player);
                return null;
            }
        }

        public async Task<object?> Save(ITDSPlayer player, ArraySegment<object> args)
        {
            string datasJson = (string)args[0];

            var datas = Serializer.FromBrowser<List<UserpanelPlayerConfiguredCommandData>>(datasJson);
            if (datas.Count == 0)
                return null;

            await ExecuteForDBAsync(async dbContext =>
            {
                foreach (var data in datas)
                {
                    var entity = await dbContext.PlayerCommands
                        .FirstOrDefaultAsync(c => c.PlayerId == player.Id && c.CommandId == data.CommandId)
                        .ConfigureAwait(false);
                    if (data.CustomCommand.Length == 0)
                    {
                        if (entity is null)
                            continue;
                        dbContext.PlayerCommands.Remove(entity);
                    }
                    else
                    {
                        if (entity is null)
                        {
                            entity = new PlayerCommands
                            {
                                PlayerId = player.Id,
                                CommandId = (short)data.CommandId,
                                CommandText = data.CustomCommand
                            };
                            dbContext.PlayerCommands.Add(entity);
                        }
                        else
                        {
                            entity.CommandText = data.CustomCommand;
                            dbContext.PlayerCommands.Update(entity);
                        }
                    }
                }
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            var newData = await GetData(player).ConfigureAwait(false);
            NAPI.Task.RunSafe(() => 
                player.TriggerEvent(ToClientEvent.SyncPlayerCommandsSettings, Serializer.ToClient(newData)));

            return null;
        }

        private async void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            try
            {
                var data = await GetData(player).ConfigureAwait(false);
                var json = Serializer.ToClient(data);
                NAPI.Task.RunSafe(() =>
                {
                    player.TriggerEvent(ToClientEvent.SyncPlayerCommandsSettings, json);
                });
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }
    }
}
