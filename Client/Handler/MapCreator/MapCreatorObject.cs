using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorObject
    {
        #region Public Fields

        public Position3D Position;
        public Position3D Rotation;

        #endregion Public Fields

        #region Private Fields

        private readonly EventsHandler _eventsHandler;
        private readonly IModAPI _modAPI;
        private Position3D _movingPosition;
        private Position3D _movingRotation;

        #endregion Private Fields

        #region Public Constructors

        public MapCreatorObject(IModAPI modAPI, MapCreatorObjectsHandler mapCreatorObjectsHandler, EventsHandler eventsHandler, IEntityBase entity, MapCreatorPositionType type,
            ushort ownerRemoteId, Position3D pos, Position3D rot, int? teamNumber = null, string objectName = null, int id = -1)
        {
            _modAPI = modAPI;
            _eventsHandler = eventsHandler;

            Entity = entity;
            Type = type;
            OwnerRemoteId = ownerRemoteId;
            TeamNumber = teamNumber;
            ObjOrVehName = objectName;

            Position3D a = new Position3D();
            Position3D b = new Position3D();
            Entity.GetModelDimensions(a, b);
            Size = b - a;

            Position = pos;
            Rotation = rot;

            if (id == -1)
            {
                ID = ++mapCreatorObjectsHandler.IdCounter;
                _eventsHandler.OnMapCreatorSyncLatestObjectID();
            }
            else
            {
                mapCreatorObjectsHandler.IdCounter = Math.Max(mapCreatorObjectsHandler.IdCounter, id);
                ID = id;
            }
            _eventsHandler.OnMapCreatorSyncLatestObjectID();

            Blip = CreateBlip();

            Entity.FreezePosition(true);
        }

        #endregion Public Constructors

        #region Public Properties

        public IBlip Blip { get; }
        public bool Deleted { get; private set; }
        public IEntityBase Entity { get; }
        public int ID { get; set; }
        public bool IsSynced { get; set; }

        public Position3D MovingPosition
        {
            get => _movingPosition;
            set
            {
                _movingPosition = value;
                Entity.Position = value;
                Blip.Position = value;
            }
        }

        public Position3D MovingRotation
        {
            get => _movingRotation;
            set
            {
                _movingRotation = value;
                Entity.Rotation = value;
                Blip.Rotation = value;
            }
        }

        public string ObjOrVehName { get; }
        public ushort OwnerRemoteId { get; }
        public Position3D Size { get; }
        public int? TeamNumber { get; }
        public MapCreatorPositionType Type { get; }

        #endregion Public Properties

        #region Public Methods

        public void ActivatePhysics()
        {
            Entity.ActivatePhysics();
        }

        public void Delete(bool syncToServer)
        {
            Entity.Destroy();
            Blip.Destroy();
            Deleted = true;
            _eventsHandler.OnMapCreatorObjectDeleted();
            if (syncToServer)
            {
                _eventsHandler.OnMapCreatorSyncObjectDeleted(this);
            }
        }

        public void Freeze(bool toggle)
        {
            Entity.FreezePosition(toggle);
        }

        public MapCreatorPosition GetDto()
        {
            return new MapCreatorPosition
            {
                Id = ID,
                Info = (object)TeamNumber ?? ObjOrVehName,
                OwnerRemoteId = OwnerRemoteId,
                PosX = Position.X,
                PosY = Position.Y,
                PosZ = Position.Z,
                RotX = Rotation.X,
                RotY = Rotation.Y,
                RotZ = Rotation.Z,
                Type = Type
            };
        }

        public MapCreatorPosData GetPosDto()
        {
            return new MapCreatorPosData
            {
                Id = ID,

                PosX = Position.X,
                PosY = Position.Y,
                PosZ = Position.Z,

                RotX = Rotation.X,
                RotY = Rotation.Y,
                RotZ = Rotation.Z
            };
        }

        public bool IsMine()
            => _modAPI.LocalPlayer.RemoteId == OwnerRemoteId;

        public void LoadEntityData()
        {
            Position = Entity.Position;
            _movingPosition = new Position3D(Position.X, Position.Y, Position.Z);
            Rotation = Entity.Rotation;
            _movingRotation = new Position3D(Rotation.X, Rotation.Y, Rotation.Z);
        }

        public void LoadPos(MapCreatorPosition pos)
        {
            MovingPosition = new Position3D(pos.PosX, pos.PosY, pos.PosZ);
            Position = new Position3D(MovingPosition);

            MovingRotation = new Position3D(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = new Position3D(MovingRotation);
        }

        public void LoadPos(MapCreatorPosData pos)
        {
            MovingPosition = new Position3D(pos.PosX, pos.PosY, pos.PosZ);
            Position = new Position3D(MovingPosition);

            MovingRotation = new Position3D(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = new Position3D(MovingRotation);
        }

        public void ResetObjectPosition()
        {
            Entity.Position = Position;
            Entity.Rotation = Rotation;
            LoadEntityData();

            Blip.Position = Position;
            Blip.Rotation = Rotation;
        }

        public void SetCollision(bool toggle, bool keepPhysics)
        {
            Entity.SetCollision(toggle, keepPhysics);
        }

        #endregion Public Methods

        #region Private Methods

        private IBlip CreateBlip()
        {
            switch (Type)
            {
                case MapCreatorPositionType.TeamSpawn:
                    return _modAPI.Blip.Create(SharedConstants.TeamSpawnBlipSprite, Position, name: ID.ToString(), dimension: _modAPI.LocalPlayer.Dimension);

                case MapCreatorPositionType.MapLimit:
                    return _modAPI.Blip.Create(SharedConstants.MapLimitBlipSprite, Position, name: ID.ToString(), dimension: _modAPI.LocalPlayer.Dimension);

                case MapCreatorPositionType.BombPlantPlace:
                    return _modAPI.Blip.Create(SharedConstants.BombPlantPlaceBlipSprite, Position, name: ID.ToString(), dimension: _modAPI.LocalPlayer.Dimension);

                case MapCreatorPositionType.MapCenter:
                    return _modAPI.Blip.Create(SharedConstants.MapCenterBlipSprite, Position, name: ID.ToString(), dimension: _modAPI.LocalPlayer.Dimension);

                case MapCreatorPositionType.Target:
                    return _modAPI.Blip.Create(SharedConstants.TargetBlipSprite, Position, name: ID.ToString(), dimension: _modAPI.LocalPlayer.Dimension);

                case MapCreatorPositionType.Object:
                    return _modAPI.Blip.Create(SharedConstants.ObjectBlipSprite, Position, name: ID.ToString(), dimension: _modAPI.LocalPlayer.Dimension);

                case MapCreatorPositionType.Vehicle:
                    return _modAPI.Blip.Create(SharedConstants.VehicleBlipSprite, Position, name: ID.ToString(), dimension: _modAPI.LocalPlayer.Dimension);
            }

            return null;
        }

        #endregion Private Methods
    }
}
