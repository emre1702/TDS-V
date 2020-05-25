namespace TDS_Server.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolAPI
    {
        #region Public Properties

        IPoolBlipsAPI Blips { get; }
        IPoolCheckpointsAPI Checkpoints { get; }
        IPoolColShapesAPI Colshapes { get; }
        IPoolDummyEntitiesAPI DummyEntities { get; }
        IPoolMapObjectsAPI MapObjects { get; }
        IPoolMarkersAPI Markers { get; }
        IPoolPedsAPI Peds { get; }
        IPoolPickupsAPI Pickups { get; }
        IPoolPlayersAPI Players { get; }
        IPoolTextLabelsAPI TextLabels { get; }
        IPoolVehiclesAPI Vehicles { get; }

        #endregion Public Properties

        #region Public Methods

        void RemoveAll();

        #endregion Public Methods
    }
}
