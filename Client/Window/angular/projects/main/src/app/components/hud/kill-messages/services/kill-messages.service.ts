import { Injectable } from '@angular/core';
import { DeathInfoData } from '../interfaces/death-info-data';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../../enums/dfromclientevent.enum';
import { EventEmitter } from 'events';
import { WeaponHash } from '../../../lobbychoice/enums/weapon-hash.enum';
import { KillInfosSettings } from '../../../../interfaces/kill-infos-settings';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { UserpanelSettingKey } from '../../../userpanel/enums/userpanel-setting-key.enum';

@Injectable({
    providedIn: 'root'
})
export class KillMessagesService {
    killInfos: DeathInfoData[] = [];
    killInfosChanged = new EventEmitter();
    killInfoSettings: KillInfosSettings;

    constructor(private rageConnector: RageConnectorService, private settings: SettingsService) {
        this.rageConnector.listen(DFromClientEvent.AddKillMessage, this.addDeathInfo.bind(this));
        this.killInfoSettings = this.settings.KillInfoSettings;
        this.settings.KillInfoSettingChanged.on(null, this.killInfoSettingChanged.bind(this));
        this.settings.KillInfoSettingsLoaded.on(null, this.killInfoSettingsLoaded.bind(this));
    }

    private addDeathInfo(deathInfoJson: string) {
        const deathInfo = JSON.parse(deathInfoJson);
        this.killInfos.push(deathInfo);
        this.killInfosChanged.emit(null);
    }

    removeFirstDeathInfo() {
        this.killInfos.splice(0, 1);
        this.killInfosChanged.emit(null);
    }

    addTestDeathInfo() {
        const deathInfo: DeathInfoData = {
            0: "Bonus",
            1: this.settings.Constants[7],
            2: this.getRandomWeaponHash()
        };
        this.killInfos.push(deathInfo);
        this.killInfosChanged.emit(null);
    }

    private killInfoSettingChanged(key: UserpanelSettingKey, value: any) {
        this.killInfoSettings[key] = value;
        this.killInfosChanged.emit(null);
    }

    private killInfoSettingsLoaded(settings: KillInfosSettings) {
        this.killInfoSettings = settings;
        this.killInfosChanged.emit(null);
    }

    private getRandomWeaponHash(): number {
        const enumValues = Object.keys(WeaponHash)
            .map(n => Number.parseInt(n, 10))
            .filter(n => !Number.isNaN(n));
        const randomIndex = Math.floor(Math.random() * enumValues.length);
        const randomEnumValue = enumValues[randomIndex];
        return randomEnumValue;
    }
}
