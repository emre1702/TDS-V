﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS_Common.Enum;
using TDS_Server.Enums;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Instance.Player
{
    partial class TDSPlayer
    {
        private Players? _entity;
        private int _lastSaveTick;

        public Players? Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                if (_entity is null)
                    return;
                if (_langEnumBeforeLogin != ELanguage.English)
                    _entity.PlayerSettings.Language = _langEnumBeforeLogin;
                PlayerRelationsPlayer = _entity.PlayerRelationsPlayer.ToList();
                PlayerRelationsTarget = _entity.PlayerRelationsTarget.ToList();
                PlayerDataSync.SetData(this, EPlayerDataKey.Money, EPlayerDataSyncMode.Player, _entity.PlayerStats.Money);
                PlayerDataSync.SetData(this, EPlayerDataKey.AdminLevel, EPlayerDataSyncMode.All, _entity.AdminLvl);
                LoadTimezone();
            }
        }

        public async Task SaveData(bool force = false)
        {
            if (!force && (Entity is null || !Entity.PlayerStats.LoggedIn))
                return;

            _lastSaveTick = Environment.TickCount;
            await ExecuteForDBAsync(async (dbContext) =>
            {
                if (CurrentLobbyStats is { } && LobbyManager.GetLobby(CurrentLobbyStats.LobbyId) is null)
                {
                    dbContext.Entry(CurrentLobbyStats).State = EntityState.Detached;
                    CurrentLobbyStats = null;
                }
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async void CheckSaveData()
        {
            if (Environment.TickCount - _lastSaveTick < SettingsManager.SavePlayerDataCooldownMinutes * 60 * 1000)
                return;

            await SaveData().ConfigureAwait(false);
        }
    }
}
