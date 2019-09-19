import { Component, ChangeDetectionStrategy, ChangeDetectorRef, OnInit, OnDestroy } from '@angular/core';
import { TeamOrder } from './enums/teamorder.enum';
import { SettingsService } from '../../services/settings.service';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';

@Component({
  selector: 'app-teamorders',
  animations: [
    trigger('hideShowAnimation', [
      transition('* => *', [
        query(':enter', [
          style({ transform: 'translateX(100%)', opacity: 0 }),
          stagger(100, [
            animate('500ms', style({ transform: 'translateX(0)', opacity: 1 })),
          ])
        ], { optional: true }),
        query(':leave', [
          style({ transform: 'translateX(0)', opacity: 1 }),
          stagger(100, [
            animate('500ms', style({ transform: 'translateX(100%)', opacity: 0 }))
          ])
        ], { optional: true })
      ])
    ])
  ],
  templateUrl: './teamorders.component.html',
  styleUrls: ['./teamorders.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
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
