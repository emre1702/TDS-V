import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';
import { TeamChoiceMenuTeamData } from './models/teamChoiceMenuTeamData';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { SettingsService } from '../../services/settings.service';

@Component({
  selector: 'app-team-choice',
  templateUrl: './team-choice.component.html',
  styleUrls: ['./team-choice.component.scss']
})
export class TeamChoiceComponent implements OnInit, OnDestroy {

  data: TeamChoiceMenuTeamData[];
  isRandomTeams: boolean;

  constructor(private rageConnector: RageConnectorService,
    private sanitizer: DomSanitizer,
    private changeDetector: ChangeDetectorRef,
    public settings: SettingsService) {

    this.rageConnector.listen(DFromClientEvent.SyncTeamChoiceMenuData, (teamsJson: string, isRandomTeams: boolean) => {
      this.isRandomTeams = isRandomTeams;
      this.data = JSON.parse(teamsJson);
      this.changeDetector.detectChanges();
    });
  }

  ngOnInit() {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy() {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
  }

  chooseTeamIndex(index: number) {
    this.rageConnector.call(DToServerEvent.ChooseTeam, index);
  }

  leaveLobby() {
    this.rageConnector.call(DToServerEvent.LeaveLobby);
  }

  getColor(team: TeamChoiceMenuTeamData): SafeStyle {
    // return this.sanitizer.bypassSecurityTrustStyle("rgba(255, 255, 255, 0.95)");
    return this.sanitizer.bypassSecurityTrustStyle(`rgba(${team.Red}, ${team.Green}, ${team.Blue}, 0.95)`);
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }
}
