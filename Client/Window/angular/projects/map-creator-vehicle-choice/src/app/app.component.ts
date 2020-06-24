import { Component, ViewChild, ChangeDetectorRef, AfterViewInit, ChangeDetectionStrategy } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatInput } from '@angular/material';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from './enums/dtoclientevent.enum';
import { MapCreatorPositionType } from './enums/mapcreatorpositiontype.enum';
import { Language } from './language/language.interface';
import { LanguageEnum } from './enums/language.enum';
import { German } from './language/german.language';
import { English } from './language/english.language';
import { DFromClientEvent } from './enums/dfromclientevent.enum';
import { VehicleEnum } from './enums/vehicle.enum';

@Component({
    selector: 'app-root',
    templateUrl: './app.template.html',
    styleUrls: ['./app.style.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent implements AfterViewInit {
    private langByLangValue = {
        [LanguageEnum.German]: new German(),
        [LanguageEnum.English]: new English()
    };

    language: Language = this.langByLangValue[LanguageEnum.English];
    vehiclesDataSource: MatTableDataSource<string>;
    selectedVehicle: string;
    filterDelay: NodeJS.Timer;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(
        private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) {
        this.rageConnector.listen(DFromClientEvent.InitLoadAngular, (language: number) => {
            this.language = this.langByLangValue[language];
        });
    }

    ngAfterViewInit(): void {

        const vehicleKeys = Object.keys(VehicleEnum);

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
        this.rageConnector.call(DToClientEvent.StartMapCreatorPosPlacing, MapCreatorPositionType.Vehicle, vehicle);
    }

    applyFilter(event: KeyboardEvent) {
        if (event.key === "Enter")
            return;
        const filterValue = (event.target as unknown as MatInput).value;
        if (this.filterDelay)
            clearTimeout(this.filterDelay);
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
            this.rageConnector.call(DToClientEvent.MapCreatorShowVehicle, row);
        }

        this.changeDetector.detectChanges();
    }
}
