import { Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from './services/settings.service';
import { RageConnectorService } from './services/rage-connector.service';
import { DFromClientEvent } from './enums/dfromclientevent.enum';

@Component({
  selector: 'app-root',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showMapCreator = false;
  showFreeroam = false;
  showLobbyChoice = false;
  showTeamChoice = false;
  showUserpanel = true;

  constructor(public settings: SettingsService, rageConnector: RageConnectorService, changeDetector: ChangeDetectorRef) {
    rageConnector.listen(DFromClientEvent.ToggleMapCreator, (bool: boolean) => {
      this.showMapCreator = bool;
      changeDetector.detectChanges();
    });

    rageConnector.listen(DFromClientEvent.ToggleFreeroam, (bool: boolean) => {
      this.showFreeroam = bool;
      changeDetector.detectChanges();
    });

    rageConnector.listen(DFromClientEvent.ToggleLobbyChoice, (bool: boolean) => {
      this.showLobbyChoice = bool;
      if (bool)
        this.showTeamChoice = false;
      changeDetector.detectChanges();
    });

    rageConnector.listen(DFromClientEvent.ToggleTeamChoiceMenu, (bool: boolean) => {
      this.showTeamChoice = bool;
      changeDetector.detectChanges();
    });

    rageConnector.listen(DFromClientEvent.ToggleUserpanel, (bool: boolean) => {
      this.showUserpanel = bool;
      changeDetector.detectChanges();
    });

    this.settings.InFightLobbyChanged.on(null, () => changeDetector.detectChanges());
  }
}
