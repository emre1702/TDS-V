import { Injectable, EventEmitter } from '@angular/core';
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { RageConnectorService } from '../../../services/rage-connector.service';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { OrderByPipe } from '../../../pipes/orderby.pipe';

@Injectable({
  providedIn: 'root'
})
export class UserpanelService {
    allCommands: UserpanelCommandDataDto[] = [];

    public commandsLoaded = new EventEmitter();

    constructor(private rageConnector: RageConnectorService, private orderByPipe: OrderByPipe) {
        console.log("Userpanel listener started.");
        rageConnector.listen(DFromClientEvent.LoadAllCommands, this.loadAllCommands.bind(this));
    }

    private loadAllCommands(json: string) {
        this.allCommands = JSON.parse(json);
        this.commandsLoaded.emit();
    }
}
