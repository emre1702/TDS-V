import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { UserpanelAdminQuestionsGroup } from '../interfaces/userpanelAdminQuestionsGroup';
import { UserpanelStatsDataDto } from '../interfaces/userpanelStatsDataDto';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { UserpanelAdminQuestionAnswerType } from '../enums/userpanel-admin-question-answer-type';

@Component({
    selector: 'app-userpanel-applications',
    templateUrl: './userpanel-applications.component.html',
    styleUrls: ['./userpanel-applications.component.scss']
})
export class UserpanelApplicationsComponent implements OnInit, OnDestroy {
    applicationData: {
        Answers: { [index: number]: any },
        Questions: UserpanelAdminQuestionsGroup[],
        Stats: UserpanelStatsDataDto
    };

    applicationStatsColumns = ["Id",
        "Name",
        "SCName",
        "Gang",
        "AdminLvl",
        "Donation",
        "IsVip",
        "Money",
        "TotalMoney",
        "PlayTime",

        "MuteTime",
        "VoiceMuteTime",

        "BansInLobbies",

        "AmountMapsCreated",
        "MapsRatedAverage",
        "CreatedMapsAverageRating",
        "AmountMapsRated",
        "LastLogin",
        "RegisterTimestamp",
        "Logs"];

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        public userpanelService: UserpanelService,
        private rageConnector: RageConnectorService) {
            this.rageConnector.listen(DFromClientEvent.LoadApplicationDataForAdmin, this.applicationDataLoadedFunc.bind(this));
        }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.applicationsLoaded.on(null, this.applicationsLoadedFunc.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.applicationsLoaded.off(null, this.applicationsLoadedFunc.bind(this));
        this.userpanelService.applications = undefined;
    }

    requestApplicationData(applicationID: number) {
        this.userpanelService.loadingData = true;
        this.rageConnector.callCallback(DToServerEvent.LoadApplicationDataForAdmin, [applicationID], this.applicationDataLoadedFunc.bind(this));
        this.changeDetector.detectChanges();
    }

    private applicationsLoadedFunc() {
        this.changeDetector.detectChanges();
    }

    private applicationDataLoadedFunc(json) {
        this.userpanelService.loadingData = false;
        this.applicationData = JSON.parse(json);
        /*for (const [questionId, answer] of Object.entries(this.applicationData.Answers)) {
            this.applicationData.Answers[questionId] = "asd";
        }*/
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
