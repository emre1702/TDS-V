import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { SettingsService } from 'src/app/services/settings.service';
import { LanguageEnum } from 'src/app/enums/language.enum';
import { MapType } from './enums/maptype.enum';
import { MatSelectChange, MatDialog, MatSnackBar } from '@angular/material';
import { MapCreateDataDto } from './models/mapCreateDataDto';
import { Constants } from '../../constants';
import { Position4D } from './models/position4d';
import { DToClientEvent } from 'src/app/enums/dtoclientevent.enum';
import { LoadMapDialog } from './loadmapdialog/load-map-dialog';
import { MapCreateError } from './enums/mapcreateerror.enum';
import { Position3D } from './models/position3d';
import { FormControl, Validators } from '@angular/forms';

enum MapCreatorNav {
  Main, MapSettings, Description, TeamSpawns, MapLimit, MapCenter, BombPlaces
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
  currentNav = MapCreatorNav.Main;
  editingTeamNumber = 0;
  selectedPosition: Position3D | Position4D;

  editingDescriptionLang: string;
  currentTitle = "MapCreator";
  possibleMaps: string[];

  displayedColumns: string[] = ["id", "x", "y", "z", "rot"];
  displayedColumns2D: string[] = ["id", "x", "y"];

  nameControl = new FormControl("", [
    Validators.required,
    Validators.minLength(Constants.MIN_MAP_CREATE_NAME_LENGTH),
    Validators.maxLength(Constants.MAX_MAP_CREATE_NAME_LENGTH)
  ]);

  @ViewChild("descriptionTextArea") descriptionTextArea: ElementRef;

  constructor(
    public settings: SettingsService,
    private rageConnector: RageConnectorService,
    private changeDetector: ChangeDetectorRef,
    public dialog: MatDialog,
    private snackBar: MatSnackBar) {
  }

  ngOnInit() {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy() {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }

  removeLastTeam() {
    this.data.TeamSpawns.pop();
    this.editingTeamNumber = this.data.TeamSpawns.length - 1;
    this.changeDetector.detectChanges();
  }

  addNewPos(addFunc: (pos: Position4D) => void) {
    this.rageConnector.callCallback(DToClientEvent.GetCurrentPositionRotation, null, (x: number, y: number, z: number, rot: number) => {
      const pos = new Position4D(x, y, z, rot);
      // need to create a new dataSource object, else table will not refresh
      addFunc.call(this, pos);
      this.changeDetector.detectChanges();
    });
  }

  removeSelectedPos(removeFunc: () => void) {
    removeFunc.call(this);
    this.changeDetector.detectChanges();
  }

  tpToSelectedPos() {
    const pos = this.selectedPosition as Position4D;
    this.rageConnector.call(DToClientEvent.TeleportToPositionRotation, pos.X, pos.Y, pos.Z, pos.Rotation);
  }

  tpToXY(x: number, y: number) {
    this.rageConnector.call(DToClientEvent.TeleportToXY, x, y);
  }


  addPosToTeamSpawns(pos: Position4D) {
    this.data.TeamSpawns[this.editingTeamNumber] = [...this.data.TeamSpawns[this.editingTeamNumber], pos];
  }

  addPosToMapLimits(pos: Position4D) {
    this.data.MapEdges = [...this.data.MapEdges, pos];
  }

  addPosToBombPlaces(pos: Position4D) {
    this.data.BombPlaces = [...this.data.BombPlaces, pos];
  }

  addPosToMapCenter(pos: Position4D) {
    this.data.MapCenter.X = pos.X;
    this.data.MapCenter.Y = pos.Y;
    this.data.MapCenter.Z = pos.Z;
  }


  removePosFromTeamSpawns() {
    const index = this.data.TeamSpawns[this.editingTeamNumber].indexOf(this.selectedPosition as Position4D);
    this.data.TeamSpawns[this.editingTeamNumber].splice(index, 1);
    // need to create a new dataSource object, else table will not refresh
    this.data.TeamSpawns[this.editingTeamNumber] = [...this.data.TeamSpawns[this.editingTeamNumber]];
  }

  removePosFromMapLimits() {
    const index = this.data.MapEdges.indexOf(this.selectedPosition as Position3D);
    this.data.MapEdges.splice(index, 1);
    // need to create a new dataSource object, else table will not refresh
    this.data.MapEdges = [...this.data.MapEdges];
  }

  removePosFromBombPlaces() {
    const index = this.data.BombPlaces.indexOf(this.selectedPosition as Position4D);
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

  startNew() {
    this.data = new MapCreateDataDto();
    this.changeDetector.detectChanges();
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
    this.rageConnector.callCallback(DToClientEvent.LoadMySavedMapNames, null, (possibleMapsJson: string) => {
      const possibleMaps = JSON.parse(possibleMapsJson) as string[];
      const dialogRef = this.dialog.open(LoadMapDialog, { data: possibleMaps, panelClass: "mat-app-background" });

      dialogRef.beforeClosed().subscribe(loadMapStr => {
        if (!loadMapStr)
          return;

        this.rageConnector.callCallback(DToClientEvent.LoadMySavedMap, loadMapStr, (json: string) => {
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

  onSelectedPositionChanged(row: Position4D) {
    if (this.selectedPosition == row)
      this.selectedPosition = undefined;
    else
      this.selectedPosition = row;
    this.changeDetector.detectChanges();
  }

  switchToMapSettings() {
    this.currentNav = MapCreatorNav.MapSettings;
    this.changeDetector.detectChanges();
  }

  switchToDescriptionEdit(lang: string) {
    this.editingDescriptionLang = lang;
    this.currentNav = MapCreatorNav.Description;
    this.changeDetector.detectChanges();
  }

  switchToTeamSpawnsEdit() {
    this.currentNav = MapCreatorNav.TeamSpawns;
    this.changeDetector.detectChanges();
  }

  switchToMapLimitEdit() {
    this.currentNav = MapCreatorNav.MapLimit;
    this.changeDetector.detectChanges();
  }

  switchToMapCenterEdit() {
    this.currentNav = MapCreatorNav.MapCenter;
    this.changeDetector.detectChanges();
  }

  switchToBombPlacesEdit() {
    this.currentNav = MapCreatorNav.BombPlaces;
    this.changeDetector.detectChanges();
  }

  saveNav() {
    switch (this.currentNav) {
      case MapCreatorNav.Description:
        this.saveDescription();
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
