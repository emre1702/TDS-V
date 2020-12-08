using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models.GangWindow;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Entities.GangSystem;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Sync;
using TDS.Shared.Core;
using TDS.Shared.Data.Utility;

namespace TDS.Server.Handler.GangSystem
{
    public class GangWindowCreateHandler : DatabaseEntityWrapper
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly IGangsProvider _gangsProvider;

        public GangWindowCreateHandler(TDSDbContext dbContext,
            LobbiesHandler lobbiesHandler, EventsHandler eventsHandler, IGangsProvider gangsProvider)
            : base(dbContext)
        {
            _lobbiesHandler = lobbiesHandler;
            _eventsHandler = eventsHandler;
            _gangsProvider = gangsProvider;
        }

        public async Task<object?> CreateGang(ITDSPlayer player, string json)
        {
            if (player.Entity is null)
                return "";

            var gangCreateData = Serializer.FromBrowser<GangCreateData>(json);

            var gangEntity = GetGangEntity(gangCreateData, player, _lobbiesHandler.GangLobby);
            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Gangs.Add(gangEntity);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            var gang = _gangsProvider.GetGang(gangEntity);
            await _eventsHandler.OnGangJoin(player, gang, gangEntity.Ranks.MaxBy(r => r.Rank).First()).ConfigureAwait(false);
            // IsInGang is set to true in Angular, not needed here
            // Permissions will get synced, too - not needed here.

            return "";
        }

        private Gangs GetGangEntity(GangCreateData data, ITDSPlayer player, IGangLobby lobby)
        {
            var highestRank = new GangRanks { Rank = 3, Name = "Rank 3", Color = "rgb(255,255,255)" };
            var highestTeamIndex = lobby.Entity.Teams.Max(t => t.Index);
            var rgbColor = SharedUtils.GetColorFromHtmlRgba(data.Color) ?? Color.White;

            return new Gangs
            {
                Name = data.Name,
                NameShort = data.Short,
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
                    StartGangAction = 3,
                    SetRanks = 3
                },
                Members = new List<GangMembers>(),
                Stats = new GangStats(),
                Team = new Teams { Lobby = lobby.Entity.Id, Index = (short)(highestTeamIndex + 1), Name = data.Short, ColorR = rgbColor.R, ColorG = rgbColor.G, ColorB = rgbColor.B }
            };
        }
    }
}
