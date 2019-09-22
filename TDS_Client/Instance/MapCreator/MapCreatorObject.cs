using System;
using RAGE;
using RAGE.Elements;
using TDS_Client.Enum;
using TDS_Common.Dto.Map.Creator;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;

namespace TDS_Client.Instance.MapCreator
{
    class MapCreatorObject
    {
        public int ID { get; }
        public EMapCreatorPositionType Type { get; } 
        public string ObjectName { get; }
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

        private Vector3 _movingPosition;
        private Vector3 _movingRotation;

        private static int _idCounter = 0;

        public MapCreatorObject(GameEntityBase entity, EMapCreatorPositionType type, int? teamNumber = null, string objectName = null, int id = -1)
        {
            Entity = entity;
            Type = type;
            TeamNumber = teamNumber;
            ObjectName = objectName;

            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
            RAGE.Game.Misc.GetModelDimensions(Entity.Model, a, b);
            Size = b - a;

            Position = entity.Position;

            if (id == -1)
                ID = ++_idCounter;
            else
            {
                _idCounter = Math.Max(ID, id);
                ID = id;
            }

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

        public void LoadEntityData()
        {
            Position = RAGE.Game.Entity.GetEntityCoords(Entity.Handle, true);
            _movingPosition = new Vector3(Position.X, Position.Y, Position.Z);
            Rotation = RAGE.Game.Entity.GetEntityRotation(Entity.Handle, 2);
            _movingRotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
        }

        public void ResetObjectPosition()
        {
            Entity.Position = Position;
            RAGE.Game.Entity.SetEntityRotation(Entity.Handle, Rotation.X, Rotation.Y, Rotation.Z, 2, false);
            LoadEntityData();

            Blip.Position = Position;
            Blip.SetRotation((int)Math.Ceiling(Rotation.Z));
        }

        public void ActivatePhysics()
        {
            RAGE.Game.Physics.ActivatePhysics(Entity.Handle);
        }

        public void Delete()
        {
            Entity.Destroy();
            Blip.Destroy();
            Deleted = true;
        }

        public static void Reset()
        {
            _idCounter = 0;
        }

        private Blip CreateBlip()
        {
            switch (Type)
            {
                case EMapCreatorPositionType.TeamSpawn:
                    return new Blip(Constants.TeamSpawnBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case EMapCreatorPositionType.MapLimit:
                    return new Blip(Constants.MapLimitBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case EMapCreatorPositionType.BombPlantPlace:
                    return new Blip(Constants.BombPlantPlaceBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case EMapCreatorPositionType.MapCenter:
                    return new Blip(Constants.MapCenterBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);

                case EMapCreatorPositionType.Object:
                    return new Blip(Constants.ObjectBlipSprite, Position, name: ID.ToString(), dimension: Player.LocalPlayer.Dimension);
            }

            return null;
        }
    }
}
