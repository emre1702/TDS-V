import * as tslib_1 from "tslib";
import { Component, ViewChild, ChangeDetectionStrategy } from '@angular/core';
import { LanguageEnum } from '../../enums/language.enum';
import { MapType } from './enums/maptype.enum';
import { MapCreateDataDto } from './models/mapCreateDataDto';
import { Constants } from '../../constants';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
import { LoadMapDialog } from './dialog/load-map-dialog';
import { MapCreateError } from './enums/mapcreateerror.enum';
import { FormControl, Validators } from '@angular/forms';
import { MapCreatorPositionType } from './enums/mapcreatorpositiontype.enum';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';
import { AreYouSureDialog } from '../../dialog/are-you-sure-dialog';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { MapCreatorPosition } from './models/mapCreatorPosition';
var MapCreatorNav;
(function (MapCreatorNav) {
    MapCreatorNav[MapCreatorNav["Main"] = 0] = "Main";
    MapCreatorNav[MapCreatorNav["MapSettings"] = 1] = "MapSettings";
    MapCreatorNav[MapCreatorNav["Description"] = 2] = "Description";
    MapCreatorNav[MapCreatorNav["TeamSpawns"] = 3] = "TeamSpawns";
    MapCreatorNav[MapCreatorNav["MapLimit"] = 4] = "MapLimit";
    MapCreatorNav[MapCreatorNav["MapCenter"] = 5] = "MapCenter";
    MapCreatorNav[MapCreatorNav["Objects"] = 6] = "Objects";
    MapCreatorNav[MapCreatorNav["BombPlaces"] = 7] = "BombPlaces";
})(MapCreatorNav || (MapCreatorNav = {}));
let MapCreatorComponent = class MapCreatorComponent {
    constructor(settings, rageConnector, changeDetector, dialog, snackBar) {
        this.settings = settings;
        this.rageConnector = rageConnector;
        this.changeDetector = changeDetector;
        this.dialog = dialog;
        this.snackBar = snackBar;
        this.data = new MapCreateDataDto();
        this.mapCreatorNav = MapCreatorNav;
        this.mapCreatorPositionType = MapCreatorPositionType;
        this.currentNav = MapCreatorNav.Main;
        this.editingTeamNumber = 0;
        this.currentTitle = "MapCreator";
        this.displayedColumns = ["id", "x", "y", "z", "rot"];
        this.displayedColumns2D = ["id", "x", "y"];
        this.displayedColumnsObject = ["id", "name", "x", "y", "z", "rotX", "rotY", "rotZ"];
        this.nameControl = new FormControl("", [
            Validators.required,
            Validators.minLength(Constants.MIN_MAP_CREATE_NAME_LENGTH),
            Validators.maxLength(Constants.MAX_MAP_CREATE_NAME_LENGTH)
        ]);
        this.rageConnector.listen(DFromClientEvent.AddPositionToMapCreatorBrowser, this.addPositionToMapCreatorBrowser.bind(this));
        this.rageConnector.listen(DFromClientEvent.RemovePositionInMapCreatorBrowser, this.RemovePositionInMapCreatorBrowser.bind(this));
    }
    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }
    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }
    addPositionToMapCreatorBrowser(id, type, posX, posY, posZ, rotX, rotY, rotZ, info) {
        const pos = new MapCreatorPosition(id, type, posX, posY, posZ, rotX, rotY, rotZ);
        switch (type) {
            case MapCreatorPositionType.TeamSpawn:
                pos.Info = info;
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
                pos.Info = info;
                this.addPosToObjects(pos);
                break;
        }
        this.changeDetector.detectChanges();
    }
    RemovePositionInMapCreatorBrowser(id, type) {
        switch (type) {
            case MapCreatorPositionType.TeamSpawn:
                for (const list of this.data.TeamSpawns) {
                    const teamSpawnPos = list.find(p => p.Id === id);
                    if (teamSpawnPos) {
                        this.selectedPosition = teamSpawnPos;
                        this.removePosFromTeamSpawns();
                        break;
                    }
                }
                break;
            case MapCreatorPositionType.MapCenter:
                this.data.MapCenter = undefined;
                break;
            case MapCreatorPositionType.BombPlantPlace:
                const bombPos = this.data.BombPlaces.find(p => p.Id === id);
                if (bombPos) {
                    this.selectedPosition = bombPos;
                    this.removePosFromBombPlaces();
                }
                break;
            case MapCreatorPositionType.MapLimit:
                const mapLimitPos = this.data.MapEdges.find(p => p.Id === id);
                if (mapLimitPos) {
                    this.selectedPosition = mapLimitPos;
                    this.removePosFromMapLimits();
                }
                break;
            case MapCreatorPositionType.Object:
                const objectPos = this.data.Objects.find(p => p.Id === id);
                if (objectPos) {
                    this.selectedPosition = objectPos;
                    this.removePosFromObjects();
                }
                break;
        }
        this.changeDetector.detectChanges();
    }
    detectChanges() {
        this.changeDetector.detectChanges();
    }
    removeLastTeam() {
        this.data.TeamSpawns.pop();
        this.editingTeamNumber = this.data.TeamSpawns.length - 1;
        this.rageConnector.call(DToClientEvent.RemoveMapCreatorTeamNumber, this.data.TeamSpawns.length);
        this.changeDetector.detectChanges();
    }
    startNewPosPlacing(type) {
        this.rageConnector.call(DToClientEvent.StartMapCreatorPosPlacing, type, this.editingTeamNumber);
    }
    removeSelectedPos(removeFunc) {
        removeFunc.call(this);
        this.rageConnector.call(DToClientEvent.RemoveMapCreatorPosition, this.selectedPosition.Id);
        this.selectedPosition = undefined;
        this.changeDetector.detectChanges();
    }
    holdSelected() {
        this.rageConnector.call(DToClientEvent.HoldMapCreatorObject, this.selectedPosition.Id);
    }
    tpToSelectedPos() {
        const pos = this.selectedPosition;
        this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, pos.PosX, pos.PosY, pos.PosZ, pos.RotZ);
    }
    tpToXYZ(x, y, z) {
        this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, x, y, z, 0);
    }
    addPosToTeamSpawns(pos) {
        if (!this.updatePosIfExists(this.data.TeamSpawns[pos.Info], pos)) {
            this.data.TeamSpawns[pos.Info] = [...this.data.TeamSpawns[pos.Info], pos];
        }
    }
    addPosToMapLimits(pos) {
        if (!this.updatePosIfExists(this.data.MapEdges, pos)) {
            this.data.MapEdges = [...this.data.MapEdges, pos];
        }
    }
    addPosToObjects(pos) {
        if (!this.updatePosIfExists(this.data.Objects, pos)) {
            this.data.Objects = [...this.data.Objects, pos];
        }
    }
    addPosToBombPlaces(pos) {
        if (!this.updatePosIfExists(this.data.BombPlaces, pos)) {
            this.data.BombPlaces = [...this.data.BombPlaces, pos];
        }
    }
    updatePosIfExists(arr, pos) {
        const entries = arr.filter(position => position.Id === pos.Id);
        if (entries.length <= 0) {
            return false;
        }
        const origPos = entries[0];
        origPos.Info = pos.Info;
        origPos.PosX = pos.PosX;
        origPos.PosY = pos.PosY;
        origPos.PosZ = pos.PosZ;
        origPos.RotX = pos.RotX;
        origPos.RotY = pos.RotY;
        origPos.RotZ = pos.RotZ;
        return true;
    }
    addPosToMapCenter(pos) {
        this.data.MapCenter = pos;
    }
    removePosFromTeamSpawns() {
        const index = this.data.TeamSpawns[this.editingTeamNumber].indexOf(this.selectedPosition);
        this.data.TeamSpawns[this.editingTeamNumber].splice(index, 1);
        // need to create a new dataSource object, else table will not refresh
        this.data.TeamSpawns[this.editingTeamNumber] = [...this.data.TeamSpawns[this.editingTeamNumber]];
    }
    removePosFromMapLimits() {
        const index = this.data.MapEdges.indexOf(this.selectedPosition);
        this.data.MapEdges.splice(index, 1);
        // need to create a new dataSource object, else table will not refresh
        this.data.MapEdges = [...this.data.MapEdges];
    }
    removePosFromObjects() {
        const index = this.data.Objects.indexOf(this.selectedPosition);
        this.data.Objects.splice(index, 1);
        this.data.Objects = [...this.data.Objects];
    }
    removePosFromBombPlaces() {
        const index = this.data.BombPlaces.indexOf(this.selectedPosition);
        this.data.BombPlaces.splice(index, 1);
        // need to create a new dataSource object, else table will not refresh
        this.data.BombPlaces = [...this.data.BombPlaces];
    }
    sendDataToClient() {
        this.fixData();
        this.changeDetector.detectChanges();
        this.rageConnector.callCallback(DToClientEvent.SendMapCreatorData, [JSON.stringify(this.data)], (err) => {
            const errName = MapCreateError[err];
            this.snackBar.open(this.settings.Lang[errName], "OK", {
                duration: undefined, panelClass: "mat-app-background"
            });
        });
    }
    removeTheMap() {
        this.dialog.open(AreYouSureDialog, { panelClass: "mat-app-background" })
            .afterClosed()
            .subscribe((bool) => {
            const map = this.data;
            this.data = new MapCreateDataDto();
            this.changeDetector.detectChanges();
            if (map.Id == 0)
                return;
            this.rageConnector.call(DToServerEvent.RemoveMap, this.data.Id);
        });
    }
    startNew() {
        this.dialog.open(AreYouSureDialog, { panelClass: "mat-app-background" })
            .afterClosed()
            .subscribe((bool) => {
            if (!bool)
                return;
            this.data = new MapCreateDataDto();
            this.changeDetector.detectChanges();
        });
    }
    saveData() {
        this.fixData();
        this.rageConnector.callCallback(DToClientEvent.SaveMapCreatorData, [JSON.stringify(this.data)], (err) => {
            const errName = MapCreateError[err];
            this.snackBar.open(this.settings.Lang[errName], "OK", {
                duration: undefined,
                panelClass: "mat-app-background"
            });
        });
    }
    loadPossibleMaps() {
        this.rageConnector.callCallback(DToServerEvent.LoadMapNamesToLoadForMapCreator, null, (possibleMapsJson) => {
            const possibleMaps = JSON.parse(possibleMapsJson);
            const dialogRef = this.dialog.open(LoadMapDialog, { data: possibleMaps, panelClass: "mat-app-background" });
            dialogRef.beforeClosed().subscribe(loadMapStr => {
                if (!loadMapStr)
                    return;
                this.rageConnector.callCallback(DToServerEvent.LoadMapForMapCreator, loadMapStr, (json) => {
                    this.data = JSON.parse(json);
                    this.snackBar.open(this.settings.Lang.SavedMapLoadSuccessful, "OK", {
                        duration: 5000,
                        panelClass: "mat-app-background"
                    });
                });
            });
            this.changeDetector.detectChanges();
        });
    }
    fixData() {
        if (typeof (this.data.MinPlayers) == "undefined")
            this.data.MinPlayers = 0;
        this.data.MinPlayers = Math.max(0, Math.min(999, Math.floor(this.data.MinPlayers)));
        if (typeof (this.data.MaxPlayers) == "undefined")
            this.data.MaxPlayers = 999;
        this.data.MaxPlayers = Math.max(0, Math.min(999, Math.floor(this.data.MaxPlayers)));
    }
    getLanguages() {
        const keys = Object.keys(LanguageEnum);
        return keys.slice(keys.length / 2);
    }
    getMapTypes() {
        const keys = Object.keys(MapType);
        return keys.slice(keys.length / 2);
    }
    getMapTypeValue(mapType) {
        return MapType[mapType];
    }
    getLanguageValue(lang) {
        return LanguageEnum[this.editingDescriptionLang];
    }
    onMapTypeChange(event) {
        this.data.Type = event.value;
        this.changeDetector.detectChanges();
    }
    onEditingTeamNumberChange(event) {
        if (event.value == "+" && this.data.TeamSpawns[this.data.TeamSpawns.length - 1].length) {
            this.editingTeamNumber = event.value - 1;
        }
        else if (event.value == "+") {
            this.data.TeamSpawns = [...this.data.TeamSpawns, []];
            this.editingTeamNumber = this.data.TeamSpawns.length - 1;
        }
        else
            this.editingTeamNumber = event.value;
        this.changeDetector.detectChanges();
    }
    onSelectedPositionChanged(row) {
        if (this.selectedPosition == row)
            this.selectedPosition = undefined;
        else
            this.selectedPosition = row;
        this.changeDetector.detectChanges();
    }
    switchToMapSettings() {
        this.currentTitle = 'MapSettings';
        this.currentNav = MapCreatorNav.MapSettings;
        this.changeDetector.detectChanges();
    }
    switchToDescriptionEdit(lang) {
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
    switchToMapCenterEdit() {
        this.currentTitle = 'MapCenter';
        this.currentNav = MapCreatorNav.MapCenter;
        this.selectedPosition = this.data.MapCenter;
        this.changeDetector.detectChanges();
    }
    switchToBombPlacesEdit() {
        this.currentTitle = 'BombPlaces';
        this.currentNav = MapCreatorNav.BombPlaces;
        this.changeDetector.detectChanges();
    }
    saveNav() {
        switch (this.currentNav) {
            case MapCreatorNav.Description:
                this.saveDescription();
                break;
            case MapCreatorNav.Objects:
                this.rageConnector.call(DToClientEvent.MapCreatorStopObjectPreview);
                break;
        }
        this.goBackNav();
    }
    goBackNav() {
        this.currentNav = MapCreatorNav.Main;
        this.selectedPosition = undefined;
        this.changeDetector.detectChanges();
    }
    saveDescription() {
        const langId = this.getLanguageValue(this.editingDescriptionLang);
        this.data.Description[langId] = this.descriptionTextArea.nativeElement.value;
    }
    isSaveableNav() {
        return this.currentNav != MapCreatorNav.Main;
    }
    isBackableNav() {
        return this.currentNav == MapCreatorNav.Description;
    }
    isTeamSpawnsValid() {
        if (this.data.TeamSpawns.length == 0)
            return false;
        for (const spawnArr of this.data.TeamSpawns) {
            if (spawnArr.length < Constants.MIN_TEAM_SPAWNS)
                return false;
        }
        return true;
    }
    isMapLimitValid() {
        return !this.data.MapEdges.length || this.data.MapEdges.length >= 3;
    }
    isBombPlacesValid() {
        return this.data.Type != MapType.Bomb || this.data.BombPlaces.length > 0;
    }
    getMinNameLength() {
        return Constants.MIN_MAP_CREATE_NAME_LENGTH;
    }
    getMaxNameLength() {
        return Constants.MAX_MAP_CREATE_NAME_LENGTH;
    }
};
tslib_1.__decorate([
    ViewChild("descriptionTextArea", { static: false })
], MapCreatorComponent.prototype, "descriptionTextArea", void 0);
MapCreatorComponent = tslib_1.__decorate([
    Component({
        selector: 'app-map-creator',
        templateUrl: './map-creator.component.html',
        styleUrls: ['./map-creator.component.scss'],
        changeDetection: ChangeDetectionStrategy.OnPush
    })
], MapCreatorComponent);
export { MapCreatorComponent };
//# sourceMappingURL=map-creator.component.js.map