import { Component, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { SettingsThemeIndex } from '../../enums/settings-theme-index.enum';
import { NumberSliderSettingRow } from 'projects/main/src/app/modules/settings/models/numberslider-setting-row';
import { ColorSettingRow } from 'projects/main/src/app/modules/settings/models/color-setting-row';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsTheme } from '../../interfaces/userpanel-settings-theme';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingChangedEvent } from 'projects/main/src/app/modules/settings/interfaces/setting-changed-event';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-userpanel-settings-theme',
    templateUrl: './userpanel-settings-theme.component.html'
})
export class UserpanelSettingsThemeComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new BooleanSliderSettingRow({ 
            defaultValue: this.settings.Settings[13], 
            dataSettingIndex: SettingsThemeIndex.UseDarkTheme, 
            label: SettingsThemeIndex[SettingsThemeIndex.UseDarkTheme], 
            tooltipLangKey: "UseDarkThemeInfo", 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new NumberSliderSettingRow({
            defaultValue: this.settings.Settings[19], 
            dataSettingIndex: SettingsThemeIndex.ToolbarDesign, 
            label: SettingsThemeIndex[SettingsThemeIndex.ToolbarDesign], 
            tooltipLangKey: "ToolbarDesignInfo", 
            containerGetter: this.getSettingsContainer.bind(this)
        }, 1, 2, 1),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[14], 
            dataSettingIndex: SettingsThemeIndex.ThemeMainColor, 
            label: SettingsThemeIndex[SettingsThemeIndex.ThemeMainColor], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[15], 
            dataSettingIndex: SettingsThemeIndex.ThemeSecondaryColor, 
            label: SettingsThemeIndex[SettingsThemeIndex.ThemeSecondaryColor], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[16], 
            dataSettingIndex: SettingsThemeIndex.ThemeWarnColor, 
            label: SettingsThemeIndex[SettingsThemeIndex.ThemeWarnColor], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[17], 
            dataSettingIndex: SettingsThemeIndex.ThemeBackgroundDarkColor, 
            label: SettingsThemeIndex[SettingsThemeIndex.ThemeBackgroundDarkColor], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[18], 
            dataSettingIndex: SettingsThemeIndex.ThemeBackgroundLightColor, 
            label: SettingsThemeIndex[SettingsThemeIndex.ThemeBackgroundLightColor], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService,
        private settings: SettingsService) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }

    save(obj: UserpanelSettingsTheme) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.Theme, obj).subscribe();
    }

    changed(data: SettingChangedEvent) {
        switch (data.index) {
            case SettingsThemeIndex.UseDarkTheme:
                this.settings.Settings[13] = data.value;
                break;
            case SettingsThemeIndex.ThemeMainColor:
                this.settings.Settings[14] = data.value;
                break;
            case SettingsThemeIndex.ThemeSecondaryColor:
                this.settings.Settings[15] = data.value;
                break;
            case SettingsThemeIndex.ThemeWarnColor:
                this.settings.Settings[16] = data.value;
                break;
            case SettingsThemeIndex.ThemeBackgroundDarkColor:
                this.settings.Settings[17] = data.value;
                break;
            case SettingsThemeIndex.ThemeBackgroundLightColor:
                this.settings.Settings[18] = data.value;
                break;
            case SettingsThemeIndex.ToolbarDesign:
                this.settings.Settings[19] = data.value;
                break;
        }
        this.settings.setThemeChange(data.index, data.value);
    }

    private getSettingsContainer(): UserpanelSettingsTheme {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.Theme] as UserpanelSettingsTheme;
    }
}   