﻿using System.Collections.Generic;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Extensions;
using TDS.Client.Handler.Entities.GTA;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Utility;

namespace TDS.Client.Handler.Lobby
{
    public class DamageTestPedsHandler
    {
        // private readonly List<TDSPed> _peds = new List<TDSPed>();

        public DamageTestPedsHandler(EventsHandler eventsHandler)
        {
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            /*if (settings.Type != LobbyType.DamageTestLobby)
                return;

            var spawnPoint = RAGE.Elements.Player.LocalPlayer.Position;
            var dimension = RAGE.Elements.Player.LocalPlayer.Dimension;
            for (int i = 0; i < 10; ++i)
            {
                var randomSpawnPoint = spawnPoint.Around((float)(SharedUtils.Rnd.NextDouble() * 20), false);
                var randomSpawnRotation = SharedUtils.Rnd.Next(0, 360);
                var pedHash = SharedUtils.GetRandom(Constants.DamageTestLobbyPedModels);
                var ped = new TDSPed((uint)pedHash, randomSpawnPoint, randomSpawnRotation, dimension);
                ped.DisableDying = true;
                _peds.Add(ped);
            }*/
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            /*if (settings.Type != LobbyType.DamageTestLobby)
                return;*/
        }
    }
}
