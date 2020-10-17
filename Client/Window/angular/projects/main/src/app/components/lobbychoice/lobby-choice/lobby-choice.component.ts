import { Component, ChangeDetectorRef, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../../../enums/dtoclientevent.enum';
import { SettingsService } from '../../../services/settings.service';
import { LobbyChoice } from './interfaces/lobby-choice';
import { trigger, transition, animate, style } from '@angular/animations';
import { DomSanitizer } from '@angular/platform-browser';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { LanguagePipe } from '../../../pipes/language.pipe';
import { ChallengeFrequency } from '../enums/challenge-frequency.enum';
import { ChallengeType } from '../enums/challenge-type.enum';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { DFromServerEvent } from '../../../enums/dfromserverevent.enum';
import { OfficialLobbyId } from '../enums/official-lobby-id.enum';

@Component({
    selector: 'app-lobby-choice',
    templateUrl: './lobby-choice.component.html',
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
    ],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class LobbyChoiceComponent implements OnInit, OnDestroy {

    constructor(
        private rageConnector: RageConnectorService,
        public settings: SettingsService,
        private sanitizer: DomSanitizer,
        private changeDetector: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.rageConnector.listen(DFromServerEvent.LeaveCustomLobbyMenu, this.leaveCustomLobbyMenu.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromServerEvent.LeaveCustomLobbyMenu, this.leaveCustomLobbyMenu.bind(this));

        // Clear it so it doesn't use fill our RAM without a reason
        this.settings.AllMapsForCustomLobby = [];
    }

    private leaveCustomLobbyMenu() {
        this.settings.InUserLobbiesMenu = false;
        this.changeDetector.detectChanges();
    }

    joinLobby(id: number) {
        if (id === OfficialLobbyId.CustomLobby)
            this.showUserLobbies();
        else
            this.rageConnector.callServer(DToServerEvent.JoinLobby, id);
    }

    showUserLobbies() {
        this.settings.InUserLobbiesMenu = true;
        this.changeDetector.detectChanges();
    }
}
