import { Injectable } from '@angular/core';
import { MapVoteDto } from '../models/mapVoteDto';
import { RageConnectorService } from 'rage-connector';
import { FromClientEvent } from '../../../enums/from-client-event.enum';
import { EventEmitter } from 'events';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { FromServerEvent } from '../../../enums/from-server-event.enum';
import { InfosHandlerService } from '../../infos-handler/services/infos-handler.service';
import { OrderByPipe } from '../../../modules/shared/pipes/orderby.pipe';
import { InitialDatas } from '../../../initial-datas';

@Injectable({
    providedIn: 'root',
})
export class MapVotingService {
    public mapsInVoting: MapVoteDto[] = InitialDatas.getMapsInVoting();
    public votedForMapId: number;

    public mapsInVotingChanged = new EventEmitter();

    private orderByPipe = new OrderByPipe();

    public voteForMapId(id: number) {
        this.votedForMapId = id;
        this.rageConnector.callServer(ToServerEvent.MapVote, id);
        this.infosHandler.setVotedMapInfo(this.mapsInVoting.find((m) => m[0] == this.votedForMapId));
    }

    private addMapToVoting(mapVoteJson: string) {
        const mapVote: MapVoteDto = JSON.parse(mapVoteJson);
        this.mapsInVoting.push(mapVote);
        this.mapsInVoting = this.orderByPipe.transform(this.mapsInVoting, ['-2', '1']);
        this.mapsInVotingChanged.emit(null);
        this.infosHandler.addMapVotingInfo(mapVote);
    }

    private loadMapVoting(mapVoteJson: string) {
        this.mapsInVoting = this.orderByPipe.transform(JSON.parse(mapVoteJson), ['-2', '1']);
        this.mapsInVotingChanged.emit(null);
        this.infosHandler.loadMapVotingInfos([...this.mapsInVoting]);
    }

    private resetMapVoting() {
        this.mapsInVoting = [];
        this.votedForMapId = undefined;
        this.mapsInVotingChanged.emit(null);
        this.infosHandler.resetMapVotingInfos();
    }

    private setMapVotes(mapId: number, amountVotes: number) {
        const index = this.mapsInVoting.findIndex((m) => m[0] == mapId);
        if (index < 0) return;
        if (amountVotes <= 0) {
            this.mapsInVoting.splice(index, 1);
        } else {
            this.mapsInVoting[index][2] = amountVotes;
        }
        this.mapsInVoting = this.orderByPipe.transform(this.mapsInVoting, ['-2', '1']);
        this.infosHandler.loadMapVotingInfos([...this.mapsInVoting]);
        this.mapsInVotingChanged.emit(null);
    }

    constructor(private rageConnector: RageConnectorService, private infosHandler: InfosHandlerService) {
        console.log('Map voting listener started.');
        rageConnector.listen(FromServerEvent.LoadMapVoting, this.loadMapVoting.bind(this));
        rageConnector.listen(FromClientEvent.ResetMapVoting, this.resetMapVoting.bind(this));
        rageConnector.listen(FromServerEvent.StopMapVoting, this.resetMapVoting.bind(this));
        rageConnector.listen(FromServerEvent.AddMapToVoting, this.addMapToVoting.bind(this));
        rageConnector.listen(FromServerEvent.SetMapVotes, this.setMapVotes.bind(this));
    }
}
