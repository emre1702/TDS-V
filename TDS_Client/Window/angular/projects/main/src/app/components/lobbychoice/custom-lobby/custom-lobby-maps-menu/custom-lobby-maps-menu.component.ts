import { Component, OnInit, ChangeDetectorRef, EventEmitter, Input, Output, ViewChild, AfterViewInit } from '@angular/core';
import { SettingsService } from '../../../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../../enums/dtoserverevent.enum';
import { MatTableDataSource, MatSort, MatTab } from '@angular/material';
import { MapDataDto } from '../../../mapvoting/models/mapDataDto';
import { MapType } from 'projects/main/src/app/enums/maptype.enum';
import { DefaultMapIds } from '../../enums/default-map-ids.enum';

@Component({
    selector: 'app-custom-lobby-maps-menu',
    templateUrl: './custom-lobby-maps-menu.component.html',
    styleUrls: ['./custom-lobby-maps-menu.component.scss']
})
export class CustomLobbyMapsMenuComponent implements OnInit {

    displayedColumns = ["Select", "Id", "Name", "Type", "CreatorName", "Rating"];
    private mapDataDtoProperties = ["Id", "Name", "Type", "Description", "CreatorName", "Rating"];
    private mapTypeByDefaultMapId = {
        [DefaultMapIds.AllWithoutGangwars]: MapType.Normal,
        [DefaultMapIds.Normals]: MapType.Normal,
        [DefaultMapIds.Bombs]: MapType.Bomb,
        [DefaultMapIds.Snipers]: MapType.Sniper,
        [DefaultMapIds.Gangwars]: MapType.Gangwar
    };
    private defaultMapIdByMapType = {
        [MapType.Normal]: DefaultMapIds.Normals,
        [MapType.Bomb]: DefaultMapIds.Bombs,
        [MapType.Sniper]: DefaultMapIds.Snipers,
        [MapType.Gangwar]: DefaultMapIds.Gangwars
    };
    private _selectedMapsInput: number[] = [];

    @Input()
    set selectedMapsInput(arg: number[]) {
        this._selectedMapsInput = arg;
        this.selectedMaps = this.settings.AllMapsForCustomLobby
                                    .filter(m => this._selectedMapsInput.indexOf(m[0]) >= 0);
        if (this.selectedMapsDataSource) {
            this.selectedMapsDataSource.data = this.selectedMaps;
        }
        this.selectedDefaultMapIds = this.selectedMaps.filter(m => m[0] < 0).map(m => m[0]);
        this.changeDetector.detectChanges();
    }
    @Input() creating: boolean;
    @Output() backClicked = new EventEmitter();

    selectedMaps: MapDataDto[] = [];
    selectedDefaultMapIds: number[] = [];

    mapType = MapType;
    defaultMapIds = DefaultMapIds;

    availableMapsDataSource: MatTableDataSource<MapDataDto>;
    selectedMapsDataSource: MatTableDataSource<MapDataDto>;
    @ViewChild("availableMapsSort", { static: true }) availableMapsSort: MatSort;
    @ViewChild("selectedMapsSort", { static: true }) selectedMapsSort: MatSort;

    constructor(
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef
    ) { }

    ngOnInit() {
        this.createDataSource();
        if (!this.settings.AllMapsForCustomLobby.length) {
            this.rageConnector.callCallbackServer(DToServerEvent.LoadAllMapsForCustomLobby, [], (mapsJson: string) => {
                this.settings.AllMapsForCustomLobby = JSON.parse(mapsJson);
                this.addDefaultMaps();

                this.availableMapsDataSource.data = this.settings.AllMapsForCustomLobby;

                this.selectedMaps = this.settings.AllMapsForCustomLobby
                                    .filter(m => this._selectedMapsInput.indexOf(m[0]) >= 0);
                this.selectedMapsDataSource.data = this.selectedMaps;
                this.selectedDefaultMapIds = this.selectedMaps.filter(m => m[0] < 0).map(m => m[0]);

                this.changeDetector.detectChanges();
            });
        }
    }

    setSelected(map: MapDataDto) {
        this.selectedMaps.unshift(map);

        if (map[0] < 0) {
            if (map[0] == DefaultMapIds.AllWithoutGangwars) {
                this.selectedMaps = this.selectedMaps.filter(m => m[2] == MapType.Gangwar || m == map);
            } else {
                const mapType = this.mapTypeByDefaultMapId[map[0]];
                this.selectedMaps = this.selectedMaps.filter(m => m[2] != mapType || m == map);
            }
        }

        this.selectedMapsDataSource.data = this.selectedMaps;
        this.selectedDefaultMapIds = this.selectedMaps.filter(m => m[0] < 0).map(m => m[0]);

        this.availableMapsDataSource.data = this.settings.AllMapsForCustomLobby
                .filter(m => this.selectedMaps.indexOf(m) < 0);
        this.changeDetector.detectChanges();
    }

    setUnselected(map: MapDataDto) {
        const index = this.selectedMaps.indexOf(map);
        this.selectedMaps.splice(index, 1);
        this.selectedMapsDataSource.data = this.selectedMaps;
        this.selectedDefaultMapIds = this.selectedMaps.filter(m => m[0] < 0).map(m => m[0]);

        this.availableMapsDataSource.data = this.settings.AllMapsForCustomLobby
                .filter(m => this.selectedMaps.indexOf(m) < 0);
        this.changeDetector.detectChanges();
    }

    canSelectMap(map: MapDataDto): boolean {
        if (!this.creating) {
            return false;
        }

        if (this.selectedDefaultMapIds.indexOf(DefaultMapIds.AllWithoutGangwars) >= 0) {
            return map[0] == DefaultMapIds.Gangwars;
        }

        if (map[0] < 0) {
            return true;
        }

        const equivalentDefaultMapId = this.defaultMapIdByMapType[map[2]];
        return this.selectedDefaultMapIds.indexOf(equivalentDefaultMapId) < 0;
    }

    backButtonClicked() {
        this.backClicked.emit(this.selectedMaps.filter(m => m[0]));
    }

    private createDataSource() {
        this.availableMapsDataSource = new MatTableDataSource(this.settings.AllMapsForCustomLobby);
        this.availableMapsDataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
        this.availableMapsDataSource.sort = this.availableMapsSort;

        this.selectedMapsDataSource = new MatTableDataSource(this.selectedMaps);
        this.selectedMapsDataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
        this.selectedMapsDataSource.sort = this.selectedMapsSort;

        this.changeDetector.detectChanges();
    }

    private addDefaultMaps() {
        this.settings.AllMapsForCustomLobby.unshift([
            DefaultMapIds.AllWithoutGangwars, "DefaultMapIdsAllWithoutGangwars", MapType.Normal,
            { [7]: "Alle Karten, die nicht Gangwar-Karten sind.", [9]: "All maps not being gangwar maps." },
            "System", 5
        ]);

        this.settings.AllMapsForCustomLobby.unshift([
            DefaultMapIds.Normals, "DefaultMapIdsNormals", MapType.Normal,
            { [7]: "Alle normalen Karten.", [9]: "All normal maps." },
            "System", 5
        ]);

        this.settings.AllMapsForCustomLobby.unshift([
            DefaultMapIds.Bombs, "DefaultMapIdsBombs", MapType.Bomb,
            { [7]: "Alle Bomben Karten.", [9]: "All bomb maps." },
            "System", 5
        ]);

        this.settings.AllMapsForCustomLobby.unshift([
            DefaultMapIds.Snipers, "DefaultMapIdsSnipers", MapType.Sniper,
            { [7]: "Alle Sniper Karten.", [9]: "All sniper maps." },
            "System", 5
        ]);

        this.settings.AllMapsForCustomLobby.unshift([
            DefaultMapIds.Gangwars, "DefaultMapIdsGangwars", MapType.Gangwar,
            { [7]: "Alle Gangwar Karten.", [9]: "All gangwar maps." },
            "System", 5
        ]);
    }

    private sortingDataAccessor(obj: MapDataDto, property: string) {
        const index = this.mapDataDtoProperties.indexOf(property);
        return obj[index];
    }
}
