using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.GangWindow;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Entities.GangSystem;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;

namespace TDS_Server.Handler.GangSystem
{
    public class GangWindowCreateHandler : DatabaseEntityWrapper
    {
        private readonly Serializer _serializer;
        private readonly IServiceProvider _serviceProvider;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly EventsHandler _eventsHandler;

        public GangWindowCreateHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IServiceProvider serviceProvider,
            DataSyncHandler dataSyncHandler, LobbiesHandler lobbiesHandler, EventsHandler eventsHandler)
            : base(dbContext, loggingHandler)
        {
            _serializer = serializer;
            _serviceProvider = serviceProvider;
            _dataSyncHandler = dataSyncHandler;
            _lobbiesHandler = lobbiesHandler;
            _eventsHandler = eventsHandler;
        }

        public async Task<object?> CreateGang(ITDSPlayer player, string json)
        {
            if (player.Entity is null)
                return "";

            var gangCreateData = _serializer.FromBrowser<GangCreateData>(json);

            var gangEntity = GetGangEntity(gangCreateData, player, _lobbiesHandler.GangLobby);
            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Gangs.Add(gangEntity);
                await dbContext.SaveChangesAsync();
            });

            var gang = ActivatorUtilities.CreateInstance<Gang>(_serviceProvider, gangEntity);
            await _eventsHandler.OnGangJoin(player, gang, gangEntity.Ranks.MaxBy(r => r.Rank).First());
            // IsInGang is set to true in Angular, not needed here
            // Permissions will get synced, too - not needed here.

            return "";
        }

        private Gangs GetGangEntity(GangCreateData data, ITDSPlayer player, GangLobby lobby)
        {
            var highestRank = new GangRanks { Rank = 3, Name = "Rank 3", Color = "rgb(255,255,255)" };
            var highestTeamIndex = lobby.Entity.Teams.Max(t => t.Index);
            var rgbColor = SharedUtils.GetColorFromHtmlRgba(data.Color) ?? Color.White;

            return new Gangs
            {
                Name = data.Name,
                Short = data.Short,
                OwnerId = player.Entity!.Id,
                Color = data.Color,
                BlipColor = data.BlipColor,
                Ranks = new List<GangRanks>
                {
                    new GangRanks { Rank = 0, Name = "Rank 0", Color = "rgb(255,255,255)" },
                    new GangRanks { Rank = 1, Name = "Rank 1", Color = "rgb(255,255,255)" },
                    new GangRanks { Rank = 2, Name = "Rank 2", Color = "rgb(255,255,255)" },
                    highestRank
                },
                RankPermissions = new GangRankPermissions
                {
                    InviteMembers = 3,
                    KickMembers = 3,
                    ManagePermissions = 3,
                    ManageRanks = 3,
                    StartGangwar = 3,
                    SetRanks = 3
                },
                Members = new List<GangMembers>(),
                Stats = new GangStats(),
                Team = new Teams { Lobby = lobby.Id, Index = (short)(highestTeamIndex + 1), Name = data.Short, ColorR = rgbColor.R, ColorG = rgbColor.G, ColorB = rgbColor.B }
            };
        }
    }
}
