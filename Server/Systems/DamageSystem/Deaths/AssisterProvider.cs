using System.Linq;
using TDS.Server.DamageSystem.Damages;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.DamageSystem.Damages;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.DamageSystem.Deaths
{
    internal class AssisterProvider
    {
        private readonly IHitterHandler _hitterHandler;

#nullable disable
        private IFightLobby _lobby;
#nullable restore

        internal AssisterProvider(IHitterHandler hitterHandler)
            => _hitterHandler = hitterHandler;

        internal void Init(IFightLobby lobby)
            => _lobby = lobby;

        internal ITDSPlayer? Get(ITDSPlayer died, ITDSPlayer killer)
        {
            if (!_hitterHandler.HasAnyHitters())
                return null;

            var minDamageForAssist = GetMinDamageForAssist();
            var assister = _hitterHandler.GetPlayersHitters(died, minDamageForAssist)
                .FirstOrDefault(p => p.Lobby == died.Lobby && p != died && p != killer && p.CurrentRoundStats is { });

            return assister;
        }

        private int GetMinDamageForAssist()
            => (_lobby.Entity.FightSettings.StartArmor + _lobby.Entity.FightSettings.StartHealth) / 2;
    }
}
