import { Injectable } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { EventEmitter } from 'events';
import { UserpanelRuleDataDto } from '../interfaces/userpanelRuleDataDto';
import { UserpanelLoadDataType } from '../enums/userpanel-load-data-type.enum';
import { UserpanelFAQDataDto } from '../interfaces/userpanelFAQDataDto';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelSettingDataDto } from '../interfaces/userpanelSettingDataDto';
import { UserpanelStatsDataDto } from '../interfaces/userpanelStatsDataDto';
import { UserpanelAdminQuestionsGroup } from '../interfaces/userpanelAdminQuestionsGroup';
import { UserpanelAdminQuestionAnswerType } from '../enums/userpanel-admin-question-answer-type';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';

@Injectable({
  providedIn: 'root'
})
export class UserpanelService {
    loadingData = false;

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
    allSettings: UserpanelSettingDataDto;
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
    adminApplyInvitations: { ID: number, AdminName: string, AdminSCName: string, Message: string }[];
    applications: { ID: number, CreateTime: string, PlayerName: string }[];

    public currentNavChanged = new EventEmitter();
    public loadingDataChanged = new EventEmitter();
    public commandsLoaded = new EventEmitter();
    public rulesLoaded = new EventEmitter();
    public faqsLoaded = new EventEmitter();
    public settingsLoaded = new EventEmitter();
    public myStatsLoaded = new EventEmitter();
    public applicationDataLoaded = new EventEmitter();
    public applicationsLoaded = new EventEmitter();

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

    loadSettings() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.Settings);
    }

    loadMyStats() {
        if (this.myStatsLoadCooldown)
            return;
        this.myStatsLoadCooldown = setTimeout(this.myStatsLoadingCooldownEnded.bind(this), 3 * 60 * 1000);
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.MyStats);
    }

    loadApplicationPage() {
      this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.ApplicationUser);
    }

    loadApplicationsPage() {
      this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.ApplicationsAdmin);
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
            case UserpanelLoadDataType.Settings:
                this.loadedAllSettings(json);
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
        }
    }

    private loadedAllCommands(json: string) {
        this.allCommands = JSON.parse(json);
        this.allCommands.sort((a, b) => a.Command < b.Command ? -1 : 1);
        this.allCommands.forEach(c => c.Aliases.sort());
        this.commandsLoaded.emit(null);
    }

    private loadedAllRules(json: string) {
        this.allRules = JSON.parse(json);
        this.allRules.sort((a, b) => a.Id < b.Id ? -1 : 1);
        this.rulesLoaded.emit(null);
    }

    private loadedAllFAQs(json: string) {
        this.allFAQs = JSON.parse(json);
        this.allFAQs.sort((a, b) => a.Id < b.Id ? -1 : 1);
        this.faqsLoaded.emit(null);
    }

    private loadedAllSettings(json: string) {
        this.allSettings = JSON.parse(json);
        this.settingsLoaded.emit(null);
    }

    private loadedMyStats(json: string) {
        this.myStats = JSON.parse(json);
        this.myStats.Logs.sort((a, b) => a.Type < b.Type ? -1 : 1);
        this.myStatsLoaded.emit(null);
    }

    private loadedApplicationDataForUser(json: string) {
        const data = JSON.parse(json);
        // data.CreateTime -> Application already exists
        if (data.CreateTime) {
            this.myApplicationCreateTime = data.CreateTime;
            this.adminApplyInvitations = data.Invitations;
        // !data.CreateTime -> No application, user can create a new one
        } else {
            this.adminQuestions = data.AdminQuestions;
            this.adminApplyInvitations = [];
            this.myApplicationCreateTime = undefined;
        }
        this.applicationDataLoaded.emit(null);
    }

    private loadedApplicationsForAdmin(json: string) {
        this.applications = JSON.parse(json);
        this.applicationsLoaded.emit(null);
    }

    private languageChanged() {
        this.allFAQs = [];
        if (this.allSettings)
            this.allSettings.Language = this.settings.LangValue;
    }


    private myStatsLoadingCooldownEnded() {
        this.myStatsLoadCooldown = undefined;
    }

    private escapeSpecialChars(json: string) {
        return json.replace(/\n/g, "\\n")
            .replace(/\r/g, "\\r")
            .replace(/\t/g, "\\t")
            .replace(/\f/g, "\\f");
    }
}
