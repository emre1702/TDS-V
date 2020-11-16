using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.PlayersSystem
{
    public class Sync : IPlayerSync
    {
        private readonly DataSyncHandler _dataSyncHandler;

#nullable disable
        private ITDSPlayer _player;
        private IPlayerEvents _events;
#nullable enable

        public Sync(DataSyncHandler dataSyncHandler)
        {
            _dataSyncHandler = dataSyncHandler;
        }

        public void Init(ITDSPlayer player, IPlayerEvents events)
        {
            _player = player;
            _events = events;

            events.EntityChanged += Events_EntityChanged;
            events.Removed += Events_Removed;
        }

        private void Events_Removed()
        {
            _events.EntityChanged -= Events_EntityChanged;
            _events.Removed -= Events_Removed;
        }

        private void Events_EntityChanged(Database.Entity.Player.Players? entity)
        {
            if (entity is null)
                return;

            NAPI.Task.RunSafe(() =>
            {
                _dataSyncHandler.SetData(_player, PlayerDataKey.Money, DataSyncMode.Player, entity.PlayerStats.Money);
                _dataSyncHandler.SetData(_player, PlayerDataKey.AdminLevel, DataSyncMode.All, entity.AdminLvl);
            });
        }

        public void TriggerBrowserEvent(params object[] eventNameAndArgs)
        {
            NAPI.Task.RunSafe(() => 
                _player.TriggerEvent(ToClientEvent.ToBrowserEvent, eventNameAndArgs));
        }
    }
}
