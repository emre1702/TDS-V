import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { SettingsVoiceIndex } from '../../enums/settings-voice-index.enum';
import { NumberSliderSettingRow } from 'projects/main/src/app/modules/settings/models/numberslider-setting-row';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsVoice } from '../../interfaces/userpanel-settings-voice';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';

@Component({
    selector: 'app-userpanel-settings-voice',
    templateUrl: './userpanel-settings-voice.component.html'
})
export class UserpanelSettingsVoiceComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new BooleanSliderSettingRow({ 
            defaultValue: false, dataSettingIndex: SettingsVoiceIndex.Voice3D, 
            label: SettingsVoiceIndex[SettingsVoiceIndex.Voice3D], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: false, dataSettingIndex: SettingsVoiceIndex.VoiceAutoVolume, 
            label: SettingsVoiceIndex[SettingsVoiceIndex.VoiceAutoVolume], 
            containerGetter: this.getSettingsContainer.bind(this),
            onValueChanged: this.voiceVolumeSettingChanged.bind(this),
        }),
        new NumberSliderSettingRow({
            defaultValue: 1, dataSettingIndex: SettingsVoiceIndex.VoiceVolume, 
            label: SettingsVoiceIndex[SettingsVoiceIndex.VoiceVolume], 
            containerGetter: this.getSettingsContainer.bind(this)
        }, 0, 10),
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService,
        private changeDetector: ChangeDetectorRef) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }

    save(obj: UserpanelSettingsVoice) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.Voice, obj).subscribe();
    }

    private voiceVolumeSettingChanged(row: SettingRow) {
        const volumeRow = this.userpanelSettings.find(s => s instanceof SettingRow && s.dataSettingIndex == SettingsVoiceIndex.VoiceVolume) as SettingRow;

        if (row.formControl.value === true) {
            volumeRow.formControl.disable();
        } else {
            volumeRow.formControl.enable();
        }
        this.changeDetector.detectChanges();
    }

    private getSettingsContainer(): UserpanelSettingsVoice {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.Voice] as UserpanelSettingsVoice;
    }
}   