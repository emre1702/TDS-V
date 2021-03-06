﻿using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Entities.GangSystem;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.GangSystem;
using TDS.Shared.Data.Default;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class GangLobbyMapHandler : BaseLobbyMapHandler
    {
        private readonly EventsHandler _globalEventsHandler;

        public GangLobbyMapHandler(IGangLobby lobby, IBaseLobbyEventsHandler events, EventsHandler globalEventsHandler, GangHousesHandler gangHousesHandler)
            : base(lobby, events)
        {
            _globalEventsHandler = globalEventsHandler;

            globalEventsHandler.GangHouseLoaded += LoadHouse;
            LoadAlreadyLoadedHouses(gangHousesHandler.GetLoadedHouses());
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);
            _globalEventsHandler.GangHouseLoaded -= LoadHouse;
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                data.Player.Spawn(data.Player.Gang.MapHandler.SpawnPosition ?? SpawnPoint, data.Player.Gang.MapHandler.SpawnHeading ?? SpawnRotation);
                data.Player.Freeze(false);
            });
            return default;
        }

        private void LoadHouse(GangHouse house)
        {
            if (house.LoadedInGangLobby)
                return;
            house.LoadedInGangLobby = true;

            NAPI.Task.RunSafe(() =>
            {
                house.TextLabel = NAPI.TextLabel.CreateTextLabel(house.TextLabelText, house.Position, 10f, 7f, 0, new Color(220, 220, 220), true, Dimension) as ITDSTextLabel;

                if (house.Entity.OwnerGang is { })
                    house.Blip = GetHouseWithOwnerBlip(house);
            });

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

        /*
        private void SetHouseOwner(GangHouse house, IGang? owner)
        {
            NAPI.Task.RunSafe(() =>
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
                    house.TextLabel.Text = house.TextLabelText;
            });
        }*/

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