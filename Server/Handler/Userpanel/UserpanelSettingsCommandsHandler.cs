using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models.PlayerCommands;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSettingsCommandsHandler : DatabaseEntityWrapper, IUserpanelPlayerCommandsHandler
    {
        #region Private Fields

        private readonly Serializer _serializer;
        private readonly UserpanelCommandsHandler _userpanelCommandsHandler;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelSettingsCommandsHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler, UserpanelCommandsHandler userpanelCommandsHandler,
            Serializer serializer, EventsHandler eventsHandler)
            : base(dbContext, loggingHandler)
        {
            _userpanelCommandsHandler = userpanelCommandsHandler;
            _serializer = serializer;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
        }

        #endregion Public Constructors

        #region Public Methods

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
                        .ToListAsync();
                });

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
                LoggingHandler.LogError(ex, player);
                return null;
            }
        }

        public async Task<object?> Save(ITDSPlayer player, ArraySegment<object> args)
        {
            string datasJson = (string)args[0];

            var datas = _serializer.FromBrowser<List<UserpanelPlayerConfiguredCommandData>>(datasJson);
            if (datas.Count == 0)
                return null;

            await ExecuteForDBAsync(async dbContext =>
            {
                foreach (var data in datas)
                {
                    var entity = await dbContext.PlayerCommands.FirstOrDefaultAsync(c => c.PlayerId == player.Id && c.CommandId == data.CommandId);
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
                await dbContext.SaveChangesAsync();
            });

            var newData = await GetData(player);
            player.TriggerEvent(ToClientEvent.SyncPlayerCommandsSettings, _serializer.ToClient(newData));

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private async void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            var data = await GetData(player);
            NAPI.Task.Run(() =>
            {
                player.TriggerEvent(ToClientEvent.SyncPlayerCommandsSettings, _serializer.ToClient(data));
            });
        }

        #endregion Private Methods
    }
}
