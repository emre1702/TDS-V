﻿using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.DamageSystem.Deaths
{
    internal class DeathSyncHandler
    {
        internal void Sync(ITDSPlayer died, ITDSPlayer killer, uint weapon, int diedPlayerLifes)
        {
            if (died.Lobby is null)
                return;

            var deathInfoData = new DeathInfoData
            {
               PlayerName = died.DisplayName,
               KillerName = died == killer ? null : killer.DisplayName,
               WeaponHash = weapon
            };
            var json = Serializer.ToBrowser(deathInfoData);
            died.Lobby.Sync.TriggerEvent(ToClientEvent.Death, died.RemoteId, died.TeamIndex, json, diedPlayerLifes > 1);
        }
    }
}