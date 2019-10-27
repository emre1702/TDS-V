import { Component, ChangeDetectorRef, OnDestroy, OnInit, ViewContainerRef } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { UserpanelNavPage } from './enums/userpanel-nav-page.enum';
import { UserpanelCommandDataDto } from './interfaces/userpanelCommandDataDto';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
import { UserpanelService } from './services/userpanel.service';
import { LanguagePipe } from '../../pipes/language.pipe';

@Component({
  selector: 'app-userpanel',
  templateUrl: './userpanel.component.html',
  styleUrls: ['./userpanel.component.scss']
})
export class UserpanelComponent implements OnInit, OnDestroy {

  langPipe = new LanguagePipe();
  userpanelNavPage = UserpanelNavPage;
  currentCommand: UserpanelCommandDataDto;
  currentNav: string = UserpanelNavPage[UserpanelNavPage.Main];
  myStatsColumns = ["Id",
    "Name",
    "SCName",
    "Gang",
    "AdminLvl",
    "Donation",
    "IsVip",
    "Money",
    "TotalMoney",
    "PlayTime",

    "MuteTime",
    "VoiceMuteTime",

    "BansInLobbies",

    "AmountMapsCreated",
    "MapsRatedAverage",
    "CreatedMapsAverageRating",
    "AmountMapsRated",
    "LastLogin",
    "RegisterTimestamp",
    "LobbyStats",
    "Logs"];

  constructor(public settings: SettingsService,
    private changeDetector: ChangeDetectorRef,
    private rageConnector: RageConnectorService,
    public userpanelService: UserpanelService) {

  }

  ngOnInit() {
    this.settings.AdminLevelChanged.on(null, this.detectChanges.bind(this));
    this.userpanelService.myStatsLoaded.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy() {
    this.settings.AdminLevelChanged.off(null, this.detectChanges.bind(this));
    this.userpanelService.myStatsLoaded.off(null, this.detectChanges.bind(this));
  }

  closeUserpanel() {
    this.rageConnector.call(DToClientEvent.CloseUserpanel);
  }

  gotoNav(nav: number) {
    this.currentNav = UserpanelNavPage[nav];
    this.currentCommand = undefined;
    this.changeDetector.detectChanges();

    if (this.currentNav.startsWith("Commands") && !this.userpanelService.allCommands.length) {
      this.userpanelService.loadCommands();
    } else if (this.currentNav.startsWith("Rules") && !this.userpanelService.allRules.length) {
      this.userpanelService.loadRules();
    } else if (nav == UserpanelNavPage.FAQ && !this.userpanelService.allFAQs.length) {
      this.userpanelService.loadFAQs();
    } else if (nav == UserpanelNavPage.Settings && !this.userpanelService.allSettings) {
      this.userpanelService.loadSettings();
    } else if (nav == UserpanelNavPage.MyStats) {
      this.userpanelService.loadMyStats();
    } else if (nav == UserpanelNavPage.Application) {
      this.userpanelService.loadApplicationPage();
    } else if (nav == UserpanelNavPage.Applications) {
      this.userpanelService.loadApplicationsPage();
    }
  }

  getNavs(): Array<string> {
    const keys = Object.keys(UserpanelNavPage);
    return keys.slice(keys.length / 2);
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }
}
