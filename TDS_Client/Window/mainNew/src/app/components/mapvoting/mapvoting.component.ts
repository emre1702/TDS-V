import { Component, ChangeDetectorRef, ViewChild, OnInit } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { MapDataDto } from './models/mapDataDto';
import { MapNav } from './enums/mapnav.enum';
import { MapVotingService } from './services/mapvoting.service';
import { RageConnectorService } from '../../services/rage-connector.service';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { transition, animate, style, trigger } from '@angular/animations';
import { DToClientEvent } from 'src/app/enums/dtoclientevent.enum';
import { MatSidenav } from '@angular/material';

@Component({
  selector: 'app-mapvoting',
  animations: [
    trigger(
      'hideShowAnimation',
      [
        transition(
          ':enter', [
            style({transform: 'translateY(100%)', opacity: 0}),
            animate('500ms', style({transform: 'translateY(0)', opacity: 0.9}))
          ]
        ),
        transition(
          ':leave', [
            animate('500ms', style({transform: 'translateY(100%)', opacity: 0})),
          ]
        )
      ]
    )
  ],
  templateUrl: './mapvoting.component.html',
  styleUrls: ['./mapvoting.component.scss']
})
export class MapVotingComponent implements OnInit {

  active = false;
  data: MapDataDto[] = [];
  selectedNav: string;
  selectedMap: MapDataDto;

  @ViewChild('snav') snav: MatSidenav;

  constructor(public settings: SettingsService, public voting: MapVotingService, private rageConnector: RageConnectorService,
              public changeDetector: ChangeDetectorRef) {
    this.rageConnector.listen(DFromClientEvent.OpenMapMenu, this.activate.bind(this));
    this.rageConnector.listen(DFromClientEvent.CloseMapMenu, () => this.deactivate(false));
  }

  ngOnInit(): void {
    this.voting.mapsInVotingChanged.on(null, () => this.changeDetector.detectChanges());
    this.settings.LanguageChanged.on(null, () => this.changeDetector.detectChanges());
    this.settings.InTeamOrderModusChanged.on(null, () => this.changeDetector.detectChanges());
    this.settings.FavoriteMapsChanged.on(null, () => this.changeDetector.detectChanges());
  }

  private activate(mapsJson: string) {
    this.data = JSON.parse(mapsJson);
    this.selectedNav = "All";
    this.selectedMap = undefined;
    this.active = true;
    this.changeDetector.detectChanges();
  }

  deactivate(sendToClient: boolean) {
    this.active = false;
    if (sendToClient) {
      this.rageConnector.call(DToClientEvent.CloseMapVotingMenu);
    }
    this.changeDetector.detectChanges();
  }

  changeSelectedMap(map: MapDataDto) {
    this.selectedMap = map;
    this.changeDetector.detectChanges();
  }

  changeToNav(nav: string) {
    this.selectedNav = nav;
    this.selectedMap = undefined;
    this.changeDetector.detectChanges();
  }

  getNavs(): Array<string> {
    const keys = Object.keys(MapNav);
    return keys.slice(keys.length / 2);
  }

  isSelectedMapInVoting() {
    return this.voting.mapsInVoting.some(m => m.Id == this.selectedMap.Id);
  }

  toggleSnav() {
    this.snav.toggle();
    this.changeDetector.detectChanges();
  }

  toggleMapIdToFavorite(mapId: number) {
    this.settings.toggleMapIdToFavorite(this.selectedMap.Id);
    this.changeDetector.detectChanges();
  }

  voteForMapId(mapId: number) {
    this.voting.voteForMapId(mapId);
    this.changeDetector.detectChanges();
  }
}
