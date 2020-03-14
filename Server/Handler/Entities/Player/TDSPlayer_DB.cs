using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS_Shared.Enum;
using TDS_Server.Enums;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.Player
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
                if (_langEnumBeforeLogin != TDS_Shared.Data.Enums.Language.English)
                    _entity.PlayerSettings.Language = _langEnumBeforeLogin;
                Language = LangUtils.GetLang(_entity.PlayerSettings.Language);
                PlayerRelationsPlayer = _entity.PlayerRelationsPlayer.ToList();
                PlayerRelationsTarget = _entity.PlayerRelationsTarget.ToList();
                PlayerDataSync.SetData(this, PlayerDataKey.Money, PlayerDataSyncMode.Player, _entity.PlayerStats.Money);
                PlayerDataSync.SetData(this, PlayerDataKey.AdminLevel, PlayerDataSyncMode.All, _entity.AdminLvl);
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
                await dbContext.SaveChangesAsync();
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
