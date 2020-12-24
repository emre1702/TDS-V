import { Component, ChangeDetectorRef, ViewContainerRef, HostListener } from '@angular/core';
import { SettingsService } from './services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { FromClientEvent } from './enums/from-client-event.enum';
import { RoundPlayerRankingStat } from './components/ranking/models/roundPlayerRankingStat';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';
import { TeamOrder } from './components/teamorders/enums/teamorder.enum';
import { DomSanitizer } from '@angular/platform-browser';
import { CharCreateData } from './components/char-creator/interfaces/charCreateData';
import { MaterialCssVarsService } from 'angular-material-css-vars';
import { PedBodyPart } from './components/userpanel/enums/ped-body-part.enum';
import { WeaponHash } from './components/lobbychoice/enums/weapon-hash.enum';
import { MatIconRegistry } from '@angular/material/icon';
import { InitialDatas } from './initial-datas';
import { NotificationService } from './modules/shared/services/notification.service';
import { SettingsThemeIndex } from './components/userpanel/userpanel-settings-normal/enums/settings-theme-index.enum';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    animations: [
        trigger('teamOrdersAnimation', [
            transition('* => *', [
                query(
                    ':enter',
                    [style({ transform: 'translateX(100%)', opacity: 0 }), stagger(150, [animate('500ms', style({ transform: 'translateX(0)', opacity: 1 }))])],
                    { optional: true }
                ),
                query(
                    ':leave',
                    [style({ transform: 'translateX(0)', opacity: 1 }), stagger(150, [animate('500ms', style({ transform: 'translateX(100%)', opacity: 0 }))])],
                    { optional: true }
                ),
            ]),
        ]),
    ],
})
export class AppComponent {
    started = InitialDatas.started;

    showMapCreator: boolean = InitialDatas.opened.mapCreator;
    showFreeroam: boolean = InitialDatas.opened.freeroam;
    showLobbyChoice: boolean = InitialDatas.opened.lobbyChoice;
    showTeamChoice: boolean = InitialDatas.opened.teamChoice;
    showRankings: boolean = InitialDatas.opened.rankings;
    showHud: boolean = InitialDatas.opened.hud;
    showCharCreator: boolean = InitialDatas.opened.charCreator;
    showGangWindow: boolean = InitialDatas.opened.gangWindow;
    showDamageTestMenu: boolean = InitialDatas.opened.damageTestMenu;

    rankings: RoundPlayerRankingStat[];
    teamOrdersLength = Object.values(TeamOrder).length;
    charCreateData: CharCreateData;
    damageTestMenuInitWeapon: WeaponHash = InitialDatas.getDamageTestInitialWeapon();

    constructor(
        public settings: SettingsService,
        rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef,
        private notificationService: NotificationService,
        public vcRef: ViewContainerRef,
        private iconRegistry: MatIconRegistry,
        private sanitizer: DomSanitizer,
        private materialCssVarsService: MaterialCssVarsService
    ) {
        this.loadSvgIcons();

        rageConnector.listen(FromClientEvent.InitLoadAngular, (constantsDataJson: string) => {
            this.settings.Constants = JSON.parse(constantsDataJson);
            this.started = true;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.RefreshAdminLevel, (adminLevel: number) => {
            this.settings.loadAdminLevel(adminLevel);
        });

        rageConnector.listen(FromClientEvent.ToggleMapCreator, (bool: boolean) => {
            this.showMapCreator = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ToggleFreeroam, (bool: boolean) => {
            this.showFreeroam = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ToggleLobbyChoice, (bool: boolean) => {
            this.showLobbyChoice = bool;
            if (bool) this.showTeamChoice = false;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ToggleTeamChoiceMenu, (bool: boolean) => {
            this.showTeamChoice = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ToggleUserpanel, (bool: boolean) => {
            this.settings.setUserpanelOpen(bool);
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ToggleGangWindow, (bool: boolean) => {
            this.showGangWindow = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ShowRankings, (rankings: string) => {
            this.rankings = JSON.parse(rankings) as RoundPlayerRankingStat[];
            this.showRankings = true;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ToggleHUD, (bool: boolean) => {
            this.showHud = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.HideRankings, () => {
            this.rankings = undefined;
            this.showRankings = false;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ToggleCharCreator, (bool: boolean, dataJson: string) => {
            if (dataJson.length) {
                this.charCreateData = JSON.parse(dataJson);
            }
            this.showCharCreator = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(FromClientEvent.ShowCooldown, () => {
            this.notificationService.showInfo('Cooldown');
        });

        rageConnector.listen(FromClientEvent.SyncUsernameChange, (newName: string) => {
            this.settings.Constants[7] = newName;
        });

        rageConnector.listen(FromClientEvent.ToggleDamageTestMenu, (toggle: boolean, json?: string, weapon?: WeaponHash) => {
            if (json) {
                this.settings.DamageTestWeaponDatas = JSON.parse(json);
                this.damageTestMenuInitWeapon = weapon;
            }
            this.showDamageTestMenu = toggle;
            this.changeDetector.detectChanges();
        });

        this.settings.InFightLobbyChanged.on(null, () => changeDetector.detectChanges());

        this.settings.ThemeSettingChangedBefore.on(null, this.onThemeSettingChanged.bind(this));
        this.settings.SettingsLoaded.on(null, this.onThemeSettingsLoaded.bind(this));
    }

    private onThemeSettingChanged(key: SettingsThemeIndex, value: any) {
        switch (key) {
            case SettingsThemeIndex.UseDarkTheme:
                this.materialCssVarsService.setDarkTheme(value);
                break;
            case SettingsThemeIndex.ThemeMainColor:
                this.materialCssVarsService.setPrimaryColor(value);
                break;
            case SettingsThemeIndex.ThemeSecondaryColor:
                this.materialCssVarsService.setAccentColor(value);
                break;
            case SettingsThemeIndex.ThemeWarnColor:
                this.materialCssVarsService.setWarnColor(value);
                break;
        }

        this.changeDetector.detectChanges();
    }

    private onThemeSettingsLoaded() {
        this.materialCssVarsService.setDarkTheme(this.settings.Settings[13]);
        this.materialCssVarsService.setPrimaryColor(this.settings.Settings[14]);
        this.materialCssVarsService.setAccentColor(this.settings.Settings[15]);
        this.materialCssVarsService.setWarnColor(this.settings.Settings[16]);
    }

    @HostListener('window:keydown', ['$event'])
    keyboardInput(event: KeyboardEvent) {
        if (event.ctrlKey && event.key === 'a' && !this.settings.InputFocused) {
            event.preventDefault();
        }
    }

    private loadSvgIcons() {
        this.iconRegistry.addSvgIcon('man', this.sanitizer.bypassSecurityTrustResourceUrl('assets/man.svg'));
        this.iconRegistry.addSvgIcon('woman', this.sanitizer.bypassSecurityTrustResourceUrl('assets/woman.svg'));

        const bodyPartKeys = Object.keys(PedBodyPart);
        for (const bodyPart of bodyPartKeys.slice(bodyPartKeys.length / 2)) {
            this.iconRegistry.addSvgIcon(bodyPart, this.sanitizer.bypassSecurityTrustResourceUrl('assets/body-parts/' + bodyPart + '.svg'));
        }

        const weaponKeys = Object.keys(WeaponHash);
        for (const weapon of weaponKeys.slice(weaponKeys.length / 2)) {
            this.iconRegistry.addSvgIcon(weapon, this.sanitizer.bypassSecurityTrustResourceUrl('assets/weapons/' + weapon + '.svg'));
        }
        this.iconRegistry.addSvgIcon('PistolColorless', this.sanitizer.bypassSecurityTrustResourceUrl('assets/weapons/PistolColorless.svg'));

        this.iconRegistry.addSvgIcon('test1', this.sanitizer.bypassSecurityTrustResourceUrl('assets/weapons/Carbinerifle.svg'));
        this.iconRegistry.addSvgIcon('test2', this.sanitizer.bypassSecurityTrustResourceUrl('assets/weapons/Assaultrifle.svg'));
    }
}
