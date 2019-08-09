import { Component, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { UserpanelNavPage } from './enums/userpanel-nav-page.enum';
import { UserpanelCommandDataDto } from './interfaces/userpanelCommandDataDto';
import { LanguageEnum } from '../../enums/language.enum';
import { LanguagePipe } from '../../pipes/language.pipe';
import { RageConnectorService } from '../../services/rage-connector.service';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';

@Component({
  selector: 'app-userpanel',
  templateUrl: './userpanel.component.html',
  styleUrls: ['./userpanel.component.scss']
})
export class UserpanelComponent {

  userpanelNavPage = UserpanelNavPage;
  currentCommand: UserpanelCommandDataDto;
  currentNav: string = UserpanelNavPage[UserpanelNavPage.Main];

  allCommands: UserpanelCommandDataDto[] = [];

  constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService) {

  }

  closeUserpanel() {
    this.rageConnector.call(DToClientEvent.CloseUserpanel);
  }

  gotoNav(nav: number) {
    this.currentNav = UserpanelNavPage[nav];
    this.currentCommand = undefined;
    this.changeDetector.detectChanges();

    if (this.currentNav.startsWith("Command") && !this.allCommands.length) {
      this.rageConnector.callCallback(DFromClientEvent.LoadAllCommands, [], (commandsJson) => {
        this.allCommands = JSON.parse(commandsJson);
        this.changeDetector.detectChanges();
      });
    }
  }

  getNavs(): Array<string> {
    const keys = Object.keys(UserpanelNavPage);
    return keys.slice(keys.length / 2);
  }
}
