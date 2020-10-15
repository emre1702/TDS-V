using GTANetworkAPI;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.GangSystem
{
    public class Gang : DatabaseEntityWrapper, IGang
    {
        public Gangs Entity { get; set; }

        // This can't be null! If it's null, we got serious problems in the code! Every gang needs a
        // team in GangLobby! Even "None" gang (spectator team)!
#nullable disable
        public ITeam GangLobbyTeam { get; set; }
#nullable restore

        //Todo: Don't forget to use this when buying, selling or losing the house
        public IGangHouse? House { get; set; }

        public bool InAction { get; set; }
        public bool Initialized { get; set; }
        public List<ITDSPlayer> PlayersOnline { get; } = new List<ITDSPlayer>();
        public Vector3? SpawnPosition => House?.Position;
        public float? SpawnHeading => House?.SpawnRotation;

        private readonly GangsHandler _gangsHandler;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly DataSyncHandler _dataSyncHandler;

        public Gang(Gangs entity, GangsHandler gangsHandler, TDSDbContext dbContext, LangHelper langHelper,
            LobbiesHandler lobbiesHandler, DataSyncHandler dataSyncHandler)
            : base(dbContext)
        {
            _langHelper = langHelper;
            _lobbiesHandler = lobbiesHandler;
            _gangsHandler = gangsHandler;
            _dataSyncHandler = dataSyncHandler;

            Entity = entity;
            gangsHandler.Add(this);

            dbContext.Attach(entity);
        }

        public void AppointNextSuitableLeader()
        {
            if (Entity.Members.Count == 0)
                return;

            var nextLeader = GetNextSuitableLeaderOnlyActive();
            if (nextLeader is null)
                nextLeader = GetNextSuitableLeaderAlsoInactive();
            if (nextLeader is null)
                nextLeader = Entity.Members.First();

            Entity.OwnerId = nextLeader.PlayerId;

            var onlinePlayer = PlayersOnline.FirstOrDefault(p => p.Entity?.Id == nextLeader.PlayerId);
            NAPI.Task.Run(() =>
            {
                onlinePlayer?.SendNotification(onlinePlayer.Language.YOUVE_BECOME_GANG_LEADER);
            });
        }

        public async Task Delete()
        {
            foreach (var player in PlayersOnline)
            {
                player.Gang = _gangsHandler.None;
                player.GangRank = _gangsHandler.NoneRank;
                _dataSyncHandler.SetData(player, PlayerDataKey.GangId, DataSyncMode.Player, player.Gang.Entity.Id);

                if (player.Lobby is IGangLobby || player.Lobby is IGangActionLobby)
                    await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0);
            }

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Gangs.Remove(Entity);
                await dbContext.SaveChangesAsync();
                await dbContext.DisposeAsync();
            });
        }

        public void FuncIterate(Action<ITDSPlayer> func)
        {
            foreach (var player in PlayersOnline)
            {
                func(player);
            }
        }

        public void SendMessage(Func<ILanguage, string> langgetter)
        {
            var returndict = new Dictionary<ILanguage, string>();
            foreach (var lang in _langHelper.LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in PlayersOnline)
            {
                player.SendChatMessage(returndict[player.Language]);
            }
        }

        public void SendNotification(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in _langHelper.LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in PlayersOnline)
            {
                player.SendNotification(returndict[player.Language]);
            }
        }

        public bool IsAllowedTo(ITDSPlayer player, GangCommand type)
        {
            if (player.IsGangOwner)
                return true;

            var rank = player.GangRank?.Rank ?? 0;

            return type switch
            {
                GangCommand.Invite => rank >= Entity.RankPermissions.InviteMembers,
                GangCommand.Kick => rank >= Entity.RankPermissions.KickMembers,
                GangCommand.RankDown => rank >= Entity.RankPermissions.SetRanks,
                GangCommand.RankUp => rank >= Entity.RankPermissions.SetRanks,
                GangCommand.ModifyRanks => rank >= Entity.RankPermissions.ManageRanks,
                GangCommand.ModifyPermissions => rank >= Entity.RankPermissions.ManagePermissions,
                _ => true
            };
        }

        private GangMembers? GetNextSuitableLeaderAlsoInactive()
        {
            var rankHighestMembers = Entity.Members.MaxBy(m => m.Rank);
            var count = rankHighestMembers.Count();

            if (count == 1)
                return rankHighestMembers.First();

            return rankHighestMembers.MaxBy(m => (DateTime.UtcNow - m.JoinTime).TotalSeconds).FirstOrDefault();
        }

        private GangMembers? GetNextSuitableLeaderOnlyActive()
        {
            var activeMembers = Entity.Members.Where(m => (DateTime.UtcNow - m.Player.PlayerStats.LastLoginTimestamp).TotalDays < 5);
            var count = activeMembers.Count();

            if (count == 0)
                return null;

            var rankHighestMembers = activeMembers.MaxBy(m => m.Rank);
            count = rankHighestMembers.Count();

            if (count == 1)
                return rankHighestMembers.First();

            return rankHighestMembers.MaxBy(m => (DateTime.UtcNow - m.JoinTime).TotalSeconds).FirstOrDefault();
        }
    }
}
