using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map.Creator
{
    public class Position3DDto
    {
        #region Public Constructors

        public Position3DDto()
        {
        }

        public Position3DDto(MapCreatorPosition pos)
        {
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
        }

        public Position3DDto(Position3D pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }

        #endregion Public Constructors

        #region Public Properties

        [XmlAttribute("x")]
        [JsonProperty("0")]
        public float X { get; set; }

        [XmlAttribute("y")]
        [JsonProperty("1")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        [JsonProperty("2")]
        public float Z { get; set; }

        public Vector3 ToVector3()
            => new Vector3(X, Y, Z);

        #endregion Public Properties

#nullable enable

        #region Public Methods

        public MapCreatorPosition ToMapCreatorPosition(int id, MapCreatorPositionType type, object? info = null, ushort ownerRemoteId = 0)
        {
            return new MapCreatorPosition
            {
                Id = id,
                Type = type,
                Info = info,
                PosX = X,
                PosY = Y,
                PosZ = Z,
                OwnerRemoteId = ownerRemoteId
            };
        }

        #endregion Public Methods
    }
}
