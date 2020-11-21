import { Component, OnInit, Input, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { UserpanelStatsDataDto as UserpanelStatsGeneralDataDto } from '../interfaces/userpanelStatsDataDto';
import { SettingsService } from '../../../services/settings.service';

@Component({
    selector: 'app-userpanel-stats-general',
    templateUrl: './userpanel-stats-general.component.html',
    styleUrls: ['./userpanel-stats-general.component.scss']
})
export class UserpanelStatsGeneralComponent implements OnInit, OnDestroy {

    array = Array;

    @Input()
    set stats(value: UserpanelStatsGeneralDataDto) {
        this._stats = value;
        this.changeDetector.detectChanges();
    }
    get stats(): UserpanelStatsGeneralDataDto {
        return this._stats;
    }

    @Input()
    set columns(value: { [index: number]: string }) {
        this._columns = value;
        this.changeDetector.detectChanges();
    }
    get columns(): { [index: number]: string } {
        return this._columns;
    }

    lobbyStatsColumns = {
        1: "Kills",
        2: "Assists",
        3: "Deaths",
        4: "Damage",

        5: "TotalKills",
        6: "TotalAssists",
        7: "TotalDeaths",
        8: "TotalDamage",
        9: "TotalRounds",

        10: "MostKillsInARound",
        11: "MostDamageInARound",
        12: "MostAssistsInARound",

        13: "MostKillsInADay",
        14: "MostDamageInADay",
        15: "MostAssistsInADay",

        16: "TotalMapsBought"
    };

    private _stats: UserpanelStatsGeneralDataDto;
    private _columns: { [index: number]: string };

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) { }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }

    getIndizes() {
        return Object.keys(this.columns);
    }

    getLobbyStatsIndizes() {
        return Object.keys(this.lobbyStatsColumns);
    }

    isNormalValue(value: any) {
        return value && value != "" && typeof(value) != "object";
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
