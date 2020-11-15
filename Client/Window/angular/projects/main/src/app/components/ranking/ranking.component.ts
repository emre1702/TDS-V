import { Component, OnInit, Input, ViewChild, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { RoundPlayerRankingStat } from './models/roundPlayerRankingStat';
import { SettingsService } from '../../services/settings.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

@Component({
    selector: 'app-ranking',
    templateUrl: './ranking.component.html',
    styleUrls: ['./ranking.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class RankingComponent implements OnInit {

    _rankings: RoundPlayerRankingStat[];

    get rankings(): RoundPlayerRankingStat[] {
        return this._rankings;
    }

    @Input("rankings")
    set rankings(rankings: RoundPlayerRankingStat[]) {
        this._rankings = rankings;
        this.dataSource = new MatTableDataSource(rankings);
        this.dataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
        if (this.sort)
            this.dataSource.sort = this.sort;
        if (this.changeDetector)
            this.changeDetector.detectChanges();
    }

    displayedColumns = ["Place", "Name", "Points", "Kills", "Assists", "Damage"];

    dataSource: MatTableDataSource<RoundPlayerRankingStat>;
    @ViewChild(MatSort) sort: MatSort;

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) { }

    ngOnInit() {
        this.dataSource.sort = this.sort;
        this.changeDetector.detectChanges();
    }

    sortingDataAccessor(obj, property) {
        const index = this.displayedColumns.indexOf(property);
        return obj[index];
    }
}
