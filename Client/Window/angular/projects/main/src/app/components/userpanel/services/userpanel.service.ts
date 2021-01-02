import { Injectable } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { EventEmitter } from 'events';
import { UserpanelRuleDataDto } from '../interfaces/userpanelRuleDataDto';
import { UserpanelLoadDataType } from '../enums/userpanel-load-data-type.enum';
import { UserpanelFAQDataDto } from '../interfaces/userpanelFAQDataDto';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelSettingNormalDataDto } from '../interfaces/userpanelSettingNormalDataDto';
import { UserpanelStatsDataDto } from '../interfaces/userpanelStatsDataDto';
import { UserpanelAdminQuestionsGroup } from '../interfaces/userpanelAdminQuestionsGroup';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { UserpanelSettingSpecialDataDto } from '../interfaces/userpanelSettingSpecialDataDto';
import { FromServerEvent } from '../../../enums/from-server-event.enum';
import { UserpanelSupportRequestListData } from '../interfaces/userpanelSupportRequestListData';
import { UserpanelSettingCommandDataDto } from '../interfaces/settings-commands/userpanelSettingCommandDataDto';
import { InitialDatas } from '../../../initial-datas';

@Injectable({
    providedIn: 'root',
})
export class UserpanelService {
    loadingData = false;

    supportTypeIcons: { [type: number]: string } = {
        [UserpanelSupportType.Question]: 'help',
        [UserpanelSupportType.Help]: 'info',
        [UserpanelSupportType.Compliment]: 'thumb_up',
        [UserpanelSupportType.Complaint]: 'thumb_down',
    };
    supportTypeBorderColors: { [type: number]: string } = {
        [UserpanelSupportType.Question]: 'rgb(0,0,150)',
        [UserpanelSupportType.Help]: 'rgb(150,150,150)',
        [UserpanelSupportType.Compliment]: 'rgb(0,150,0)',
        [UserpanelSupportType.Complaint]: 'rgb(150,0,0)',
    };

    private _currentNav: string = UserpanelNavPage[UserpanelNavPage.Main];

    get currentNav(): string {
        return this._currentNav;
    }
    set currentNav(value: string) {
        this._currentNav = value;
        this.currentNavChanged.emit(null);
    }

    allRules: UserpanelRuleDataDto[] = InitialDatas.getAllRules();
    allFAQs: UserpanelFAQDataDto[] = InitialDatas.getAllFAQs();
    allSettingsSpecial: UserpanelSettingSpecialDataDto;
    allSettingsNormal: UserpanelSettingNormalDataDto;
    settingsCommandsData: UserpanelSettingCommandDataDto = InitialDatas.getSettingsCommandData();
    myStatsGeneral: UserpanelStatsDataDto /*= {
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
    myStatsWeaponsUsed: string[];
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
    supportRequests: UserpanelSupportRequestListData[];
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
    public rulesLoaded = new EventEmitter();
    public faqsLoaded = new EventEmitter();
    public settingsSpecialLoaded = new EventEmitter();
    public settingsCommandsDataLoaded = new EventEmitter();
    public myStatsGeneralLoaded = new EventEmitter();
    public applicationDataLoaded = new EventEmitter();
    public applicationsLoaded = new EventEmitter();
    public supportRequestsLoaded = new EventEmitter();
    public offlineMessagesLoaded = new EventEmitter();
    public myStatsWeaponsUsedLoaded = new EventEmitter();

    private myStatsGeneralLoadCooldown: NodeJS.Timeout;

    constructor(private rageConnector: RageConnectorService, private settings: SettingsService) {
        rageConnector.listen(FromServerEvent.LoadUserpanelData, this.loadUserpanelData.bind(this));
        settings.LanguageChanged.on(null, this.languageChanged.bind(this));
    }

    setLoadingData(toggle: boolean) {
        this.loadingData = toggle;
        this.loadingDataChanged.emit(null);
    }

    loadRules() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.Rules);
    }

    loadFAQs() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.FAQs);
    }

    loadSettingsSpecial() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SettingsSpecial);
    }

    loadSettingsCommands() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SettingsCommands);

        /*this.settingsCommandsData = {
            0: [
                { 0: 1, 1: "AdminSay" },
                { 0: 2, 1: "AdminChat" },
                { 0: 3, 1: "Goto1" },
                { 0: 4, 1: "Goto2" },
                { 0: 5, 1: "Goto3" },
                { 0: 6, 1: "Goto4" },
                { 0: 7, 1: "Goto5" },
                { 0: 8, 1: "Goto6" },
                { 0: 9, 1: "Goto7" },
                { 0: 10, 1: "Goto8" },
                { 0: 11, 1: "Goto9" },
                { 0: 12, 1: "Goto10" },
                { 0: 13, 1: "Goto11" },
                { 0: 14, 1: "Goto12" },
                { 0: 15, 1: "Goto13" },
                { 0: 16, 1: "Goto14" },
            ],
            1: []
        };
        this.loadingData = false;
        this.loadingDataChanged.emit(null);*/
    }

    loadMyStatsGeneral() {
        if (this.myStatsGeneralLoadCooldown) {
            this.loadingData = false;
            this.loadingDataChanged.emit(null);
            return;
        }

        this.myStatsGeneralLoadCooldown = setTimeout(this.myStatsGeneralLoadingCooldownEnded.bind(this), 3 * 60 * 1000);
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.MyStatsGeneral);
    }

    loadMyStatsWeapon() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.MyStatsWeapon);

        /*this.myStatsWeaponsUsed = [ "assaultrifle", "carbinerifle", "advancedrifle" ];
        this.myStatsWeaponsUsedLoaded.emit(null);

        this.loadingData = false;
        this.loadingDataChanged.emit(null);*/
    }

    loadApplicationPage() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.ApplicationUser);
    }

    loadApplicationsPage() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.ApplicationsAdmin);
    }

    loadUserSupportRequests() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SupportUser);
    }

    loadSupportRequestsForAdmin() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.SupportAdmin);
    }

    loadOfflineMessages() {
        this.rageConnector.call(ToServerEvent.LoadUserpanelData, UserpanelLoadDataType.OfflineMessages);
    }

    private loadUserpanelData(type: UserpanelLoadDataType, json: string) {
        json = json.escapeJson();

        this.loadingData = false;
        this.loadingDataChanged.emit(null);

        switch (type) {
            case UserpanelLoadDataType.Rules:
                this.loadedAllRules(json);
                break;
            case UserpanelLoadDataType.FAQs:
                this.loadedAllFAQs(json);
                break;
            case UserpanelLoadDataType.SettingsSpecial:
                this.loadedAllSettingsSpecial(json);
                break;
            case UserpanelLoadDataType.SettingsCommands:
                this.loadedSettingsCommandsData(json);
                break;
            case UserpanelLoadDataType.MyStatsGeneral:
                this.loadedMyStatsGeneral(json);
                break;
            case UserpanelLoadDataType.MyStatsWeapon:
                this.loadedMyStatsWeapon(json);
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

    private loadedAllRules(json: string) {
        this.allRules = JSON.parse(json);
        this.allRules.sort((a, b) => (a[0] < b[0] ? -1 : 1));
        this.rulesLoaded.emit(null);
    }

    private loadedAllFAQs(json: string) {
        this.allFAQs = JSON.parse(json);
        this.allFAQs.sort((a, b) => (a[0] < b[0] ? -1 : 1));
        this.faqsLoaded.emit(null);
    }

    private loadedAllSettingsSpecial(json: string) {
        this.allSettingsSpecial = JSON.parse(json);
        this.settingsSpecialLoaded.emit(null);
    }

    private loadedSettingsCommandsData(json: string) {
        this.settingsCommandsData = JSON.parse(json);
        if (typeof this.settingsCommandsData[0] === 'string') {
            this.settingsCommandsData[0] = JSON.parse(this.settingsCommandsData[0]);
        }
        this.settingsCommandsDataLoaded.emit(null);
    }

    private loadedMyStatsGeneral(json: string) {
        this.myStatsGeneral = JSON.parse(json);
        this.myStatsGeneral[20].sort((a, b) => (a[1] < b[1] ? -1 : 1));
        this.myStatsGeneralLoaded.emit(null);
    }

    private loadedMyStatsWeapon(json: string) {
        this.myStatsWeaponsUsed = JSON.parse(json);
        this.myStatsWeaponsUsedLoaded.emit(null);
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
            if (typeof data[1] === 'string') {
                data[1] = JSON.parse(data[1]);
            }
            this.adminApplyInvitations = data[1];
            // !data.CreateTime -> No application, user can create a new one
        } else {
            if (typeof data[2] === 'string') {
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
        if (this.allSettingsNormal) this.allSettingsNormal[1] = this.settings.LangValue;
    }

    myStatsGeneralLoadingCooldownEnded() {
        this.myStatsGeneralLoadCooldown = undefined;
    }
}
