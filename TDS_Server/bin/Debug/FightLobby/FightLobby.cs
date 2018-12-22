using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Entity;
using TDS.Instance.Lobby.Interfaces;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{

    partial class FightLobby : Lobby, IFight
    {

        public FightLobby(Lobbies entity) : base(entity)
        {
            this.DmgSys = new Damagesys.Damagesys(this);
        }

        protected override void Remove()
        {
            base.Remove();
            this.DmgSys = null;
        }

        public override async Task<bool> AddPlayer(Character character, uint teamid)
        {
            if (!await base.AddPlayer(character, teamid))
                return false;

            character.Player.Freeze(false);
            return true;
        }

        

        private void KillPlayer(Client player, string reason)
        {
            player.Kill();
            NAPI.Notification.SendNotificationToPlayer(player, reason);
        }


    }
}
