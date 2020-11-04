using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.GamemodesSystem.Specials;
using TDS_Server.GangGamemodesSystem.Gamemodes;

namespace TDS_Server.GangGamemodesSystem.Specials
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
