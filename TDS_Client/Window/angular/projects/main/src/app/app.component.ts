import { Component, ChangeDetectionStrategy, ChangeDetectorRef, ViewContainerRef } from '@angular/core';
import { SettingsService } from './services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from './enums/dfromclientevent.enum';
import { MatSnackBar } from '@angular/material';
import { RoundPlayerRankingStat } from './components/ranking/models/roundPlayerRankingStat';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';
import { TeamOrder } from './components/teamorders/enums/teamorder.enum';
import { Invitation } from './components/utils/invitation/models/invitation';

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
    showMapCreator = false;
    showFreeroam = false;
    showLobbyChoice = true;
    showTeamChoice = false;
    showRankings = false;
    showHUD = false;

    rankings: RoundPlayerRankingStat[];

    constructor(
        public settings: SettingsService,
        rageConnector: RageConnectorService,
        changeDetector: ChangeDetectorRef,
        snackBar: MatSnackBar) {

        rageConnector.listen(DFromClientEvent.InitLoadAngular, (constantsDataJson: string, challengesJson: string) => {
            this.settings.Constants = JSON.parse(constantsDataJson);
            this.settings.ChallengeGroups = JSON.parse(challengesJson);
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
            this.settings.UserpanelOpened = bool;
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

        rageConnector.listen(DFromClientEvent.ShowCooldown, () => {
            snackBar.open("Cooldown", undefined, { duration: 3000 });
        });

        this.settings.InFightLobbyChanged.on(null, () => changeDetector.detectChanges());
    }


    getTeamOrdersLength(): number {
        return Object.values(TeamOrder).length;
    }
}
