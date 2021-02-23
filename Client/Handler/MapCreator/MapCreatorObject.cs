using RAGE;
using RAGE.Elements;
using System;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Handler.Entities.GTA;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Data.Models.Map.Creator;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorObject
    {
        public Vector3 Position;
        public Vector3 Rotation;

        private readonly EventsHandler _eventsHandler;

        private Vector3 _movingPosition;
        private Vector3 _movingRotation;

        public MapCreatorObject(MapCreatorObjectsHandler mapCreatorObjectsHandler, EventsHandler eventsHandler, GameEntityBase entity, MapCreatorPositionType type,
            ushort ownerRemoteId, Vector3 pos, Vector3 rot, int? teamNumber = null, string objectName = null, int id = -1)
        {
            _eventsHandler = eventsHandler;

            Entity = entity;
            Type = type;
            OwnerRemoteId = ownerRemoteId;
            TeamNumber = teamNumber;
            ObjOrVehName = objectName;

            var a = new Vector3();
            var b = new Vector3(9999f, 9999f, 9999f);
            RAGE.Game.Misc.GetModelDimensions(Entity.Model, a, b);
            Size = b - a;

            Position = pos;
            Rotation = rot;

            if (id == -1)
            {
                Id = ++mapCreatorObjectsHandler.IdCounter;
                _eventsHandler.OnMapCreatorSyncLatestObjectID();
            }
            else
            {
                mapCreatorObjectsHandler.IdCounter = Math.Max(mapCreatorObjectsHandler.IdCounter, id);
                Id = id;
            }
            _eventsHandler.OnMapCreatorSyncLatestObjectID();

            Blip = CreateBlip();

            Entity.FreezePosition(true);
        }

        public ITDSBlip Blip { get; }
        public bool Deleted { get; private set; }
        public GameEntityBase Entity { get; }
        public int Id { get; set; }
        public bool IsSynced { get; set; }

        public Vector3 MovingPosition
        {
            get => _movingPosition;
            set
            {
                _movingPosition = value;
                EntityPosition = value;
                Blip.Position = value;
            }
        }

        public Vector3 MovingRotation
        {
            get => _movingRotation;
            set
            {
                _movingRotation = value;
                Entity.SetRotation(value.X, value.Y, value.Z, 2, true);
                Blip.SetRotation((int)value.Z);
            }
        }

        public Vector3 EntityPosition
        {
            get => RAGE.Game.Entity.GetEntityCoords(Entity.Handle, true);
            set
            {
                Entity.Position = value;
                RAGE.Game.Entity.SetEntityCoordsNoOffset(Entity.Handle, value.X, value.Y, value.Z, false, false, false);
            }
        }

        public string ObjOrVehName { get; }
        public ushort OwnerRemoteId { get; }
        public Vector3 Size { get; }
        public int? TeamNumber { get; }
        public MapCreatorPositionType Type { get; }

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
                Id = Id,
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
                Id = Id,

                PosX = Position.X,
                PosY = Position.Y,
                PosZ = Position.Z,

                RotX = Rotation.X,
                RotY = Rotation.Y,
                RotZ = Rotation.Z
            };
        }

        public bool IsMine()
            => Player.LocalPlayer.RemoteId == OwnerRemoteId;

        public void LoadEntityData()
        {
            Position = EntityPosition;
            _movingPosition = new Vector3(Position.X, Position.Y, Position.Z);
            Rotation = Entity.GetRotation(2);
            _movingRotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
        }

        public void LoadPos(MapCreatorPosition pos)
        {
            MovingPosition = new Vector3(pos.PosX, pos.PosY, pos.PosZ);
            Position = new Vector3(MovingPosition.X, MovingPosition.Y, MovingPosition.Z);

            MovingRotation = new Vector3(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = new Vector3(MovingRotation.X, MovingRotation.Y, MovingRotation.Z);
        }

        public void LoadPos(MapCreatorPosData pos)
        {
            MovingPosition = new Vector3(pos.PosX, pos.PosY, pos.PosZ);
            Position = new Vector3(MovingPosition.X, MovingPosition.Y, MovingPosition.Z);

            MovingRotation = new Vector3(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = new Vector3(MovingRotation.X, MovingRotation.Y, MovingRotation.Z);
        }

        public void ResetObjectPosition()
        {
            EntityPosition = Position;
            Entity.SetRotation(Rotation.X, Rotation.Y, Rotation.Z, 2, true);
            LoadEntityData();

            Blip.Position = Position;
            Blip.SetRotation((int)Rotation.Z);
        }

        public void SetCollision(bool toggle, bool keepPhysics)
        {
            Entity.SetCollision(toggle, keepPhysics);
        }

        private ITDSBlip CreateBlip()
        {
            var dimension = Player.LocalPlayer.Dimension;
            switch (Type)
            {
                case MapCreatorPositionType.TeamSpawn:
                    return new TDSBlip(SharedConstants.TeamSpawnBlipSprite, Position, name: Id.ToString(), dimension: dimension);

                case MapCreatorPositionType.MapLimit:
                    return new TDSBlip(SharedConstants.MapLimitBlipSprite, Position, name: Id.ToString(), dimension: dimension);

                case MapCreatorPositionType.BombPlantPlace:
                    return new TDSBlip(SharedConstants.BombPlantPlaceBlipSprite, Position, name: Id.ToString(), dimension: dimension);

                case MapCreatorPositionType.MapCenter:
                    return new TDSBlip(SharedConstants.MapCenterBlipSprite, Position, name: Id.ToString(), dimension: dimension);

                case MapCreatorPositionType.Target:
                    return new TDSBlip(SharedConstants.TargetBlipSprite, Position, name: Id.ToString(), dimension: dimension);

                case MapCreatorPositionType.Object:
                    return new TDSBlip(SharedConstants.ObjectBlipSprite, Position, name: Id.ToString(), dimension: dimension);

                case MapCreatorPositionType.Vehicle:
                    return new TDSBlip(SharedConstants.VehicleBlipSprite, Position, name: Id.ToString(), dimension: dimension);
            }

            return null;
        }
    }
}