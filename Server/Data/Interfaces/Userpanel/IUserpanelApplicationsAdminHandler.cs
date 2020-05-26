﻿using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelApplicationsAdminHandler
    {
        #region Public Methods

        Task<string> GetData(ITDSPlayer player);

        Task<object> SendApplicationData(ITDSPlayer player, ArraySegment<object> args);

        Task<object> SendInvitation(ITDSPlayer player, ArraySegment<object> args);

        #endregion Public Methods
    }
}