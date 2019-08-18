import { Component, OnInit, Input, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { UserpanelStatsDataDto } from '../interfaces/userpanelStatsDataDto';
import { SettingsService } from '../../../services/settings.service';

@Component({
  selector: 'app-userpanel-stats',
  templateUrl: './userpanel-stats.component.html',
  styleUrls: ['./userpanel-stats.component.scss']
})
export class UserpanelStatsComponent implements OnInit, OnDestroy {

  objectKeys = Object.keys;
  array = Array;

  @Input()
  set stats(value: UserpanelStatsDataDto) {
    this._stats = value;
    this.changeDetector.detectChanges();
  }
  get stats(): UserpanelStatsDataDto {
    return this._stats;
  }

  @Input() columns: string[];

  private _stats: UserpanelStatsDataDto;

  constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) { }

  ngOnInit() {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy() {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }
}
