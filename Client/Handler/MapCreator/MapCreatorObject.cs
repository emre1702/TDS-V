using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorObject
    {
        public int ID { get; set; }
        public MapCreatorPositionType Type { get; }
        public string ObjOrVehName { get; }
        public int? TeamNumber { get; }
        public bool Deleted { get; private set; }
        public IEntityBase Entity { get; }
        public IBlip Blip { get; }
        public Position3D Size { get; }
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
        public Position3D Position;
        public Position3D Rotation;
        public ushort OwnerRemoteId { get; }
        public bool IsSynced { get; set; }

        private Position3D _movingPosition;
        private Position3D _movingRotation;

        private readonly IModAPI _modAPI;
        private readonly EventsHandler _eventsHandler;

        public MapCreatorObject(IModAPI modAPI, MapCreatorObjectsHandler mapCreatorObjectsHandler, EventsHandler eventsHandler, IEntityBase entity, MapCreatorPositionType type, 
            ushort ownerRemoteId, int? teamNumber = null, string objectName = null, int id = -1)
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

            Position = entity.Position;

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

        public void LoadPos(MapCreatorPosition pos)
        {
            MovingPosition = new Position3D(pos.PosX, pos.PosY, pos.PosZ);
            Position = MovingPosition;

            MovingRotation = new Position3D(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = MovingRotation;
        }

        public void LoadPos(MapCreatorPosData pos)
        {
            MovingPosition = new Position3D(pos.PosX, pos.PosY, pos.PosZ);
            Position = MovingPosition;

            MovingRotation = new Position3D(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = MovingRotation;
        }

        public void LoadEntityData()
        {
            Position = Entity.Position;
            _movingPosition = new Position3D(Position.X, Position.Y, Position.Z);
            Rotation = Entity.Rotation;
            _movingRotation = new Position3D(Rotation.X, Rotation.Y, Rotation.Z);
        }

        public void ResetObjectPosition()
        {
            Entity.Position = Position;
            Entity.Rotation = Rotation;
            LoadEntityData();

            Blip.Position = Position;
            Blip.Rotation = Rotation;
        }

        public void ActivatePhysics()
        {
            Entity.ActivatePhysics();
        }

        public void Freeze(bool toggle)
        {
            Entity.FreezePosition(toggle);
        }

        public void Delete(bool syncToServer)
        {
            Entity.Destroy();
            Blip.Destroy();
            Deleted = true;
            if (syncToServer)
            {
                _eventsHandler.OnMapCreatorSyncObjectDeleted(this);
            }
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
    }
}
