using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.FakePickup;
using TDS.Shared.Default;

namespace TDS.Server.Handler.FakePickups
{
    public class FakePickup
    {
        public ushort? RemoteId => _prop?.RemoteId;
        public string? LightDataJson { get; private set; }
        public Action<ITDSPlayer, CancelEventArgs>? OnCollect { get; set; }

        private ITDSObject? _prop;
        private ITDSColshape? _colshape;
        private FakePickupLightData? _lightData;

        private bool _isCollected;
        private bool _isDestroyed;
        private TDSTimer? _respawnTimer;

        private readonly int _model;
        private readonly Vector3 _position;
        private readonly float _pickupRange;
        private readonly uint? _respawnTime;
        public IBaseLobby Lobby { get; }
        

        private readonly FakePickupsHandler _fakePickupsHandler;

        public FakePickup(int model, Vector3 position, float pickupRange, uint? respawnTime, IBaseLobby lobby, FakePickupsHandler fakePickupsHandler)
        {
            _model = model;
            _position = position;
            _pickupRange = pickupRange;
            _respawnTime = respawnTime;
            Lobby = lobby;
            _fakePickupsHandler = fakePickupsHandler;

            CreateProp();
            CreateColshape();

            _fakePickupsHandler.Add(this);
        }

        public void SetLightData(byte red, byte green, byte blue, float range, float intensity, float shadow)
        {
            _lightData = new FakePickupLightData 
            { 
                Red = red, 
                Green = green, 
                Blue = blue, 
                Range = range, 
                Intensity = intensity, 
                Shadow = shadow
            };
            LightDataJson = Serializer.ToClient(_lightData);

            if (_prop?.RemoteId != null)
                Lobby.Sync.TriggerEvent(ToClientEvent.SyncFakePickupLightData, _prop.RemoteId, LightDataJson);
        }

        public void ResetLightData()
        {
            _lightData = null;
            LightDataJson = null;
            if (_prop?.RemoteId is { } remoteId)
                Lobby.Sync.TriggerEvent(ToClientEvent.SyncFakePickupLightData, remoteId);
        }

        public void Respawn()
        {
            if (!_isCollected || _isDestroyed)
                return;

            RemoveProp();
            RemoveTimer();
            
            _isCollected = false;
            CreateProp();
        }

        public void Delete()
        {
            _isDestroyed = true;

            RemoveProp();
            RemoveTimer();
            RemoveColshape();

            _fakePickupsHandler.Remove(this);
        }

        private void CreateProp()
        {
            if (_prop is { })
                return;
            NAPI.Task.RunSafe(() =>
            {
                _prop = (ITDSObject)NAPI.Object.CreateObject(_model, _position, new Vector3(), dimension: Lobby.MapHandler.Dimension);
            });
        }

        private void RemoveProp()
        {
            if (_prop is null)
                return;

            var prop = _prop;
            _prop = null;
            NAPI.Task.RunSafe(() =>
            {
                prop.Delete();
            });
        }

        private void CreateColshape()
        {
            if (_colshape is { })
                return;

            NAPI.Task.RunSafe(() =>
            {
                _colshape = (ITDSColshape)NAPI.ColShape.CreateSphereColShape(_position, _pickupRange, Lobby.MapHandler.Dimension);
                _colshape.PlayerEntered += SetCollected;
            });
        }

        private void RemoveColshape()
        {
            if (_colshape is null)
                return;

            var colshape = _colshape;
            _colshape = null;
            NAPI.Task.RunSafe(() =>
            {
                colshape.Delete();
            });
        }

        private void SetTimer()
        {
            if (_respawnTimer is { })
                _respawnTimer.Kill();

            if (_respawnTime.HasValue)
                _respawnTimer = new TDSTimer(Respawn, _respawnTime.Value);
            else
                _respawnTimer = null;
        }

        private void RemoveTimer()
        {
            if (_respawnTimer is null)
                return;
            _respawnTimer.Kill();
            _respawnTimer = null;
        }

        private void SetCollected(ITDSPlayer collector)
        {
            if (_isCollected || _isDestroyed)
                return;

            var cancel = new CancelEventArgs();
            OnCollect?.Invoke(collector, cancel);
            
            if (cancel.Cancel)
                return;

            _isCollected = true;

            RemoveProp();
            SetTimer();
        }
    }
}
