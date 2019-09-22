import { Component, ChangeDetectorRef, ViewChild, OnInit, HostListener, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { MapDataDto } from './models/mapDataDto';
import { MapNav } from './enums/mapnav.enum';
import { MapVotingService } from './services/mapvoting.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { transition, animate, style, trigger } from '@angular/animations';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
import { MatSidenav } from '@angular/material';

@Component({
  selector: 'app-mapvoting',
  animations: [
    trigger(
      'hideShowAnimation',
      [
        transition(
          ':enter', [
            style({ transform: 'translateY(100%)', opacity: 0 }),
            animate('500ms', style({ transform: 'translateY(0)', opacity: 0.9 }))
          ]
        ),
        transition(
          ':leave', [
            animate('500ms', style({ transform: 'translateY(100%)', opacity: 0 })),
          ]
        )
      ]
    )
  ],
  templateUrl: './mapvoting.component.html',
  styleUrls: ['./mapvoting.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MapVotingComponent implements OnInit, OnDestroy {
  private static readonly Numpad1KeyCode = 97;
  private static readonly Numpad9KeyCode = 105;

  active = false;
  data: MapDataDto[] = [];
  selectedNav: string;
  selectedMap: MapDataDto;

  @ViewChild('snav', { static: false }) snav: MatSidenav;

  constructor(public settings: SettingsService, public voting: MapVotingService, private rageConnector: RageConnectorService,
    public changeDetector: ChangeDetectorRef) {
    this.rageConnector.listen(DFromClientEvent.OpenMapMenu, this.activate.bind(this));
    this.rageConnector.listen(DFromClientEvent.CloseMapMenu, () => this.deactivate(false));
    this.voting.mapsInVotingChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnInit(): void {
    this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    this.settings.InTeamOrderModusChanged.on(null, this.detectChanges.bind(this));
    this.settings.FavoriteMapsChanged.on(null, this.detectChanges.bind(this));
    this.settings.InFightLobbyChanged.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy(): void {
    this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    this.settings.InTeamOrderModusChanged.off(null, this.detectChanges.bind(this));
    this.settings.FavoriteMapsChanged.off(null, this.detectChanges.bind(this));
    this.settings.InFightLobbyChanged.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
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
    this.settings.toggleMapIdToFavorite(mapId);
    this.changeDetector.detectChanges();
  }

  voteForMapId(mapId: number) {
    this.voting.voteForMapId(mapId);
    this.changeDetector.detectChanges();
  }

  @HostListener("document:keyup", ["$event"])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (!this.voting.mapsInVoting.length || this.settings.InTeamOrderModus)
      return;
    // tslint:disable-next-line: deprecation
    if (event.keyCode < MapVotingComponent.Numpad1KeyCode || event.keyCode > MapVotingComponent.Numpad9KeyCode)
      return;
    // tslint:disable-next-line: deprecation
    const voteIndex = event.keyCode - MapVotingComponent.Numpad1KeyCode;
    if (this.voting.mapsInVoting.length <= voteIndex)
      return;
    this.voting.voteForMapId(this.voting.mapsInVoting[voteIndex].Id);
  }
}