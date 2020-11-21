import { Component, ChangeDetectorRef, OnInit, OnDestroy } from '@angular/core';
import { TeamOrder } from './enums/teamorder.enum';
import { SettingsService } from '../../services/settings.service';

@Component({
  selector: 'app-teamorders',
  templateUrl: './teamorders.component.html',
  styleUrls: ['./teamorders.component.scss']
})
export class TeamOrdersComponent implements OnInit, OnDestroy {
  constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) {
    this.settings.InTeamOrderModusChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnInit(): void {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy(): void {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }

  getOrders(): Array<string> {
    return Object.values(TeamOrder);
  }
}
