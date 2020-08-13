﻿using AltV.Net.Data;
using Newtonsoft.Json;
using System.Xml.Serialization;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map.Creator
{
#nullable enable

    public class Position4DDto
    {
        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        public Position ToAltV()
        {
            return new Position(X, Y, Z);
        }

        public PositionDto To3DDto()
        {
            return new PositionDto { X = X, Y = Y, Z = Z };
        }

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

        #endregion Public Methods
    }
}
