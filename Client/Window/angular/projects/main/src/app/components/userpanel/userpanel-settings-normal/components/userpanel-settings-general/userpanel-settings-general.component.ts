import { Component, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { EnumSettingRow } from 'projects/main/src/app/modules/settings/models/enum-setting-row';
import { LanguageEnum } from 'projects/main/src/app/enums/language.enum';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { TimezoneEnum } from '../../../enums/timezone.enum';
import { DateTimeEnumSettingRow } from 'projects/main/src/app/modules/settings/models/datetimeenum-setting-row';
import { DateTimeFormatEnum } from '../../../enums/datetime-format.enum';
import { NumberSettingRow } from 'projects/main/src/app/modules/settings/models/number-setting-row';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsGeneral } from '../../interfaces/userpanel-settings-general';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingsGeneralIndex } from '../../enums/settings-general-index.enum';

@Component({
    selector: 'app-userpanel-settings-general',
    templateUrl: './userpanel-settings-general.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserpanelSettingsGeneralComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new EnumSettingRow(LanguageEnum, { 
            defaultValue: LanguageEnum.English, 
            dataSettingIndex: SettingsGeneralIndex.Language, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.Language], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: false, 
            dataSettingIndex: SettingsGeneralIndex.AllowDataTransfer, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.AllowDataTransfer], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "AllowDataTransferSettingInfo"
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: true, 
            dataSettingIndex: SettingsGeneralIndex.ShowConfettiAtRanking, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.ShowConfettiAtRanking], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "ShowConfettiAtRankingSettingInfo"
        }),
        new EnumSettingRow(TimezoneEnum, { 
            defaultValue: TimezoneEnum["(UTC) Coordinated Universal Time"], 
            dataSettingIndex: SettingsGeneralIndex.Timezone, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.Timezone], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new DateTimeEnumSettingRow(DateTimeFormatEnum, { 
            defaultValue: DateTimeFormatEnum["yyyy'-'MM'-'dd HH':'mm':'ss"], 
            dataSettingIndex: SettingsGeneralIndex.DateTimeFormat, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.DateTimeFormat], 
            containerGetter: this.getSettingsContainer.bind(this),
        }),
        new NumberSettingRow({ 
            defaultValue: 0, 
            dataSettingIndex: SettingsGeneralIndex.DiscordUserId, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.DiscordUserId], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "DiscordUserIdSettingInfo", 
            nullable: true},
        0, undefined, true, true),
        new BooleanSliderSettingRow({ 
            defaultValue: true, 
            dataSettingIndex: SettingsGeneralIndex.CheckAFK, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.CheckAFK], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "CheckAFKSettingInfo"
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: true, 
            dataSettingIndex: SettingsGeneralIndex.WindowsNotifications, 
            label: SettingsGeneralIndex[SettingsGeneralIndex.WindowsNotifications], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "WindowsNotificationsInfo"
        }),
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }

    save(obj: UserpanelSettingsGeneral) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.General, obj).subscribe();
    }

    private getSettingsContainer(): UserpanelSettingsGeneral {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.General] as UserpanelSettingsGeneral;
    }
}   