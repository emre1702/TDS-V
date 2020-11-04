using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.PlayersSystem
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
            _player.TriggerEvent(ToClientEvent.ToBrowserEvent, eventNameAndArgs);
        }
    }
}
