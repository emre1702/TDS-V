﻿using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface IGang : IDatabaseEntityWrapper
    {
        #region Public Properties

        Gangs Entity { get; set; }
        ITeam GangLobbyTeam { get; set; }
        IGangHouse? House { get; set; }
        bool InAction { get; set; }
        bool Initialized { get; set; }
        List<ITDSPlayer> PlayersOnline { get; }

        #endregion Public Properties

        #region Public Methods

        void FuncIterate(Action<ITDSPlayer> player);

        void SendMessage(Func<ILanguage, string> langGetter);

        void SendNotification(Func<ILanguage, string> langGetter);

        #endregion Public Methods
    }
}
