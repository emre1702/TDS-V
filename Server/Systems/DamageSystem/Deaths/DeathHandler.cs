using GTANetworkAPI;
using TDS.Server.DamageSystem.KillingSprees;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.DamageSystem.Damages;
using TDS.Server.Data.Interfaces.DamageSystem.Deaths;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.DamageSystem.Deaths
{
    public class DeathHandler : IDeathHandler
    {
        private readonly KillingSpreeHandler _killingSpreeHandler;
        private readonly KillerProvider _killerProvider;
        private readonly AssisterProvider _assisterProvider;
        private readonly DeathSyncHandler _deathSyncHandler;
        private readonly ILoggingHandler _logger;

#nullable disable
        private IFightLobby _lobby;
#nullable restore

        internal DeathHandler(IHitterHandler hitterHandler, KillingSpreeHandler killingSpreeHandler, ILoggingHandler logger)
        {
            _logger = logger;

            _killingSpreeHandler = killingSpreeHandler;
            _killerProvider = new KillerProvider(hitterHandler);
            _assisterProvider = new AssisterProvider(hitterHandler);
            _deathSyncHandler = new DeathSyncHandler();
        }

        public void Init(IFightLobby lobby)
        {
            _lobby = lobby;

            _assisterProvider.Init(lobby);
        }

        public void PlayerDeath(ITDSPlayer died, ITDSPlayer killReason, uint weapon, int diedPlayerLifes)
        {
            died.Freeze(true);

            var killer = _killerProvider.Get(died, killReason);
            _killingSpreeHandler.PlayerDeath(died, killer);

            if (diedPlayerLifes <= 0)
                return;

            AddStats(died, killer);
            CheckForAssist(died, killer);
            _deathSyncHandler.Sync(died, killer, weapon, diedPlayerLifes);

            died.Deathmatch.LastHitter = null;

            if (killer != died && killer != killReason)
                killer.SendNotification(string.Format(killer.Language.GOT_LAST_HITTED_KILL, died.DisplayName));

            if (_lobby.Players.SavePlayerLobbyStats)
                _logger.LogKill(died, killer, weapon);
        }

        private void AddStats(ITDSPlayer died, ITDSPlayer killer)
        {
            if (_lobby.Players.SavePlayerLobbyStats)
                AddLobbyStats(died);
            AddRoundStats(died, killer);
        }

        private void AddLobbyStats(ITDSPlayer died)
        {
            // Only death is added instantly
            // Kills etc. are added at round end
            if (died.LobbyStats is { })
            {
                ++died.LobbyStats.Deaths;
                ++died.LobbyStats.TotalDeaths;
            }
        }

        private void AddRoundStats(ITDSPlayer died, ITDSPlayer killer)
        {
            if (died == killer)
                return;

            if (killer.CurrentRoundStats is { })
                ++killer.CurrentRoundStats.Kills;
        }

        private void CheckForAssist(ITDSPlayer died, ITDSPlayer killer)
        {
            if (died == killer)
                return;
            var assister = _assisterProvider.Get(died, killer);
            if (assister is null)
                return;

            ++assister.CurrentRoundStats!.Assists;
            NAPI.Task.RunSafe(() =>
                assister.SendNotification(string.Format(assister.Language.GOT_ASSIST, died.DisplayName)));
        }

        public void RewardLastHitter(ITDSPlayer died, out ITDSPlayer? killer)
        {
            killer = null;
            if (died.Deathmatch.LastHitter is null)
                return;

            var lastHitter = died.Deathmatch.LastHitter;
            died.Deathmatch.LastHitter = null;

            if (died.Lobby != lastHitter.Lobby)
                return;

            if (lastHitter.Lifes == 0)
                return;

            if (lastHitter.CurrentRoundStats != null)
                ++lastHitter.CurrentRoundStats.Kills;
            _killingSpreeHandler.PlayerDeath(died, lastHitter);
            NAPI.Task.RunSafe(() => 
                lastHitter.SendNotification(string.Format(lastHitter.Language.GOT_LAST_HITTED_KILL, died.DisplayName)));
            killer = lastHitter;
        }
    }
}
