using System.Drawing;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Entities.GangSystem
{
    public class GangHouse : IGangHouse
    {
        public GangHouses Entity { get; }
        public Position3D Position { get; }
        public float SpawnRotation => Entity.Rot;

        private readonly ITextLabel _textLabel;
        private IBlip? _blip;

        private readonly GangLobby _lobby;
        private readonly int _cost;

        private readonly IModAPI _modAPI;

        public GangHouse(GangHouses entity, GangLobby lobby, int cost, IModAPI modAPI)
        {
            _modAPI = modAPI;

            Entity = entity;
            _lobby = lobby;
            _cost = cost;

            Position = new Position3D(entity.PosX, entity.PosY, entity.PosZ);

            _textLabel = modAPI.TextLabel.Create(GetTextLabelText(), Position, 10d, 7f, 0, Color.FromArgb(220, 220, 220), true, lobby);

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


        public void SetOwner(IGang? owner)
        {
            if (owner is null)
            {
                _blip?.Delete();
                _blip = null;
            } 
            else
            {
                _blip = _modAPI.Blip.Create(
                    SharedConstants.GangHouseOccupiedBlipModel,
                    Position, dimension: _lobby.Dimension, color: Entity.OwnerGang is null ? (byte)1 : Entity.OwnerGang.BlipColor,
                    shortRange: true, alpha: Entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{Entity.NeededGangLevel}] " + (Entity.OwnerGang is null ? "-" : Entity.OwnerGang.Name));
            }
            _textLabel.Text = GetTextLabelText();
        }


        private string GetTextLabelText()
            => Entity.OwnerGang switch
            {
                { } => $"{Entity.OwnerGang}",
                null => $"-\nLevel {Entity.NeededGangLevel}\n${_cost}"
            };
    }
}
