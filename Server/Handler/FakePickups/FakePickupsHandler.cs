using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.FakePickup;
using TDS.Shared.Default;

namespace TDS.Server.Handler.FakePickups
{
    public class FakePickupsHandler
    {
        private readonly List<FakePickup> _pickups = new List<FakePickup>();

        public FakePickupsHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerJoinedLobby += EventsHandler_PlayerJoinedLobby;
        }

        public FakePickup Create(int model, Vector3 position, float pickupRange, uint respawnTime, IBaseLobby lobby)
            => new FakePickup(model, position, pickupRange, respawnTime, lobby, this);

        public FakePickup Create(string name, Vector3 position, float pickupRange, uint respawnTime, IBaseLobby lobby)
            => Create((int)NAPI.Util.GetHashKey(name), position, pickupRange, respawnTime, lobby);

        public void Add(FakePickup fakePickup)
        {
            lock (_pickups)
            {
                _pickups.Add(fakePickup);
            }

            var dataJson = Serializer.ToClient(new FakePickupSyncData
            {
                RemoteId = fakePickup.RemoteId!.Value,
                LightDataJson = fakePickup.LightDataJson
            });
            fakePickup.Lobby.Sync.TriggerEvent(ToClientEvent.SpawnFakePickup, dataJson);
        }

        public void Remove(FakePickup fakePickup)
        {
            lock (_pickups)
            {
                _pickups.Remove(fakePickup);
            }
        }

        public void DeleteAll(IBaseLobby forLobby)
        {
            lock (_pickups)
            {
                foreach (var pickup in _pickups)
                    if (pickup.Lobby == forLobby)
                        pickup.Delete();
                _pickups.RemoveAll(p => p.Lobby == forLobby);
            }
        }

        public List<FakePickup> GetAll(IBaseLobby forLobby)
        {
            lock (_pickups)
            {
                return _pickups.Where(p => p.Lobby == forLobby).ToList();
            }
        }

        private void EventsHandler_PlayerJoinedLobby(ITDSPlayer player, IBaseLobby lobby)
        {
            var pickups = GetAll(lobby).Where(p => p.RemoteId.HasValue);
            if (!pickups.Any())
            {
                NAPI.Task.RunSafe(() =>
                player.TriggerEvent(ToClientEvent.SyncFakePickups));
                return;
            }

            var data = pickups.Select(p => new FakePickupSyncData { RemoteId = p.RemoteId!.Value, LightDataJson = p.LightDataJson });
            var dataJson = Serializer.ToClient(data);
            NAPI.Task.RunSafe(() => 
                player.TriggerEvent(ToClientEvent.SyncFakePickups, data));
        }
    }
}
