import { Injectable } from '@angular/core';
import { MapVoteDto } from '../models/mapVoteDto';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { EventEmitter } from 'events';
import { OrderByPipe } from '../../../pipes/orderby.pipe';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { DFromServerEvent } from '../../../enums/dfromserverevent.enum';

@Injectable({
  providedIn: 'root'
})
export class MapVotingService {
  public mapsInVoting: MapVoteDto[] = [];
  public votedForMapId: number;

  public mapsInVotingChanged = new EventEmitter();

  public voteForMapId(id: number) {
    this.votedForMapId = id;
    this.rageConnector.callServer(DToServerEvent.MapVote, id);
  }

  private addMapToVoting(mapVoteJson: string) {
    this.mapsInVoting.push(JSON.parse(mapVoteJson));
    this.mapsInVoting = this.orderByPipe.transform(this.mapsInVoting, ["-2", "1"]);
    this.mapsInVotingChanged.emit(null);
  }

  private loadMapVoting(mapVoteJson: string) {
    this.mapsInVoting = this.orderByPipe.transform(JSON.parse(mapVoteJson), ["-2", "1"]);
    this.mapsInVotingChanged.emit(null);
  }

  private resetMapVoting() {
    this.mapsInVoting = [];
    this.votedForMapId = undefined;
    this.mapsInVotingChanged.emit(null);
  }

  private setMapVotes(mapId: number, amountVotes: number) {
    const index = this.mapsInVoting.findIndex(m => m[0] == mapId);
    if (index < 0)
      return;
    if (amountVotes <= 0) {
      this.mapsInVoting.splice(index, 1);
    } else {
      this.mapsInVoting[index][2] = amountVotes;
    }
    this.mapsInVoting = this.orderByPipe.transform(this.mapsInVoting, ["-2", "1"]);
    this.mapsInVotingChanged.emit(null);
  }


  constructor(private rageConnector: RageConnectorService, private orderByPipe: OrderByPipe) {
    console.log("Map voting listener started.");
    rageConnector.listen(DFromServerEvent.LoadMapVoting, this.loadMapVoting.bind(this));
    rageConnector.listen(DFromClientEvent.ResetMapVoting, this.resetMapVoting.bind(this));
    rageConnector.listen(DFromServerEvent.StopMapVoting, this.resetMapVoting.bind(this));
    rageConnector.listen(DFromServerEvent.AddMapToVoting, this.addMapToVoting.bind(this));
    rageConnector.listen(DFromServerEvent.SetMapVotes, this.setMapVotes.bind(this));
  }
}