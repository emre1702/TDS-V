import { Component, OnInit, ChangeDetectorRef, Input, OnChanges, SimpleChanges, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { LanguagePipe } from '../../../pipes/language.pipe';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { ClipboardService } from 'ngx-clipboard';

@Component({
  selector: 'app-userpanel-commands',
  templateUrl: './userpanel-commands.component.html',
  styleUrls: ['./userpanel-commands.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
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

  constructor(private changeDetector: ChangeDetectorRef, public settings: SettingsService, public userpanelService: UserpanelService,
    private clipboardService: ClipboardService) { }

  gotoCommand(command: UserpanelCommandDataDto) {
    this.currentCommand = command;
    this.changeDetector.detectChanges();
  }

  copyCommand(command: UserpanelCommandDataDto) {
    this.clipboardService.copy("/" + command[0] + " ");
  }

  ngOnInit() {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    this.userpanelService.commandsLoaded.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy(): void {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    this.userpanelService.commandsLoaded.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }

}
