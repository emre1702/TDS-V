import { Component, OnInit, Input, ViewChild, ChangeDetectorRef } from '@angular/core';
import { RoundPlayerRankingStat } from './models/roundPlayerRankingStat';
import { SettingsService } from '../../services/settings.service';
import { MatSort, MatTableDataSource } from '@angular/material';

@Component({
    selector: 'app-ranking',
    templateUrl: './ranking.component.html',
    styleUrls: ['./ranking.component.scss']
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
      this.dataSource.sortingDataAccessor = this.sortingDataAccessor;
      if (this.sort)
        this.dataSource.sort = this.sort;
      if (this.changeDetector)
        this.changeDetector.detectChanges();
    }

    displayedColumns = ["Place", "Name", "Points", "Kills", "Assists", "Damage"];

    dataSource: MatTableDataSource<RoundPlayerRankingStat>;
    @ViewChild(MatSort, {static: false}) sort: MatSort;

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit() {
      this.dataSource.sort = this.sort;
      this.changeDetector.detectChanges();
    }

    sortingDataAccessor(obj, property) {
      console.log(obj);
      console.log(property);
      return obj[property];
    }
}
