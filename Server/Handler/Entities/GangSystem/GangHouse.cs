using System.Drawing;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Entities.GangSystem
{
    public class GangHouse
    {
        public GangHouses Entity { get; }
        public Position3D Position { get; }

        private readonly ITextLabel _textLabel;
        private readonly IBlip? _blip;

        public GangHouse(GangHouses entity, GangLobby lobby, int cost, IModAPI modAPI)
        {
            Entity = entity;

            Position = new Position3D(entity.PosX, entity.PosY, entity.PosZ);

            string msg = entity.OwnerGang switch
            {
                { } => $"{entity.OwnerGang}",
                null => $"-\nLevel {entity.NeededGangLevel}\n${cost}" 
            };
            _textLabel = modAPI.TextLabel.Create(msg, Position, 10d, 7f , 0, Color.FromArgb(220, 220, 220), true, lobby);

            if (entity.OwnerGang is { })
            {
                _blip = modAPI.Blip.Create(
                    SharedConstants.GangHouseOccupiedBlipModel,
                    Position, dimension: lobby.Dimension, color: entity.OwnerGang is null ? (byte)1 : entity.OwnerGang.BlipColor,
                    shortRange: true, alpha: entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{entity.NeededGangLevel}] " + (entity.OwnerGang is null ? "-" : entity.OwnerGang.Name));
            }
            /*else
            {
                BlipData = new GangHouseClientsideData
                {
                    Position = Position,
                    OwnerName = entity.OwnerGang.Name,
                    Level = entity.NeededGangLevel
                };
            }*/

        }


    }
}
