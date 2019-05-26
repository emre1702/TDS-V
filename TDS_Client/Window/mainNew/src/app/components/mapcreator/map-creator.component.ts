import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, ChangeDetectionStrategy } from '@angular/core';
import { RageConnectorService } from 'src/app/services/rage-connector.service';
import { SettingsService } from 'src/app/services/settings.service';
import { MapDataDto } from 'src/app/models/mapDataDto';
import { LanguageEnum } from 'src/app/enums/language.enum';
import { MapType } from './enums/maptype.enum';
import { MatSelectChange, MatInput } from '@angular/material';

@Component({
  selector: 'app-map-creator',
  templateUrl: './map-creator.component.html',
  styleUrls: ['./map-creator.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MapCreatorComponent implements OnInit {
  data = new MapDataDto();

  editingDescriptionLang: string;

  @ViewChild("descriptionTextArea") descriptionTextArea: ElementRef;

  constructor(public settings: SettingsService, private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef) {
    settings.LanguageChanged.on(null, () => changeDetector.detectChanges());
  }

  ngOnInit() { }

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
    this.changeDetector.detectChanges();
  }

  saveDescription() {
    const value = this.getLanguageValue(this.editingDescriptionLang);
    this.editingDescriptionLang = undefined;
    this.data.Description[value] = this.descriptionTextArea.nativeElement.value;
    this.changeDetector.detectChanges();
  }

}
