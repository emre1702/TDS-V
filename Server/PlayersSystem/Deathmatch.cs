using GTANetworkAPI;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.PlayersSystem
{
    public class Deathmatch : IPlayerDeathmatch
    {
        private short _killingSpree;
        private short _shortTimeKillingSpree;

        private readonly ISettingsHandler _settingsHandler;

#nullable disable
        private ITDSPlayer _player;
        private IPlayerEvents _events;
#nullable enable

        public Deathmatch(ISettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;
        }

        public void Init(ITDSPlayer player, IPlayerEvents events)
        {
            _player = player;

            events.WeaponSwitch += Events_WeaponSwitch;
            events.Removed += Events_Removed;
        }

        private void Events_Removed()
        {
            _events.WeaponSwitch -= Events_WeaponSwitch;
            _events.Removed -= Events_Removed;
        }

        public short KillingSpree
        {
            get => _killingSpree;
            set
            {
                if (_killingSpree + 1 == value)
                {
                    ++_shortTimeKillingSpree;
                }
                _killingSpree = value;
                if (_player.Lobby?.IsOfficial == true)
                    _player.Challenges.AddToChallenge(ChallengeType.Killstreak, _killingSpree, true);
            }
        }

        public ITDSPlayer? LastHitter { get; set; }
        public DateTime? LastKillAt { get; set; }
        public WeaponHash LastWeaponOnHand { get; set; } = WeaponHash.Unarmed;

        public short ShortTimeKillingSpree
        {
            get
            {
                if (LastKillAt is null)
                    return _shortTimeKillingSpree;

                var timeSpanSinceLastKill = DateTime.UtcNow - LastKillAt.Value;
                if (timeSpanSinceLastKill.TotalSeconds <= _settingsHandler.ServerSettings.KillingSpreeMaxSecondsUntilNextKill)
                {
                    return _shortTimeKillingSpree;
                }
                _shortTimeKillingSpree = 1;
                return 1;
            }
        }

        private void Events_WeaponSwitch(WeaponHash previousWeapon, WeaponHash newWeapon)
        {
            LastWeaponOnHand = newWeapon;
        }
    }
}
