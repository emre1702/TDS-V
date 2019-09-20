import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { SettingsService } from '../../services/settings.service';
import { LanguageEnum } from '../../enums/language.enum';
import { MapType } from './enums/maptype.enum';
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

enum MapCreatorNav {
  Main, MapSettings, Description, TeamSpawns, MapLimit, MapCenter, Objects, BombPlaces
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
  currentNav = MapCreatorNav.Main;
  editingTeamNumber = 0;
  selectedPosition: MapCreatorPosition;

  editingDescriptionLang: string;
  currentTitle = "MapCreator";
  possibleMaps: string[];

  displayedColumns: string[] = ["id", "x", "y", "z", "rot"];
  displayedColumns2D: string[] = ["id", "x", "y"];
  displayedColumnsObject: string[] = ["id", "name", "x", "y", "z", "rotX", "rotY", "rotZ"];

  nameControl = new FormControl("", [
    Validators.required,
    Validators.minLength(Constants.MIN_MAP_CREATE_NAME_LENGTH),
    Validators.maxLength(Constants.MAX_MAP_CREATE_NAME_LENGTH)
  ]);

  @ViewChild("descriptionTextArea", { static: false }) descriptionTextArea: ElementRef;

  constructor(
    public settings: SettingsService,
    private rageConnector: RageConnectorService,
    private changeDetector: ChangeDetectorRef,
    public dialog: MatDialog,
    private snackBar: MatSnackBar) {
      this.rageConnector.listen(DFromClientEvent.AddPositionToMapCreatorBrowser, this.addPositionToMapCreatorBrowser.bind(this));
      this.rageConnector.listen(DFromClientEvent.RemovePositionInMapCreatorBrowser, this.RemovePositionInMapCreatorBrowser.bind(this));
  }

  ngOnInit() {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy() {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
  }

  private addPositionToMapCreatorBrowser(id: number, type: MapCreatorPositionType, posX: number, posY: number, posZ: number, rotX: number, rotY: number, rotZ: number, info?: string|number) {
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

  private RemovePositionInMapCreatorBrowser(id: number, type: MapCreatorPositionType) {
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

  private detectChanges() {
    this.changeDetector.detectChanges();
  }

  removeLastTeam() {
    this.data.TeamSpawns.pop();
    this.editingTeamNumber = this.data.TeamSpawns.length - 1;
    this.rageConnector.call(DToClientEvent.RemoveMapCreatorTeamNumber, this.data.TeamSpawns.length);
    this.changeDetector.detectChanges();
  }

  startNewPosPlacing(type: MapCreatorPositionType) {
    this.rageConnector.call(DToClientEvent.StartMapCreatorPosPlacing, type, this.editingTeamNumber);
  }

  removeSelectedPos(removeFunc: () => void) {
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

  tpToXYZ(x: number, y: number, z: number) {
    this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, x, y, z, 0);
  }

  addPosToTeamSpawns(pos: MapCreatorPosition) {
    if (!this.updatePosIfExists(this.data.TeamSpawns[pos.Info], pos)) {
      this.data.TeamSpawns[pos.Info] = [...this.data.TeamSpawns[pos.Info], pos];
    }
  }

  addPosToMapLimits(pos: MapCreatorPosition) {
    if (!this.updatePosIfExists(this.data.MapEdges, pos)) {
      this.data.MapEdges = [...this.data.MapEdges, pos];
    }
  }

  addPosToObjects(pos: MapCreatorPosition) {
    if (!this.updatePosIfExists(this.data.Objects, pos)) {
      this.data.Objects = [...this.data.Objects, pos];
    }
  }

  addPosToBombPlaces(pos: MapCreatorPosition) {
    if (!this.updatePosIfExists(this.data.BombPlaces, pos)) {
      this.data.BombPlaces = [...this.data.BombPlaces, pos];
    }
  }

  private updatePosIfExists(arr: MapCreatorPosition[], pos: MapCreatorPosition): boolean {
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

  addPosToMapCenter(pos: MapCreatorPosition) {
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
    this.rageConnector.callCallback(DToClientEvent.SendMapCreatorData, [JSON.stringify(this.data)], (err: number) => {
      const errName = MapCreateError[err];
      this.snackBar.open(this.settings.Lang[errName], "OK", {
        duration: undefined, panelClass: "mat-app-background"
      });
    });
  }

  removeTheMap() {
    this.dialog.open(AreYouSureDialog, {panelClass: "mat-app-background"})
      .afterClosed()
      .subscribe((bool: boolean) => {
      const map = this.data;
      this.data = new MapCreateDataDto();
      this.changeDetector.detectChanges();
      if (map.Id == 0)
        return;

      this.rageConnector.call(DToServerEvent.RemoveMap, this.data.Id);
    });
  }

  startNew() {
    this.dialog.open(AreYouSureDialog, {panelClass: "mat-app-background"})
      .afterClosed()
      .subscribe((bool: boolean) => {
      if (!bool)
        return;
      this.data = new MapCreateDataDto();
      this.changeDetector.detectChanges();
    });
  }

  saveData() {
    this.fixData();
    this.rageConnector.callCallback(DToClientEvent.SaveMapCreatorData, [JSON.stringify(this.data)], (err: number) => {
      const errName = MapCreateError[err];
      this.snackBar.open(this.settings.Lang[errName], "OK", {
        duration: undefined,
        panelClass: "mat-app-background"
      });
    });
  }

  loadPossibleMaps() {
    this.rageConnector.callCallback(DToServerEvent.LoadMapNamesToLoadForMapCreator, null, (possibleMapsJson: string) => {
      const possibleMaps = JSON.parse(possibleMapsJson) as LoadMapDialogGroupDto[];
      const dialogRef = this.dialog.open(LoadMapDialog, { data: possibleMaps, panelClass: "mat-app-background" });

      dialogRef.beforeClosed().subscribe(loadMapStr => {
        if (!loadMapStr)
          return;

        this.rageConnector.callCallback(DToServerEvent.LoadMapForMapCreator, loadMapStr, (json: string) => {
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

  private fixData() {
    if (typeof (this.data.MinPlayers) == "undefined")
      this.data.MinPlayers = 0;
    this.data.MinPlayers = Math.max(0, Math.min(999, Math.floor(this.data.MinPlayers)));

    if (typeof (this.data.MaxPlayers) == "undefined")
      this.data.MaxPlayers = 999;
    this.data.MaxPlayers = Math.max(0, Math.min(999, Math.floor(this.data.MaxPlayers)));
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

  onMapTypeChange(event: MatSelectChange) {
    this.data.Type = event.value;
    this.changeDetector.detectChanges();
  }

  onEditingTeamNumberChange(event: MatSelectChange) {
    if (event.value == "+") {
      this.data.TeamSpawns = [...this.data.TeamSpawns, []];
      this.editingTeamNumber = this.data.TeamSpawns.length - 1;
    } else
      this.editingTeamNumber = event.value;
    this.changeDetector.detectChanges();
  }

  onSelectedPositionChanged(row: MapCreatorPosition) {
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

  private saveDescription() {
    const langId = this.getLanguageValue(this.editingDescriptionLang);
    this.data.Description[langId] = this.descriptionTextArea.nativeElement.value;
  }

  isSaveableNav() {
    return this.currentNav != MapCreatorNav.Main;
  }

  isBackableNav() {
    return this.currentNav == MapCreatorNav.Description;
  }

  isTeamSpawnsValid(): boolean {
    if (this.data.TeamSpawns.length == 0)
      return false;
    for (const spawnArr of this.data.TeamSpawns) {
      if (spawnArr.length < Constants.MIN_TEAM_SPAWNS)
        return false;
    }
    return true;
  }

  isMapLimitValid(): boolean {
    return !this.data.MapEdges.length || this.data.MapEdges.length >= 3;
  }

  isBombPlacesValid(): boolean {
    return this.data.Type != MapType.Bomb || this.data.BombPlaces.length > 0;
  }

  getMinNameLength() {
    return Constants.MIN_MAP_CREATE_NAME_LENGTH;
  }

  getMaxNameLength() {
    return Constants.MAX_MAP_CREATE_NAME_LENGTH;
  }
}
