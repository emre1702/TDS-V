import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { killMessagesAnimation } from './animations/kill-messages.animation';
import { WeaponHash } from '../../lobbychoice/enums/weapon-hash.enum';
import { AnimationEvent } from '@angular/animations';
import { KillMessagesService } from './services/kill-messages.service';
import { SettingsService } from '../../../services/settings.service';

@Component({
    selector: 'app-kill-messages',
    templateUrl: './kill-messages.component.html',
    styleUrls: ['./kill-messages.component.scss'],
    animations: [killMessagesAnimation]
})
export class KillMessagesComponent implements OnInit, OnDestroy {

    weaponHash = WeaponHash;
    private killInfoHideTimeouts: NodeJS.Timeout[] = [];

    constructor(
        public killMessagesService: KillMessagesService,
        private changeDetector: ChangeDetectorRef,
        public settings: SettingsService) { }

    onAnimationDoneEvent(event: AnimationEvent) {
        if (typeof event.fromState === "string") {
            return;
        }
        if (event.fromState > event.toState) {
            return;
        }
        const timeout = setTimeout(this.removeFirstDeathInfo.bind(this), this.settings.Settings[11] * 1000);
        this.killInfoHideTimeouts.push(timeout);
    }

    ngOnInit() {
        this.killMessagesService.killInfosChanged.on(null, this.settingsChanged.bind(this));
        this.settings.KillInfoSettingsChanged.on(null, this.settingsChanged.bind(this));
        this.settings.SettingsLoaded.on(null, this.settingsChanged.bind(this));
    }

    ngOnDestroy() {
        this.killMessagesService.killInfosChanged.off(null, this.settingsChanged.bind(this));
        this.settings.KillInfoSettingsChanged.on(null, this.settingsChanged.bind(this));
        this.settings.SettingsLoaded.on(null, this.settingsChanged.bind(this));
    }

    private removeFirstDeathInfo() {
        this.killInfoHideTimeouts.splice(0, 1);
        this.killMessagesService.removeFirstDeathInfo();
    }

    private clearTooLongRunningTimeouts() {
        for (let i = this.killInfoHideTimeouts.length - 1; i >= 0; --i) {
            const timeout = this.killInfoHideTimeouts[i];
            const secLeft = this.getSecLeft(timeout);
            if (secLeft > this.settings.Settings[11]) {
                this.killInfoHideTimeouts.splice(i, 1);
                clearTimeout(timeout);
            }
        }
    }

    private settingsChanged() {
        this.clearTooLongRunningTimeouts();
        this.changeDetector.detectChanges();
    }

    private getSecLeft(timeout: NodeJS.Timeout) {
        const usePrivateTimeout = timeout as any;
        return Math.ceil((usePrivateTimeout._idleStart + usePrivateTimeout._idleTimeout - Date.now()) / 1000);
    }

}
