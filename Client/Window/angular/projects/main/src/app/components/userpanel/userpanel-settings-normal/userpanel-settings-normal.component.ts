import { Component, OnInit, ChangeDetectorRef, OnDestroy, ViewContainerRef, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SettingType } from '../../../enums/setting-type';
import { UserpanelSettingsPanel } from '../interfaces/userpanelSettingsPanel';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { UserpanelSettingKey } from '../enums/userpanel-setting-key.enum';
import { LanguageEnum } from '../../../enums/language.enum';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { UserpanelSettingRow } from '../interfaces/userpanelSettingRow';
import { DToClientEvent } from '../../../enums/dtoclientevent.enum';
import { TimezoneEnum } from '../enums/timezone.enum';
import { DateTimeFormatEnum } from '../enums/datetime-format.enum';

@Component({
    selector: 'app-userpanel-settings-normal',
    templateUrl: './userpanel-settings-normal.component.html',
    styleUrls: ['./userpanel-settings-normal.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserpanelSettingsNormalComponent implements OnInit, OnDestroy {

    userpanelSettingType = SettingType;
    currentDate: Date;
    userpanelSettingKey = UserpanelSettingKey;

    // CARE:
    // nullable only implemented for Colors yet

    settingPanel: UserpanelSettingsPanel[] = [
        {
            title: "General", rows: [
                {
                    type: SettingType.enum, dataSettingIndex: UserpanelSettingKey.Language, defaultValue: LanguageEnum.English,
                    enum: LanguageEnum, nullable: false,
                    formControl: new FormControl(LanguageEnum.English)
                },
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.AllowDataTransfer, defaultValue: false,
                    formControl: new FormControl(false), nullable: false,
                    tooltipLangKey: "AllowDataTransferSettingInfo"
                },
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.ShowConfettiAtRanking, defaultValue: true,
                    formControl: new FormControl(true), nullable: false,
                    tooltipLangKey: "ShowConfettiAtRankingSettingInfo"
                },
                {
                    type: SettingType.enum, dataSettingIndex: UserpanelSettingKey.Timezone,
                    defaultValue: TimezoneEnum["(UTC) Coordinated Universal Time"], enum: TimezoneEnum,
                    formControl: new FormControl(TimezoneEnum["(UTC) Coordinated Universal Time"]),
                    nullable: false,
                },
                {
                    type: SettingType.dateTimeFormatEnum, dataSettingIndex: UserpanelSettingKey.DateTimeFormat,
                    defaultValue: DateTimeFormatEnum[""], enum: DateTimeFormatEnum, nullable: false,
                    formControl: new FormControl(DateTimeFormatEnum["yyyy'-'MM'-'dd HH':'mm':'ss"])
                },
                {
                    type: SettingType.number, dataSettingIndex: UserpanelSettingKey.DiscordUserId, defaultValue: 0,
                    formControl: new FormControl(0), nullable: false,
                    tooltipLangKey: "DiscordUserIdSettingInfo"
                }
            ]
        },

        {
            title: "Fight", rows: [
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.Bloodscreen, defaultValue: true,
                    formControl: new FormControl(true), nullable: false,
                },
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.Hitsound, defaultValue: true,
                    formControl: new FormControl(true), nullable: false,
                },
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.FloatingDamageInfo, defaultValue: true,
                    formControl: new FormControl(true), nullable: false,
                },
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.CheckAFK, defaultValue: true,
                    formControl: new FormControl(true), nullable: false,
                    tooltipLangKey: "CheckAFKSettingInfo"
                }
            ],
        },

        {
            title: "Voice", rows: [
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.Voice3D, defaultValue: false,
                    formControl: new FormControl(false), nullable: false,
                },
                {
                    type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey.VoiceAutoVolume, defaultValue: false,
                    formControl: new FormControl(false),
                    onValueChanged: this.voiceVolumeSettingChanged.bind(this),
                    nullable: false,
                },
                {
                    type: SettingType.numberSlider, dataSettingIndex: UserpanelSettingKey.VoiceVolume, defaultValue: 1,
                    min: 0, max: 10, nullable: false,
                    formControl: new FormControl(1)
                },
            ]
        },

        {
            title: "Colors", rows: [
                {
                    type: SettingType.color, dataSettingIndex: UserpanelSettingKey.MapBorderColor, defaultValue: "rgba(150,0,0,0.35)",
                    formControl: new FormControl("rgba(150,0,0,0.35)"), nullable: false,
                },
                {
                    type: SettingType.color, dataSettingIndex: UserpanelSettingKey.NametagDeadColor, defaultValue: "rgba(0, 0, 0, 1)",
                    formControl: new FormControl("rgba(0, 0, 0, 1)"), nullable: true,
                    tooltipLangKey: "NametagDeadColorSettingInfo"
                },
                {
                    type: SettingType.color, dataSettingIndex: UserpanelSettingKey.NametagHealthEmptyColor, defaultValue: "rgba(50, 0, 0, 1)",
                    formControl: new FormControl("rgba(50, 0, 0, 1)"), nullable: false,
                    tooltipLangKey: "NametagHealthEmptyColorSettingInfo"
                },
                {
                    type: SettingType.color, dataSettingIndex: UserpanelSettingKey.NametagHealthFullColor, defaultValue: "rgba(0, 255, 0, 1)",
                    formControl: new FormControl("rgba(0, 255, 0, 1)"), nullable: false,
                    tooltipLangKey: "NametagHealthFullColorSettingInfo"
                },
                {
                    type: SettingType.color, dataSettingIndex: UserpanelSettingKey.NametagArmorEmptyColor, defaultValue: undefined,
                    formControl: new FormControl(undefined), nullable: true,
                    tooltipLangKey: "NametagArmorEmptyColorSettingInfo"
                },
                {
                    type: SettingType.color, dataSettingIndex: UserpanelSettingKey.NametagArmorFullColor, defaultValue: "rgba(255, 255, 255, 1)",
                    formControl: new FormControl("rgba(255, 255, 255, 1)"), nullable: false,
                    tooltipLangKey: "NametagArmorFullColorSettingInfo"
                },
            ]
        },

        {
            title: "Times", rows: [
                {
                    type: SettingType.number, dataSettingIndex: UserpanelSettingKey.BloodscreenCooldownMs, defaultValue: 150,
                    formControl: new FormControl(150), min: 0, max: 1000000,
                    onlyInt: true, tooltipLangKey: "BloodscreenCooldownMsSettingInfo", nullable: false,
                },
                {
                    type: SettingType.number, dataSettingIndex: UserpanelSettingKey.HudAmmoUpdateCooldownMs, defaultValue: 100,
                    formControl: new FormControl(100), min: -1, max: 1000000,
                    onlyInt: true, tooltipLangKey: "HudAmmoUpdateCooldownMsSettingInfo", nullable: false,
                },
                {
                    type: SettingType.number, dataSettingIndex: UserpanelSettingKey.HudHealthUpdateCooldownMs, defaultValue: 100,
                    formControl: new FormControl(100), min: -1, max: 1000000,
                    onlyInt: true, tooltipLangKey: "HudHealthUpdateCooldownMsSettingInfo", nullable: false,
                },
                {
                    type: SettingType.number, dataSettingIndex: UserpanelSettingKey.AFKKickAfterSeconds, defaultValue: 25,
                    formControl: new FormControl(25), min: 0, max: 1000000,
                    onlyInt: true, tooltipLangKey: "AFKKickAfterSecondsSettingInfo", nullable: false,
                },
                {
                    type: SettingType.number, dataSettingIndex: UserpanelSettingKey.AFKKickShowWarningLastSeconds, defaultValue: 10,
                    formControl: new FormControl(10), min: 0, max: 1000000,
                    onlyInt: true, tooltipLangKey: "AFKKickShowWarningLastSecondsSettingInfo", nullable: false,
                },
                {
                    type: SettingType.number, dataSettingIndex: UserpanelSettingKey.ShowFloatingDamageInfoDurationMs, defaultValue: 1000,
                    formControl: new FormControl(1000), min: 0, max: 1000000,
                    onlyInt: true, tooltipLangKey: "ShowFloatingDamageInfoDurationMsSettingInfo", nullable: false,
                }
            ]
        }
    ];

    constructor(
        public settings: SettingsService,
        private userpanelService: UserpanelService,
        public changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }

    ngOnInit() {
        this.userpanelService.settingsNormalLoaded.on(null, this.loadSettings.bind(this));

        if (this.userpanelService.allSettingsNormal)
            this.loadSettings();

        // this.currentDate = new Date();
        this.currentDate = new Date(2019, 5, 25, 15, 20, 40);
    }

    ngOnDestroy() {
        this.userpanelService.settingsNormalLoaded.off(null, this.loadSettings.bind(this));
    }

    private voiceVolumeSettingChanged() {
        const autoVolumeControl = this.getFormControl("Voice", UserpanelSettingKey.VoiceAutoVolume);
        const volumeControl = this.getFormControl("Voice", UserpanelSettingKey.VoiceVolume);

        if (autoVolumeControl.value === true) {
            volumeControl.disable();
        } else {
            volumeControl.enable();
        }
        this.changeDetector.detectChanges();
    }

    save() {
        const discordUserIdControl = this.getFormControl("General", UserpanelSettingKey.DiscordUserId);
        if (discordUserIdControl.value == "") {
            discordUserIdControl.setValue(0);
        }
        for (const group of this.settingPanel) {
            for (const row of group.rows) {
                this.userpanelService.allSettingsNormal[row.dataSettingIndex] = row.formControl.value;
            }
        }

        const json = JSON.stringify(this.userpanelService.allSettingsNormal);
        this.rageConnector.callServer(DToServerEvent.SaveSettings, json);

        this.userpanelService.myStatsLoadingCooldownEnded();
    }

    revertAll() {
        for (const group of this.settingPanel) {
            for (const row of group.rows) {
                this.userpanelService.allSettingsNormal[row.dataSettingIndex] = row.initialValue;
                row.formControl.setValue(row.initialValue, { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
            }
        }
        this.changeDetector.detectChanges();
    }

    setDefault() {
        for (const group of this.settingPanel) {
            for (const row of group.rows) {
                this.userpanelService.allSettingsNormal[row.dataSettingIndex] = row.defaultValue;
                row.formControl.setValue(row.defaultValue, { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
            }
        }
        this.changeDetector.detectChanges();
    }

    private loadSettings() {
        for (const group of this.settingPanel) {
            for (const row of group.rows) {
                const value = this.userpanelService.allSettingsNormal[row.dataSettingIndex];
                row.formControl.setValue(value, { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
                row.initialValue = value;
            }
        }
        this.changeDetector.detectChanges();
    }

    private getFormControl(title: string, setting: UserpanelSettingKey) {
        return this.settingPanel
            .filter(p => p.title === title)
            .map(p => p.rows.filter(row => row.dataSettingIndex == setting)[0])
            .map(r => r.formControl)[0];
    }

    getEnumKeys(e: {}) {
        return Object.keys(e).filter(x => !(parseInt(x, 10) >= 0));
    }

    onColorChange(setting: UserpanelSettingRow) {
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.OnColorSettingChange, setting.formControl.value, setting.dataSettingIndex);
    }
}
