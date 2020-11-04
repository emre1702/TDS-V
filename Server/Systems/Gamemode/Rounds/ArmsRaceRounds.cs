using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Data.RoundEndReasons;

namespace TDS_Server.GamemodesSystem.Rounds
{
    public class ArmsRaceRounds : BaseGamemodeRounds, IArmsRaceGamemodeRounds
    {
        private readonly IRoundFightLobby _lobby;
        private readonly IArmsRaceGamemode _gamemode;

        public ArmsRaceRounds(IRoundFightLobby lobby, IArmsRaceGamemode gamemode)
        {
            _lobby = lobby;
            _gamemode = gamemode;
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);
            events.InitNewMap += Events_InitNewMap;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);
            events.InitNewMap -= Events_InitNewMap;
        }

        public override bool CanJoinDuringRound(ITDSPlayer player, ITeam team)
            => true;

        public bool CheckHasKillerWonTheRound(ITDSPlayer killer)
        {
            if (!_gamemode.Weapons.GetNextWeapon(killer, out WeaponHash? weaponHash))
                return false;

            // WeaponHash == null => Round end
            if (weaponHash.HasValue)
                return false;

            _lobby.Rounds.RoundStates.EndRound(new PlayerWonRoundEndReason(killer));
            return true;
        }

        private void Events_InitNewMap(Data.Models.Map.MapDto mapDto)
        {
            _lobby.Deathmatch.AmountLifes = short.MaxValue;
        }

        protected override ValueTask RoundClear()
        {
            var valueTask = base.RoundClear();
            _lobby.Deathmatch.AmountLifes = (_lobby.Entity.FightSettings?.AmountLifes ?? 0);
            return valueTask;
        }
    }
}
