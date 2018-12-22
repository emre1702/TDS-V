using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Enum;

namespace TDS.Instance.Lobby
{
    partial class Arena
    {

        private void SetAllPlayersInCountdown()
        {
            spectatingMe.Clear();
            FuncIterateAllPlayers((character, team) =>
            {
                SetPlayerReadyForRound(character);
                NAPI.ClientEvent.TriggerClientEvent(character.Player, "onClientCountdownStart");
                if (team.IsSpectatorTeam)
                    SpectateAllTeams(character);
            });
            if (currentMap.SyncData.Type == EMapType.Bomb)
                GiveBombToRandomTerrorist();
        }

        private void SendPlayerAmountInFightInfo(Client player)
        {
#warning Todo after next efcore.bat
            /*List<uint> amountinteams = new List<uint>();
            List<uint> amountaliveinteams = new List<uint>();
            foreach (var team in teamPlayers)
            {

            }

            for (int i = 1; i < teamPlayers.Count; i++)
            {
                amountinteams.Add((uint)teamPlayers[i].Count);
                amountaliveinteams.Add((uint)AlivePlayers[i].Count);
            }
            PlayerAmountInFightSync(player, amountinteams, amountaliveinteams);*/
        }
    }
}
