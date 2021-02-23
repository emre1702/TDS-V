import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { SettingsService } from '../../services/settings.service';
import { LanguageEnum } from '../../enums/language.enum';
import { Constants } from '../../constants';
import { ToClientEvent } from '../../enums/to-client-event.enum';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MapCreatorPositionType } from './enums/map-creator-position-type';
import { ToServerEvent } from '../../enums/to-server-event.enum';
import { FromClientEvent } from '../../enums/from-client-event.enum';
import { MapCreatorPosition } from './models/map-creator-position';
import { MapCreatorInfoType } from './enums/map-creator-info-type';
import { FromServerEvent } from '../../enums/from-server-event.enum';
import { MatDialog } from '@angular/material/dialog';
import { NotificationService } from '../../modules/shared/services/notification.service';
import { InitialDatas } from '../../initial-datas';
import { MapCreatorNav } from './enums/map-creator-nav';
import { teamSpawnsValidator } from './validators/team-spawns.validator';
import { mapLimitsValidator } from './validators/map-limits.validator';
import { MapCreateDataKey } from './enums/map-create-data-key';
import { MapCreatorService } from './services/map-creator.service';
import { MapCreatorProdService } from './services/map-creator.prod.service';
import { MapCreatorDebugService } from './services/map-creator.debug.service';
import { valueLessThanOrEqualValidator } from './validators/value-less-than-or-equal.validator';
import { valueMoreThanOrEqualValidator } from './validators/value-more-than-or-equal.validator';
import { bombPlaceValidator } from './validators/bomb-place.validator';

@Component({
    selector: 'app-map-creator',
    templateUrl: './map-creator.component.html',
    styleUrls: ['./map-creator.component.scss'],
    providers: [{ provide: MapCreatorService, useFactory: createService, deps: [RageConnectorService] }],
})
export class MapCreatorComponent implements OnInit, OnDestroy {
    formGroup: FormGroup;
    mapCreatorNav = MapCreatorNav;
    currentNav = MapCreatorNav.Main;
    editingTeamNumber = 0;

    possibleMaps: string[];

    displayedColumns: string[] = ['id', 'x', 'y', 'z', 'rot'];
    displayedColumns2D: string[] = ['id', 'x', 'y'];
    displayedColumnsName: string[] = ['id', 'name', 'x', 'y', 'z', 'rotX', 'rotY', 'rotZ'];

    positionDatas = [
        { type: MapCreatorPositionType.MapCenter, info: 'MapCenterInfo' },
        { type: MapCreatorPositionType.Target, info: 'TargetInfo' },
    ];
    positionsArrayDatas = [
        { type: MapCreatorPositionType.BombPlantPlace, columns: this.displayedColumns, canAdd: true },
        { type: MapCreatorPositionType.MapLimit, columns: this.displayedColumns2D, canAdd: true },
        { type: MapCreatorPositionType.Object, columns: this.displayedColumnsName, canAdd: false },
        { type: MapCreatorPositionType.Vehicle, columns: this.displayedColumnsName, canAdd: false },
    ];

    @ViewChild('descriptionTextArea') descriptionTextArea: ElementRef;

    constructor(
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef,
        private notificationService: NotificationService,
        private fb: FormBuilder,
        public service: MapCreatorService
    ) {}

    ngOnInit() {
        this.rageConnector.listen(FromClientEvent.AddPositionToMapCreatorBrowser, this.addPositionToMapCreatorBrowser.bind(this));
        this.rageConnector.listen(FromClientEvent.RemovePositionInMapCreatorBrowser, this.removePositionInMapCreatorBrowser.bind(this));
        this.rageConnector.listen(FromClientEvent.RemoveTeamPositionsInMapCreatorBrowser, this.removeTeamPositionsInMapCreatorBrowser.bind(this));
        this.rageConnector.listen(FromServerEvent.MapCreatorSyncData, this.onSyncData.bind(this));
        this.rageConnector.listen(FromClientEvent.MapCreatorSyncCurrentMapToServer, this.syncCurrentMapToServer.bind(this));
        this.rageConnector.listen(FromClientEvent.MapCreatorSetAddedMapCreatorObjectId, this.setAddedMapCreatorObjectId.bind(this));
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.IsLobbyOwnerChanged.on(null, this.isLobbyOwnerChanged.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));

        this.createFormGroup();
        this.isLobbyOwnerChanged();
        this.service.initMap(this.formGroup);
        this.service.addSyncListeners(this.formGroup);
    }

    ngOnDestroy() {
        this.rageConnector.remove(FromClientEvent.AddPositionToMapCreatorBrowser, this.addPositionToMapCreatorBrowser.bind(this));
        this.rageConnector.remove(FromClientEvent.RemovePositionInMapCreatorBrowser, this.removePositionInMapCreatorBrowser.bind(this));
        this.rageConnector.remove(FromClientEvent.RemoveTeamPositionsInMapCreatorBrowser, this.removeTeamPositionsInMapCreatorBrowser.bind(this));
        this.rageConnector.remove(FromServerEvent.MapCreatorSyncData, this.onSyncData.bind(this));
        this.rageConnector.remove(FromClientEvent.MapCreatorSyncCurrentMapToServer, this.syncCurrentMapToServer.bind(this));
        this.rageConnector.remove(FromClientEvent.MapCreatorSetAddedMapCreatorObjectId, this.setAddedMapCreatorObjectId.bind(this));
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.IsLobbyOwnerChanged.off(null, this.isLobbyOwnerChanged.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    loadMap(mapId: number) {
        this.service.loadMap(mapId).subscribe((data) => {
            this.formGroup.patchValue(data);
            this.notificationService.showSuccess('SavedMapLoadSuccessful');
        });
    }

    startNewMap() {
        try {
            this.service.startNewMap();
        } catch (ex) {
            console.error(ex);
        }

        this.service.initMap(this.formGroup);
    }

    goToNav(nav: MapCreatorNav) {
        if (nav === MapCreatorNav.Object) {
            this.rageConnector.call(ToClientEvent.MapCreatorStartObjectChoice);
        } else if (nav === MapCreatorNav.Vehicle) {
            this.rageConnector.call(ToClientEvent.MapCreatorStartVehicleChoice);
        }

        if (this.currentNav === MapCreatorNav.Object) {
            this.rageConnector.call(ToClientEvent.MapCreatorStopObjectPreview);
        } else if (this.currentNav === MapCreatorNav.Vehicle) {
            this.rageConnector.call(ToClientEvent.MapCreatorStopVehiclePreview);
        }

        this.currentNav = nav;
        this.changeDetector.detectChanges();
    }

    goBackNav() {
        this.goToNav(MapCreatorNav.Main);
        this.rageConnector.call(ToClientEvent.MapCreatorHighlightPos, -1);
        this.changeDetector.detectChanges();
    }

    private createFormGroup() {
        const typeFormControl = new FormControl(undefined, { validators: [Validators.required], updateOn: 'blur' });
        const minPlayersControl = new FormControl(undefined, { updateOn: 'blur' });
        const maxPlayersControl = new FormControl(undefined, {
            validators: [Validators.required, Validators.min(2), Validators.max(1000), valueMoreThanOrEqualValidator(minPlayersControl)],
            updateOn: 'blur',
        });

        minPlayersControl.setValidators([Validators.required, Validators.min(0), Validators.max(20), valueLessThanOrEqualValidator(maxPlayersControl)]);

        this.formGroup = this.fb.group({
            [MapCreateDataKey.Id]: [],
            [MapCreateDataKey.Name]: new FormControl(undefined, {
                validators: [
                    Validators.required,
                    Validators.minLength(Constants.MIN_MAP_CREATE_NAME_LENGTH),
                    Validators.maxLength(Constants.MAX_MAP_CREATE_NAME_LENGTH),
                ],
                updateOn: 'blur',
            }),
            [MapCreateDataKey.Type]: typeFormControl,
            [MapCreateDataKey.Settings]: this.fb.group({
                [0]: minPlayersControl,
                [1]: maxPlayersControl,
            }),
            [MapCreateDataKey.Description]: this.fb.group({
                [LanguageEnum.German]: new FormControl(undefined, { validators: [Validators.maxLength(1000)], updateOn: 'blur' }),
                [LanguageEnum.English]: new FormControl(undefined, { validators: [Validators.maxLength(1000)], updateOn: 'blur' }),
            }),
            [MapCreateDataKey.Objects]: new FormControl(undefined),
            [MapCreateDataKey.TeamSpawns]: new FormControl([], {
                validators: [Validators.required, teamSpawnsValidator(() => typeFormControl.value)],
                updateOn: 'blur',
            }),
            [MapCreateDataKey.MapEdges]: new FormControl([], {
                validators: [mapLimitsValidator(() => typeFormControl.value)],
                updateOn: 'blur',
            }),
            [MapCreateDataKey.BombPlaces]: new FormControl(undefined),
            [MapCreateDataKey.MapCenter]: new FormControl(undefined),
            [MapCreateDataKey.Target]: new FormControl(undefined, [bombPlaceValidator(() => typeFormControl.value)]),
            [MapCreateDataKey.Vehicles]: new FormControl(undefined),
            [MapCreateDataKey.Location]: new FormControl(undefined),
        });
    }

    private addPositionToMapCreatorBrowser(
        id: number,
        type: MapCreatorPositionType,
        posX: number,
        posY: number,
        posZ: number,
        rotX: number,
        rotY: number,
        rotZ: number,
        ownerRemoteId: number,
        info?: string | number
    ) {
        const pos: MapCreatorPosition = { 0: id, 1: type, 2: info, 3: posX, 4: posY, 5: posZ, 6: rotX, 7: rotY, 8: rotZ, 9: ownerRemoteId };
        switch (type) {
            case MapCreatorPositionType.TeamSpawn:
                const teamPositions = this.formGroup.controls[type].value as MapCreatorPosition[][];
                while (info >= teamPositions.length) {
                    teamPositions.push([]);
                }
                const positionsT = teamPositions[info] as MapCreatorPosition[];
                this.updateOrAddPos(pos, positionsT);
                this.formGroup.controls[type].patchValue([...teamPositions], { emitEvent: true });
                break;

            case MapCreatorPositionType.BombPlantPlace:
            case MapCreatorPositionType.MapLimit:
            case MapCreatorPositionType.Object:
            case MapCreatorPositionType.Vehicle:
                const positions = this.formGroup.controls[type].value as MapCreatorPosition[];
                this.updateOrAddPos(pos, positions);
                this.formGroup.controls[type].patchValue([...positions], { emitEvent: true });
                break;

            case MapCreatorPositionType.MapCenter:
            case MapCreatorPositionType.Target:
                this.formGroup.controls[type].patchValue(pos, { emitEvent: true });
                break;
        }
        this.changeDetector.detectChanges();
    }

    private updateOrAddPos(pos: MapCreatorPosition, array: MapCreatorPosition[]) {
        const oldPos = array.find((p) => p[0] === pos[0]);
        if (!oldPos) {
            array.push(pos);
            return;
        }
        // 2x because a key could be missing in one of those positions
        for (const key of Object.keys(oldPos)) {
            oldPos[key] = pos[key];
        }
        for (const key of Object.keys(pos)) {
            oldPos[key] = pos[key];
        }
    }

    private removePositionInMapCreatorBrowser(id: number, type: MapCreatorPositionType) {
        switch (type) {
            case MapCreatorPositionType.TeamSpawn:
                const teamPositions = this.formGroup.controls[type].value as MapCreatorPosition[][];
                for (const positionsT of teamPositions) {
                    const posIndexT = positionsT.findIndex((p) => p[0] === id);
                    if (posIndexT >= 0) {
                        positionsT.splice(posIndexT, 1);
                        this.formGroup.controls[type].patchValue([...teamPositions]);
                        this.changeDetector.detectChanges();
                        return;
                    }
                }
                break;

            case MapCreatorPositionType.BombPlantPlace:
            case MapCreatorPositionType.MapLimit:
            case MapCreatorPositionType.Object:
            case MapCreatorPositionType.Vehicle:
                const positions = this.formGroup.controls[type].value as MapCreatorPosition[];
                const posIndex = positions.findIndex((p) => p[0] === id);
                if (posIndex >= 0) {
                    positions.splice(posIndex, 1);
                    this.formGroup.controls[type].patchValue([...positions], { emitEvent: true });
                }
                break;

            case MapCreatorPositionType.MapCenter:
            case MapCreatorPositionType.Target:
                this.formGroup.controls[type].patchValue(undefined, { emitEvent: true });
                break;
        }

        this.changeDetector.detectChanges();
    }

    private removeTeamPositionsInMapCreatorBrowser(teamNumber: number) {
        const formControl = this.formGroup.controls[MapCreatorPositionType.TeamSpawn];
        const teamPositions = formControl.value as MapCreatorPosition[][];
        if (teamNumber < teamPositions.length) {
            teamPositions.splice(teamNumber, 1);
            formControl.patchValue([...teamPositions], { emitEvent: true });
            this.changeDetector.detectChanges();
        }
    }

    private isLobbyOwnerChanged() {
        if (this.settings.IsLobbyOwner) {
            this.formGroup.controls[MapCreateDataKey.Name].enable({ onlySelf: false });
            this.formGroup.controls[MapCreateDataKey.Type].enable({ onlySelf: false });
        } else {
            this.formGroup.controls[MapCreateDataKey.Name].disable({ onlySelf: false });
            this.formGroup.controls[MapCreateDataKey.Type].disable({ onlySelf: false });
        }

        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }

    private syncCurrentMapToServer(tdsPlayerId: number, idCounter: number) {
        this.rageConnector.callServer(ToServerEvent.MapCreatorSyncCurrentMapToServer, JSON.stringify(this.formGroup), tdsPlayerId, idCounter);
    }

    private setAddedMapCreatorObjectId(id: number, type: MapCreatorPositionType) {
        let pos: MapCreatorPosition;
        switch (type) {
            case MapCreatorPositionType.TeamSpawn:
                const teamPositions = this.formGroup.controls[type].value as MapCreatorPosition[][];
                for (const positionsT of teamPositions) {
                    const possiblePos = positionsT.find((p) => p[0] === -1);
                    if (possiblePos) {
                        pos = possiblePos;
                        break;
                    }
                }
                break;

            case MapCreatorPositionType.BombPlantPlace:
            case MapCreatorPositionType.MapLimit:
            case MapCreatorPositionType.Object:
            case MapCreatorPositionType.Vehicle:
                const positions = this.formGroup.controls[type].value as MapCreatorPosition[];
                pos = positions.find((p) => p[0] === -1);
                break;

            case MapCreatorPositionType.MapCenter:
            case MapCreatorPositionType.Target:
                pos = this.formGroup.controls[type].value;
                break;
        }

        if (pos) {
            pos[0] = id;
            this.changeDetector.detectChanges();
        }
    }

    private onSyncData(infoType: MapCreatorInfoType, data: any) {
        switch (infoType) {
            case MapCreatorInfoType.DescriptionEnglish:
                this.formGroup.controls[MapCreateDataKey.Description].value[LanguageEnum.English] = data;
                break;
            case MapCreatorInfoType.DescriptionGerman:
                this.formGroup.controls[MapCreateDataKey.Description].value[LanguageEnum.German] = data;
                break;
            case MapCreatorInfoType.Settings:
                this.formGroup.controls[infoType].patchValue(JSON.parse(data), { emitEvent: true });
                break;
            default:
                this.formGroup.controls[infoType].patchValue(data, { emitEvent: true });
        }
        this.changeDetector.detectChanges();
    }
}

export function createService(rageConnector: RageConnectorService) {
    if (InitialDatas.inDebug) {
        return new MapCreatorDebugService();
    } else {
        return new MapCreatorProdService(rageConnector);
    }
}
