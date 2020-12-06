import { Component, OnInit } from "@angular/core";
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingsHudIndex } from '../../enums/settings-hud-index.enum';
import { HudDesign } from '../../../../hud/enums/hud-design.enum';
import { EnumSettingRow } from 'projects/main/src/app/modules/settings/models/enum-setting-row';
import { UserpanelSettingsHud } from '../../interfaces/userpanel-settings-hud';
import { ColorSettingRow } from 'projects/main/src/app/modules/settings/models/color-setting-row';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { SettingChangedEvent } from 'projects/main/src/app/modules/settings/interfaces/setting-changed-event';

@Component({
    selector: "app-userpanel-settings-hud",
    templateUrl: "./userpanel-settings-hud.component.html"
})
export class UserpanelSettingsHudComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new EnumSettingRow(HudDesign, { 
            defaultValue: this.settings.Settings[20], dataSettingIndex: SettingsHudIndex.HudDesign,
            label: SettingsHudIndex[SettingsHudIndex.HudDesign], 
            tooltipLangKey: "HudDesignInfo",
            containerGetter: () => this.getSettingsContainer(),
            onValueChanged: row => this.hasSelectedAnyHudDesign = row.formControl.value != HudDesign.NoHudDesign
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[21], 
            dataSettingIndex: SettingsHudIndex.HudBackgroundColor, 
            label: SettingsHudIndex[SettingsHudIndex.HudBackgroundColor], 
            nullable: true,
            containerGetter: this.getSettingsContainer.bind(this),
            condition: () => this.hasSelectedAnyHudDesign
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[22], 
            dataSettingIndex: SettingsHudIndex.RoundStatsBackgroundColor, 
            label: SettingsHudIndex[SettingsHudIndex.RoundStatsBackgroundColor], 
            nullable: true,
            containerGetter: this.getSettingsContainer.bind(this),
            condition: () => this.hasSelectedAnyHudDesign
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[23], 
            dataSettingIndex: SettingsHudIndex.HudFontColor, 
            label: SettingsHudIndex[SettingsHudIndex.HudFontColor], 
            nullable: true,
            containerGetter: this.getSettingsContainer.bind(this),
            condition: () => this.hasSelectedAnyHudDesign
        }),
        new ColorSettingRow({
            defaultValue: this.settings.Settings[24], 
            dataSettingIndex: SettingsHudIndex.RoundStatsFontColor, 
            label: SettingsHudIndex[SettingsHudIndex.RoundStatsFontColor], 
            nullable: true,
            containerGetter: this.getSettingsContainer.bind(this),
            condition: () => this.hasSelectedAnyHudDesign
        }),
    ];

    private hasSelectedAnyHudDesign: boolean;
    
    constructor(private userpanelSettingsService: UserpanelSettingsNormalService, private settings: SettingsService) {}

    ngOnInit() {
        this.initializeHasSelectedAnyHudDesign();
    }

    save(obj: UserpanelSettingsHud) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.Hud, obj).subscribe();
    }

    changed(data: SettingChangedEvent) {
        switch (data.index) {
            case SettingsHudIndex.HudDesign: 
                this.settings.Settings[20] = data.value;
                break;
            case SettingsHudIndex.HudBackgroundColor: 
                this.settings.Settings[21] = data.value;
                break;
            case SettingsHudIndex.RoundStatsBackgroundColor: 
                this.settings.Settings[22] = data.value;
                break;
            case SettingsHudIndex.HudFontColor: 
                this.settings.Settings[23] = data.value;
                break;
            case SettingsHudIndex.RoundStatsFontColor: 
                this.settings.Settings[24] = data.value;
                break;
        }
        this.settings.HudSettingsChanged.emit(null);
    }

    private initializeHasSelectedAnyHudDesign() {
        const settingRow = (this.userpanelSettings.find(s => s instanceof SettingRow && s.dataSettingIndex === SettingsHudIndex.HudDesign) as SettingRow);
        settingRow.onValueChanged(settingRow);
    }

    private getSettingsContainer(): UserpanelSettingsHud {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.Hud] as UserpanelSettingsHud;
    }
}
