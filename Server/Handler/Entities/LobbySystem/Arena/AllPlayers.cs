using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Models;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        private void SetAllPlayersInCountdown()
        {
            var mapCenter = _currentMap?.LimitInfo.Center.SwitchNamespace();
            FuncIterateAllPlayers((player, team) =>
            {
                if (team?.IsSpectator == false)
                {
                    RemoveAsSpectator(player);
                    team.SpectateablePlayers?.Add(player);
                }
                else
                {
                    MakeSurePlayerSpectatesAnyone(player);
                    if (player.Spectates is { })
                        player.Position = (mapCenter?.ToVector3() ?? player.Spectates.Position).AddToZ(10);
                }
                SetPlayerReadyForRound(player);
                player.CurrentRoundStats?.Clear();
                player.TriggerEvent(ToClientEvent.CountdownStart, team is null || team.IsSpectator);
            });
        }

        private void StartRoundForAllPlayer()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                StartRoundForPlayer(player);
            });

            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            string json = Serializer.ToClient(amounts);
            TriggerEvent(ToClientEvent.AmountInFightSync, json);
        }
    }
}
