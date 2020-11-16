using TDS.Server.Data.Interfaces.GamemodesSystem.Deathmatch;
using TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS.Server.Data.Interfaces.GamemodesSystem.Players;
using TDS.Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS.Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS.Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS.Server.Data.Interfaces.GamemodesSystem.Weapons;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models.Map;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes
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
