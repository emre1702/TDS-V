using AltV.Net.Data;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Properties

        public Dictionary<WeaponHash, Dictionary<BodyPart, PlayerWeaponBodypartStats>>? WeaponBodyPartsStats { get; private set; }
        public Dictionary<WeaponHash, PlayerWeaponStats>? WeaponStats { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Called if we deal damage and if we only shoot.
        /// </summary>
        /// <param name="weaponHash"></param>
        /// <param name="pedBodyPart">Null if coming from shooting (not damage)</param>
        /// <param name="damage">Null if coming from shooting (not damage)</param>
        public void AddWeaponShot(WeaponHash weaponHash, BodyPart? bodyPart, int? damage, bool killed)
        {
            if (Entity is null)
                return;
            if (WeaponStats is null)
                return;
            if (WeaponBodyPartsStats is null)
                return;

            bool isOfficialLobby = Lobby?.IsOfficial == true;
            var weaponStats = GetWeaponStats(weaponHash);

            // If we deal damage, we also shoot To prevent double adding, don't add to AmountShots
            // if bodyPart is {} (so we dealt damage)
            if (!bodyPart.HasValue || damage is null)
            {
                ++weaponStats.AmountShots;
                if (isOfficialLobby)
                    ++weaponStats.AmountOfficialShots;
                return;
            }

            var bodyPartStats = GetWeaponBodyPartStats(weaponHash, bodyPart.Value);

            ++weaponStats.AmountHits;
            weaponStats.DealtDamage += damage.Value;

            ++bodyPartStats.AmountHits;
            bodyPartStats.DealtDamage += damage.Value;

            if (killed)
            {
                ++weaponStats.Kills;
                ++bodyPartStats.Kills;
            }

            if (bodyPart == BodyPart.Head)
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

                if (bodyPart == BodyPart.Head)
                {
                    ++weaponStats.AmountOfficialHeadshots;
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private PlayerWeaponBodypartStats GetWeaponBodyPartStats(WeaponHash weaponHash, BodyPart bodyPart)
        {
            if (!WeaponBodyPartsStats!.TryGetValue(weaponHash, out Dictionary<BodyPart, PlayerWeaponBodypartStats>? bodyPartStatsDict))
            {
                bodyPartStatsDict = new Dictionary<BodyPart, PlayerWeaponBodypartStats>();
                WeaponBodyPartsStats[weaponHash] = bodyPartStatsDict;
            }

            if (!bodyPartStatsDict.TryGetValue(bodyPart, out PlayerWeaponBodypartStats? bodyPartStats))
            {
                bodyPartStats = new PlayerWeaponBodypartStats { BodyPart = bodyPart, PlayerId = Entity!.Id, WeaponHash = weaponHash };
                bodyPartStatsDict[bodyPart] = bodyPartStats;
                Entity.WeaponBodypartStats.Add(bodyPartStats);
            }

            return bodyPartStats;
        }

        private PlayerWeaponStats GetWeaponStats(WeaponHash weaponHash)
        {
            if (!WeaponStats!.TryGetValue(weaponHash, out PlayerWeaponStats? weaponStats))
            {
                weaponStats = new PlayerWeaponStats { WeaponHash = weaponHash, PlayerId = Entity!.Id };
                WeaponStats[weaponHash] = weaponStats;
                Entity.WeaponStats.Add(weaponStats);
            }
            return weaponStats;
        }

        private void LoadWeaponStats()
        {
            if (Entity is null)
                return;
            WeaponBodyPartsStats = Entity.WeaponBodypartStats.GroupBy(e => e.WeaponHash).ToDictionary(e => e.Key, e => e.ToDictionary(w => w.BodyPart, w => w));
            WeaponStats = Entity.WeaponStats.ToDictionary(e => e.WeaponHash, e => e);
        }

        #endregion Private Methods
    }
}
