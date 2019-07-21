import { Component, OnInit } from '@angular/core';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { DFromClientEvent } from 'src/app/enums/dfromclientevent.enum';

@Component({
  selector: 'app-team-choice',
  templateUrl: './team-choice.component.html',
  styleUrls: ['./team-choice.component.scss']
})
export class TeamChoiceComponent implements OnInit {

  constructor(private rageConnector: RageConnectorService) {
    this.rageConnector.listen(DFromClientEvent.ShowTeamChoice, (teamsJson: string) => {

    });
  }

  ngOnInit() {

  }


}
