using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.GamemodesSystem.Specials;
using TDS.Server.GangGamemodesSystem.Gamemodes;

namespace TDS.Server.GangGamemodesSystem.Specials
{
    public class GangGangwarSpecials : GangwarSpecials, IGangGangwarGamemodeSpecials
    {
        public GangGangwarSpecials(IGangActionLobby lobby, IGangGangwarGamemode gamemode, ISettingsHandler settingsHandler)
            : base(lobby, gamemode, settingsHandler)
        {
        }

        public override ValueTask InRound()
        {
            // Replace "TargetMan" behaviour 
            return default;
        }
    }
}
