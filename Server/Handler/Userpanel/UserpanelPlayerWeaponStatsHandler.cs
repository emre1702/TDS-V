using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Models.Userpanel.Stats;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelPlayerWeaponStatsHandler : IUserpanelPlayerWeaponStatsHandler
    {
        #region Private Fields

        private readonly ILoggingHandler _loggingHandler;
        private readonly Serializer _serializer;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelPlayerWeaponStatsHandler(ILoggingHandler loggingHandler, Serializer serializer)
            => (_loggingHandler, _serializer) = (loggingHandler, serializer);

        #endregion Public Constructors

        #region Public Methods

        public string? GetData(ITDSPlayer player)
        {
            try
            {
                if (player.Entity is null)
                    return null;
                var weaponHashes = GetPlayerWeaponsUsed(player);
                return _serializer.ToBrowser(weaponHashes);
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                _loggingHandler.LogError("UserpanelPlayerWeaponStatsHandler GetData failed: " + baseEx.Message, ex.StackTrace ?? Environment.StackTrace,
                    ex.GetType().Name + "|" + baseEx.GetType().Name, player);
                return null;
            }
        }

        public object? GetPlayerWeaponStats(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.WeaponStats is null)
                return null;
            var weaponName = args[0].ToString();
            if (weaponName is null)
                return null;
            if (!Enum.TryParse(weaponName, out WeaponHash weaponHash))
                return null;

            var weaponStats = player.WeaponStats.TryGetValue(weaponHash, out PlayerWeaponStats? value) ? value : new PlayerWeaponStats();

            var data = new UserpanelPlayerWeaponStatsData
            {
                WeaponName = weaponHash.ToString(),
                BodyPartsStats = new List<UserpanelPlayerWeaponBodyPartStatsData>(),
                Kills = weaponStats.Kills,
                OfficialKills = weaponStats.OfficialKills,
                AmountHeadshots = weaponStats.AmountHeadshots,
                AmountHits = weaponStats.AmountHits,
                AmountOfficialHeadshots = weaponStats.AmountOfficialHeadshots,
                AmountOfficialHits = weaponStats.AmountOfficialHits,
                AmountOfficialShots = weaponStats.AmountOfficialShots,
                AmountShots = weaponStats.AmountShots,
                DealtDamage = weaponStats.DealtDamage,
                DealtOfficialDamage = weaponStats.DealtOfficialDamage
            };

            if (player.WeaponBodyPartsStats is { } && player.WeaponBodyPartsStats.TryGetValue(weaponHash, out Dictionary<PedBodyPart, PlayerWeaponBodypartStats>? bodyStats))
            {
                foreach (var entry in bodyStats.OrderBy(b => (int)b.Key))
                {
                    data.BodyPartsStats.Add(new UserpanelPlayerWeaponBodyPartStatsData
                    {
                        BodyPart = entry.Value.BodyPart,
                        AmountHits = entry.Value.AmountHits,
                        AmountOfficialHits = entry.Value.AmountOfficialHits,
                        DealtDamage = entry.Value.DealtDamage,
                        DealtOfficialDamage = entry.Value.DealtOfficialDamage,
                        Kills = entry.Value.Kills,
                        OfficialKills = entry.Value.OfficialKills
                    });
                }
            }

            return _serializer.ToBrowser(data);
        }

        #endregion Public Methods

        #region Private Methods

        private List<string> GetPlayerWeaponsUsed(ITDSPlayer player)
        {
            return player.WeaponStats.OrderBy(w => w.Value.DealtDamage).Select(w => w.Key.ToString()).ToList();
        }

        #endregion Private Methods
    }
}
