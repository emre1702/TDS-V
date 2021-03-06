﻿using GTANetworkAPI;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.CustomLobby;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Players
{
    public class ArenaPlayers : RoundFightLobbyPlayers
    {
        public new IArena Lobby => (IArena)base.Lobby;

        public ArenaPlayers(IArena arena, IRoundFightLobbyEventsHandler events)
            : base(arena, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, 0).ConfigureAwait(false);
            if (!worked)
                return false;

            var spawnPos = Lobby.CurrentMap?.LimitInfo?.Center?.ToVector3()?.AddToZ(10) ?? Lobby.MapHandler.SpawnPoint;
            var teamChoiceData = await Lobby.Teams.DoForList(teams =>
                teams.Select(t => new TeamChoiceMenuTeamData(t.Entity.Name, t.Entity.ColorR, t.Entity.ColorG, t.Entity.ColorB)))
                .ConfigureAwait(false);
            var teamChoiceDataJson = Serializer.ToBrowser(teamChoiceData);
            Lobby.Spectator.SetPlayerInSpectateMode(player);

            NAPI.Task.RunSafe(() =>
            {
                player.Spawn(spawnPos);
                StartTeamChoice(player, teamChoiceDataJson);
            });

            return true;
        }

        private void StartTeamChoice(ITDSPlayer player, string teamChoiceDataJson)
            => player.TriggerEvent(ToClientEvent.SyncTeamChoiceMenuData, teamChoiceDataJson, Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound);

        public virtual async ValueTask ChooseTeam(ITDSPlayer player, int teamIndex)
        {
            if (teamIndex != 0)
            {
                var team = Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound
                    ? await Lobby.Teams.GetTeamWithFewestPlayer().ConfigureAwait(false)
                    : await Lobby.Teams.GetTeam((short)teamIndex).ConfigureAwait(false);
                Lobby.Teams.SetPlayerTeam(player, team);
                bool hasBeenSetInRound = await Lobby.Rounds.PlayerJoinedRound(player).ConfigureAwait(false);
                if (!hasBeenSetInRound)
                    Lobby.Spectator.SetPlayerInSpectateMode(player);
            }
            else
            {
                Lobby.Spectator.SetPlayerInSpectateMode(player);
            }
        }

        protected override void Events_PlayersPreparation()
        {
            base.Events_PlayersPreparation();
        }

        protected override void Events_Countdown()
        {
            base.Events_Countdown();
            DoInMain(player =>
            {
                player.TriggerEvent(ToClientEvent.CountdownStart, player.Team is null || player.Team.IsSpectator);
            });
        }
    }
}
