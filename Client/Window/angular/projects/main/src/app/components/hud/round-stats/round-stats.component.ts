import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { FromServerEvent } from '../../../enums/dfromserverevent.enum';
import { ClipboardService } from 'ngx-clipboard';
import { HudDesign } from '../enums/hud-design.enum';

@Component({
    selector: 'app-round-stats',
    templateUrl: './round-stats.component.html',
    styleUrls: ['./round-stats.component.scss'],
})
export class RoundStatsComponent implements OnInit, OnDestroy {
    hudDesign = HudDesign;

    kills = 0;
    assists = 0;
    damage = 0;

    constructor(
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef,
        private clipboardService: ClipboardService
    ) {}

    ngOnInit() {
        this.rageConnector.listen(FromServerEvent.SetKillsForRoundStats, this.setKills.bind(this));
        this.rageConnector.listen(FromServerEvent.SetAssistsForRoundStats, this.setAssists.bind(this));
        this.rageConnector.listen(FromServerEvent.SetDamageForRoundStats, this.setDamage.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(FromServerEvent.SetKillsForRoundStats, this.setKills.bind(this));
        this.rageConnector.remove(FromServerEvent.SetAssistsForRoundStats, this.setAssists.bind(this));
        this.rageConnector.remove(FromServerEvent.SetDamageForRoundStats, this.setDamage.bind(this));
    }

    copyToClipboard() {
        const text = `===============
==== TDS-V ====
= Round-Stats =
===============

Kills: ${this.kills}
Assists: ${this.assists}
Damage: ${this.damage}`;
        this.clipboardService.copy(text);
    }

    private setKills(kills: number) {
        this.kills = kills;
        this.changeDetector.detectChanges();
    }

    private setAssists(assists: number) {
        this.assists = assists;
        this.changeDetector.detectChanges();
    }

    private setDamage(damage: number) {
        this.damage = damage;
        this.changeDetector.detectChanges();
    }
}
