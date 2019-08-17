import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Validators, FormControl } from '@angular/forms';
import { SettingType } from '../../../enums/setting-type';
import { UserpanelSettingsPanel } from '../interfaces/userpanelSettingsPanel';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { UserpanelSettingKey } from '../enums/userpanel-setting-key.enum';
import { LanguageEnum } from '../../../enums/language.enum';
import { RageConnectorService } from '../../../services/rage-connector.service';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';

@Component({
  selector: 'app-userpanel-settings',
  templateUrl: './userpanel-settings.component.html',
  styleUrls: ['./userpanel-settings.component.scss']
})
export class UserpanelSettingsComponent implements OnInit, OnDestroy {

  userpanelSettingType = SettingType;

  settingPanel: UserpanelSettingsPanel[] = [
    {
      title: "General", rows: [
        {
          type: SettingType.enum, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.Language], defaultValue: LanguageEnum.English,
          enum: LanguageEnum,
          formControl: new FormControl(LanguageEnum.English)
        },
        {
          type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.AllowDataTransfer], defaultValue: false,
          formControl: new FormControl(false)
        },
      ]
    },

    {
      title: "Fight", rows: [
        {
          type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.Bloodscreen], defaultValue: true,
          formControl: new FormControl(true)
        },
        {
          type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.Hitsound], defaultValue: true,
          formControl: new FormControl(true)
        },
        {
          type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.FloatingDamageInfo], defaultValue: true,
          formControl: new FormControl(true)
        }
      ],
    },

    {
      title: "Voice", rows: [
        {
          type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.Voice3D], defaultValue: false,
          formControl: new FormControl(false)
        },
        {
          type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.VoiceAutoVolume], defaultValue: false,
          formControl: new FormControl(false),
          onValueChanged: this.voiceVolumeSettingChanged.bind(this)
        },
        {
          type: SettingType.numberSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.VoiceVolume], defaultValue: 1,
          min: 0, max: 10,
          formControl: new FormControl(1)
        },
      ]
    }
  ];

  constructor(
    public settings: SettingsService,
    private userpanelService: UserpanelService,
    public changeDetector: ChangeDetectorRef,
    private rageConnector: RageConnectorService) { }

  ngOnInit() {
    this.userpanelService.settingsLoaded.on(null, this.loadSettings.bind(this));
  }

  ngOnDestroy() {
    this.userpanelService.settingsLoaded.off(null, this.loadSettings.bind(this));
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
    for (const group of this.settingPanel) {
      for (const row of group.rows) {
        this.userpanelService.allSettings[row.dataSettingIndex] = row.formControl.value;
      }
    }

    const json = JSON.stringify(this.userpanelService.allSettings);
    this.rageConnector.call(DToServerEvent.SaveSettings, json);
  }

  revertAll() {
    for (const group of this.settingPanel) {
      for (const row of group.rows) {
        this.userpanelService.allSettings[row.dataSettingIndex] = row.initialValue;
      }
    }
    this.changeDetector.detectChanges();
  }

  setDefault() {
    for (const group of this.settingPanel) {
      for (const row of group.rows) {
        this.userpanelService.allSettings[row.dataSettingIndex] = row.defaultValue;
      }
    }
    this.changeDetector.detectChanges();
  }

  private loadSettings() {
    for (const group of this.settingPanel) {
      for (const row of group.rows) {
        const value = this.userpanelService.allSettings[row.dataSettingIndex];
        row.formControl.setValue(value);
        row.initialValue = value;
      }
    }
    this.changeDetector.detectChanges();
  }

  private getFormControl(title: string, setting: UserpanelSettingKey) {
    return this.settingPanel
              .filter(p => p.title === title)
              .map(p => p.rows.filter(row => row.dataSettingIndex === UserpanelSettingKey[setting])[0])
              .map(r => r.formControl)[0];
  }

  getEnumKeys(e: {}) {
    const keys = Object.keys(e);
    return keys.slice(keys.length / 2);
  }
}
