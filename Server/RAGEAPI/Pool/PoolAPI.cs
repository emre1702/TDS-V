using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolAPI : IPoolAPI
    {
        #region Public Constructors

        public PoolAPI()
        {
            Blips = new PoolBlipsAPI();
            Checkpoints = new PoolCheckpointsAPI();
            Colshapes = new PoolColShapesAPI();
            DummyEntities = new PoolDummyEntitiesAPI();
            MapObjects = new PoolMapObjectsAPI();
            Markers = new PoolMarkersAPI();
            Peds = new PoolPedsAPI();
            Pickups = new PoolPickupsAPI();
            Players = new PoolPlayersAPI();
            TextLabels = new PoolTextLabelsAPI();
            Vehicles = new PoolVehiclesAPI();
        }

        #endregion Public Constructors

        #region Public Properties

        public IPoolBlipsAPI Blips { get; }
        public IPoolCheckpointsAPI Checkpoints { get; }
        public IPoolColShapesAPI Colshapes { get; }
        public IPoolDummyEntitiesAPI DummyEntities { get; }
        public IPoolMapObjectsAPI MapObjects { get; }
        public IPoolMarkersAPI Markers { get; }
        public IPoolPedsAPI Peds { get; }
        public IPoolPickupsAPI Pickups { get; }
        public IPoolPlayersAPI Players { get; }
        public IPoolTextLabelsAPI TextLabels { get; }
        public IPoolVehiclesAPI Vehicles { get; }

        #endregion Public Properties

        #region Public Methods

        public void RemoveAll()
        {
            NAPI.Task.Run(() =>
            {
                List<GTANetworkAPI.Blip> blips = NAPI.Pools.GetAllBlips();
                foreach (GTANetworkAPI.Blip blip in blips)
                    NAPI.Entity.DeleteEntity(blip);

                List<GTANetworkAPI.Checkpoint> checkpoints = NAPI.Pools.GetAllCheckpoints();
                foreach (GTANetworkAPI.Checkpoint checkpoint in checkpoints)
                    NAPI.Entity.DeleteEntity(checkpoint);

                List<GTANetworkAPI.ColShape> colShapes = NAPI.Pools.GetAllColShapes();
                foreach (GTANetworkAPI.ColShape colShape in colShapes)
                    NAPI.Entity.DeleteEntity(colShape);

                List<GTANetworkAPI.DummyEntity> dummyEntities = NAPI.Pools.GetAllDummyEntities();
                foreach (GTANetworkAPI.DummyEntity dummyEntitie in dummyEntities)
                    NAPI.Entity.DeleteEntity(dummyEntitie);

                List<GTANetworkAPI.Marker> markers = NAPI.Pools.GetAllMarkers();
                foreach (GTANetworkAPI.Marker marker in markers)
                    NAPI.Entity.DeleteEntity(marker);

                List<GTANetworkAPI.Ped> peds = NAPI.Pools.GetAllPeds();
                foreach (GTANetworkAPI.Ped ped in peds)
                    NAPI.Entity.DeleteEntity(ped);

                List<GTANetworkAPI.Pickup> pickups = NAPI.Pools.GetAllPickups();
                foreach (GTANetworkAPI.Pickup pickup in pickups)
                    NAPI.Entity.DeleteEntity(pickup);

                List<GTANetworkAPI.TextLabel> textLabels = NAPI.Pools.GetAllTextLabels();
                foreach (GTANetworkAPI.TextLabel textLabel in textLabels)
                    NAPI.Entity.DeleteEntity(textLabel);

                List<GTANetworkAPI.Vehicle> vehicles = NAPI.Pools.GetAllVehicles();
                foreach (GTANetworkAPI.Vehicle vehicle in vehicles)
                    NAPI.Entity.DeleteEntity(vehicle);

                List<GTANetworkAPI.Object> objects = NAPI.Pools.GetAllObjects();
                foreach (GTANetworkAPI.Object obj in objects)
                    NAPI.Entity.DeleteEntity(obj);
            });
        }

        #endregion Public Methods
    }
}
