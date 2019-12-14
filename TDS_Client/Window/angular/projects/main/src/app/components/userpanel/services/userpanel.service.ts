import { Injectable } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { EventEmitter } from 'events';
import { UserpanelRuleDataDto } from '../interfaces/userpanelRuleDataDto';
import { UserpanelLoadDataType } from '../enums/userpanel-load-data-type.enum';
import { UserpanelFAQDataDto } from '../interfaces/userpanelFAQDataDto';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelSettingNormalDataDto } from '../interfaces/userpanelSettingNormalDataDto';
import { UserpanelStatsDataDto } from '../interfaces/userpanelStatsDataDto';
import { UserpanelAdminQuestionsGroup } from '../interfaces/userpanelAdminQuestionsGroup';
import { UserpanelAdminQuestionAnswerType } from '../enums/userpanel-admin-question-answer-type';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { UserpanelSettingSpecialDataDto } from '../interfaces/userpanelSettingSpecialDataDto';
import { LanguageEnum } from '../../../enums/language.enum';
import { TimezoneEnum } from '../enums/timezone.enum';
import { DateTimeFormatEnum } from '../enums/datetime-format.enum';

@Injectable({
    providedIn: 'root'
})
export class UserpanelService {
    loadingData = false;

    supportTypeIcons: { [type: number]: string} = {
        [UserpanelSupportType.Question]: "help",
        [UserpanelSupportType.Help]: "info",
        [UserpanelSupportType.Compliment]: "thumb_up",
        [UserpanelSupportType.Complaint]: "thumb_down"
    };
    supportTypeBorderColors: { [type: number]: string} = {
        [UserpanelSupportType.Question]: "rgb(0,0,150)",
        [UserpanelSupportType.Help]: "rgb(150,150,150)",
        [UserpanelSupportType.Compliment]: "rgb(0,150,0)",
        [UserpanelSupportType.Complaint]: "rgb(150,0,0)"
    };

    private _currentNav: string = UserpanelNavPage[UserpanelNavPage.Main];

    get currentNav(): string {
        return this._currentNav;
    }
    set currentNav(value: string) {
        this._currentNav = value;
        this.currentNavChanged.emit(null);
    }

    allCommands: UserpanelCommandDataDto[] = [];
    allRules: UserpanelRuleDataDto[] = [];
    allFAQs: UserpanelFAQDataDto[] = [];
    allSettingsSpecial: UserpanelSettingSpecialDataDto;
    allSettingsNormal: UserpanelSettingNormalDataDto;
    myStats: UserpanelStatsDataDto /*= {
        Id: 1,
        AdminLvl: 3,
        AmountMapsCreated: 5,
        AmountMapsRated: 2,
        BansInLobbies: ["Bim bam", "Bim bim bam"],
        CreatedMapsAverageRating: 3.3,
        Donation: 0,
        IsVip: true,
        LastLogin: "12312 21 321",
        MapsRatedAverage: 21,
        Money: 10000,
        Name: "Bonus",
        PlayTime: 123,
        RegisterTimestamp: "12. 21.312.3.13.12321",
        SCName: "Boaksdk poaspodasdkpoa",
        TotalMoney: 12000,
        Logs: [
            { Admin: "Bonus", AsDonator: false, AsVip: true, Lobby: "Arena",
            Reason: "Hat dieas doias doijasoi djoas jdoiasoi djaosid jio", Timestamp: "a901290391 12 30912", Type: "Ban", LengthOrEndTime: "asdpaspo dkposad"},
            { Admin: "Bonus", AsDonator: false, AsVip: false, Lobby: "Arena",
            Reason: "Hat dieas doias doijasoi djoas jdoiasoi djaosid jio", Timestamp: "a901290391 12 30912", Type: "Ban", LengthOrEndTime: "23"},
            { Admin: "Bonus", AsDonator: false, AsVip: true, Lobby: "Arena",
            Reason: "Hat dieas doias doijasoi djoas jdoiasoi djaosid jio", Timestamp: "a901290391 12 30912", Type: "Ban"}
        ]
      }*/;
    adminQuestions: UserpanelAdminQuestionsGroup[] = [];
    myApplicationCreateTime: string = undefined;
    adminApplyInvitations: [
        /** ID */
        number,
        /** AdminName */
        string,
        /** AdminSCName */
        string,
        /** Message */
        string
    ][];
    applications: [
        /** ID */
        number,
        /** CreateTime */
        string,
        /** PlayerName */
        string
    ][];
    supportRequests: [
        /** ID */
        number,
        /** PlayerName */
        string,
        /** CreateTime */
        string,
        /** Title */
        string,
        /** Type */
        UserpanelSupportType,
        /** Closed */
        boolean
    ][];
    offlineMessages: [
        /** ID */
        number,
        /** PlayerName */
        string,
        /** CreateTime */
        string,
        /** Text */
        string,
        /** Seen */
        boolean
    ][];

    public currentNavChanged = new EventEmitter();
    public loadingDataChanged = new EventEmitter();
    public commandsLoaded = new EventEmitter();
    public rulesLoaded = new EventEmitter();
    public faqsLoaded = new EventEmitter();
    public settingsSpecialLoaded = new EventEmitter();
    public settingsNormalLoaded = new EventEmitter();
    public myStatsLoaded = new EventEmitter();
    public applicationDataLoaded = new EventEmitter();
    public applicationsLoaded = new EventEmitter();
    public supportRequestsLoaded = new EventEmitter();
    public offlineMessagesLoaded = new EventEmitter();

    private myStatsLoadCooldown: NodeJS.Timeout;

    constructor(private rageConnector: RageConnectorService, private settings: SettingsService) {
        rageConnector.listen(DToServerEvent.LoadUserpanelData, this.loadUserpanelData.bind(this));
        settings.LanguageChanged.on(null, this.languageChanged.bind(this));
    }

    loadCommands() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.Commands);
    }

    loadRules() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.Rules);
    }

    loadFAQs() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.FAQs);
    }

    loadSettingsSpecial() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SettingsSpecial);
    }

    loadSettingsNormal() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SettingsNormal);

        this.settings.Constants = [
            1, 1, 10000, 60, 12, 23
        ];
        this.allSettingsNormal = [0, LanguageEnum.English, true, true, TimezoneEnum["(UTC) Coordinated Universal Time"],
        "", true, true, true, true, true, 0, "asd", DateTimeFormatEnum["dd'-'MM'-'yyyy HH':'mm':'ss"]];
        this.settingsNormalLoaded.emit(null);
        this.loadingData = false;
        this.loadingDataChanged.emit(null);
    }

    loadMyStats() {
        if (this.myStatsLoadCooldown) {
            this.loadingData = false;
            this.loadingDataChanged.emit(null);
            return;
        }

        this.myStatsLoadCooldown = setTimeout(this.myStatsLoadingCooldownEnded.bind(this), 3 * 60 * 1000);
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.MyStats);
    }

    loadApplicationPage() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.ApplicationUser);
    }

    loadApplicationsPage() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.ApplicationsAdmin);
    }

    loadUserSupportRequests() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SupportUser);
    }

    loadSupportRequestsForAdmin() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SupportAdmin);
    }

    loadOfflineMessages() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.OfflineMessages);
    }

    private loadUserpanelData(type: UserpanelLoadDataType, json: string) {
        json = this.escapeSpecialChars(json);

        this.loadingData = false;
        this.loadingDataChanged.emit(null);

        switch (type) {
            case UserpanelLoadDataType.Commands:
                this.loadedAllCommands(json);
                break;
            case UserpanelLoadDataType.Rules:
                this.loadedAllRules(json);
                break;
            case UserpanelLoadDataType.FAQs:
                this.loadedAllFAQs(json);
                break;
            case UserpanelLoadDataType.SettingsSpecial:
                this.loadedAllSettingsSpecial(json);
                break;
            case UserpanelLoadDataType.SettingsNormal:
                this.loadedAllSettingsNormal(json);
                break;
            case UserpanelLoadDataType.MyStats:
                this.loadedMyStats(json);
                break;
            case UserpanelLoadDataType.ApplicationUser:
                this.loadedApplicationDataForUser(json);
                break;
            case UserpanelLoadDataType.ApplicationsAdmin:
                this.loadedApplicationsForAdmin(json);
                break;
            case UserpanelLoadDataType.SupportUser:
                this.loadedUserSupportRequests(json);
                break;
            case UserpanelLoadDataType.SupportAdmin:
                this.loadedSupportRequestsForAdmin(json);
                break;
            case UserpanelLoadDataType.OfflineMessages:
                this.loadedOfflineMessages(json);
                break;
        }
    }

    private loadedAllCommands(json: string) {
        this.allCommands = JSON.parse(json);
        this.allCommands.sort((a, b) => a[0] < b[0] ? -1 : 1);
        this.allCommands.forEach(c => c[6].sort());
        this.commandsLoaded.emit(null);
    }

    private loadedAllRules(json: string) {
        this.allRules = JSON.parse(json);
        this.allRules.sort((a, b) => a[0] < b[0] ? -1 : 1);
        this.rulesLoaded.emit(null);
    }

    private loadedAllFAQs(json: string) {
        this.allFAQs = JSON.parse(json);
        this.allFAQs.sort((a, b) => a[0] < b[0] ? -1 : 1);
        this.faqsLoaded.emit(null);
    }

    private loadedAllSettingsSpecial(json: string) {
        this.allSettingsSpecial = JSON.parse(json);
        this.settingsSpecialLoaded.emit(null);
    }

    private loadedAllSettingsNormal(json: string) {
        this.allSettingsNormal = JSON.parse(json);
        this.settingsNormalLoaded.emit(null);
    }

    private loadedMyStats(json: string) {
        this.myStats = JSON.parse(json);
        this.myStats[20].sort((a, b) => a[1] < b[1] ? -1 : 1);
        this.myStatsLoaded.emit(null);
    }

    private loadedApplicationDataForUser(json: string) {
        const data = JSON.parse(json) as [
            /** CreateTime */
            string,
            /** Invitations */
            any[],
            /** AdminQuestions */
            any[]
        ];
        // data.CreateTime -> Application already exists
        if (data[0]) {
            this.myApplicationCreateTime = data[0];
            if (typeof (data[1]) === "string") {
                data[1] = JSON.parse(data[1]);
            }
            this.adminApplyInvitations = data[1];
            // !data.CreateTime -> No application, user can create a new one
        } else {
            if (typeof (data[2]) === "string") {
                data[2] = JSON.parse(data[2]);
            }
            this.adminQuestions = data[2];
            this.adminApplyInvitations = [];
            this.myApplicationCreateTime = undefined;
        }
        this.applicationDataLoaded.emit(null);
    }

    private loadedApplicationsForAdmin(json: string) {
        this.applications = JSON.parse(json);
        this.applicationsLoaded.emit(null);
    }

    private loadedUserSupportRequests(json: string) {
        this.supportRequests = JSON.parse(json);
        this.supportRequestsLoaded.emit(null);
    }

    private loadedSupportRequestsForAdmin(json: string) {
        this.supportRequests = JSON.parse(json);
        this.supportRequestsLoaded.emit(null);
    }

    private loadedOfflineMessages(json: string) {
        this.offlineMessages = JSON.parse(json);
        this.offlineMessagesLoaded.emit(null);
    }

    private languageChanged() {
        this.allFAQs = [];
        if (this.allSettingsNormal)
            this.allSettingsNormal[1] = this.settings.LangValue;
    }


    myStatsLoadingCooldownEnded() {
        this.myStatsLoadCooldown = undefined;
    }

    private escapeSpecialChars(json: string) {
        return json.replace(/\n/g, "\\n")
            .replace(/\r/g, "\\r")
            .replace(/\t/g, "\\t")
            .replace(/\f/g, "\\f");
    }
}
