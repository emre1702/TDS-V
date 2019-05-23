import { Injectable } from '@angular/core';
import { MapVoteDto } from '../models/mapVoteDto';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { DFromClientEvent } from 'src/app/enums/dfromclientevent.enum';
import { DToClientEvent } from 'src/app/enums/dtoclientevent.enum';

@Injectable({
  providedIn: 'root'
})
export class MapVotingService {
  public mapsInVoting: MapVoteDto[] = [];
  public votedForMapId: number;

  public voteForMapId(id: number) {
    if (this.voteForMapId) {
      this.rageConnector.call(DToClientEvent.RemoveMapVote, this.voteForMapId);
    }
    this.votedForMapId = id;
    this.rageConnector.call(DToClientEvent.AddMapVote, id);

    // this.addVoteToMap(id); // debug
  }

  private addMapToVoting(mapVoteJson: string) {
    this.mapsInVoting.push(JSON.parse(mapVoteJson));
  }

  private addVoteToMap(id: number) {
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

  constructor(private rageConnector: RageConnectorService) {
    rageConnector.listen(DFromClientEvent.LoadMapVoting, this.loadMapVoting.bind(this));
    rageConnector.listen(DFromClientEvent.ResetMapVoting, this.resetMapVoting.bind(this));
    rageConnector.listen(DFromClientEvent.AddVoteToMap, this.addVoteToMap.bind(this));
    rageConnector.listen(DFromClientEvent.AddMapToVoting, this.addMapToVoting.bind(this));
  }
}
