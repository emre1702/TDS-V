import { Component, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsFightEffect } from '../../interfaces/userpanel-settings-fight-effect';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingsFightEffectIndex } from '../../enums/settings-fight-effect-index.enum';

@Component({
    selector: 'app-userpanel-settings-fight-effect',
    templateUrl: './userpanel-settings-fight-effect.component.html'
})
export class UserpanelSettingsFightEffectComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new BooleanSliderSettingRow({ 
            defaultValue: true, dataSettingIndex: SettingsFightEffectIndex.Bloodscreen, 
            label: SettingsFightEffectIndex[SettingsFightEffectIndex.Bloodscreen], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: true, dataSettingIndex: SettingsFightEffectIndex.Hitsound, 
            label: SettingsFightEffectIndex[SettingsFightEffectIndex.Hitsound], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: true, dataSettingIndex: SettingsFightEffectIndex.FloatingDamageInfo, 
            label: SettingsFightEffectIndex[SettingsFightEffectIndex.FloatingDamageInfo], 
            containerGetter: this.getSettingsContainer.bind(this)
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
    
    save(obj: UserpanelSettingsFightEffect) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.FightEffect, obj).subscribe();
    }

    private getSettingsContainer(): UserpanelSettingsFightEffect {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.FightEffect] as UserpanelSettingsFightEffect;
    }
}   