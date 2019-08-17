import { Injectable } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { RageConnectorService } from '../../../services/rage-connector.service';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { EventEmitter } from 'events';
import { UserpanelRuleDataDto } from '../interfaces/userpanelRuleDataDto';
import { UserpanelLoadDataType } from '../enums/userpanel-load-data-type.enum';
import { UserpanelFAQDataDto } from '../interfaces/userpanelFAQDataDto';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelSettingDataDto } from '../interfaces/userpanelSettingDataDto';

@Injectable({
  providedIn: 'root'
})
export class UserpanelService {
    allCommands: UserpanelCommandDataDto[] = [];
    allRules: UserpanelRuleDataDto[] = [];
    allFAQs: UserpanelFAQDataDto[] = [];
    allSettings: UserpanelSettingDataDto;

    public commandsLoaded = new EventEmitter();
    public rulesLoaded = new EventEmitter();
    public faqsLoaded = new EventEmitter();
    public settingsLoaded = new EventEmitter();

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

    private loadUserpanelData(type: UserpanelLoadDataType, json: string) {
        json = this.escapeSpecialChars(json);
        switch (type) {
            case UserpanelLoadDataType.Commands:
                this.loadAllCommands(json);
                break;
            case UserpanelLoadDataType.Rules:
                this.loadAllRules(json);
                break;
            case UserpanelLoadDataType.FAQs:
                this.loadAllFAQs(json);
                break;
            case UserpanelLoadDataType.Settings:
                this.loadAllSettings(json);
                break;
        }
    }

    private loadAllCommands(json: string) {
        this.allCommands = JSON.parse(json);
        this.allCommands.sort((a, b) => a.Command < b.Command ? -1 : 1);
        this.allCommands.forEach(c => c.Aliases.sort());
        this.commandsLoaded.emit(null);
    }

    private loadAllRules(json: string) {
        this.allRules = JSON.parse(json);
        this.allRules.sort((a, b) => a.Id < b.Id ? -1 : 1);
        this.rulesLoaded.emit(null);
    }

    private loadAllFAQs(json: string) {
        this.allFAQs = JSON.parse(json);
        this.allFAQs.sort((a, b) => a.Id < b.Id ? -1 : 1);
        this.faqsLoaded.emit(null);
    }

    private loadAllSettings(json: string) {
        this.allSettings = JSON.parse(json);
        this.settingsLoaded.emit(null);
    }

    private languageChanged() {
        this.allFAQs = [];
        this.allSettings.Language = this.settings.LangValue;
    }

    private escapeSpecialChars(json: string) {
        return json.replace(/\n/g, "\\n")
            .replace(/\r/g, "\\r")
            .replace(/\t/g, "\\t")
            .replace(/\f/g, "\\f");
    }
}
