using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.StartRequirements;
using TDS.Server.Data.Interfaces.GangsSystem;
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
                    SetHasCooldown(_settingsHandler.ServerSettings.GangActionAreaAttackCooldownMinutes);
                else
                    SetHasNoCooldown();
            }
        }

        private int CooldownMinutes => HasCooldown ? (int)Math.Ceiling(_cooldownTimer!.RemainingMsToExecute / 60000f) : 0;

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

        public bool CheckIsAttackable(ITDSPlayer outputTo) 
        {
            if (HasCooldown)
            {
                outputTo.SendNotification(string.Format(outputTo.Language.GANG_ACTION_AREA_IN_COOLDOWN, CooldownMinutes));
                return false;
            }

            if (Area.GangsHandler.Owner?.Action.InAction == true)
            {
                outputTo.SendNotification(outputTo.Language.GANG_ACTION_AREA_OWNER_IN_ACTION);
                return false;
            }

            if (Area.GangsHandler.Owner is { } && Area.GangsHandler.Owner.Players.CountOnline < _settingsHandler.ServerSettings.MinPlayersOnlineForGangAction)
            {
                outputTo.SendNotification(
                    string.Format(outputTo.Language.NOT_ENOUGH_PLAYERS_ONLINE_IN_TARGET_GANG, _settingsHandler.ServerSettings.MinPlayersOnlineForGangAction));
                return false;
            }

            return true;
        }

        private void CheckCooldownOnInit()
        {
            if (Area.DatabaseHandler.Entity?.CooldownStartTime is not { } cooldown)
                return;

            var timeSinceCooldownStart = DateTime.UtcNow - cooldown;
            if (timeSinceCooldownStart.TotalMinutes >= _settingsHandler.ServerSettings.GangActionAreaAttackCooldownMinutes)
            {
                Area.DatabaseHandler.Entity.CooldownStartTime = null;
                return;
            }

            var cooldownLeftMinutes = (int)Math.Floor(_settingsHandler.ServerSettings.GangActionAreaAttackCooldownMinutes - timeSinceCooldownStart.TotalMinutes);
            SetHasCooldown(cooldownLeftMinutes);
        }

        private void SetHasCooldown(int minutes)
        {
            _cooldownTimer?.Kill();
            _cooldownTimer = new TDSTimer(OnCooldownEnded, (uint)minutes);
            if (Area.DatabaseHandler.Entity is { })
            {
                var timeAlreadyElapsedMinutes = _settingsHandler.ServerSettings.GangActionAreaAttackCooldownMinutes - minutes;
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
