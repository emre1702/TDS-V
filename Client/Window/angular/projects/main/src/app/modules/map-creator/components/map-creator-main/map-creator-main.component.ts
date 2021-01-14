import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { Constants } from 'projects/main/src/app/constants';
import { AreYouSureDialog } from 'projects/main/src/app/dialog/are-you-sure-dialog';
import { MapType } from 'projects/main/src/app/enums/maptype.enum';
import { InitialDatas } from 'projects/main/src/app/initial-datas';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SelectGroupDialogData } from '../../../shared/dialogs/select-group-dialog/models/select-group-dialog-data';
import { SelectGroupDialogComponent } from '../../../shared/dialogs/select-group-dialog/select-group-dialog.component';
import { CustomErrorCheck, ErrorService, FormControlCheck } from '../../../shared/services/error.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { MapCreateDataKey } from '../../enums/map-create-data-key';
import { MapCreateError } from '../../enums/map-create-error';
import { MapCreatorNav } from '../../enums/map-creator-nav';
import { getLocationDataByName, locations } from '../../location-data';
import { convertLocationDataToShared } from '../../map-creator.helper';
import { MapCreatorMainDebugService } from './services/map-creator-main.debug.service';
import { MapCreatorMainProdService } from './services/map-creator-main.prod.service';
import { MapCreatorMainService } from './services/map-creator-main.service';

@Component({
    selector: 'app-map-creator-main',
    templateUrl: './map-creator-main.component.html',
    styleUrls: ['./map-creator-main.component.scss'],
    providers: [{ provide: MapCreatorMainService, useFactory: createService, deps: [RageConnectorService] }],
})
export class MapCreatorMainComponent implements OnInit {
    @Input() formGroup: FormGroup;
    @Output() navChanged = new EventEmitter<MapCreatorNav>();
    @Output() newMap = new EventEmitter();
    @Output() loadMap = new EventEmitter<number>();

    sendErrorService: ErrorService;
    saveErrorService: ErrorService;
    teamSpawnsErrorService: ErrorService;
    mapLimitErrorService: ErrorService;
    targetErrorService: ErrorService;
    bombPlacesErrorService: ErrorService;
    mapSettingsErrorService: ErrorService;
    currentLocation: string;
    activeLocationGroups: string[] = [];

    mapType = MapType;
    mapCreateDataKey = MapCreateDataKey;
    mapCreatorNav = MapCreatorNav;
    minNameLength = Constants.MIN_MAP_CREATE_NAME_LENGTH;
    maxNameLength = Constants.MAX_MAP_CREATE_NAME_LENGTH;
    locationGroups = locations;

    constructor(
        public settings: SettingsService,
        private service: MapCreatorMainService,
        private notificationService: NotificationService,
        private dialog: MatDialog
    ) {}

    ngOnInit() {
        this.addValidationsForSend();
        this.addValidationsForSave();
    }

    getMapTypes(): string[] {
        const keys = Object.keys(MapType);
        return keys.slice(keys.length / 2);
    }

    sendData() {
        this.service.sendData(this.formGroup).subscribe((result) => this.sendOrSaveDataCallback(result));
    }

    saveData() {
        this.service.sendData(this.formGroup).subscribe((result) => this.sendOrSaveDataCallback(result));
    }

    selectLocation(event: MatSelectChange) {
        if (!event.value) {
            this.unselectLocation();
            return;
        }
        const locationData = getLocationDataByName(event.value);
        if (!locationData) return;
        this.currentLocation = event.value;
        this.formGroup.controls[MapCreateDataKey.Location].setValue(convertLocationDataToShared(locationData));

        this.service.changeLocation(locationData);
    }

    private unselectLocation() {
        this.currentLocation = undefined;
        this.formGroup.controls[MapCreateDataKey.Location].patchValue(undefined);
        this.service.changeLocation(undefined);
    }

    toggleActiveLocationGroup(groupName: string) {
        const index = this.activeLocationGroups.indexOf(groupName);
        if (index >= 0) this.activeLocationGroups.splice(index, 1);
        else this.activeLocationGroups.push(groupName);
    }

    private sendOrSaveDataCallback(result: MapCreateError) {
        const msg = MapCreateError[result];
        if (result == MapCreateError.MapCreatedSuccessfully) {
            this.notificationService.showSuccess(msg);
        } else {
            this.notificationService.showError(msg);
        }
    }

    removeTheMap() {
        this.dialog
            .open(AreYouSureDialog, { panelClass: 'mat-app-background' })
            .afterClosed()
            .subscribe((bool: boolean) => {
                if (!bool) return;
                this.service.removeMap(this.formGroup);
                this.newMap.emit();
            });
    }

    startNew() {
        this.dialog
            .open(AreYouSureDialog, { panelClass: 'mat-app-background' })
            .afterClosed()
            .subscribe((bool: boolean) => {
                if (!bool) return;
                this.newMap.emit();
            });
    }

    showPossibleMaps() {
        this.service.loadPossibleMapNames().subscribe((mapNames) => {
            const dialogData: SelectGroupDialogData = { title: 'ChooseAMapFromList', selectLabel: 'Map', groups: mapNames };
            const dialogRef = this.dialog.open(SelectGroupDialogComponent, { data: dialogData, panelClass: 'mat-app-background' });

            dialogRef.afterClosed().subscribe((loadMapId?: number) => {
                if (loadMapId === undefined) return;
                this.loadMap.emit(loadMapId);
            });
        });
    }

    private addValidationsForSend() {
        this.sendErrorService = new ErrorService(this.settings);

        const notLobbyOwnerCheck = new CustomErrorCheck('NotLobbyOwnerCheck', () => this.settings.IsLobbyOwner, 'ErrorNotLobbyOwner');
        this.sendErrorService.add(notLobbyOwnerCheck);

        this.sendErrorService.add(new FormControlCheck('MapLimitCheck', this.formGroup.controls[MapCreateDataKey.MapEdges]));
        this.sendErrorService.add(new FormControlCheck('TeamSpawnsCheck', this.formGroup.controls[MapCreateDataKey.TeamSpawns]));
        this.sendErrorService.add(new FormControlCheck('BombPlacesCheck', this.formGroup.controls[MapCreateDataKey.BombPlaces]));
        this.sendErrorService.add(new FormControlCheck('TargetCheck', this.formGroup.controls[MapCreateDataKey.Target]));
        this.sendErrorService.add(new FormControlCheck('NameCheck', this.formGroup.controls[MapCreateDataKey.Name]));
        this.sendErrorService.add(new FormControlCheck('MinPlayersCheck', (this.formGroup.controls[MapCreateDataKey.Settings] as FormGroup).controls[0]));
        this.sendErrorService.add(new FormControlCheck('MaxPlayersCheck', (this.formGroup.controls[MapCreateDataKey.Settings] as FormGroup).controls[1]));

        this.teamSpawnsErrorService = new ErrorService(this.settings);
        this.teamSpawnsErrorService.add(new FormControlCheck('TeamSpawnsCheck', this.formGroup.controls[MapCreateDataKey.TeamSpawns]));

        this.mapLimitErrorService = new ErrorService(this.settings);
        this.mapLimitErrorService.add(new FormControlCheck('MapLimitCheck', this.formGroup.controls[MapCreateDataKey.MapEdges]));

        this.targetErrorService = new ErrorService(this.settings);
        this.targetErrorService.add(new FormControlCheck('TargetCheck', this.formGroup.controls[MapCreateDataKey.Target]));

        this.bombPlacesErrorService = new ErrorService(this.settings);
        this.bombPlacesErrorService.add(new FormControlCheck('BombPlacesCheck', this.formGroup.controls[MapCreateDataKey.BombPlaces]));

        this.mapSettingsErrorService = new ErrorService(this.settings);
        this.mapSettingsErrorService.add(
            new FormControlCheck('MinPlayersCheck', (this.formGroup.controls[MapCreateDataKey.Settings] as FormGroup).controls[0])
        );
        this.mapSettingsErrorService.add(
            new FormControlCheck('MaxPlayersCheck', (this.formGroup.controls[MapCreateDataKey.Settings] as FormGroup).controls[1])
        );
    }

    private addValidationsForSave() {
        this.saveErrorService = new ErrorService(this.settings);

        const notLobbyOwnerCheck = new CustomErrorCheck('NotLobbyOwnerCheck', () => this.settings.IsLobbyOwner, 'ErrorNotLobbyOwner');
        this.sendErrorService.add(notLobbyOwnerCheck);

        this.sendErrorService.add(new FormControlCheck('NameCheck', this.formGroup.controls[MapCreateDataKey.Name]));
    }
}

export function createService(rageConnectorService: RageConnectorService) {
    if (InitialDatas.inDebug) {
        return new MapCreatorMainDebugService();
    } else {
        return new MapCreatorMainProdService(rageConnectorService);
    }
}
