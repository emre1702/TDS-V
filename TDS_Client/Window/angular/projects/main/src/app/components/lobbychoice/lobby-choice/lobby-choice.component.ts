import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
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
import { Challenge } from '../models/challenge';

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
export class LobbyChoiceComponent implements OnInit {
    lobbyChoices: LobbyChoice[] = [
        { index: 0, name: "Arena", func: this.joinArena.bind(this), imgUrl: "assets/arenachoice.png" },
        { index: 1, name: "MapCreator", func: this.joinMapCreator.bind(this), imgUrl: "assets/mapcreatorchoice.png" },
        { index: 2, name: "UserLobbies", func: this.showUserLobbies.bind(this), imgUrl: "assets/customlobbychoice.png" },
        // { index: 3, name: "Gang", func: this.joinGang.bind(this), imgUrl: "assets/gangchoice.png" },
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

        this.rageConnector.listen(DFromClientEvent.LeaveCustomLobbyMenu, () => {
            this.settings.InUserLobbiesMenu = false;
            this.changeDetector.detectChanges();
        });
    }

    ngOnInit() {
        this.refreshTimeToWeeklyChallengesRestart();
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

    refreshTimeToWeeklyChallengesRestart(): void {
        const currentDate = new Date();
        let restartDate = new Date();
        restartDate = new Date(restartDate.setDate(currentDate.getDate() - currentDate.getDay() + 7 + 1));
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

    getChallengeInfo(challenge: Challenge) {
        return this.sanitizer.bypassSecurityTrustHtml(
            this.languagePipe.transform('Challenge_' + this.challengeType[challenge[0]], this.settings.Lang, this.getColorText(challenge[1], "orange"))
            + " (" + this.languagePipe.transform("Current:", this.settings.Lang)
            + " " + this.getColorText(challenge[2], "yellow") + ")");
    }

    getColorText(text: string|number, color: string) {
        return "<span style='color: " + color + "'>" + text + "</span>";
    }

    bypassTrustHtml(text: string) {
        return this.sanitizer.bypassSecurityTrustHtml(text);
    }
}
