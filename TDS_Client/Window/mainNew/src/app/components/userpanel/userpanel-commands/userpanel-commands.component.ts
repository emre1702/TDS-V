import { Component, OnInit, ChangeDetectorRef, Input, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { LanguagePipe } from '../../../pipes/language.pipe';
import { SettingsService } from '../../../services/settings.service';
import { LanguageEnum } from '../../../enums/language.enum';
import { UserpanelService } from '../services/userpanel.service';

@Component({
  selector: 'app-userpanel-commands',
  templateUrl: './userpanel-commands.component.html',
  styleUrls: ['./userpanel-commands.component.scss']
})
export class UserpanelCommandsComponent implements OnInit, OnDestroy {

  /*allCommands: UserpanelCommandDataDto[] = [
    {Command: "Goto", Aliases: ["GotoPlayer", "PlayerGoto"], VIPCanUse: true, Description: {[LanguageEnum.German]: "Go to a player"}, LobbyOwnerCanUse: false,
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
  ];*/

  langPipe = new LanguagePipe();

  @Input() currentCommand: UserpanelCommandDataDto;
  @Input() currentNav: string;

  constructor(private changeDetector: ChangeDetectorRef, public settings: SettingsService, public userpanelService: UserpanelService) { }

  gotoCommand(command: UserpanelCommandDataDto) {
    this.currentCommand = command;
    this.changeDetector.detectChanges();
  }

  ngOnInit() {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    this.userpanelService.commandsLoaded.on(null, this.detectChanges.bind(this));

    if (!this.userpanelService.allCommands.length)
      this.userpanelService.loadCommands();
  }

  ngOnDestroy(): void {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    this.userpanelService.commandsLoaded.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }

}
