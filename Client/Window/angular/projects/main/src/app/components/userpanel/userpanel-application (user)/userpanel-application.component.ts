import { Component, OnInit, ViewChild, Input, OnDestroy, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { MatVerticalStepper, MatCheckboxChange, MatDialog } from '@angular/material';
import { RageConnectorService } from 'rage-connector';
import { UserpanelService } from '../services/userpanel.service';
import { AreYouSureDialog } from '../../../dialog/are-you-sure-dialog';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';

@Component({
    selector: 'app-userpanel-application',
    templateUrl: './userpanel-application.component.html',
    styleUrls: ['./userpanel-application.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserpanelApplicationComponent implements OnInit, OnDestroy {
    @ViewChild("stepper", { static: false }) stepper: MatVerticalStepper;

    private amountUnchecked = 0;
    applicationAlreadyCreated = false;

    answersToAdminQuestions: { [index: number]: any } = {};

    constructor(public settings: SettingsService,
        public userpanelService: UserpanelService,
        private rageConnector: RageConnectorService,
        public changeDetector: ChangeDetectorRef,
        private dialog: MatDialog) { }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.applicationDataLoaded.on(null, this.dataLoadedFunc.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.applicationDataLoaded.off(null, this.dataLoadedFunc.bind(this));
    }

    acceptInvitation(id: number) {
        this.rageConnector.callServer(DToServerEvent.AcceptTDSTeamInvitation, id);
        this.userpanelService.currentNav = UserpanelNavPage[UserpanelNavPage.Main];
    }

    rejectInvitation(id: number) {
        this.rageConnector.callServer(DToServerEvent.RejectTDSTeamInvitation, id);
    }

    stepperAnimationDone() {
        const stepId = this.stepper._getStepLabelId(this.stepper.selectedIndex);
        const stepElement = document.getElementById(stepId);

        if (!stepElement)
            return;

        if (!this.isStepCompleted(stepElement)) {
            this.stepper.selected.completed = false;
            this.stepper.selected.hasError = true;
        } else {
            this.stepper.selected.completed = true;
            this.stepper.selected.hasError = false;
        }

        stepElement.scrollIntoView({ block: 'start', inline: 'nearest', behavior: 'smooth' });

        if (this.stepper.selectedIndex == this.stepper.steps.length - 1) {
            this.dialog.open(AreYouSureDialog, { panelClass: "mat-app-background" })
                .afterClosed()
                .subscribe((bool: boolean) => {
                    if (!bool)
                        return;
                    for (const index of Object.keys(this.answersToAdminQuestions)) {
                        this.answersToAdminQuestions[index] = String(this.answersToAdminQuestions[index]);
                    }
                    const answersJson = JSON.stringify(this.answersToAdminQuestions);

                    this.rageConnector.callServer(DToServerEvent.SendApplication, answersJson);
                    this.userpanelService.currentNav = UserpanelNavPage[UserpanelNavPage.Main];
                });
        }
    }

    checkboxChecked(event: MatCheckboxChange) {
        const prevAmountUnchecked = this.amountUnchecked;
        if (event.checked)
            this.amountUnchecked--;
        else
            this.amountUnchecked++;

        if (prevAmountUnchecked == 0 && this.amountUnchecked > 0) {
            this.stepper.selected.completed = false;
            this.stepper.selected.hasError = true;
        } else if (prevAmountUnchecked > 0 && this.amountUnchecked == 0) {
            this.stepper.selected.completed = true;
            this.stepper.selected.hasError = false;
        }
    }

    isStepCompleted(stepElement: HTMLElement): boolean {
        const element = stepElement.nextSibling as HTMLElement;
        this.amountUnchecked = element.querySelectorAll('input[aria-checked="false"]:not([required=true])').length;
        return this.amountUnchecked == 0;
    }

    private dataLoadedFunc() {
        if (this.userpanelService.myApplicationCreateTime) {
            this.applicationAlreadyCreated = true;
        } else {
            this.applicationAlreadyCreated = false;
        }
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
