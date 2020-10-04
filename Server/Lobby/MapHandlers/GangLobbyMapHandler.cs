using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Entities.GangSystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Shared.Data.Default;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class GangLobbyMapHandler : BaseLobbyMapHandler
    {
        public GangLobbyMapHandler(IGangLobby lobby, IBaseLobbyEventsHandler events, EventsHandler globalEventsHandler, GangHousesHandler gangHousesHandler)
            : base(lobby, events)
        {
            globalEventsHandler.GangHouseLoaded += LoadHouse;
            LoadAlreadyLoadedHouses(gangHousesHandler.GetLoadedHouses());
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.Run(() =>
            {
                data.Player.Spawn(data.Player.Gang.SpawnPosition ?? SpawnPoint, data.Player.Gang.SpawnHeading ?? SpawnRotation);
                data.Player.Freeze(false);
            });
            return default;
        }

        private void LoadHouse(GangHouse house)
        {
            if (house.LoadedInGangLobby)
                return;
            house.LoadedInGangLobby = true;

            house.TextLabel = NAPI.TextLabel.CreateTextLabel(house.GetTextLabelText(), house.Position, 10f, 7f, 0, new Color(220, 220, 220), true, Dimension) as ITDSTextLabel;

            if (house.Entity.OwnerGang is { })
                house.Blip = GetHouseWithOwnerBlip(house);
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

        private void SetHouseOwner(GangHouse house, IGang? owner)
        {
            NAPI.Task.Run(() =>
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
            });
        }

        private ITDSBlip GetHouseWithOwnerBlip(GangHouse house)
            => (ITDSBlip)NAPI.Blip.CreateBlip(
                    SharedConstants.GangHouseOccupiedBlipModel, house.Position,
                    1f, house.Entity.OwnerGang is null ? (byte)1 : house.Entity.OwnerGang.BlipColor,
                    dimension: Dimension,
                    shortRange: true, alpha: house.Entity.OwnerGang is null ? (byte)180 : (byte)255,
                    name: $"[{house.Entity.NeededGangLevel}] " + (house.Entity.OwnerGang is null ? "-" : house.Entity.OwnerGang.Name));

        private void LoadAlreadyLoadedHouses(List<GangHouse> houses)
        {
            foreach (var house in houses)
                LoadHouse(house);
        }
    }
}
