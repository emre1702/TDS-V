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
  title = 'TDS-V Angular';

  showMapCreator = false;
  showFreeroam = false;

  constructor(public settings: SettingsService, rageConnector: RageConnectorService, changeDetector: ChangeDetectorRef) {
    rageConnector.listen(DFromClientEvent.ToggleMapCreator, (bool: boolean) => {
      this.showMapCreator = bool;
      changeDetector.detectChanges();
    });

    rageConnector.listen(DFromClientEvent.ToggleFreeroam, (bool: boolean) => {
      this.showFreeroam = bool;
      changeDetector.detectChanges();
    });

    this.settings.InFightLobbyChanged.on(null, () => changeDetector.detectChanges());
  }
}
