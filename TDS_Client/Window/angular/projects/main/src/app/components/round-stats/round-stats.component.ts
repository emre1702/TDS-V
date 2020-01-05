import { Component, OnInit, OnDestroy, ChangeDetectorRef, Sanitizer } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DFromServerEvent } from '../../enums/dfromserverevent.enum';
import { ClipboardService } from 'ngx-clipboard';

@Component({
    selector: 'app-round-stats',
    templateUrl: './round-stats.component.html',
    styleUrls: ['./round-stats.component.scss']
})
export class RoundStatsComponent implements OnInit, OnDestroy {

    kills = 0;
    assists = 0;
    damage = 0;

    constructor(
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef,
        private clipboardService: ClipboardService) { }

    ngOnInit() {
        this.rageConnector.listen(DFromServerEvent.SetKillsForRoundStats, this.setKills.bind(this));
        this.rageConnector.listen(DFromServerEvent.SetAssistsForRoundStats, this.setAssists.bind(this));
        this.rageConnector.listen(DFromServerEvent.SetDamageForRoundStats, this.setDamage.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromServerEvent.SetKillsForRoundStats, this.setKills.bind(this));
        this.rageConnector.remove(DFromServerEvent.SetAssistsForRoundStats, this.setAssists.bind(this));
        this.rageConnector.remove(DFromServerEvent.SetDamageForRoundStats, this.setDamage.bind(this));
    }

    copyToClipboard() {
        const text =
`===============
==== TDS-V ====
= Round-Stats =
===============

Kills: ${this.kills}
Assists: ${this.assists}
Damage: ${this.damage}`;
        this.clipboardService.copyFromContent(text);
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
