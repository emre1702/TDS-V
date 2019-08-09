import { Injectable } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { RageConnectorService } from '../../../services/rage-connector.service';
import { OrderByPipe } from '../../../pipes/orderby.pipe';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { EventEmitter } from 'events';
import { SettingsService } from '../../../services/settings.service';

@Injectable({
  providedIn: 'root'
})
export class UserpanelService {
    allCommands: UserpanelCommandDataDto[] = [];

    public commandsLoaded = new EventEmitter();

    constructor(private rageConnector: RageConnectorService, private settings: SettingsService) {}

    loadCommands() {
        this.rageConnector.callCallback(DToServerEvent.LoadAllCommands, undefined, this.loadAllCommands.bind(this));
    }

    private loadAllCommands(json: string) {
        this.allCommands = JSON.parse(json);
        this.allCommands.sort((a, b) => a.Command < b.Command ? -1 : 1);
        this.allCommands.forEach(c => c.Aliases.sort());
        this.commandsLoaded.emit(null);
    }
}
