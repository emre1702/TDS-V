import { Component, OnInit, ChangeDetectorRef, OnDestroy, EventEmitter, Output, HostListener, Input, Injector, NgZone } from '@angular/core';
import { AnimationEvent } from '@angular/animations';
import { SettingsService } from '../../../services/settings.service';
import { ChallengeType } from '../enums/challenge-type.enum';
import { ChallengeFrequency } from '../enums/challenge-frequency.enum';
import { LobbyChoice } from '../lobby-choice/interfaces/lobby-choice';
import { OfficialLobbyId } from '../enums/official-lobby-id.enum';
import { DomSanitizer } from '@angular/platform-browser';
import { ToClientEvent } from '../../../enums/to-client-event.enum';
import { RageConnectorService } from 'rage-connector';
import { topToBottomMainMenuAnimation } from './animations/top-to-bottom-main-menu.animation';
import { leftToRightMainMenuAnimation } from './animations/left-to-right-main-menu.animation';
import { rightToLeftMainMenuAnimation } from './animations/right-to-left-main-menu.animation';
import { LanguagePipe } from '../../../modules/shared/pipes/language.pipe';
import { InitialDatas } from '../../../initial-datas';
import { MainMenuDebugService } from './services/main-menu.debug.service';
import { MainMenuProdService } from './services/main-menu.prod.service';
import { MainMenuService } from './services/main-menu.service';
import { tap } from 'rxjs/operators';

@Component({
    selector: 'app-main-menu',
    templateUrl: './main-menu.component.html',
    styleUrls: ['./main-menu.component.scss'],
    animations: [topToBottomMainMenuAnimation, leftToRightMainMenuAnimation, rightToLeftMainMenuAnimation],
    providers: [{ provide: MainMenuService, useFactory: createService, deps: [Injector] }],
})
export class MainMenuComponent implements OnInit, OnDestroy {
    @Input() showRegisterLogin: boolean;
    @Output() lobbyJoin = new EventEmitter<number>();

    announcements$ = this.service.loadAnnouncements();
    changelogs$ = this.service.loadChangelogs();

    challengeType = ChallengeType;
    challengeFrequency = ChallengeFrequency;
    timeToNextWeeklyChallengesRestartInfo: string;
    animState = 'open';
    private selectedLobbyId: number;

    languagePipe = new LanguagePipe();
    private initialLobbyChoices: LobbyChoice[] = [
        { id: OfficialLobbyId.Arena, name: 'Arena', imgUrl: 'assets/arenachoice.png' },
        { id: OfficialLobbyId.CustomLobby, name: 'UserLobbies', imgUrl: 'assets/customlobbychoice.png' },
        // { id: OfficialLobbyId.GangLobby, name: "Gang", imgUrl: "assets/gangchoice.png" },
        { id: OfficialLobbyId.DamageTestLobby, name: 'DamageTestLobby', imgUrl: 'assets/arenachoice.png' },
        { id: OfficialLobbyId.MapCreateLobby, name: 'MapCreator', imgUrl: 'assets/mapcreatorchoice.png' },
        { id: OfficialLobbyId.CharCreateLobby, name: 'CharCreator', imgUrl: 'assets/charcreatorchoice.png' },
    ];
    lobbyChoices: LobbyChoice[];

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private sanitizer: DomSanitizer,
        private rageConnector: RageConnectorService,
        private service: MainMenuService
    ) {}

    ngOnInit(): void {
        this.refreshTimeToWeeklyChallengesRestart();
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.ChallengesLoaded.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
        this.settings.UserpanelOpenChanged.on(null, this.detectChanges.bind(this));
        this.lobbyChoices = [...this.initialLobbyChoices];
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.ChallengesLoaded.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
        this.settings.UserpanelOpenChanged.off(null, this.detectChanges.bind(this));
        this.lobbyChoices = undefined;
    }

    setLanguage(languageId: number) {
        this.settings.loadLanguage(languageId);
        this.rageConnector.call(ToClientEvent.LanguageChange, languageId);
    }

    getImageUrl(url: string) {
        return this.sanitizer.bypassSecurityTrustStyle('url(' + url + ') no-repeat');
    }

    triggerJoinLobby(id: number) {
        if (this.settings.UserpanelOpen) {
            return;
        }
        if (this.animState === 'close') {
            return;
        }
        this.selectedLobbyId = id;
        this.animState = 'close';
        this.lobbyChoices = [];
        this.changeDetector.detectChanges();
    }

    onAnimationEvent(event: AnimationEvent) {
        if (event.triggerName === 'topToBottom' && event.phaseName === 'done' && event.toState === 'close') {
            this.lobbyJoin.emit(this.selectedLobbyId);
        }
    }

    private detectChanges() {
        this.refreshTimeToWeeklyChallengesRestart();
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
            this.timeToNextWeeklyChallengesRestartInfo = this.languagePipe.transform(
                'DaysLeft',
                this.settings.Lang,
                Math.ceil(hoursToWeeklyChallengesRestart / 24)
            );
        } else {
            this.timeToNextWeeklyChallengesRestartInfo = this.languagePipe.transform(
                'HoursLeft',
                this.settings.Lang,
                Math.ceil(hoursToWeeklyChallengesRestart % 24)
            );
        }
    }

    @HostListener('window:keydown', ['$event'])
    keyEvent(event: KeyboardEvent) {
        if (this.settings.ChatInputOpen) {
            return;
        }
        if (this.settings.UserpanelOpen) {
            return;
        }
        if (this.showRegisterLogin) {
            return;
        }

        if (!isNaN(parseInt(event.key, 10))) {
            const index = parseInt(event.key, 10) - 1;
            if (index < this.lobbyChoices.length && index >= 0) {
                const id = this.lobbyChoices[index].id;
                this.triggerJoinLobby(id);
            }
        }
    }
}

export function createService(injector: Injector) {
    if (InitialDatas.inDebug) {
        return new MainMenuDebugService();
    } else {
        return new MainMenuProdService(injector.get(RageConnectorService), injector.get(SettingsService));
    }
}
