using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;

namespace TDS_Server.Handler.Entities.GangSystem
{
    public class Gang : DatabaseEntityWrapper, IGang
    {
        #region Private Fields

        private readonly LangHelper _langHelper;

        #endregion Private Fields

        #region Public Constructors

        public Gang(Gangs entity, GangsHandler gangsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler, LangHelper langHelper) : base(dbContext, loggingHandler)
        {
            _langHelper = langHelper;

            Entity = entity;
            gangsHandler.Add(this);

            dbContext.Attach(entity);
        }

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}
