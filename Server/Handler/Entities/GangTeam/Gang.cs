using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;

namespace TDS_Server.Handler.Entities.GangTeam
{
    public class Gang : DatabaseEntityWrapper
    {
        
        public Gangs Entity { get; set; }
        public List<TDSPlayer> PlayersOnline { get; } = new List<TDSPlayer>();
#nullable disable
        // This can't be null! 
        // If it's null, we got serious problems in the code!
        // Every gang needs a team in GangLobby! Even "None" gang (spectator team)!
        public Team GangLobbyTeam { get; set; }
#nullable restore

        public bool InAction { get; set; }

        public Gang(Gangs entity, GangsHandler gangsHandler, TDSDbContext dbContext, LoggingHandler loggingHandler) : base(dbContext, loggingHandler)
        {
            Entity = entity;
            gangsHandler.Add(this);

            dbContext.Attach(entity);
        }


        public static Gang GetById(int id)
        {
            return _gangById[id];
        }

        public static Gang? GetByTeamId(int teamId)
        {
            return _gangById.Values.FirstOrDefault(g => g.Entity.TeamId == teamId);
        }

        

        public void FuncIterate(Action<TDSPlayer> func)
        {
            foreach (var player in PlayersOnline)
            {
                func(player);
            }
        }

        public void SendMessage(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LangUtils.LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in PlayersOnline)
            {
                player.SendMessage(returndict[player.Language]);
            }
        }

        public void SendNotification(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LangUtils.LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in PlayersOnline)
            {
                player.SendNotification(returndict[player.Language]);
            }
        }


        public static Task LoadAll(TDSDbContext dbContext)
        {
            return dbContext.Gangs
                .Include(g => g.Members)
                .ThenInclude(m => m.RankNavigation)
                .Include(g => g.RankPermissions)
                .Include(g => g.Ranks)
                .AsNoTracking()
                .ForEachAsync(g =>
            {
                _ = new Gang(g);
            });
        }
    }
}
