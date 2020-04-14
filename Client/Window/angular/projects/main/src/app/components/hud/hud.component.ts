import { Component, OnInit, OnDestroy, ChangeDetectorRef, Input, ChangeDetectionStrategy } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { HUDDataType } from './enums/huddatatype.enum';
import { FiringMode } from './enums/firingmode.enum';

@Component({
    selector: 'app-hud',
    templateUrl: './hud.component.html',
    styleUrls: ['./hud.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class HudComponent implements OnInit, OnDestroy {

    showRoundStats = true;

    armor = 0;
    armorExtra = 0;
    hp = 0;
    money = 0;
    ammoInClip = 0;
    ammoTotal = 0;
    firingMode = FiringMode[0];

    @Input() rankingShowing: boolean;

    constructor(
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef) {

    }

    ngOnInit() {
        this.rageConnector.listen(DFromClientEvent.ToggleRoundStats, this.toggleRoundStats.bind(this));
        this.rageConnector.listen(DFromClientEvent.SyncHUDDataChange, this.hudDataChange.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromClientEvent.ToggleRoundStats, this.toggleRoundStats.bind(this));
        this.rageConnector.remove(DFromClientEvent.SyncHUDDataChange, this.hudDataChange.bind(this));
    }

    toggleRoundStats(toggle: boolean) {
        this.showRoundStats = toggle;
        this.changeDetector.detectChanges();
    }

    private hudDataChange(type: HUDDataType, value: number) {
        switch (type) {
            case HUDDataType.Armor:
                this.armor = Math.min(value, 100);
                this.armorExtra = Math.max(value - this.armor, 0);
                break;
            case HUDDataType.HP:
                this.hp = value;
                break;
            case HUDDataType.Money:
                this.money = value;
                break;
            case HUDDataType.AmmoInClip:
                this.ammoInClip = value;
                break;
            case HUDDataType.AmmoTotal:
                this.ammoTotal = value;
                break;
            case HUDDataType.FiringMode:
                this.firingMode = FiringMode[value];
                break;
        }
        this.changeDetector.detectChanges();
    }
}