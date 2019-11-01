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
        }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.AdminLevelChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.applicationsLoaded.on(null, this.applicationsLoadedFunc.bind(this));
        this.rageConnector.listen(DFromClientEvent.LoadApplicationDataForAdmin, this.applicationDataLoadedFunc.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.AdminLevelChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.applicationsLoaded.off(null, this.applicationsLoadedFunc.bind(this));
        this.userpanelService.applications = undefined;
        this.rageConnector.remove(DFromClientEvent.LoadApplicationDataForAdmin, this.applicationDataLoadedFunc.bind(this));
    }

    requestApplicationData(applicationID: number) {
        this.userpanelService.loadingData = true;
        this.rageConnector.callCallback(DToServerEvent.LoadApplicationDataForAdmin, [applicationID], this.applicationDataLoadedFunc.bind(this));
        this.changeDetector.detectChanges();
    }

    private applicationsLoadedFunc() {
        this.changeDetector.detectChanges();
    }

    private applicationDataLoadedFunc(json: string) {
        this.userpanelService.loadingData = false;
        this.applicationData = JSON.parse(json);
        if (typeof(this.applicationData.Answers) === "string") {
            this.applicationData.Answers = JSON.parse(this.applicationData.Answers);
        }
        if (typeof(this.applicationData.Questions) === "string") {
            this.applicationData.Questions = JSON.parse(this.applicationData.Questions);
        }
        if (typeof(this.applicationData.Stats) === "string") {
            this.applicationData.Stats = JSON.parse(this.applicationData.Stats);
        }
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
