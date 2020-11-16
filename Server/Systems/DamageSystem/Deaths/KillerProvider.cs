using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.DamageSystem.Damages;

namespace TDS.Server.DamageSystem.Deaths
{
    internal class KillerProvider
    {
        private readonly IHitterHandler _hitterHandler;

        internal KillerProvider(IHitterHandler hitterHandler)
            => _hitterHandler = hitterHandler;

        internal ITDSPlayer Get(ITDSPlayer died, ITDSPlayer? possibleKiller)
        {
            possibleKiller ??= GetPossibleLastHitterAsKiller(died);
            possibleKiller ??= GetPossibleMostHitterAsKiller(died);
            possibleKiller ??= died;

            return possibleKiller;
        }

        private ITDSPlayer? GetPossibleLastHitterAsKiller(ITDSPlayer died)
        {
            if (died.Deathmatch.LastHitter is { } lastHitter && lastHitter.Lobby == died.Lobby)
                return lastHitter;
            return null;
        }

        private ITDSPlayer? GetPossibleMostHitterAsKiller(ITDSPlayer died)
            => _hitterHandler.GetPlayersMostHitter(died);
    }
}
