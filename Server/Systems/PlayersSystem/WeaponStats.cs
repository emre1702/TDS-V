using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.PlayersSystem
{
    public class WeaponStats : IPlayerWeaponStats
    {
#nullable disable
        private Dictionary<WeaponHash, Dictionary<PedBodyPart, PlayerWeaponBodypartStats>> _weaponBodyPartsStats { get; set; }
        private Dictionary<WeaponHash, PlayerWeaponStats> _weaponStats { get; set; }

        private ITDSPlayer _player;
        private IPlayerEvents _events;
#nullable enable

        public void Init(ITDSPlayer player, IPlayerEvents events)
        {
            _player = player;
            _events = events;

            events.EntityChanged += LoadWeaponStats;
            events.Removed += Events_Removed;
        }

        private void Events_Removed()
        {
            _events.EntityChanged -= LoadWeaponStats;
            _events.Removed -= Events_Removed;
        }

        public void AddWeaponShot(WeaponHash weaponHash)
        {
            var weaponStats = GetWeaponStats(weaponHash);
            if (weaponStats is null)
                return;
            ++weaponStats.AmountShots;
            if (_player.Lobby?.IsOfficial == true)
                ++weaponStats.AmountOfficialShots;
            return;
        }

        public void AddWeaponDamage(WeaponHash weaponHash, PedBodyPart pedBodyPart, int damage, bool killed)
        {
            var weaponStats = GetWeaponStats(weaponHash);

            if (weaponStats is { })
                AddWeaponDamageToWeaponStats(weaponStats, pedBodyPart, damage, killed);

            var bodyPartStats = GetWeaponBodyPartStats(weaponHash, pedBodyPart);
            if (bodyPartStats is { })
                AddWeaponDamageToBodyPartStats(bodyPartStats, damage, killed);
        }

        private void AddWeaponDamageToWeaponStats(PlayerWeaponStats weaponStats, PedBodyPart pedBodyPart, int damage, bool killed)
        {
            ++weaponStats.AmountHits;
            weaponStats.DealtDamage += damage;
            if (killed)
                ++weaponStats.Kills;
            if (pedBodyPart == PedBodyPart.Head)
                ++weaponStats.AmountHeadshots;
            if (_player.Lobby?.IsOfficial == true)
            {
                ++weaponStats.AmountOfficialHits;
                weaponStats.DealtOfficialDamage += damage;
                if (killed)
                    ++weaponStats.OfficialKills;
                if (pedBodyPart == PedBodyPart.Head)
                    ++weaponStats.AmountOfficialHeadshots;
            }
        }

        private void AddWeaponDamageToBodyPartStats(PlayerWeaponBodypartStats bodyPartStats, int damage, bool killed)
        {
            ++bodyPartStats.AmountHits;
            bodyPartStats.DealtDamage += damage;
            if (killed)
                ++bodyPartStats.Kills;
            if (_player.Lobby?.IsOfficial == true)
            {
                ++bodyPartStats.AmountOfficialHits;
                bodyPartStats.DealtOfficialDamage += damage;
                if (killed)
                    ++bodyPartStats.OfficialKills;
            }
        }

        public PlayerWeaponBodypartStats? GetWeaponBodyPartStats(WeaponHash weaponHash, PedBodyPart pedBodyPart)
        {
            Dictionary<PedBodyPart, PlayerWeaponBodypartStats>? bodyPartStatsDict;
            lock (_weaponBodyPartsStats)
            {
                if (!_weaponBodyPartsStats.TryGetValue(weaponHash, out bodyPartStatsDict))
                {
                    bodyPartStatsDict = new Dictionary<PedBodyPart, PlayerWeaponBodypartStats>();
                    _weaponBodyPartsStats[weaponHash] = bodyPartStatsDict;
                }
            }

            PlayerWeaponBodypartStats? bodyPartStats;
            lock (bodyPartStatsDict)
            {
                if (!bodyPartStatsDict.TryGetValue(pedBodyPart, out bodyPartStats))
                {
                    bodyPartStats = new PlayerWeaponBodypartStats { BodyPart = pedBodyPart, PlayerId = _player.Entity!.Id, WeaponHash = weaponHash };
                    bodyPartStatsDict[pedBodyPart] = bodyPartStats;
                    _player.Entity.WeaponBodypartStats.Add(bodyPartStats);
                }
            }

            return bodyPartStats;
        }

        public PlayerWeaponStats? GetWeaponStats(WeaponHash weaponHash)
        {
            PlayerWeaponStats? weaponStats;
            lock (_weaponStats)
            {
                if (!_weaponStats!.TryGetValue(weaponHash, out weaponStats))
                {
                    weaponStats = new PlayerWeaponStats { WeaponHash = weaponHash, PlayerId = _player.Entity!.Id };
                    _weaponStats[weaponHash] = weaponStats;
                    _player.Entity.WeaponStats.Add(weaponStats);
                }
            }

            return weaponStats;
        }

        private void LoadWeaponStats(Players? entity)
        {
            if (entity is null)
                return;
            _weaponBodyPartsStats = entity.WeaponBodypartStats.GroupBy(e => e.WeaponHash).ToDictionary(e => e.Key, e => e.ToDictionary(w => w.BodyPart, w => w));
            _weaponStats = entity.WeaponStats.ToDictionary(e => e.WeaponHash, e => e);
        }

        public void DoForBodyPartStats(WeaponHash weaponHash, Action<Dictionary<PedBodyPart, PlayerWeaponBodypartStats>> action)
        {
            lock (_weaponBodyPartsStats)
            {
                if (!_weaponBodyPartsStats.TryGetValue(weaponHash, out var bodyStats))
                    return;
                lock (bodyStats)
                {
                    action(bodyStats);
                }
            }
        }

        public List<string> GetWeaponHashesUsedSoFar()
        {
            lock (_weaponStats)
                return _weaponStats?.OrderBy(w => w.Value.DealtDamage).Select(w => w.Key.ToString()).ToList() ?? new List<string>();
        }
    }
}
