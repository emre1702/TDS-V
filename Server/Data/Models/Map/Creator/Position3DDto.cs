using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Data.Models.Map.Creator;

namespace TDS.Server.Data.Models.Map.Creator
{
    public class Position3DDto
    {

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

#nullable enable

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

    }
}
