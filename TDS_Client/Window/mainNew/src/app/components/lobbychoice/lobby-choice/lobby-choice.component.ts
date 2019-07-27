import { Component, ChangeDetectorRef } from '@angular/core';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { DToClientEvent } from 'src/app/enums/dtoclientevent.enum';
import { SettingsService } from 'src/app/services/settings.service';
import { LobbyChoice } from './interfaces/lobby-choice';
import { trigger, transition, animate, style } from '@angular/animations';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-lobby-choice',
  templateUrl: './lobby-choice.component.html',
  styleUrls: ['./lobby-choice.component.scss'],
  animations: [
    trigger('hideShowAnimation', [
      transition(
        ':enter', [
          style({ transform: 'translateX(-100%)', opacity: 0 }),
          animate('800ms', style({ transform: 'translateX(0)', opacity: 0.95 }))
        ]
      ),
      transition(
        ':leave', [
          animate('800ms', style({ transform: 'translateX(-100%)', opacity: 0 })),
        ]
      )]
    )
  ]
})
export class LobbyChoiceComponent {
  lobbyChoices: LobbyChoice[] = [
    { name: "Arena", func: this.joinArena.bind(this), imgUrl: "/mainNew/assets/arenachoice.png" },
    { name: "Gang", func: this.joinGang.bind(this), imgUrl: "/mainNew/assets/gangchoice.png", disabled: true },
    { name: "MapCreator", func: this.joinMapCreator.bind(this), imgUrl: "/mainNew/assets/mapcreatorchoice.png" },
    { name: "UserLobbies", func: this.showUserLobbies.bind(this), imgUrl: "/mainNew/assets/customlobbychoice.png" },
  ];

  constructor(private rageConnector: RageConnectorService, public settings: SettingsService,
    private sanitizer: DomSanitizer, private changeDetector: ChangeDetectorRef) { }

  setLanguage(languageId: number) {
    this.settings.loadLanguage(languageId);
    this.rageConnector.call(DToClientEvent.LanguageChange, languageId);
  }

  getImageUrl(url: string) {
    return this.sanitizer.bypassSecurityTrustStyle("url(" + url + ") no-repeat");
  }

  joinArena() {
    this.rageConnector.call(DToClientEvent.ChooseArenaToJoin);
  }

  joinGang() {

  }

  joinMapCreator() {
    this.rageConnector.call(DToClientEvent.ChooseMapCreatorToJoin);
  }

  showUserLobbies() {
    this.settings.InUserLobbiesMenu = true;
    this.rageConnector.call(DToClientEvent.JoinedCustomLobbiesMenu);
    this.changeDetector.detectChanges();
  }
}
