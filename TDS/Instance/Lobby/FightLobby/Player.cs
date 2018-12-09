using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class FightLobby
    {
        public override async Task<bool> AddPlayer(Character character, uint teamid)
        {
            if (!await base.AddPlayer(character, teamid))
                return false;

            character.Player.Freeze(false);
            return true;
        }

        public override void RemovePlayer(Character character)
        {
            base.RemovePlayer(character);

            AliveOrNotDisappearedPlayers[character.Team.Index].Remove(character);
        }

        private void KillPlayer(Client player, string reason)
        {
            player.Kill();
            player.SendChatMessage(reason);
        }

        public void DamagedPlayer(Character target, Character source, WeaponHash weapon, bool headshot)
        {
            damagesys.DamagePlayer(target, weapon, headshot, source);
        }
    }
}
