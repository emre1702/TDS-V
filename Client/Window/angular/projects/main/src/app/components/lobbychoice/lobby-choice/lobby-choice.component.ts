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
    ],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class LobbyChoiceComponent implements OnInit, OnDestroy {
    lobbyChoices: LobbyChoice[] = [
        { index: 0, name: "Arena", func: this.joinArena.bind(this), imgUrl: "assets/arenachoice.png" },
        { index: 1, name: "MapCreator", func: this.joinMapCreator.bind(this), imgUrl: "assets/mapcreatorchoice.png" },
        { index: 2, name: "UserLobbies", func: this.showUserLobbies.bind(this), imgUrl: "assets/customlobbychoice.png" },
        { index: 3, name: "Gang", func: this.joinGang.bind(this), imgUrl: "assets/gangchoice.png" },
        { index: 4, name: "CharCreator", func: this.joinCharCreator.bind(this), imgUrl: "assets/charcreatorchoice.png" }
    ];

    timeToNextWeeklyChallengesRestartInfo: string;
    challengeType = ChallengeType;
    challengeFrequency = ChallengeFrequency;
    languagePipe = new LanguagePipe();

    constructor(
        private rageConnector: RageConnectorService,
        public settings: SettingsService,
        private sanitizer: DomSanitizer,
        private changeDetector: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.refreshTimeToWeeklyChallengesRestart();
        this.rageConnector.listen(DFromServerEvent.LeaveCustomLobbyMenu, this.leaveCustomLobbyMenu.bind(this));
        this.settings.LanguageChanged.on(null, this.languageChanged.bind(this));
        this.settings.ChallengesLoaded.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingsLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromServerEvent.LeaveCustomLobbyMenu, this.leaveCustomLobbyMenu.bind(this));
        this.settings.LanguageChanged.off(null, this.languageChanged.bind(this));
        this.settings.ChallengesLoaded.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingsLoaded.off(null, this.detectChanges.bind(this));

        // Clear it so it doesn't use fill our RAM without a reason
        this.settings.AllMapsForCustomLobby = [];
    }

    private leaveCustomLobbyMenu() {
        this.settings.InUserLobbiesMenu = false;
        this.changeDetector.detectChanges();
    }

    private languageChanged() {
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.refreshTimeToWeeklyChallengesRestart();
        this.changeDetector.detectChanges();
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
        this.rageConnector.call(DToClientEvent.ChooseGangLobbyToJoin);
    }

    joinMapCreator() {
        this.rageConnector.call(DToClientEvent.ChooseMapCreatorToJoin);
    }

    joinCharCreator() {
        this.rageConnector.call(DToClientEvent.ChooseCharCreatorToJoin);
    }

    showUserLobbies() {
        this.settings.InUserLobbiesMenu = true;
        this.changeDetector.detectChanges();
    }

    refreshTimeToWeeklyChallengesRestart(): void {
        const currentDate = new Date();
        let restartDate = new Date();
        restartDate = new Date(restartDate.setDate(currentDate.getDate() - currentDate.getDay() + 7));
        restartDate.setUTCHours(5);
        restartDate.setMinutes(0);
        restartDate.setSeconds(0);

        const hoursToWeeklyChallengesRestart = (restartDate.getTime() - currentDate.getTime()) / 1000 / 60 / 60;

        if (hoursToWeeklyChallengesRestart / 24 >= 1) {
            this.timeToNextWeeklyChallengesRestartInfo
                = this.languagePipe.transform("DaysLeft", this.settings.Lang, Math.ceil(hoursToWeeklyChallengesRestart / 24));
        } else {
            this.timeToNextWeeklyChallengesRestartInfo
                = this.languagePipe.transform("HoursLeft", this.settings.Lang, Math.ceil(hoursToWeeklyChallengesRestart % 24));
        }
    }
}
