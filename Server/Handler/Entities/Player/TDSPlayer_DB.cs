using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
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
                Language = _langHelper.GetLang(_entity.PlayerSettings.Language);
                PlayerRelationsPlayer = _entity.PlayerRelationsPlayer.ToList();
                PlayerRelationsTarget = _entity.PlayerRelationsTarget.ToList();
                _dataSyncHandler.SetData(this, PlayerDataKey.Money, PlayerDataSyncMode.Player, _entity.PlayerStats.Money);
                _dataSyncHandler.SetData(this, PlayerDataKey.AdminLevel, PlayerDataSyncMode.All, _entity.AdminLvl);
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
                if (LobbyStats is { } && _lobbyHandler.GetLobby(LobbyStats.LobbyId) is null)
                {
                    dbContext.Entry(LobbyStats).State = EntityState.Detached;
                    LobbyStats = null;
                }
                await dbContext.SaveChangesAsync();
            }).ConfigureAwait(false);
        }

        public async void CheckSaveData()
        {
            if (Environment.TickCount - _lastSaveTick < _settingsHandler.ServerSettings.SavePlayerDataCooldownMinutes * 60 * 1000)
                return;

            await SaveData().ConfigureAwait(false);
        }
    }
}
