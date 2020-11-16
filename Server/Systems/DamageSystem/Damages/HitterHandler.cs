using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.DamageSystem.Damages;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.DamageSystem.Damages
{
    public class HitterHandler : IHitterHandler
    {
        private readonly Dictionary<ITDSPlayer, Dictionary<ITDSPlayer, int>> _allHitters = new Dictionary<ITDSPlayer, Dictionary<ITDSPlayer, int>>();

        public void Init(IFightLobby fightLobby)
        {
            if (fightLobby is IRoundFightLobby roundFightLobby)
            {
                roundFightLobby.Events.RoundClear += Clear;
            }
            fightLobby.Events.RemoveAfter += Remove;
        }

        private void Remove(IBaseLobby lobby)
        {
            if (!(lobby is IFightLobby fightLobby))
                return;

            if (fightLobby is IRoundFightLobby roundFightLobby)
            {
                if (roundFightLobby.Events.RoundClear is { })
                    roundFightLobby.Events.RoundClear -= Clear;
            }
            fightLobby.Events.RemoveAfter -= Remove;
        }

        public void SetLastHitter(ITDSPlayer target, ITDSPlayer source, int damage)
        {
            lock (_allHitters)
            {
                if (!_allHitters.TryGetValue(target, out var lastHitterDict))
                {
                    lastHitterDict = new Dictionary<ITDSPlayer, int>();
                    _allHitters[target] = lastHitterDict;
                }

                lastHitterDict.TryGetValue(source, out var currentDamage);
                lastHitterDict[source] = currentDamage + damage;

            }

            target.Deathmatch.LastHitter = source;
        }

        public ITDSPlayer? GetPlayersMostHitter(ITDSPlayer player)
        {
            lock (_allHitters)
            {
                if (!_allHitters.TryGetValue(player, out var lastHitterDict))
                    return null;

                return lastHitterDict
                    .MaxBy(entry => entry.Value)
                    .Select(entry => entry.Key)
                    .FirstOrDefault();
            }
        }

        public IEnumerable<ITDSPlayer> GetPlayersHitters(ITDSPlayer player, int atleastDamage = 0)
        {
            lock (_allHitters)
            {
                if (!_allHitters.TryGetValue(player, out var lastHitterDict))
                    return Enumerable.Empty<ITDSPlayer>();
                return lastHitterDict
                    .Where(entry => entry.Value >= atleastDamage)
                    .Select(entry => entry.Key);
            }
        }

        public bool HasAnyHitters()
        {
            lock (_allHitters)
                return _allHitters.Count > 0;
        }

        private ValueTask Clear()
        {
            lock (_allHitters)
                _allHitters.Clear();
            return default;
        }
    }
}
