using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.GangSystem;
using TDS_Shared.Data.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        public void LoadHouse(GangHouse house)
        {
            house.TextLabel = NAPI.TextLabel.CreateTextLabel(house.GetTextLabelText(), house.Position, 10f, 7f, 0, new Color(220, 220, 220), true, Dimension) as ITDSTextLabel;

            if (house.Entity.OwnerGang is { })
            {
                house.Blip = NAPI.Blip.CreateBlip(
                    SharedConstants.GangHouseOccupiedBlipModel, house.Position,
                    1f, house.Entity.OwnerGang is null ? (byte)1 : house.Entity.OwnerGang.BlipColor,
                    dimension: Dimension,
                    shortRange: true, alpha: house.Entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{house.Entity.NeededGangLevel}] " + (house.Entity.OwnerGang is null ? "-" : house.Entity.OwnerGang.Name)) as ITDSBlip;
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
                house.Blip = NAPI.Blip.CreateBlip(
                    SharedConstants.GangHouseOccupiedBlipModel,
                    house.Position, 1f, dimension: Dimension, color: house.Entity.OwnerGang is null ? (byte)1 : house.Entity.OwnerGang.BlipColor,
                    shortRange: true, alpha: house.Entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{house.Entity.NeededGangLevel}] " + (house.Entity.OwnerGang is null ? "-" : house.Entity.OwnerGang.Name)) as ITDSBlip;
            }
            if (house.TextLabel is { })
                house.TextLabel.Text = house.GetTextLabelText();
        }
    }
}
