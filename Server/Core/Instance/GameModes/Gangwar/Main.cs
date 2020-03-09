﻿using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Core.Instance.GameModes.Gangwar
{
    partial class Gangwar : GameMode
    {
        private readonly GangwarArea? _gangwarArea;

        public Gangwar(Arena lobby, MapDto map) : base(lobby, map) 
        { 
            var gangwarArea = GangwarAreasManager.GetById(map.BrowserSyncedData.Id);  
            if (gangwarArea is null)
            {
                /*lobby.SetRoundStatus(Enums.ERoundStatus.RoundEnd, Enums.ERoundEndReason.Error);
                return;*/
                // Create dummy gangwar area
                gangwarArea = new GangwarArea(map);
            }
            else if (!lobby.IsGangActionLobby)
            {
                gangwarArea = new GangwarArea(gangwarArea);
            }
            _gangwarArea = gangwarArea;
        }

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}