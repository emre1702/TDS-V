import { Component, ChangeDetectionStrategy, ChangeDetectorRef, ViewContainerRef, HostListener, Sanitizer, HostBinding } from '@angular/core';
import { SettingsService } from './services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from './enums/dfromclientevent.enum';
import { MatSnackBar, MatIconRegistry } from '@angular/material';
import { RoundPlayerRankingStat } from './components/ranking/models/roundPlayerRankingStat';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';
import { TeamOrder } from './components/teamorders/enums/teamorder.enum';
import { DomSanitizer } from '@angular/platform-browser';
import { CharCreateData } from './components/char-creator/interfaces/charCreateData';
import { OverlayContainer } from '@angular/cdk/overlay';
import { MaterialCssVarsService } from 'angular-material-css-vars';
import { UserpanelSettingKey } from './components/userpanel/enums/userpanel-setting-key.enum';
import { ThemeSettings } from './interfaces/theme-settings';
import { PedBodyPart } from './components/userpanel/enums/ped-body-part.enum';
import { WeaponHash } from './components/lobbychoice/enums/weapon-hash.enum';

@Component({
    selector: 'app-root',
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    animations: [

        trigger('teamOrdersAnimation', [
            transition('* => *', [
                query(':enter', [
                    style({ transform: 'translateX(100%)', opacity: 0 }),
                    stagger(150, [
                        animate('500ms', style({ transform: 'translateX(0)', opacity: 1 })),
                    ])
                ], { optional: true }),
                query(':leave', [
                    style({ transform: 'translateX(0)', opacity: 1 }),
                    stagger(150, [
                        animate('500ms', style({ transform: 'translateX(100%)', opacity: 0 }))
                    ])
                ], { optional: true })
            ])
        ])
    ],
})
export class AppComponent {
    started = true;

    showMapCreator = false;
    showFreeroam = false;
    showLobbyChoice = false;
    showTeamChoice = false;
    showRankings = false;
    showHUD = false;
    showCharCreator = false;
    showGangWindow = false;

    rankings: RoundPlayerRankingStat[];
    teamOrdersLength = Object.values(TeamOrder).length;
    charCreateData: CharCreateData;

    constructor(
        public settings: SettingsService,
        rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef,
        snackBar: MatSnackBar,
        public vcRef: ViewContainerRef,
        private iconRegistry: MatIconRegistry,
        private sanitizer: DomSanitizer,
        private materialCssVarsService: MaterialCssVarsService) {

        this.loadSvgIcons();

        rageConnector.listen(DFromClientEvent.InitLoadAngular, (constantsDataJson: string) => {
            this.settings.Constants = JSON.parse(constantsDataJson);
            this.started = true;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.RefreshAdminLevel, (adminLevel: number) => {
            this.settings.loadAdminLevel(adminLevel);
        });

        rageConnector.listen(DFromClientEvent.ToggleMapCreator, (bool: boolean) => {
            this.showMapCreator = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ToggleFreeroam, (bool: boolean) => {
            this.showFreeroam = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ToggleLobbyChoice, (bool: boolean) => {
            this.showLobbyChoice = bool;
            if (bool)
                this.showTeamChoice = false;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ToggleTeamChoiceMenu, (bool: boolean) => {
            this.showTeamChoice = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ToggleUserpanel, (bool: boolean) => {
            this.settings.setUserpanelOpened(bool);
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ToggleGangWindow, (bool: boolean) => {
            this.showGangWindow = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ShowRankings, (rankings: string) => {
            this.rankings = JSON.parse(rankings) as RoundPlayerRankingStat[];
            this.showRankings = true;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ToggleHUD, (bool: boolean) => {
            this.showHUD = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.HideRankings, () => {
            this.rankings = undefined;
            this.showRankings = false;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ToggleCharCreator, (bool: boolean, dataJson: string) => {
            if (dataJson.length) {
                this.charCreateData = JSON.parse(dataJson);
            }
            this.showCharCreator = bool;
            changeDetector.detectChanges();
        });

        rageConnector.listen(DFromClientEvent.ShowCooldown, () => {
            snackBar.open("Cooldown", undefined, { duration: 3000, panelClass: "mat-app-background" });
        });

        rageConnector.listen(DFromClientEvent.SyncUsernameChange, (newName: string) => {
            this.settings.Constants[7] = newName;
        });

        this.settings.InFightLobbyChanged.on(null, () => changeDetector.detectChanges());

        this.settings.ThemeSettingChangedBefore.on(null, this.onThemeSettingChanged.bind(this));
        this.settings.ThemeSettingsLoaded.on(null, this.onThemeSettingsLoaded.bind(this));
    }

    private onThemeSettingChanged(key: UserpanelSettingKey, value: any) {
        switch (key) {
            case UserpanelSettingKey.UseDarkTheme:
                this.materialCssVarsService.setDarkTheme(value);
                break;
            case UserpanelSettingKey.ThemeMainColor:
                this.materialCssVarsService.setPrimaryColor(value);
                break;
            case UserpanelSettingKey.ThemeSecondaryColor:
                this.materialCssVarsService.setAccentColor(value);
                break;
            case UserpanelSettingKey.ThemeWarnColor:
                this.materialCssVarsService.setWarnColor(value);
                break;
        }

        this.changeDetector.detectChanges();
    }

    private onThemeSettingsLoaded(settings: ThemeSettings) {
        this.materialCssVarsService.setDarkTheme(settings[1000]);
        this.materialCssVarsService.setPrimaryColor(settings[1001]);
        this.materialCssVarsService.setAccentColor(settings[1002]);
        this.materialCssVarsService.setWarnColor(settings[1003]);
    }

    @HostListener("window:keydown", ["$event"])
    keyboardInput(event: KeyboardEvent) {
        if (event.ctrlKey && event.key === "a" && !this.settings.InputFocused) {
            event.preventDefault();
        }
    }


    private loadSvgIcons() {
        this.iconRegistry.addSvgIcon("man", this.sanitizer.bypassSecurityTrustResourceUrl('assets/man.svg'));
        this.iconRegistry.addSvgIcon("woman", this.sanitizer.bypassSecurityTrustResourceUrl('assets/woman.svg'));

        const bodyPartKeys = Object.keys(PedBodyPart);
        for (const bodyPart of bodyPartKeys.slice(bodyPartKeys.length / 2)) {
            this.iconRegistry.addSvgIcon(bodyPart, this.sanitizer.bypassSecurityTrustResourceUrl("assets/body-parts/" + bodyPart + ".svg"));
        }

        const weaponKeys = Object.keys(WeaponHash);
        for (const weapon of weaponKeys.slice(weaponKeys.length / 2)) {
            this.iconRegistry.addSvgIcon(weapon, this.sanitizer.bypassSecurityTrustResourceUrl("assets/weapons/" + weapon + ".svg"));
        }
        this.iconRegistry.addSvgIcon("PistolColorless", this.sanitizer.bypassSecurityTrustResourceUrl('assets/weapons/PistolColorless.svg'));

        this.iconRegistry.addSvgIcon("test1", this.sanitizer.bypassSecurityTrustResourceUrl('assets/weapons/Carbinerifle.svg'));
        this.iconRegistry.addSvgIcon("test2", this.sanitizer.bypassSecurityTrustResourceUrl('assets/weapons/Assaultrifle.svg'));
    }
}
