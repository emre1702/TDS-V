using RAGE.Game;
using System.Collections.Generic;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Shared.Core;
using TDS_Shared.Data.Models.FakePickup;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler.FakePickups
{
    public class FakePickupsHandler
    {
        private const float _rotationAmount = 90f;

        private readonly Dictionary<ushort, FakePickupSyncData> _fakePickupsData = new Dictionary<ushort, FakePickupSyncData>();
        private readonly List<ITDSObject> _streamedFakePickups = new List<ITDSObject>();

        public FakePickupsHandler()
        {
            Add(ToClientEvent.SyncFakePickups, SyncFakePickups);
            Add(ToClientEvent.SpawnFakePickup, SpawnFakePickup);
            Add(ToClientEvent.SyncFakePickupLightData, SyncFakePickupLightData);

            Tick += OnTick;
            RAGE.Events.OnEntityStreamIn += OnEntityStreamIn; 
            RAGE.Events.OnEntityStreamOut += OnEntityStreamOut;
            RAGE.Events.OnEntityDestroyed += OnEntityDestroyed;
        }

        private void UpdateProp(ITDSObject obj)
        {
            if (obj is null)
                return;

            obj.NotifyStreaming = true;
            if (obj.Exists)
                _streamedFakePickups.Add(obj);
        }

        private void SyncFakePickups(object[] args)
        {
            ClearFakePickups();
            if (args is null || args.Length == 0)
                return;

            var datas = Serializer.FromServer<List<FakePickupSyncData>>((string)args[0]);
            foreach (var fakePickupData in datas)
                NewFakePickup(fakePickupData);
        }

        private void SpawnFakePickup(object[] args)
        {
            var fakePickupData = Serializer.FromServer<FakePickupSyncData>((string)args[0]);
            NewFakePickup(fakePickupData);
        }

        private void NewFakePickup(FakePickupSyncData fakePickupData)
        {
            var obj = RAGE.Elements.Entities.Objects.GetAtRemote(fakePickupData.RemoteId);
            UpdateProp(obj as ITDSObject);
            _fakePickupsData[fakePickupData.RemoteId] = fakePickupData;

            if (!string.IsNullOrEmpty(fakePickupData.LightDataJson))
                fakePickupData.LightData = Serializer.FromServer<FakePickupLightData>(fakePickupData.LightDataJson);
        }

        private void SyncFakePickupLightData(object[] args)
        {
            var remoteId = ushort.Parse(args[0].ToString());
            if (args.Length == 1)
                ClearFakePickupLightData(remoteId);
            else 
                SetFakePickupLightData(remoteId, args[1].ToString());
        }

        private void ClearFakePickupLightData(ushort remoteId)
        {
            if (!_fakePickupsData.TryGetValue(remoteId, out var fakePickupData))
                return;
            fakePickupData.LightData = null;
            fakePickupData.LightDataJson = null;
        }

        private void SetFakePickupLightData(ushort remoteId, string lightDataJson)
        {
            var lightData = Serializer.FromServer<FakePickupLightData>(lightDataJson);
            if (!_fakePickupsData.TryGetValue(remoteId, out var fakePickupData))
            {
                fakePickupData = new FakePickupSyncData();
                _fakePickupsData[remoteId] = fakePickupData;
            }
            fakePickupData.LightData = lightData;
            fakePickupData.LightDataJson = lightDataJson;
        }

        private void ClearFakePickups()
        {
            _streamedFakePickups.Clear();
            _fakePickupsData.Clear();
        }

        private void OnTick(List<TickNametagData> _)
        {
            var frameTime = Invoker.Invoke<float>(Natives.Timestep);

            foreach (var streamedFakePickup in _streamedFakePickups)
            {
                if (!_fakePickupsData.TryGetValue(streamedFakePickup.RemoteId, out var pickupData))
                    continue;

                ApplyRotation(streamedFakePickup, frameTime);
                if (!(pickupData.LightData is null))
                    ApplyLight(streamedFakePickup, pickupData.LightData);
            }
        }

        private void ApplyRotation(ITDSObject fakePickup, float frameTime)
        {
            var rotation = fakePickup.GetRotation(2);
            fakePickup.SetRotation(rotation.X, rotation.Y, rotation.Z + (_rotationAmount * frameTime), 2, true);
        }

        private void ApplyLight(ITDSObject fakePickup, FakePickupLightData lightData)
        {
            var pos = fakePickup.Position;
            Graphics.DrawLightWithRangeAndShadow(pos.X, pos.Y, pos.Z, lightData.Red, lightData.Green, lightData.Blue, lightData.Range, lightData.Intensity, lightData.Shadow);
        }

        private void OnEntityStreamIn(RAGE.Elements.Entity entity)
        {
            if (!(entity is ITDSObject obj))
                return;
            if (!_fakePickupsData.ContainsKey(obj.RemoteId))
                return;

            obj.SetCollision(false, false);
            if (!_streamedFakePickups.Contains(obj))
                _streamedFakePickups.Add(obj);
        }

        private void OnEntityStreamOut(RAGE.Elements.Entity entity)
        {
            if (!(entity is ITDSObject obj))
                return;

            _streamedFakePickups.Remove(obj);
        }

        private void OnEntityDestroyed(RAGE.Elements.Entity entity)
        {
            if (!(entity is ITDSObject obj))
                return;

            _streamedFakePickups.Remove(obj);
            _fakePickupsData.Remove(obj.RemoteId);
        }
    }
}
