import { Component, OnInit } from '@angular/core';
import { SettingsService } from 'src/app/services/settings.service';
import { Vehicle } from './enums/vehicle.enum';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { DToClientEvent } from 'src/app/enums/dtoclientevent.enum';

@Component({
  selector: 'app-freeroam',
  templateUrl: './freeroam.component.html',
  styleUrls: ['./freeroam.component.scss']
})
export class FreeroamComponent {

  currentTitle = "FreeroamMenu";

  teleportToPos = { X: 0, Y: 0, Z: 0 };

  VehicleEnum = Vehicle;

  constructor(public settings: SettingsService, private rageConnector: RageConnectorService) { }

  tpToPos() {
    this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, this.teleportToPos.X, this.teleportToPos.Y, this.teleportToPos.Z);
  }

  getVehicle(veh: Vehicle) {
    this.rageConnector.call(DToClientEvent.GetVehicle, veh);
  }

}
