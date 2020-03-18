using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Server.Handler.Helper;

namespace TDS_Server.Handler.Entities.GangTeam
{
    public class Gang : DatabaseEntityWrapper, IGang
    {
        
        public Gangs Entity { get; set; }
        public List<ITDSPlayer> PlayersOnline { get; } = new List<ITDSPlayer>();
#nullable disable
        // This can't be null! 
        // If it's null, we got serious problems in the code!
        // Every gang needs a team in GangLobby! Even "None" gang (spectator team)!
        public ITeam GangLobbyTeam { get; set; }
#nullable restore

        public bool InAction { get; set; }

        private readonly LangHelper _langHelper;

        public Gang(Gangs entity, GangsHandler gangsHandler, TDSDbContext dbContext, LoggingHandler loggingHandler, LangHelper langHelper) : base(dbContext, loggingHandler)
        {
            _langHelper = langHelper;

            Entity = entity;
            gangsHandler.Add(this);

            dbContext.Attach(entity);
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
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in _langHelper.LanguageByID.Values)
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
            foreach (ILanguage lang in _langHelper.LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in PlayersOnline)
            {
                player.SendNotification(returndict[player.Language]);
            }
        }
    }
}
