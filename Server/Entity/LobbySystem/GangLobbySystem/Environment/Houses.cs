using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Default;

namespace TDS_Server.Entity.LobbySystem.GangLobbySystem
{
    partial class GangLobby
    {
        public void LoadHouse(IGangHouse house)
        {
            house.TextLabel = _tdsTextLabelHandler.Create(house.GetTextLabelText(), house.Position, 10d, 7f, 0, new Color(220, 220, 220), true, (int)Dimension);

            if (house.Entity.OwnerGang is { })
            {
                house.Blip = _tdsBlipHandler.Create(
                    SharedConstants.GangHouseOccupiedBlipModel,
                    house.Position, dimension: (int)Dimension, color: house.Entity.OwnerGang is null ? (byte)1 : house.Entity.OwnerGang.BlipColor,
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

        private void SetHouseOwner(IGangHouse house, IGang? owner)
        {
            if (owner is null)
            {
                house.Blip?.Delete();
                house.Blip = null;
            }
            else
            {
                house.Blip = _tdsBlipHandler.Create(
                    SharedConstants.GangHouseOccupiedBlipModel,
                    house.Position, dimension: (int)Dimension, color: house.Entity.OwnerGang is null ? (byte)1 : house.Entity.OwnerGang.BlipColor,
                    shortRange: true, alpha: house.Entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{house.Entity.NeededGangLevel}] " + (house.Entity.OwnerGang is null ? "-" : house.Entity.OwnerGang.Name));
            }
            if (house.TextLabel is { })
                house.TextLabel.Text = house.GetTextLabelText();
        }

    }
}
