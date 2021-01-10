using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Userpanel.Stats;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelPlayerWeaponStatsHandler : IUserpanelPlayerWeaponStatsHandler
    {
        private readonly ILoggingHandler _loggingHandler;

        public UserpanelPlayerWeaponStatsHandler(ILoggingHandler loggingHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            _loggingHandler = loggingHandler;

            remoteBrowserEventsHandler.Add(ToServerEvent.LoadPlayerWeaponStats, GetPlayerWeaponStats);
        }

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

        private object? GetPlayerWeaponStats(RemoteBrowserEventArgs args)
        {
            if (args.Player.WeaponStats is null)
                return null;
            var weaponName = args.Args[0].ToString();
            if (weaponName is null)
                return null;
            if (!Enum.TryParse(weaponName, out WeaponHash weaponHash))
                return null;

            var weaponStats = args.Player.WeaponStats.GetWeaponStats(weaponHash) ?? new PlayerWeaponStats();

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

            args.Player.WeaponStats.DoForBodyPartStats(weaponHash, bodyStats =>
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