using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.StartRequirements;
using TDS.Shared.Core;

namespace TDS.Server.GangActionAreaSystem.StartRequirements
{
    public class BaseAreaStartRequirements : IBaseGangActionAreaStartRequirements
    {
        public bool HasCooldown
        {
            get => _cooldownTimer is { IsRunning: true };
            set
            {
                if (value)
                    SetHasCooldown(_settingsHandler.ServerSettings.GangwarAreaAttackCooldownMinutes);
                else
                    SetHasNoCooldown();
            }
        }

        #nullable disable
        protected IBaseGangActionArea Area { get; private set; }
        #nullable restore

        private TDSTimer? _cooldownTimer;
        private readonly ISettingsHandler _settingsHandler;

        public BaseAreaStartRequirements(ISettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;
        }

        public void Init(IBaseGangActionArea area)
        {
            Area = area;
            CheckCooldownOnInit();
        }

        private void CheckCooldownOnInit()
        {
            if (Area.DatabaseHandler.Entity?.CooldownStartTime is not { } cooldown)
                return;

            var timeSinceCooldownStart = DateTime.UtcNow - cooldown;
            if (timeSinceCooldownStart.TotalMinutes >= _settingsHandler.ServerSettings.GangwarAreaAttackCooldownMinutes)
            {
                Area.DatabaseHandler.Entity.CooldownStartTime = null;
                return;
            }

            var cooldownLeftMinutes = (int)Math.Floor(_settingsHandler.ServerSettings.GangwarAreaAttackCooldownMinutes - timeSinceCooldownStart.TotalMinutes);
            SetHasCooldown(cooldownLeftMinutes);
        }

        private void SetHasCooldown(int minutes)
        {
            _cooldownTimer?.Kill();
            _cooldownTimer = new TDSTimer(OnCooldownEnded, (uint)minutes);
            if (Area.DatabaseHandler.Entity is { })
            {
                var timeAlreadyElapsedMinutes = _settingsHandler.ServerSettings.GangwarAreaAttackCooldownMinutes - minutes;
                Area.DatabaseHandler.Entity.CooldownStartTime = DateTime.UtcNow.AddMinutes(-timeAlreadyElapsedMinutes);
            }
            Area.Events.TriggerCooldownStarted();
        }

        private void SetHasNoCooldown()
        {
            _cooldownTimer?.Kill();
            _cooldownTimer = null;
            if (Area.DatabaseHandler.Entity is { })
                Area.DatabaseHandler.Entity.CooldownStartTime = null;
            Area.Events.TriggerCooldownEnded();
        }

        private void OnCooldownEnded()
        {
            HasCooldown = false;
        }
    }
}
