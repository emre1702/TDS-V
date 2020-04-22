import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { SettingsService } from '../../services/settings.service';
import { LanguageEnum } from '../../enums/language.enum';
import { MapType } from '../../enums/maptype.enum';
import { MatSelectChange, MatDialog, MatSnackBar } from '@angular/material';
import { MapCreateDataDto } from './models/mapCreateDataDto';
import { Constants } from '../../constants';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
import { LoadMapDialog } from './dialog/load-map-dialog';
import { MapCreateError } from './enums/mapcreateerror.enum';
import { FormControl, Validators } from '@angular/forms';
import { MapCreatorPositionType } from './enums/mapcreatorpositiontype.enum';
import { LoadMapDialogGroupDto } from './models/loadMapDialogGroupDto';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';
import { AreYouSureDialog } from '../../dialog/are-you-sure-dialog';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { MapCreatorPosition } from './models/mapCreatorPosition';
import { MapCreateSettings } from './models/mapCreateSettings';
import { MapCreatorInfoType } from './enums/mapcreatorinfotype.enum';
import { DFromServerEvent } from '../../enums/dfromserverevent.enum';
import { isNumber } from 'util';

enum MapCreatorNav {
    Main, MapSettings, Description, TeamSpawns, MapLimit, MapCenter, Objects, Vehicles, BombPlaces, Target
}

@Component({
    selector: 'app-map-creator',
    templateUrl: './map-creator.component.html',
    styleUrls: ['./map-creator.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class MapCreatorComponent implements OnInit, OnDestroy {
    data = new MapCreateDataDto();
    mapCreatorNav = MapCreatorNav;
    mapCreatorPositionType = MapCreatorPositionType;
    mapType = MapType;
    currentNav = MapCreatorNav.Main;
    editingTeamNumber = 0;
    selectedPosition: MapCreatorPosition;

    editingDescriptionLang: string;
    currentTitle = "MapCreator";
    possibleMaps: string[];

    displayedColumns: string[] = ["id", "x", "y", "z", "rot"];
    displayedColumns2D: string[] = ["id", "x", "y"];
    displayedColumnsObject: string[] = ["id", "name", "x", "y", "z", "rotX", "rotY", "rotZ"];
    displayedColumnsVehicle: string[] = ["id", "name", "x", "y", "z", "rotX", "rotY", "rotZ"];

    nameControl = new FormControl("", [
        Validators.required,
        Validators.minLength(Constants.MIN_MAP_CREATE_NAME_LENGTH),
        Validators.maxLength(Constants.MAX_MAP_CREATE_NAME_LENGTH)
    ]);
    mapTypeControl = new FormControl(MapType.Normal, [
        Validators.required
    ]);

    @ViewChild("descriptionTextArea", { static: false }) descriptionTextArea: ElementRef;

    constructor(
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef,
        public dialog: MatDialog,
        private snackBar: MatSnackBar) {
        this.rageConnector.listen(DFromClientEvent.AddPositionToMapCreatorBrowser, this.addPositionToMapCreatorBrowser.bind(this));
        this.rageConnector.listen(DFromClientEvent.RemovePositionInMapCreatorBrowser, this.removePositionInMapCreatorBrowser.bind(this));
        this.rageConnector.listen(DFromClientEvent.RemoveTeamPositionsInMapCreatorBrowser, this.removeTeamPositionsInMapCreatorBrowser.bind(this));
    }

    ngOnInit() {
        this.rageConnector.listen(DFromServerEvent.MapCreatorSyncData, this.onSyncData.bind(this));
        this.rageConnector.listen(DFromClientEvent.LoadMapForMapCreator, this.onLoadMap.bind(this));
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.IsLobbyOwnerChanged.on(null, this.isLobbyOwnerChanged.bind(this));

        this.isLobbyOwnerChanged();
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromServerEvent.MapCreatorSyncData, this.onSyncData.bind(this));
        this.rageConnector.remove(DFromClientEvent.LoadMapForMapCreator, this.onLoadMap.bind(this));
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.IsLobbyOwnerChanged.off(null, this.isLobbyOwnerChanged.bind(this));
    }

    private addPositionToMapCreatorBrowser(id: number, type: MapCreatorPositionType, posX: number, posY: number, posZ: number,
        rotX: number, rotY: number, rotZ: number, ownerRemoteId: number, info?: string | number) {
        const pos = new MapCreatorPosition(id, type, posX, posY, posZ, rotX, rotY, rotZ, ownerRemoteId);
        switch (type) {
            case MapCreatorPositionType.TeamSpawn:
                pos[2] = info;
                this.addPosToTeamSpawns(pos);
                break;
            case MapCreatorPositionType.MapCenter:
                this.addPosToMapCenter(pos);
                break;
            case MapCreatorPositionType.BombPlantPlace:
                this.addPosToBombPlaces(pos);
                break;
            case MapCreatorPositionType.MapLimit:
                this.addPosToMapLimits(pos);
                break;
            case MapCreatorPositionType.Object:
                pos[2] = info;
                this.addPosToObjects(pos);
                break;
            case MapCreatorPositionType.Vehicle:
                pos[2] = info;
                this.addPosToVehicles(pos);
                break;
            case MapCreatorPositionType.Target:
                this.addPosToTarget(pos);
                break;
        }
        this.changeDetector.detectChanges();
    }

    private removePositionInMapCreatorBrowser(id: number, type: MapCreatorPositionType) {
        switch (type) {
            case MapCreatorPositionType.TeamSpawn:
                for (let i = 0; i < this.data[6].length; ++i) {
                    const teamSpawnPos = this.data[6][i].find(p => p[0] === id);
                    if (teamSpawnPos) {
                        const prevEditingTeam = this.editingTeamNumber;
                        this.editingTeamNumber = i;
                        this.selectedPosition = teamSpawnPos;
                        this.removePosFromTeamSpawns();
                        this.editingTeamNumber = prevEditingTeam;
                        break;
                    }
                }
                break;
            case MapCreatorPositionType.MapCenter:
                this.data[9] = undefined;
                break;
            case MapCreatorPositionType.BombPlantPlace:
                const bombPos = this.data[8].find(p => p[0] === id);
                if (bombPos) {
                    this.selectedPosition = bombPos;
                    this.removePosFromBombPlaces();
                }
                break;
            case MapCreatorPositionType.MapLimit:
                const mapLimitPos = this.data[7].find(p => p[0] === id);
                if (mapLimitPos) {
                    this.selectedPosition = mapLimitPos;
                    this.removePosFromMapLimits();
                }
                break;
            case MapCreatorPositionType.Object:
                const objectPos = this.data[5].find(p => p[0] === id);
                if (objectPos) {
                    this.selectedPosition = objectPos;
                    this.removePosFromObjects();
                }
                break;
            case MapCreatorPositionType.Target:
                this.data[10] = undefined;
                break;
            case MapCreatorPositionType.Vehicle:
                const vehiclePos = this.data[11].find(p => p[0] === id);
                if (vehiclePos) {
                    this.selectedPosition = vehiclePos;
                    this.removePosFromVehicles();
                }
                break;
        }
        this.rageConnector.call(DToClientEvent.MapCreatorHighlightPos, -1);
        this.changeDetector.detectChanges();
    }

    private removeTeamPositionsInMapCreatorBrowser(teamNumber: number) {
        this.data[6].pop();
        if (this.editingTeamNumber == this.data[6].length) {
            this.editingTeamNumber--;
        }
        this.changeDetector.detectChanges();
    }

    private isLobbyOwnerChanged() {
        if (this.settings.IsLobbyOwner) {
            this.nameControl.enable({ onlySelf: false });
            this.mapTypeControl.enable({ onlySelf: false });
        } else {
            this.nameControl.disable({ onlySelf: false });
            this.mapTypeControl.disable({ onlySelf: false });
        }

        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }

    removeLastTeam() {
        if (this.data[6].length == 1) {
            this.data[6][0] = [];
            return;
        }

        this.data[6].pop();
        this.editingTeamNumber = this.data[6].length - 1;
        this.rageConnector.call(DToClientEvent.RemoveMapCreatorTeamNumber, this.data[6].length);
        this.changeDetector.detectChanges();
    }

    startNewPosPlacing(type: MapCreatorPositionType) {
        if (isNaN(Number(this.editingTeamNumber)))
            return;
        this.rageConnector.call(DToClientEvent.StartMapCreatorPosPlacing, type, Number(this.editingTeamNumber));
    }

    removeSelectedPos(removeFunc: () => void) {
        removeFunc.call(this);
        this.rageConnector.call(DToClientEvent.RemoveMapCreatorPosition, this.selectedPosition[0]);
        this.selectedPosition = undefined;
        this.changeDetector.detectChanges();
    }

    holdSelected() {
        this.rageConnector.call(DToClientEvent.HoldMapCreatorObject, this.selectedPosition[0]);
    }

    tpToSelectedPos() {
        const pos = this.selectedPosition;
        this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, pos[3], pos[4], pos[5], pos[8]);
    }

    tpToXYZ(x: number, y: number, z: number) {
        this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, x, y, z, 0);
    }

    addPosToTeamSpawns(pos: MapCreatorPosition) {
        if (!this.data[6][pos[2]])
            this.data[6][pos[2]] = [];
        if (!this.updatePosIfExists(this.data[6][pos[2]], pos)) {
            this.data[6][pos[2]] = [...this.data[6][pos[2]], pos];
        }
    }

    addPosToMapLimits(pos: MapCreatorPosition) {
        if (!this.data[7])
            this.data[7] = [];
        if (!this.updatePosIfExists(this.data[7], pos)) {
            this.data[7] = [...this.data[7], pos];
        }
    }

    addPosToObjects(pos: MapCreatorPosition) {
        if (!this.data[5])
            this.data[5] = [];
        if (!this.updatePosIfExists(this.data[5], pos)) {
            this.data[5] = [...this.data[5], pos];
        }
    }

    addPosToBombPlaces(pos: MapCreatorPosition) {
        if (!this.data[8])
            this.data[8] = [];
        if (!this.updatePosIfExists(this.data[8], pos)) {
            this.data[8] = [...this.data[8], pos];
        }
    }

    addPosToVehicles(pos: MapCreatorPosition) {
        if (!this.data[11])
            this.data[11] = [];
        if (!this.updatePosIfExists(this.data[11], pos)) {
            this.data[11] = [...this.data[11], pos];
        }
    }

    private updatePosIfExists(arr: MapCreatorPosition[], pos: MapCreatorPosition): boolean {
        const entries = arr.filter(position => position[0] === pos[0]);
        if (entries.length <= 0) {
            return false;
        }
        const origPos = entries[0];
        origPos[2] = pos[2];
        origPos[3] = pos[3];
        origPos[4] = pos[4];
        origPos[5] = pos[5];
        origPos[6] = pos[6];
        origPos[7] = pos[7];
        origPos[8] = pos[8];
        return true;
    }

    addPosToMapCenter(pos: MapCreatorPosition) {
        this.data[9] = pos;
        if (this.currentNav === MapCreatorNav.MapCenter) {
            this.selectedPosition = pos;
            this.rageConnector.call(DToClientEvent.MapCreatorHighlightPos, pos[0]);
        }
    }

    addPosToTarget(pos: MapCreatorPosition) {
        this.data[10] = pos;
        if (this.currentNav === MapCreatorNav.Target) {
            this.selectedPosition = pos;
            this.rageConnector.call(DToClientEvent.MapCreatorHighlightPos, pos[0]);
        }
    }

    removePosFromTeamSpawns() {
        const index = this.data[6][this.editingTeamNumber].indexOf(this.selectedPosition);
        this.data[6][this.editingTeamNumber].splice(index, 1);
        // need to create a new dataSource object, else table will not refresh
        this.data[6][this.editingTeamNumber] = [...this.data[6][this.editingTeamNumber]];
    }

    removePosFromMapLimits() {
        const index = this.data[7].indexOf(this.selectedPosition);
        this.data[7].splice(index, 1);
        // need to create a new dataSource object, else table will not refresh
        this.data[7] = [...this.data[7]];
    }

    removePosFromObjects() {
        const index = this.data[5].indexOf(this.selectedPosition);
        this.data[5].splice(index, 1);
        this.data[5] = [...this.data[5]];
    }

    removePosFromVehicles() {
        const index = this.data[11].indexOf(this.selectedPosition);
        this.data[11].splice(index, 1);
        this.data[11] = [...this.data[11]];
    }

    removePosFromBombPlaces() {
        const index = this.data[8].indexOf(this.selectedPosition);
        this.data[8].splice(index, 1);
        // need to create a new dataSource object, else table will not refresh
        this.data[8] = [...this.data[8]];
    }

    removeMapCenter() {
        this.data[9] = undefined;
    }

    removeTarget() {
        this.data[10] = undefined;
    }

    sendDataToClient() {
        this.fixData();
        this.changeDetector.detectChanges();
        this.rageConnector.callCallbackServer(DToServerEvent.SendMapCreatorData, [JSON.stringify(this.data)], (err: number) => {
            const errName = MapCreateError[err];
            this.snackBar.open(this.settings.Lang[errName], "OK", {
                duration: undefined,
                panelClass: "mat-app-background"
            });
        });
    }

    removeTheMap() {
        this.dialog.open(AreYouSureDialog, { panelClass: "mat-app-background" })
            .afterClosed()
            .subscribe((bool: boolean) => {
                const map = this.data;
                this.data = new MapCreateDataDto();
                this.nameControl.setValue(this.data[1]);
                this.mapTypeControl.setValue(this.data[2]);
                this.changeDetector.detectChanges();

                this.rageConnector.call(DToServerEvent.RemoveMap, map[0]);
                this.rageConnector.call(DToClientEvent.MapCreatorStartNew);
            });
    }

    startNew() {
        this.dialog.open(AreYouSureDialog, { panelClass: "mat-app-background" })
            .afterClosed()
            .subscribe((bool: boolean) => {
                if (!bool)
                    return;
                this.data = new MapCreateDataDto();
                this.nameControl.setValue(this.data[1]);
                this.mapTypeControl.setValue(this.data[2]);
                this.changeDetector.detectChanges();
                this.rageConnector.call(DToClientEvent.MapCreatorStartNew);
            });
    }

    saveData() {
        this.fixData();
        this.rageConnector.callCallbackServer(DToServerEvent.SaveMapCreatorData, [JSON.stringify(this.data)], (err: number) => {
            const errName = MapCreateError[err];
            this.snackBar.open(this.settings.Lang[errName], "OK", {
                duration: undefined,
                panelClass: "mat-app-background"
            });
        });
    }

    loadPossibleMaps() {
        this.rageConnector.callCallbackServer(DToServerEvent.LoadMapNamesToLoadForMapCreator, null, (possibleMapsJson: string) => {
            const possibleMaps = JSON.parse(possibleMapsJson) as LoadMapDialogGroupDto[];
            const dialogRef = this.dialog.open(LoadMapDialog, { data: possibleMaps, panelClass: "mat-app-background" });

            dialogRef.beforeClosed().subscribe((loadMapId?: number) => {
                if (loadMapId === undefined)
                    return;

                this.rageConnector.callServer(DToServerEvent.LoadMapForMapCreator, loadMapId);
            });

            this.changeDetector.detectChanges();
        });
    }

    private onLoadMap(json: string) {
        this.data = JSON.parse(json);
        this.nameControl.setValue(this.data[1]);
        this.mapTypeControl.setValue(this.data[2]);
        this.fixData();
        this.snackBar.open(this.settings.Lang.SavedMapLoadSuccessful, "OK", {
            duration: 5000,
            panelClass: "mat-app-background"
        });
        this.changeDetector.detectChanges();
    }

    private fixData() {
        this.data[1] = this.nameControl.value;
        if (!this.data[1])
            this.data[1] = "";

        this.data[2] = this.mapTypeControl.value;
        if (!this.data[2])
            this.data[2] = 0;

        if (!this.data[3])
            this.data[3] = new MapCreateSettings();
        if (this.data[3][0] == undefined)
            this.data[3][0] = 0;
        this.data[3][0] = Math.max(0, Math.min(999, Math.floor(this.data[3][0])));
        if (this.data[3][1] == undefined)
            this.data[3][1] = 999;
        this.data[3][1] = Math.max(0, Math.min(999, Math.floor(this.data[3][1])));

        if (!this.data[4])
            this.data[4] = { [LanguageEnum.German]: "", [LanguageEnum.English]: "" };
        if (!this.data[4][LanguageEnum.German])
            this.data[4][LanguageEnum.German] = "";
        if (!this.data[4][LanguageEnum.English])
            this.data[4][LanguageEnum.English] = "";

        if (!this.data[5])
            this.data[5] = [];

        if (!this.data[6])
            this.data[6] = [[]];

        if (!this.data[7])
            this.data[7] = [];

        if (!this.data[8])
            this.data[8] = [];
    }

    getLanguages(): string[] {
        const keys = Object.keys(LanguageEnum);
        return keys.slice(keys.length / 2);
    }

    getMapTypes(): string[] {
        const keys = Object.keys(MapType);
        return keys.slice(keys.length / 2);
    }

    getMapTypeValue(mapType: string): number {
        return MapType[mapType];
    }

    getLanguageValue(lang: string): number {
        return LanguageEnum[this.editingDescriptionLang];
    }

    onMapNameChange() {
        this.changeDetector.detectChanges();
        const value = this.nameControl.value;
        this.rageConnector.callServer(DToServerEvent.MapCreatorSyncData, MapCreatorInfoType.Name, value);
        this.data[1] = value;
    }

    onMapTypeChange(event: MatSelectChange) {
        if (this.mapTypeControl.value == MapType.Bomb && event.value != MapType.Bomb) {
            this.data[8] = [];
        }
        this.data[2] = event.value;
        this.changeDetector.detectChanges();
        this.rageConnector.callServer(DToServerEvent.MapCreatorSyncData, MapCreatorInfoType.Type, event.value);
    }

    onEditingTeamNumberChange(event: MatSelectChange) {
        if (event.value == "+" && !this.data[6][this.data[6].length - 1].length) {
            this.editingTeamNumber = this.data[6].length - 1;
        } else if (event.value == "+") {
            if (!this.settings.IsLobbyOwner)
                return;
            this.data[6] = [...this.data[6], []];
            this.editingTeamNumber = this.data[6].length - 1;
        } else
            this.editingTeamNumber = event.value;
        this.changeDetector.detectChanges();
    }

    onSelectedPositionChanged(row: MapCreatorPosition) {
        if (this.selectedPosition == row)
            this.selectedPosition = undefined;
        else
            this.selectedPosition = row;
        this.rageConnector.call(DToClientEvent.MapCreatorHighlightPos, this.selectedPosition ? this.selectedPosition[0] : -1);
        this.changeDetector.detectChanges();
    }

    onSyncData(infoType: MapCreatorInfoType, data: any) {
        switch (infoType) {
            case MapCreatorInfoType.Name:
                this.nameControl.setValue(data);
                break;
            case MapCreatorInfoType.Type:
                this.mapTypeControl.setValue(data);
                break;
            case MapCreatorInfoType.DescriptionEnglish:
                this.data[4][LanguageEnum.English] = data;
                break;
            case MapCreatorInfoType.DescriptionGerman:
                this.data[4][LanguageEnum.English] = data;
                break;
            case MapCreatorInfoType.Settings:
                this.data[3] = JSON.parse(data);
                break;
        }
        this.changeDetector.detectChanges();
    }

    switchToMapSettings() {
        this.currentTitle = 'MapSettings';
        this.currentNav = MapCreatorNav.MapSettings;
        this.changeDetector.detectChanges();
    }

    switchToDescriptionEdit(lang: string) {
        this.currentTitle = lang;
        this.editingDescriptionLang = lang;
        this.currentNav = MapCreatorNav.Description;
        this.changeDetector.detectChanges();
    }

    switchToTeamSpawnsEdit() {
        this.currentTitle = 'TeamSpawns';
        this.currentNav = MapCreatorNav.TeamSpawns;
        this.changeDetector.detectChanges();
    }

    switchToMapLimitEdit() {
        this.currentTitle = 'MapLimit';
        this.currentNav = MapCreatorNav.MapLimit;
        this.changeDetector.detectChanges();
    }

    switchToObjects() {
        this.rageConnector.call(DToClientEvent.MapCreatorStartObjectChoice);

        this.currentTitle = 'Objects';
        this.currentNav = MapCreatorNav.Objects;
        this.changeDetector.detectChanges();
    }

    switchToVehicles() {
        this.rageConnector.call(DToClientEvent.MapCreatorStartVehicleChoice);

        this.currentTitle = 'Vehicles';
        this.currentNav = MapCreatorNav.Vehicles;
        this.changeDetector.detectChanges();
    }

    switchToMapCenterEdit() {
        this.currentTitle = 'MapCenter';
        this.selectedPosition = this.data[9];
        this.currentNav = MapCreatorNav.MapCenter;
        this.changeDetector.detectChanges();

        if (this.data[9])
            this.rageConnector.call(DToClientEvent.MapCreatorHighlightPos, this.data[9][0]);
    }

    switchToBombPlacesEdit() {
        this.currentTitle = 'BombPlaces';
        this.currentNav = MapCreatorNav.BombPlaces;
        this.changeDetector.detectChanges();
    }

    switchToTargetEdit() {
        this.currentTitle = 'Target';
        this.selectedPosition = this.data[10];
        this.currentNav = MapCreatorNav.Target;
        this.changeDetector.detectChanges();

        if (this.data[10])
            this.rageConnector.call(DToClientEvent.MapCreatorHighlightPos, this.data[10][0]);
    }

    saveNav() {
        switch (this.currentNav) {
            case MapCreatorNav.Description:
                this.saveDescription();
                break;
            case MapCreatorNav.Objects:
                this.rageConnector.call(DToClientEvent.MapCreatorStopObjectPreview);
                break;
            case MapCreatorNav.Vehicles:
                this.rageConnector.call(DToClientEvent.MapCreatorStopVehiclePreview);
                break;
            case MapCreatorNav.MapSettings:
                this.rageConnector.call(DToServerEvent.MapCreatorSyncData, MapCreatorInfoType.Settings, JSON.stringify(this.data[3]));
                break;
        }
        this.goBackNav();
    }

    goBackNav() {
        this.currentNav = MapCreatorNav.Main;
        this.selectedPosition = undefined;
        this.rageConnector.call(DToClientEvent.MapCreatorHighlightPos, -1);
        this.changeDetector.detectChanges();
    }

    private saveDescription() {
        const langId = this.getLanguageValue(this.editingDescriptionLang);
        this.data[4][langId] = this.descriptionTextArea.nativeElement.value;
        switch (langId) {
            case LanguageEnum.English:
                this.rageConnector.call(DToServerEvent.MapCreatorSyncData, MapCreatorInfoType.DescriptionEnglish, this.data[4][langId]);
                break;
            case LanguageEnum.German:
                this.rageConnector.call(DToServerEvent.MapCreatorSyncData, MapCreatorInfoType.DescriptionGerman, this.data[4][langId]);
                break;
        }

    }

    isSaveableNav() {
        return this.currentNav != MapCreatorNav.Main;
    }

    isBackableNav() {
        return this.currentNav == MapCreatorNav.Description;
    }

    isTeamSpawnsValid(): boolean {
        if (this.data[6].length == 0)
            return false;

        if (this.mapTypeControl.value == MapType.Gangwar || this.mapTypeControl.value == MapType.Bomb) {
            if (this.data[6].length != 2) {
                return false;
            }
        }

        for (const spawnArr of this.data[6]) {
            if (spawnArr.length < Constants.MIN_TEAM_SPAWNS)
                return false;
        }
        return true;
    }

    isMapLimitValid(): boolean {
        return (this.mapTypeControl.value != MapType.Gangwar && !this.data[7].length) || this.data[7].length >= 3;
    }

    isBombPlacesValid(): boolean {
        return this.mapTypeControl.value != MapType.Bomb || this.data[8].length > 0;
    }

    isTargetValid(): boolean {
        return this.mapTypeControl.value != MapType.Gangwar || this.data[10] != undefined;
    }

    getMinNameLength() {
        return Constants.MIN_MAP_CREATE_NAME_LENGTH;
    }

    getMaxNameLength() {
        return Constants.MAX_MAP_CREATE_NAME_LENGTH;
    }

    isOwner(pos: MapCreatorPosition) {
        return pos[9] == this.settings.Constants[1];
    }
}
