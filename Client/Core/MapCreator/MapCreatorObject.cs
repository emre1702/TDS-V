using System;
using RAGE;
using RAGE.Elements;
using TDS_Client.Enum;
using TDS_Client.Manager.MapCreator;
using TDS_Shared.Dto.Map.Creator;
using TDS_Shared.Enum;
using TDS_Shared.Core;

namespace TDS_Client.Instance.MapCreator
{
    class MapCreatorObject
    {
        public int ID { get; set; }
        public MapCreatorPositionType Type { get; } 
        public string ObjOrVehName { get; }
        public int? TeamNumber { get; }
        public bool Deleted { get; private set; }
        public GameEntityBase Entity { get; }
        public Blip Blip { get; }
        public Vector3 Size { get; }
        public Vector3 MovingPosition 
        { 
            get => _movingPosition; 
            set 
            { 
                _movingPosition = value;
                RAGE.Game.Entity.SetEntityCoordsNoOffset(Entity.Handle, value.X, value.Y, value.Z, true, false, false);
                Entity.Position = _movingPosition;
                Blip.Position = _movingPosition;
            }
        }
        public Vector3 MovingRotation
        {
            get => _movingRotation;
            set
            {
                _movingRotation = value;
                RAGE.Game.Entity.SetEntityRotation(Entity.Handle, value.X, value.Y, value.Z, 2, false);
                Blip.SetRotation((int)Math.Ceiling(value.Z));
            }
        }
        public Vector3 Position;
        public Vector3 Rotation;
        public ushort OwnerRemoteId { get; }
        public bool IsSynced { get; set; }

        private Vector3 _movingPosition;
        private Vector3 _movingRotation;

        public static int IdCounter = 0;

        public MapCreatorObject(GameEntityBase entity, MapCreatorPositionType type, ushort ownerRemoteId, int? teamNumber = null, string objectName = null, int id = -1)
        {
            Entity = entity;
            Type = type;
            OwnerRemoteId = ownerRemoteId;
            TeamNumber = teamNumber;
            ObjOrVehName = objectName;

            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
            RAGE.Game.Misc.GetModelDimensions(Entity.Model, a, b);
            Size = b - a;

            Position = entity.Position;

            if (id == -1)
            {
                ID = ++IdCounter;
                Sync.SyncLatestIdToServer();
            } 
            else
            {
                IdCounter = Math.Max(IdCounter, id);
                ID = id;
            }
            Sync.SyncLatestIdToServer();

            Blip = CreateBlip();

            Entity.FreezePosition(true);
        }

        public void LoadPos(MapCreatorPosition pos)
        {
            MovingPosition = new Vector3(pos.PosX, pos.PosY, pos.PosZ);
            Position = MovingPosition;

            MovingRotation = new Vector3(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = MovingRotation;
        }

        public void LoadPos(MapCreatorPosData pos)
        {
            MovingPosition = new Vector3(pos.PosX, pos.PosY, pos.PosZ);
            Position = MovingPosition;

            MovingRotation = new Vector3(pos.RotX, pos.RotY, pos.RotZ);
            Rotation = MovingRotation;
        }

        public void LoadEntityData()
        {
            Position = RAGE.Game.Entity.GetEntityCoords(Entity.Handle, true);
            _movingPosition = new Vector3(Position.X, Position.Y, Position.Z);
            Rotation = RAGE.Game.Entity.GetEntityRotation(Entity.Handle, 2);
            _movingRotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
        }

        public void ResetObjectPosition()
        {
            RAGE.Game.Entity.SetEntityCoordsNoOffset(Entity.Handle, Position.X, Position.Y, Position.Z, true, false, false);
            RAGE.Game.Entity.SetEntityRotation(Entity.Handle, Rotation.X, Rotation.Y, Rotation.Z, 2, false);
            LoadEntityData();

            Blip.Position = Position;
            Blip.SetRotation((int)Math.Ceiling(Rotation.Z));
        }

        public void ActivatePhysics()
        {
            RAGE.Game.Physics.ActivatePhysics(Entity.Handle);
        }

        public void Freeze(bool toggle)
        {
            RAGE.Game.Entity.FreezeEntityPosition(Entity.Handle, toggle);
        }

        public void Delete(bool syncToServer)
        {
            Entity.Destroy();
            Blip.Destroy();
            Deleted = true;
            if (syncToServer)
            {
                Sync.SyncObjectRemoveToLobby(this);
            }
        }

        public static void Reset()
        {
            IdCounter = 0;
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

        public static MapCreatorObject FromDto(MapCreatorPosition dto)
        {
            var obj = ObjectsManager.CreateMapCreatorObject(dto.Type, dto.Info, dto.OwnerRemoteId);

            obj.LoadPos(dto);

            return obj;
        }

        private Blip CreateBlip()
        {
            switch (Type)
            {
                case MapCreatorPositionType.TeamSpawn:
                    return new Blip(SharedConstants.TeamSpawnBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case MapCreatorPositionType.MapLimit:
                    return new Blip(SharedConstants.MapLimitBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case MapCreatorPositionType.BombPlantPlace:
                    return new Blip(SharedConstants.BombPlantPlaceBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case MapCreatorPositionType.MapCenter:
                    return new Blip(SharedConstants.MapCenterBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case MapCreatorPositionType.Target:
                    return new Blip(SharedConstants.TargetBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case MapCreatorPositionType.Object:
                    return new Blip(SharedConstants.ObjectBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case MapCreatorPositionType.Vehicle:
                    return new Blip(SharedConstants.VehicleBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);
            }

            return null;
        }
    }
}
