import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatInput } from '@angular/material/input';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { MapCreatorPositionType } from '../../enums/map-creator-position-type';
import { Vehicle } from '../../enums/vehicle.enum';

@Component({
    selector: 'app-map-creator-vehicle-choice',
    templateUrl: './map-creator-vehicle-choice.component.html',
    styleUrls: ['./map-creator-vehicle-choice.component.scss'],
})
export class MapCreatorVehicleChoiceComponent implements AfterViewInit {
    vehiclesDataSource: MatTableDataSource<string>;
    selectedVehicle: string;
    filterDelay: NodeJS.Timer;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(public settings: SettingsService, private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef) {}

    ngAfterViewInit(): void {
        const vehicleKeys = Object.keys(Vehicle);

        this.vehiclesDataSource = new MatTableDataSource(vehicleKeys.slice(vehicleKeys.length / 2));
        this.vehiclesDataSource.paginator = this.paginator;

        this.vehiclesDataSource.sort = this.sort;
        this.vehiclesDataSource.sortingDataAccessor = (item: string, property: string) => {
            return item;
        };

        this.vehiclesDataSource.filterPredicate = (item: string, filter: string) => {
            return item.indexOf(filter) >= 0;
        };

        this.changeDetector.detectChanges();
    }

    selectVehicle(vehicle: string) {
        this.rageConnector.call(ToClientEvent.StartMapCreatorPosPlacing, MapCreatorPositionType.Vehicle, vehicle);
    }

    applyFilter(event: KeyboardEvent) {
        if (event.key === 'Enter') return;
        const filterValue = ((event.target as unknown) as MatInput).value;
        if (this.filterDelay) clearTimeout(this.filterDelay);
        this.filterDelay = setTimeout(this.applyFilterAfterDelay.bind(this, filterValue), 1000);
    }

    applyFilterAfterDelay(filterValue: string) {
        this.vehiclesDataSource.filter = filterValue.trim().toLowerCase();
        if (this.filterDelay) {
            clearTimeout(this.filterDelay);
            this.filterDelay = undefined;
        }
        this.changeDetector.detectChanges();
    }

    onSelectedVehicleChanged(row: string) {
        if (this.selectedVehicle == row) {
            this.selectedVehicle = undefined;
        } else {
            this.selectedVehicle = row;
            this.rageConnector.call(ToClientEvent.MapCreatorShowVehicle, row);
        }

        this.changeDetector.detectChanges();
    }
}
