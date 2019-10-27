import { Component, OnInit, ViewChild, Input, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { MatVerticalStepper, MatCheckboxChange, MatDialog } from '@angular/material';
import { RageConnectorService } from 'rage-connector';
import { UserpanelAdminQuestion } from '../interfaces/userpanelAdminQuestion';
import { UserpanelService } from '../services/userpanel.service';
import { UserpanelAdminQuestionAnswerType } from '../enums/userpanel-admin-question-answer-type';
import { AreYouSureDialog } from '../../../dialog/are-you-sure-dialog';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';

@Component({
    selector: 'app-userpanel-application',
    templateUrl: './userpanel-application.component.html',
    styleUrls: ['./userpanel-application.component.scss']
})
export class UserpanelApplicationComponent implements OnInit, OnDestroy {
    @ViewChild("stepper", { static: false }) stepper: MatVerticalStepper;

    private amountUnchecked = 0;
    answerType = UserpanelAdminQuestionAnswerType;
    applicationAlreadyCreated = false;
    dataLoaded = false;

    answersToAdminQuestions: { [index: number]: any } = {};

    constructor(public settings: SettingsService,
        public userpanelService: UserpanelService,
        private rageConnector: RageConnectorService,
        public changeDetector: ChangeDetectorRef,
        private dialog: MatDialog) { }

    ngOnInit() {
        this.dataLoaded = false;
        this.settings.LanguageChanged.on(null, this.detectChanges);
        this.userpanelService.applicationDataLoaded.on(null, this.dataLoadedFunc);
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges);
        this.userpanelService.applicationDataLoaded.off(null, this.dataLoadedFunc);
    }

    acceptInvitation(id: number) {
        this.rageConnector.call(DToServerEvent.AcceptInvitation, id);
    }

    rejectInvitation(id: number) {
        this.rageConnector.call(DToServerEvent.RejectInvitation, id);
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

                    this.rageConnector.call(DToServerEvent.SendApplication, answersJson);
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
        this.dataLoaded = true;
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
