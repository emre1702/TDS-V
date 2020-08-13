﻿using RAGE;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Blip
{
    internal class Blip : RAGE.Elements.Blip, IBlip
    {
        #region Public Constructors

        public Blip(ushort id, ushort remoteId) : base(id, remoteId)
        { }

        public Blip(uint sprite, Vector3 position, string name = "", float scale = 1, int color = 0, int alpha = 255, float drawDistance = 0,
            bool shortRange = false, int rotation = 0, float radius = 0, uint dimension = 0)
            : base(sprite, position, name, scale, color, alpha, drawDistance, shortRange, rotation, radius, dimension)
        { }

        #endregion Public Constructors

        #region Public Properties

        public int Alpha
        {
            get => GetAlpha();
            set => SetAlpha(value);
        }

        public new Position Position
        {
            get => base.Position.ToPosition();
            set => base.Position = value.ToVector3();
        }

        public int Rotation
        {
            set => SetRotation(value);
        }

        public new EntityType Type => (EntityType)base.Type;

        #endregion Public Properties

        #region Public Methods

        public bool Equals(IEntity other)
        {
            return Id == other?.Id;
        }

        public new Position GetCoords()
            => base.GetCoords().ToPosition();

        public new Position GetInfoIdCoord()
            => base.GetInfoIdCoord().ToPosition();

        public void GetModelDimensions(Position a, Position b)
        {
            var aV = a.ToVector3();
            var bV = b.ToVector3();
            RAGE.Game.Misc.GetModelDimensions(Model, aV, bV);

            a.X = aV.X;
            a.Y = aV.Y;
            a.Z = aV.Z;
            b.X = bV.X;
            b.Y = bV.Y;
            b.Z = bV.Z;
        }

        public Position GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(Handle, offsetX, offsetY, offsetZ).ToPosition();

        #endregion Public Methods
    }
}
