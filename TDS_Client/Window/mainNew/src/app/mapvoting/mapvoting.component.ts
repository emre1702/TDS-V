import { Component } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { MapType } from './enums/maptype.enum';
import { MapDataDto } from './models/mapDataDto';
import { MapNav } from './enums/mapnav.enum';
import { LanguageEnum } from '../enums/language.enum';
import { MapVotingService } from './services/mapvoting.service';
import { RageConnectorService } from '../services/rage-connector.service';
import { DToClientEvent } from '../enums/dtoclientevent.enum';

@Component({
  selector: 'app-mapvoting',
  templateUrl: './mapvoting.component.html',
  styleUrls: ['./mapvoting.component.scss']
})
export class MapVotingComponent {

  // todo: Always sort
  data: MapDataDto[] = [
    {
      Id: 1,
      CreatorName: "Bonus",
      Name: "Test Map 1",
      Description: { [LanguageEnum.German]: "Test Map", [LanguageEnum.English]: "TeTest English Map" },
      Rating: 5,
      Type: MapType.Normal
    },
    {
      Id: 2,
      CreatorName: "Bonus",
      Name: "Test Map 2",
      Description: { [LanguageEnum.German]: "Test Map", [LanguageEnum.English]: "Test English Map" },
      Rating: 5,
      Type: MapType.Bomb
    },
    {
      Id: 3,
      CreatorName: "Bonus",
      Name: "Test Map 3",
      Description: { [LanguageEnum.German]: "Test Map", [LanguageEnum.English]: "Test English Map" },
      Rating: 5,
      Type: MapType.Normal
    },
    {
      Id: 4,
      CreatorName: "Bonus",
      Name: "Test Map 4",
      Description: { [LanguageEnum.German]: "Test Map", [LanguageEnum.English]: "Test English Map" },
      Rating: 5,
      Type: MapType.Normal
    },
    {
      Id: 5,
      CreatorName: "Bonus",
      Name: "Test Map 5",
      Description: { [LanguageEnum.German]: "Test Map", [LanguageEnum.English]: "Test English Map" },
      Rating: 5,
      Type: MapType.Normal
    }
  ];
  selectedNav: string;
  selectedMap: MapDataDto;

  constructor(public settings: SettingsService, public voting: MapVotingService, private rageConnector: RageConnectorService) { }

  changeToNav(nav: string) {
    this.selectedNav = nav;
    this.selectedMap = undefined;
  }

  getNavs(): Array<string> {
    const keys = Object.keys(MapNav);
    return keys.slice(keys.length / 2);
  }

  isSelectedMapInVoting() {
    return this.voting.mapsInVoting.some(m => m.Id == this.selectedMap.Id);
  }
}
