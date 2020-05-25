﻿using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSupportUserHandler
    {
        #region Public Methods

        Task<string> GetData(ITDSPlayer player);

        #endregion Public Methods
    }
}
