﻿using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Lobby;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.GameModes
{
    partial class Normal : GameMode
    {
        public Normal(Arena lobby, MapDto map) : base(lobby, map) { }

        public static void Init(TDSNewContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
