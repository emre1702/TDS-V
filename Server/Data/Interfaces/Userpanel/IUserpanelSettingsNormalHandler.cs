﻿using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    public interface IUserpanelSettingsNormalHandler
    {
        #region Public Methods

        Task<string> ConfirmDiscordUserId(ulong userId);

        Task<object> SaveSettings(ITDSPlayer player, ArraySegment<object> args);

        #endregion Public Methods
    }
}
