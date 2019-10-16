import { Component, OnInit, ChangeDetectorRef, OnDestroy, ViewContainerRef } from '@angular/core';
import {  FormControl } from '@angular/forms';
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
        {
          type: SettingType.booleanSlider, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.ShowConfettiAtRanking], defaultValue: true,
          formControl: new FormControl(true)
        },
        {
          type: SettingType.string, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.DiscordIdentity], defaultValue: "",
          formControl: new FormControl(""), placeholder: "Name#ID"
        }
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
    },

    {
      title: "Graphical", rows: [
        {
          type: SettingType.color, dataSettingIndex: UserpanelSettingKey[UserpanelSettingKey.MapBorderColor], defaultValue: "rgba(150,0,0,0.35)",
          formControl: new FormControl("rgba(150,0,0,0.35)")
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

    if (this.userpanelService.allSettings)
      this.loadSettings();
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
        row.formControl.setValue(row.initialValue, { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
      }
    }
    this.changeDetector.detectChanges();
  }

  setDefault() {
    for (const group of this.settingPanel) {
      for (const row of group.rows) {
        this.userpanelService.allSettings[row.dataSettingIndex] = row.defaultValue;
        row.formControl.setValue(row.defaultValue, { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
      }
    }
    this.changeDetector.detectChanges();
  }

  private loadSettings() {
    for (const group of this.settingPanel) {
      for (const row of group.rows) {
        const value = this.userpanelService.allSettings[row.dataSettingIndex];
        row.formControl.setValue(value, { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
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

  onColorChange(setting: UserpanelSettingRow) {
    this.changeDetector.detectChanges();

    this.rageConnector.call(DToClientEvent.OnColorSettingChange, setting.formControl.value, setting.dataSettingIndex);
  }
}
