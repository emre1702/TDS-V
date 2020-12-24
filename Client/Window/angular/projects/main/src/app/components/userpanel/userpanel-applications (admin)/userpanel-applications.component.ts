import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { RageConnectorService } from 'rage-connector';
import { UserpanelAdminQuestionsGroup } from '../interfaces/userpanelAdminQuestionsGroup';
import { UserpanelStatsDataDto } from '../interfaces/userpanelStatsDataDto';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { ApplicationInviteDialog } from '../../../dialog/application-invite-dialog';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'app-userpanel-applications',
    templateUrl: './userpanel-applications.component.html',
    styleUrls: ['./userpanel-applications.component.scss'],
})
export class UserpanelApplicationsComponent implements OnInit, OnDestroy {
    applicationData: {
        /** ApplicationID */
        0: number;
        /** Answers */
        1: { [index: number]: any };
        /** Questions */
        2: UserpanelAdminQuestionsGroup[];
        /** Stats */
        3: UserpanelStatsDataDto;
        /** AlreadyInvited */
        4: boolean;
    };

    applicationStatsColumns = {
        0: 'Id',
        1: 'Name',
        2: 'SCName',
        3: 'Gang',
        4: 'AdminLvl',
        5: 'Donation',
        6: 'IsVip',
        7: 'Money',
        8: 'TotalMoney',
        9: 'PlayTime',

        10: 'MuteTime',
        11: 'VoiceMuteTime',

        12: 'BansInLobbies',

        13: 'AmountMapsCreated',
        14: 'MapsRatedAverage',
        15: 'CreatedMapsAverageRating',
        16: 'AmountMapsRated',
        17: 'LastLogin',
        18: 'RegisterTimestamp',
        20: 'Logs',
        21: 'AmountLowPriorityIssues',
        22: 'AmountMediumPriorityIssues',
        23: 'AmountHighPriorityIssues',
        24: 'AmountUrgentPriorityIssues',
    };

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        public userpanelService: UserpanelService,
        private rageConnector: RageConnectorService,
        private dialog: MatDialog
    ) {}

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.AdminLevelChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.applicationsLoaded.on(null, this.applicationsLoadedFunc.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.AdminLevelChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.applicationsLoaded.off(null, this.applicationsLoadedFunc.bind(this));
        this.userpanelService.applications = undefined;
    }

    requestApplicationData(applicationID: number) {
        this.rageConnector.callCallbackServer(ToServerEvent.LoadApplicationDataForAdmin, [applicationID], this.applicationDataLoadedFunc.bind(this));
        this.changeDetector.detectChanges();
    }

    invite() {
        if (!this.canInvite()) {
            return;
        }
        this.dialog
            .open(ApplicationInviteDialog, { panelClass: 'mat-app-background' })
            .afterClosed()
            .subscribe((message: string | undefined) => {
                if (message == undefined) {
                    return;
                }

                this.rageConnector.callServer(ToServerEvent.SendApplicationInvite, this.applicationData[0], message);
                this.userpanelService.currentNav = UserpanelNavPage[UserpanelNavPage.Main];
            });
    }

    canInvite(): boolean {
        return !this.applicationData[4] && this.settings.AdminLevel == this.settings.AdminLevelForApplicationInvites;
    }

    private applicationsLoadedFunc() {
        this.changeDetector.detectChanges();
    }

    private applicationDataLoadedFunc(json: string) {
        this.applicationData = JSON.parse(json);
        if (typeof this.applicationData[1] === 'string') {
            this.applicationData[1] = JSON.parse(this.applicationData[1]);
        }
        if (typeof this.applicationData[2] === 'string') {
            this.applicationData[2] = JSON.parse(this.applicationData[2]);
        }
        if (typeof this.applicationData[3] === 'string') {
            this.applicationData[3] = JSON.parse(this.applicationData[3]);
        }
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
