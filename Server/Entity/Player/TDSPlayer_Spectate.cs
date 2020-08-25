﻿using System.Collections.Generic;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private ITDSPlayer? _spectates;

        #endregion Private Fields

        #region Public Properties

        public ITDSPlayer? Spectates
        {
            get => _spectates;
            set
            {
                _spectateHandler.SetPlayerToSpectatePlayer(this, value);
                _spectates = value;
            }
        }

        public HashSet<ITDSPlayer> Spectators { get; } = new HashSet<ITDSPlayer>();

        #endregion Public Properties
    }
}