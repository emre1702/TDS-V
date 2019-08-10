import { Injectable } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { RageConnectorService } from '../../../services/rage-connector.service';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { EventEmitter } from 'events';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelRuleDataDto } from '../interfaces/userpanelRuleDataDto';
import { UserpanelRulesCategory } from '../enums/userpanel-rules-category.enum';
import { UserpanelRulesTarget } from '../enums/userpanel-rules-target.enum';

@Injectable({
  providedIn: 'root'
})
export class UserpanelService {
    allCommands: UserpanelCommandDataDto[] = [];
    allRules: UserpanelRuleDataDto[] = [];

    public commandsLoaded = new EventEmitter();
    public rulesLoaded = new EventEmitter();

    constructor(private rageConnector: RageConnectorService) {}

    loadCommands() {
        this.rageConnector.callCallback(DToServerEvent.LoadAllCommands, undefined, this.loadAllCommands.bind(this));
    }

    loadRules() {
        this.rageConnector.callCallback(DToServerEvent.LoadAllRules, undefined, this.loadAllRules.bind(this));
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
}
