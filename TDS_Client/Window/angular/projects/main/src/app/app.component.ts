import { Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from './services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from './enums/dfromclientevent.enum';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-root',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showMapCreator = false;
  showFreeroam = false;
  showLobbyChoice = true;
  showTeamChoice = false;
  showUserpanel = false;

  constructor(
    public settings: SettingsService,
    rageConnector: RageConnectorService,
    changeDetector: ChangeDetectorRef,
    snackBar: MatSnackBar) {

    rageConnector.listen(DFromClientEvent.InitLoadAngular, (adminLevel: number) => {
      this.settings.loadAdminLevel(adminLevel);
    });

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

    rageConnector.listen(DFromClientEvent.ShowCooldown, () => {
      snackBar.open("Cooldown", undefined, { duration: 2000 });
    });

    this.settings.InFightLobbyChanged.on(null, () => changeDetector.detectChanges());
  }
}