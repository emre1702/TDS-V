import { Component, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { UserpanelNavPage } from './enums/userpanel-nav-page.enum';
import { UserpanelCommandDataDto } from './interfaces/userpanelCommandDataDto';
import { LanguageEnum } from '../../enums/language.enum';
import { LanguagePipe } from '../../pipes/language.pipe';

@Component({
  selector: 'app-userpanel',
  templateUrl: './userpanel.component.html',
  styleUrls: ['./userpanel.component.scss']
})
export class UserpanelComponent {

  allCommands: UserpanelCommandDataDto[] = [
    {Command: "Goto", Aliases: ["GotoPlayer", "PlayerGoto"], VIPCanUse: false, Description: {[LanguageEnum.German]: "Go to a player"}, LobbyOwnerCanUse: false,
      Syntaxes: [
        {Parameters: [
          {Name: "target", Type: "TDSPlayer"}
        ]}
      ]
    },
    {Command: "mute", Aliases: ["tmute", "pmute"], VIPCanUse: true, Description: {[9]: "Mute a player"},
      LobbyOwnerCanUse: true, MinAdminLevel: 4, MinDonation: 20,
      Syntaxes: [
        {Parameters: [
          {Name: "target", Type: "TDSPlayer"},
          {Name: "time", Type: "Int32", DefaultValue: 0}
        ]},
        {Parameters: [
          {Name: "target", Type: "Players"},
          {Name: "time", Type: "Int32"}
        ]},
        {Parameters: [
          {Name: "target", Type: "TDSPlayer"},
          {Name: "time", Type: "DateTime"}
        ]},
        {Parameters: [
          {Name: "target", Type: "Players"},
          {Name: "time", Type: "DateTime"}
        ]}
      ]
    },
  ];

  userpanelNavPage = UserpanelNavPage;
  currentNav: string = UserpanelNavPage[UserpanelNavPage.Main];
  currentCommand: UserpanelCommandDataDto;
  langPipe = new LanguagePipe();

  constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) {}

  gotoNav(nav: number) {
    this.currentNav = UserpanelNavPage[nav];
    this.currentCommand = undefined;
    this.changeDetector.detectChanges();
  }

  gotoCommand(command: UserpanelCommandDataDto) {
    this.currentCommand = command;
    this.changeDetector.detectChanges();
  }

  getNavs(): Array<string> {
    const keys = Object.keys(UserpanelNavPage);
    return keys.slice(keys.length / 2);
  }
}
