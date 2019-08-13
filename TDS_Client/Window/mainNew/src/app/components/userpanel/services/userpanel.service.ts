import { Injectable } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { RageConnectorService } from '../../../services/rage-connector.service';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { EventEmitter } from 'events';
import { UserpanelRuleDataDto } from '../interfaces/userpanelRuleDataDto';
import { UserpanelLoadDataType } from '../enums/userpanel-load-data-type.enum';

@Injectable({
  providedIn: 'root'
})
export class UserpanelService {
    allCommands: UserpanelCommandDataDto[] = [];
    allRules: UserpanelRuleDataDto[] = [];

    public commandsLoaded = new EventEmitter();
    public rulesLoaded = new EventEmitter();

    constructor(private rageConnector: RageConnectorService) {
        this.rageConnector.listen(DToServerEvent.LoadUserpanelData, this.loadUserpanelData.bind(this));
    }

    loadCommands() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.Commands);
    }

    loadRules() {
        this.rageConnector.call(DToServerEvent.LoadUserpanelData, UserpanelLoadDataType.Rules);
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

    private escapeSpecialChars(json: string) {
        return json.replace(/\n/g, "\\n")
            .replace(/\r/g, "\\r")
            .replace(/\t/g, "\\t")
            .replace(/\f/g, "\\f");
    }
}
