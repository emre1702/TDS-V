using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private Dictionary<WeaponHash, Dictionary<PedBodyPart, PlayerWeaponBodypartStats>>? _weaponBodyPartsStats;
        private Dictionary<WeaponHash, PlayerWeaponStats>? _weaponStats;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Called if we deal damage and if we only shoot.
        /// </summary>
        /// <param name="weaponHash"></param>
        /// <param name="pedBodyPart">Null if coming from shooting (not damage)</param>
        /// <param name="damage">Null if coming from shooting (not damage)</param>
        public void AddWeaponShot(WeaponHash weaponHash, PedBodyPart? pedBodyPart, int? damage, bool killed)
        {
            if (Entity is null)
                return;
            if (_weaponStats is null)
                return;
            if (_weaponBodyPartsStats is null)
                return;

            bool isOfficialLobby = Lobby?.IsOfficial == true;
            var weaponStats = GetWeaponStats(weaponHash);

            // If we deal damage, we also shoot To prevent double adding, don't add to AmountShots
            // if pedBodyPart is {} (so we dealt damage)
            if (!pedBodyPart.HasValue || damage is null)
            {
                ++weaponStats.AmountShots;
                if (isOfficialLobby)
                    ++weaponStats.AmountOfficialShots;
                return;
            }

            var bodyPartStats = GetWeaponBodyPartStats(weaponHash, pedBodyPart.Value);

            ++weaponStats.AmountHits;
            weaponStats.DealtDamage += damage.Value;

            ++bodyPartStats.AmountHits;
            bodyPartStats.DealtDamage += damage.Value;

            if (killed)
            {
                ++weaponStats.Kills;
                ++bodyPartStats.Kills;
            }

            if (pedBodyPart == PedBodyPart.Head)
            {
                ++weaponStats.AmountHeadshots;
            }

            if (isOfficialLobby)
            {
                ++weaponStats.AmountOfficialHits;
                weaponStats.DealtOfficialDamage += damage.Value;

                ++bodyPartStats.AmountOfficialHits;
                bodyPartStats.DealtOfficialDamage += damage.Value;

                if (killed)
                {
                    ++weaponStats.OfficialKills;
                    ++bodyPartStats.OfficialKills;
                }

                if (pedBodyPart == PedBodyPart.Head)
                {
                    ++weaponStats.AmountOfficialHeadshots;
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private PlayerWeaponBodypartStats GetWeaponBodyPartStats(WeaponHash weaponHash, PedBodyPart pedBodyPart)
        {
            if (!_weaponBodyPartsStats!.TryGetValue(weaponHash, out Dictionary<PedBodyPart, PlayerWeaponBodypartStats>? bodyPartStatsDict))
            {
                bodyPartStatsDict = new Dictionary<PedBodyPart, PlayerWeaponBodypartStats>();
                _weaponBodyPartsStats[weaponHash] = bodyPartStatsDict;
            }

            if (!bodyPartStatsDict.TryGetValue(pedBodyPart, out PlayerWeaponBodypartStats? bodyPartStats))
            {
                bodyPartStats = new PlayerWeaponBodypartStats { BodyPart = pedBodyPart, PlayerId = Entity!.Id, WeaponHash = weaponHash };
                bodyPartStatsDict[pedBodyPart] = bodyPartStats;
                Entity.WeaponBodypartStats.Add(bodyPartStats);
            }

            return bodyPartStats;
        }

        private PlayerWeaponStats GetWeaponStats(WeaponHash weaponHash)
        {
            if (!_weaponStats!.TryGetValue(weaponHash, out PlayerWeaponStats? weaponStats))
            {
                weaponStats = new PlayerWeaponStats { WeaponHash = weaponHash, PlayerId = Entity!.Id };
                _weaponStats[weaponHash] = weaponStats;
                Entity.WeaponStats.Add(weaponStats);
            }
            return weaponStats;
        }

        private void LoadWeaponStats()
        {
            if (Entity is null)
                return;
            _weaponBodyPartsStats = Entity.WeaponBodypartStats.GroupBy(e => e.WeaponHash).ToDictionary(e => e.Key, e => e.ToDictionary(w => w.BodyPart, w => w));
            _weaponStats = Entity.WeaponStats.ToDictionary(e => e.WeaponHash, e => e);
        }

        #endregion Private Methods
    }
}
