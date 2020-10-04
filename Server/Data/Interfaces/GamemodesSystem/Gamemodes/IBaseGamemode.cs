using TDS_Server.Data.Interfaces.GamemodesSystem.Deathmatch;
using TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS_Server.Data.Interfaces.GamemodesSystem.Players;
using TDS_Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS_Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS_Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS_Server.Data.Interfaces.GamemodesSystem.Weapons;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes
{
    public interface IBaseGamemode
    {
        IBaseGamemodeDeathmatch Deathmatch { get; }
        IBaseGamemodeMapHandler MapHandler { get; }
        IBaseGamemodePlayers Players { get; }
        IBaseGamemodeRounds Rounds { get; }
        IBaseGamemodeSpecials Specials { get; }
        IBaseGamemodeTeams Teams { get; }
        IBaseGamemodeWeapons Weapons { get; }

        void Init(IRoundFightLobby lobby, MapDto map);
    }
}
