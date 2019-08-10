import { Component, OnInit, OnDestroy, ChangeDetectorRef, Input } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';

@Component({
  selector: 'app-userpanel-rules',
  templateUrl: './userpanel-rules.component.html',
  styleUrls: ['./userpanel-rules.component.scss']
})
export class UserpanelRulesComponent implements OnInit, OnDestroy {

  @Input() currentNav: string;

  constructor(
    public settings: SettingsService,
    public userpanelService: UserpanelService,
    private changeDetector: ChangeDetectorRef) { }

  ngOnInit() {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    this.userpanelService.rulesLoaded.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy(): void {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    this.userpanelService.rulesLoaded.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }
}
