import { Component, OnInit, OnDestroy, ChangeDetectorRef, Input } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { FromClientEvent } from '../../enums/from-client-event.enum';
import { HUDDataType } from './enums/huddatatype.enum';
import { FiringMode } from './enums/firingmode.enum';
import { HudDesign } from './enums/hud-design.enum';

@Component({
    selector: 'app-hud',
    templateUrl: './hud.component.html',
    styleUrls: ['./hud.component.scss'],
})
export class HudComponent implements OnInit, OnDestroy {
    showRoundStats = true;

    hpRectPercentage = 0;
    armorRectPercentage = 0;

    armor = 0;
    hp = 0;
    money = 0;
    ammoInClip = 0;
    ammoTotal = 0;
    firingMode = FiringMode[0];
    hudDesign = HudDesign;

    @Input() hudShowing: boolean;
    @Input() rankingShowing: boolean;

    constructor(public settings: SettingsService, private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit() {
        this.rageConnector.listen(FromClientEvent.ToggleRoundStats, this.toggleRoundStats.bind(this));
        this.rageConnector.listen(FromClientEvent.SyncHUDDataChange, this.hudDataChange.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
        this.settings.HudSettingsChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(FromClientEvent.ToggleRoundStats, this.toggleRoundStats.bind(this));
        this.rageConnector.remove(FromClientEvent.SyncHUDDataChange, this.hudDataChange.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
        this.settings.HudSettingsChanged.off(null, this.detectChanges.bind(this));
    }

    toggleRoundStats(toggle: boolean) {
        this.showRoundStats = toggle;
        this.changeDetector.detectChanges();
    }

    private hudDataChange(type: HUDDataType, value: number) {
        switch (type) {
            case HUDDataType.Armor:
                this.armor = value;
                this.armorRectPercentage = Math.min(value, 100);
                break;
            case HUDDataType.HP:
                this.hp = value;
                this.hpRectPercentage = Math.min(100, value);
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

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
