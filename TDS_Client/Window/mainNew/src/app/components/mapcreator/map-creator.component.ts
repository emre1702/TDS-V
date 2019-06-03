import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, ChangeDetectionStrategy } from '@angular/core';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { SettingsService } from 'src/app/services/settings.service';
import { LanguageEnum } from 'src/app/enums/language.enum';
import { MapType } from './enums/maptype.enum';
import { MatSelectChange } from '@angular/material';
import { MapCreateDataDto } from './models/mapCreateDataDto';
import { Constants } from '../../constants';

enum MapCreatorNav {
  Main, Description, TeamSpawns, MapLimit, MapCenter, BombPlaces
}

@Component({
  selector: 'app-map-creator',
  templateUrl: './map-creator.component.html',
  styleUrls: ['./map-creator.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MapCreatorComponent {
  data = new MapCreateDataDto();
  mapCreatorNav = MapCreatorNav;
  currentNav = MapCreatorNav.Main;

  editingDescriptionLang: string;

  @ViewChild("descriptionTextArea") descriptionTextArea: ElementRef;

  constructor(public settings: SettingsService, private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef) {
    settings.LanguageChanged.on(null, () => changeDetector.detectChanges());
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
    this.changeDetector.detectChanges();
  }

  private saveDescription() {
    const langId = this.getLanguageValue(this.editingDescriptionLang);
    this.data.Description[langId] = this.descriptionTextArea.nativeElement.value;
  }

  isSaveableNav() {
    return this.currentNav == MapCreatorNav.Description;
  }

  isBackableNav() {
    return !this.isSaveableNav() && this.currentNav != MapCreatorNav.Main;
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

}
