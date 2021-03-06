import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { Vehicle } from './enums/vehicle.enum';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from '../../enums/to-client-event.enum';
import { MatButton } from '@angular/material/button';
import { ToServerEvent } from '../../enums/to-server-event.enum';

@Component({
    selector: 'app-freeroam',
    templateUrl: './freeroam.component.html',
    styleUrls: ['./freeroam.component.scss'],
})
export class FreeroamComponent implements OnInit, OnDestroy {
    currentTitle = 'FreeroamMenu';

    teleportToPos = { X: 0, Y: 0, Z: 0 };

    VehicleEnum = Vehicle;

    constructor(public settings: SettingsService, private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }

    tpToPos(btn: MatButton) {
        btn._elementRef.nativeElement.blur();
        this.rageConnector.call(ToClientEvent.TeleportToPositionRotation, this.teleportToPos.X, this.teleportToPos.Y, this.teleportToPos.Z, 0);
    }

    getVehicle(veh: Vehicle, btn: MatButton) {
        btn._elementRef.nativeElement.blur();
        this.rageConnector.callServer(ToServerEvent.GetVehicle, veh);
    }
}
