﻿using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelApplicationUserHandler
    {
        #region Public Methods

        Task<object> AcceptInvitation(ITDSPlayer player, ArraySegment<object> args);

        Task<object> CreateApplication(ITDSPlayer player, ArraySegment<object> args);

        Task<string> GetData(ITDSPlayer player);

        Task<object> RejectInvitation(ITDSPlayer player, ArraySegment<object> args);

        #endregion Public Methods
    }
}
