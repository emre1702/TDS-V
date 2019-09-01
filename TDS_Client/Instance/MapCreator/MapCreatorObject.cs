using System;
using RAGE;
using RAGE.Elements;

namespace TDS_Client.Instance.MapCreator
{
    class MapCreatorObject
    {
        public int ID { get; }
        public GameEntity Entity { get; }
        public Vector3 Size { get; }
        public Vector3 MovingPosition 
        { 
            get => _movingPosition; 
            set 
            { 
                _movingPosition = value; 
                Entity.Position = _movingPosition;
            }
        }
        public Vector3 MovingRotation
        {
            get => _movingRotation;
            set
            {
                _movingRotation = value;
                RAGE.Game.Entity.SetEntityRotation(Entity.Handle, value.X, value.Y, value.Z, 2, false);
            }
        }
        public Vector3 Position;
        public Vector3 Rotation;

        private Vector3 _movingPosition;
        private Vector3 _movingRotation;

        private static int _idCounter = 0;

        public MapCreatorObject(GameEntity entity)
        {
            Entity = entity;

            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
            RAGE.Game.Misc.GetModelDimensions(Entity.Model, a, b);
            Size = b - a;

            Position = entity.Position;

            ID = ++_idCounter;
        }

        public void LoadEntityData()
        {
            Position = Entity.Position;
            _movingPosition = new Vector3(Position.X, Position.Y, Position.Z);
            Rotation = RAGE.Game.Entity.GetEntityRotation(Entity.Handle, 2);
            _movingRotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
        }

        public void ResetObjectPosition()
        {
            Entity.Position = Position;
            RAGE.Game.Entity.SetEntityRotation(Entity.Handle, Rotation.X, Rotation.Y, Rotation.Z, 2, false);
            LoadEntityData();
        }

        public void Delete()
        {
            Entity.Destroy();
        }

        public static void Reset()
        {
            _idCounter = 0;
        }
    }
}
