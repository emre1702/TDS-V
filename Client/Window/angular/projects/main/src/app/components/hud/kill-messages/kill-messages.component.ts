import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { killMessagesAnimation } from './animations/kill-messages.animation';
import { DeathInfoData } from './interfaces/death-info-data';
import { WeaponHash } from '../../lobbychoice/enums/weapon-hash.enum';
import { AnimationEvent } from '@angular/animations';
import { KillMessagesService } from './services/kill-messages.service';
import { SettingsService } from '../../../services/settings.service';

declare const window: any;

@Component({
    selector: 'app-kill-messages',
    templateUrl: './kill-messages.component.html',
    styleUrls: ['./kill-messages.component.scss'],
    animations: [killMessagesAnimation]
})
export class KillMessagesComponent implements OnInit, OnDestroy {

    weaponHash = WeaponHash;

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
        setTimeout(this.killMessagesService.removeFirstDeathInfo.bind(this), 10000);
    }

    ngOnInit() {
        this.killMessagesService.killInfosChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.killMessagesService.killInfosChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }

}
