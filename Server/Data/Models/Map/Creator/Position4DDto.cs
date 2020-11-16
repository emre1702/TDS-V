using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Data.Models.Map.Creator;

namespace TDS.Server.Data.Models.Map.Creator
{
#nullable enable

    public class Position4DDto
    {

        public Position4DDto()
        {
        }

        public Position4DDto(MapCreatorPosition pos)
        {
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
            Rotation = pos.RotZ;
        }

        [XmlAttribute("rot")]
        [JsonProperty("3")]
        public float Rotation { get; set; }

        [XmlAttribute("x")]
        [JsonProperty("0")]
        public float X { get; set; }

        [XmlAttribute("y")]
        [JsonProperty("1")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        [JsonProperty("2")]
        public float Z { get; set; }

        public Position3D To3D()
        {
            return new Position3D { X = X, Y = Y, Z = Z };
        }

        public Position3DDto To3DDto()
        {
            return new Position3DDto { X = X, Y = Y, Z = Z };
        }

        public Vector3 ToVector3()
            => new Vector3(X, Y, Z);

        public MapCreatorPosition ToMapCreatorPosition(int id, object? info = null)
        {
            return new MapCreatorPosition
            {
                Id = id,
                Info = info,
                PosX = X,
                PosY = Y,
                PosZ = Z,
                RotZ = Rotation
            };
        }

    }
}
