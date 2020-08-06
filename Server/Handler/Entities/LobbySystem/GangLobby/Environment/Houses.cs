using System.Drawing;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.GangSystem;
using TDS_Shared.Data.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        public void LoadHouse(GangHouse house)
        {
            house.TextLabel = ModAPI.TextLabel.Create(house.GetTextLabelText(), house.Position, 10d, 7f, 0, Color.FromArgb(220, 220, 220), true, this);

            if (house.Entity.OwnerGang is { })
            {
                house.Blip = ModAPI.Blip.Create(
                    SharedConstants.GangHouseOccupiedBlipModel,
                    house.Position, dimension: Dimension, color: house.Entity.OwnerGang is null ? (byte)1 : house.Entity.OwnerGang.BlipColor,
                    shortRange: true, alpha: house.Entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{house.Entity.NeededGangLevel}] " + (house.Entity.OwnerGang is null ? "-" : house.Entity.OwnerGang.Name));
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


        private void LoadHouses()
        {
            foreach (var house in _gangHousesHandler.Houses)
            {
                LoadHouse(house);
            }
        }

        private void SetHouseOwner(GangHouse house, IGang? owner)
        {
            if (owner is null)
            {
                house.Blip?.Delete();
                house.Blip = null;
            }
            else
            {
                house.Blip = ModAPI.Blip.Create(
                    SharedConstants.GangHouseOccupiedBlipModel,
                    house.Position, dimension: Dimension, color: house.Entity.OwnerGang is null ? (byte)1 : house.Entity.OwnerGang.BlipColor,
                    shortRange: true, alpha: house.Entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{house.Entity.NeededGangLevel}] " + (house.Entity.OwnerGang is null ? "-" : house.Entity.OwnerGang.Name));
            }
            if (house.TextLabel is { })
                house.TextLabel.Text = house.GetTextLabelText();
        }

    }
}
