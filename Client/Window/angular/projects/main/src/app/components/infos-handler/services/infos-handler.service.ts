import { Injectable } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { InfoType } from '../enums/info-type.enum';
import { FromClientEvent } from '../../../enums/from-client-event.enum';
import { EventEmitter } from 'events';
import { MapVoteDto } from '../../mapvoting/models/mapVoteDto';

@Injectable({
    providedIn: 'root',
})
export class InfosHandlerService {
    showCursorInfo: boolean;
    showLobbyLeaveInfo: boolean;
    mapVotingInfos: MapVoteDto[] = [];
    votedMapInfo: MapVoteDto;

    infosChanged = new EventEmitter();

    constructor(private rageConnector: RageConnectorService) {
        this.rageConnector.listen(FromClientEvent.ToggleInfo, this.toggleInfo.bind(this));
    }

    addMapVotingInfo(info: MapVoteDto) {
        this.mapVotingInfos.push(info);
        this.infosChanged.emit(null);
    }

    loadMapVotingInfos(infos: MapVoteDto[]) {
        this.mapVotingInfos = infos;
        this.infosChanged.emit(null);
    }

    resetMapVotingInfos() {
        this.mapVotingInfos = [];
        this.votedMapInfo = undefined;
        this.infosChanged.emit(null);
    }

    setVotedMapInfo(info: MapVoteDto) {
        this.votedMapInfo = info;
        this.infosChanged.emit(null);
    }

    private toggleInfo(type: InfoType, toggle: boolean) {
        switch (type) {
            case InfoType.Cursor:
                this.showCursorInfo = toggle;
                break;
            case InfoType.LobbyLeave:
                this.showLobbyLeaveInfo = toggle;
                break;
        }
        this.infosChanged.emit(null);
    }
}
