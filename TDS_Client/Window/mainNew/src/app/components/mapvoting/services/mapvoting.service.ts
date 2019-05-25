import { Injectable } from '@angular/core';
import { MapVoteDto } from '../models/mapVoteDto';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { DFromClientEvent } from 'src/app/enums/dfromclientevent.enum';
import { DToClientEvent } from 'src/app/enums/dtoclientevent.enum';
import { EventEmitter } from 'events';

@Injectable({
  providedIn: 'root'
})
export class MapVotingService {
  public mapsInVoting: MapVoteDto[] = [];
  public votedForMapId: number;

  public mapsInVotingChanged = new EventEmitter();

  public voteForMapId(id: number) {
    /*if (this.voteForMapId) {
      this.rageConnector.call(DToClientEvent.RemoveMapVote, this.voteForMapId);
    }*/
    this.votedForMapId = id;
    this.rageConnector.call(DToClientEvent.AddMapVote, id);
  }

  private addMapToVoting(mapVoteJson: string) {
    this.mapsInVoting.push(JSON.parse(mapVoteJson));
    this.mapsInVotingChanged.emit(null);
  }

  private addVoteToMap(id: number, oldId: number) {
    if (oldId >= 0) {
      const oldMapIndex = this.mapsInVoting.findIndex(m => m.Id == oldId);
      if (oldMapIndex >= 0)
        this.mapsInVoting.splice(oldMapIndex, 1);
    }

    // Shouldn't happen
    if (!this.mapsInVoting.some(m => m.Id == id)) {
      const mapVote = new MapVoteDto();
      mapVote.Id = id;
      mapVote.AmountVotes = 0;
      mapVote.Name = "?";
      this.mapsInVoting.push(mapVote);
    }
    this.mapsInVoting.filter(m => m.Id == id)[0].AmountVotes++;
  }

  private loadMapVoting(mapVoteJson: string) {
    this.mapsInVoting = JSON.parse(mapVoteJson);
  }

  private resetMapVoting() {
    this.mapsInVoting = [];
    this.votedForMapId = undefined;
  }

  private removeMapFromVoting(mapId: number) {
    const index = this.mapsInVoting.findIndex(m => m.Id == mapId);
    if (index >= 0)
      this.mapsInVoting.splice(index, 1);
  }

  constructor(private rageConnector: RageConnectorService) {
    console.log("Map voting listener started.");
    rageConnector.listen(DFromClientEvent.LoadMapVoting, this.loadMapVoting.bind(this));
    rageConnector.listen(DFromClientEvent.ResetMapVoting, this.resetMapVoting.bind(this));
    rageConnector.listen(DFromClientEvent.AddVoteToMap, this.addVoteToMap.bind(this));
    rageConnector.listen(DFromClientEvent.AddMapToVoting, this.addMapToVoting.bind(this));
    rageConnector.listen(DFromClientEvent.RemoveMapFromVoting, this.removeMapFromVoting.bind(this));
  }
}
