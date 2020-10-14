using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
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
        private readonly ILoggingHandler _loggingHandler;

        public UserpanelPlayerWeaponStatsHandler(ILoggingHandler loggingHandler)
            => (_loggingHandler) = (loggingHandler);

        public string? GetData(ITDSPlayer player)
        {
            try
            {
                if (player.Entity is null)
                    return null;
                var weaponHashes = player.WeaponStats.GetWeaponHashesUsedSoFar();
                return Serializer.ToBrowser(weaponHashes);
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

            var weaponStats = player.WeaponStats.GetWeaponStats(weaponHash) ?? new PlayerWeaponStats();

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

            player.WeaponStats.DoForBodyPartStats(weaponHash, bodyStats =>
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
            });

            return Serializer.ToBrowser(data);
        }
    }
}
