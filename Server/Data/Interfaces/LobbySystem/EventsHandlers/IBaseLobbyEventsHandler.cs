﻿using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity.Player;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers
{
#nullable enable

    public interface IBaseLobbyEventsHandler
    {
        public delegate void LobbyCreatedAfterDelegate(LobbyDb entity);

        public delegate void LobbyDelegate(IBaseLobby lobby);

        public delegate void PlayerDelegate(ITDSPlayer player);

        public delegate void BanDelegate(PlayerBans ban);

        public delegate void PlayerColshapeDelegate(ITDSColshape colshape, ITDSPlayer player);

        bool IsRemoved { get; }
        AsyncTaskEvent<LobbyDb>? Created { get; set; }
        AsyncTaskEvent<IBaseLobby>? Remove { get; set; }
        AsyncValueTaskEvent<(ITDSPlayer Player, int HadLifes)>? PlayerLeft { get; set; }

        event LobbyCreatedAfterDelegate? CreatedAfter;

        event LobbyDelegate? RemoveAfter;

        AsyncValueTaskEvent<(ITDSPlayer Player, int TeamIndex)>? PlayerJoined { get; set; }
        AsyncValueTaskEvent<(ITDSPlayer Player, int TeamIndex)>? PlayerJoinedAfter { get; set; }

        AsyncValueTaskEvent<(ITDSPlayer Player, int HadLifes)>? PlayerLeftAfter { get; set; }

        event BanDelegate? NewBan;

        event PlayerColshapeDelegate? PlayerEnteredColshape;

        event PlayerDelegate? PlayerSpawned;

        Task TriggerCreated(LobbyDb entity);

        Task TriggerRemove();

        ValueTask TriggerPlayerJoined(ITDSPlayer player, int teamIndex);

        ValueTask TriggerPlayerLeft(ITDSPlayer player, int hadLifes);

        void TriggerNewBan(PlayerBans ban, ulong? targetDiscordUserId);

        void TriggerPlayerEnteredColshape(ITDSColshape colshape, ITDSPlayer player);

        void TriggerPlayerSpawned(ITDSPlayer player);
    }
}
