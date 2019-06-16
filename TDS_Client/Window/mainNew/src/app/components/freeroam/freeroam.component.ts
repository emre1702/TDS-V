import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from 'src/app/services/settings.service';
import { Vehicle } from './enums/vehicle.enum';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { DToClientEvent } from 'src/app/enums/dtoclientevent.enum';
import { MatButton } from '@angular/material';

@Component({
  selector: 'app-freeroam',
  templateUrl: './freeroam.component.html',
  styleUrls: ['./freeroam.component.scss']
})
export class FreeroamComponent {

  currentTitle = "FreeroamMenu";

  teleportToPos = { X: 0, Y: 0, Z: 0 };

  VehicleEnum = Vehicle;

  constructor(public settings: SettingsService, private rageConnector: RageConnectorService, changeDetector: ChangeDetectorRef) {
    settings.LanguageChanged.on(null, () => changeDetector.detectChanges());
  }

  tpToPos(btn: MatButton) {
    btn._elementRef.nativeElement.blur();
    this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, this.teleportToPos.X, this.teleportToPos.Y, this.teleportToPos.Z, 0);
  }

  getVehicle(veh: Vehicle, btn: MatButton) {
    btn._elementRef.nativeElement.blur();
    this.rageConnector.call(DToClientEvent.GetVehicle, veh);
  }

}
