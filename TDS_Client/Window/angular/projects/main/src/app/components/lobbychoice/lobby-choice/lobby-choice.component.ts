import { Component, ChangeDetectorRef } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../../../enums/dtoclientevent.enum';
import { SettingsService } from '../../../services/settings.service';
import { LobbyChoice } from './interfaces/lobby-choice';
import { trigger, transition, animate, style } from '@angular/animations';
import { DomSanitizer } from '@angular/platform-browser';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';

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
    { name: "Arena", func: this.joinArena.bind(this), imgUrl: "assets/arenachoice.png" },
    { name: "Gang", func: this.joinGang.bind(this), imgUrl: "assets/gangchoice.png", disabled: true },
    { name: "MapCreator", func: this.joinMapCreator.bind(this), imgUrl: "assets/mapcreatorchoice.png" },
    { name: "UserLobbies", func: this.showUserLobbies.bind(this), imgUrl: "assets/customlobbychoice.png" },
  ];

  constructor(private rageConnector: RageConnectorService, public settings: SettingsService,
    private sanitizer: DomSanitizer, private changeDetector: ChangeDetectorRef) {

    this.rageConnector.listen(DFromClientEvent.LeaveCustomLobbyMenu, () => {
      this.settings.InUserLobbiesMenu = false;
      this.changeDetector.detectChanges();
    });
  }

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
