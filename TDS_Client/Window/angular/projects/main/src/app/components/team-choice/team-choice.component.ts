import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';
import { TeamChoiceMenuTeamData } from './models/teamChoiceMenuTeamData';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';

@Component({
  selector: 'app-team-choice',
  templateUrl: './team-choice.component.html',
  styleUrls: ['./team-choice.component.scss']
})
export class TeamChoiceComponent implements OnInit {

  data: TeamChoiceMenuTeamData[];
  isRandomTeams: boolean;

  constructor(private rageConnector: RageConnectorService,
    private sanitizer: DomSanitizer,
    private changeDetector: ChangeDetectorRef) {

    this.rageConnector.listen(DFromClientEvent.SyncTeamChoiceMenuData, (teamsJson: string, isRandomTeams: boolean) => {
      this.isRandomTeams = isRandomTeams;
      this.data = JSON.parse(teamsJson);
      this.data.forEach(data => data.PlayerNames.sort());
      this.changeDetector.detectChanges();
    });

    this.rageConnector.listen(DFromClientEvent.SetPlayerNameToTeamChoiceData, (name: string, newTeam: string, oldTeam: string) => {
      if (oldTeam) {
        this.removeFromOldTeam(name, oldTeam);
      }

      if (newTeam) {
        this.addToNewTeam(name, newTeam);
      }

      this.changeDetector.detectChanges();
    });
  }

  ngOnInit() {

  }

  chooseTeamIndex(index: number) {
    this.rageConnector.call(DToServerEvent.ChooseTeam, index);
  }

  getColor(team: TeamChoiceMenuTeamData): SafeStyle {
    // return this.sanitizer.bypassSecurityTrustStyle("rgba(255, 255, 255, 0.95)");
    return this.sanitizer.bypassSecurityTrustStyle(`rgba(${team.Red}, ${team.Green}, ${team.Blue}, 0.95)`);
  }

  private addToNewTeam(name: string, newTeam: string) {
    const team = this.data.find(d => d.Name == newTeam);
    if (team) {
      team.PlayerNames.push(name);
      team.PlayerNames.sort();
    }
  }

  private removeFromOldTeam(name: string, oldTeam: string) {
    const team = this.data.find(d => d.Name == oldTeam);
    if (team) {
      const index = team.PlayerNames.indexOf(name);
      if (index >= 0) {
        team.PlayerNames.splice(index, 1);
      }
    }
  }
}
