import { Component, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { UserpanelNavPage } from './enums/userpanel-nav-page.enum';
import { UserpanelCommandDataDto } from './interfaces/userpanelCommandDataDto';
import { RageConnectorService } from '../../services/rage-connector.service';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';
import { UserpanelService } from './services/userpanel.service';
import { LanguagePipe } from '../../pipes/language.pipe';

@Component({
  selector: 'app-userpanel',
  templateUrl: './userpanel.component.html',
  styleUrls: ['./userpanel.component.scss']
})
export class UserpanelComponent {

  langPipe = new LanguagePipe();
  userpanelNavPage = UserpanelNavPage;
  currentCommand: UserpanelCommandDataDto;
  currentNav: string = UserpanelNavPage[UserpanelNavPage.Main];

  constructor(public settings: SettingsService,
    private changeDetector: ChangeDetectorRef,
    private rageConnector: RageConnectorService,
    private userpanelService: UserpanelService) {

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
    }
  }

  getNavs(): Array<string> {
    const keys = Object.keys(UserpanelNavPage);
    return keys.slice(keys.length / 2);
  }
}
