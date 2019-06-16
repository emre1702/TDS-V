import { Component, ChangeDetectionStrategy, ChangeDetectorRef, OnInit } from '@angular/core';
import { TeamOrder } from './enums/teamorder.enum';
import { SettingsService } from 'src/app/services/settings.service';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';

@Component({
  selector: 'app-teamorders',
  animations: [
    trigger('hideShowAnimation', [
      transition('* => *', [
        query(':enter', [
          style({transform: 'translateX(100%)', opacity: 0}),
          stagger(100, [
            animate('500ms', style({transform: 'translateX(0)', opacity: 1})),
          ])
        ], {optional: true}),
        query(':leave', [
          style({transform: 'translateX(0)', opacity: 1}),
          stagger(100, [
            animate('500ms', style({transform: 'translateX(100%)', opacity: 0}))
          ])
        ], {optional: true})
      ])
    ])
  ],
  templateUrl: './teamorders.component.html',
  styleUrls: ['./teamorders.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TeamOrdersComponent implements OnInit {
  constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) {
    settings.LanguageChanged.on(null, () => changeDetector.detectChanges());
  }

  ngOnInit(): void {
    this.settings.LanguageChanged.on(null, () => this.changeDetector.detectChanges());
    this.settings.InTeamOrderModusChanged.on(null, () => this.changeDetector.detectChanges());
  }

  getOrders(): Array<string> {
    return Object.values(TeamOrder);
  }
}
