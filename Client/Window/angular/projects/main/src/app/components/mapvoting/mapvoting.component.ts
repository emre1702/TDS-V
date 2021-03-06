import { Component, ChangeDetectorRef, ViewChild, OnInit, HostListener, OnDestroy } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { MapDataDto } from './models/mapDataDto';
import { MapNav } from './enums/mapnav.enum';
import { MapVotingService } from './services/mapvoting.service';
import { RageConnectorService } from 'rage-connector';
import { FromClientEvent } from '../../enums/from-client-event.enum';
import { ToClientEvent } from '../../enums/to-client-event.enum';
import { ToServerEvent } from '../../enums/to-server-event.enum';
import { bottomToTopEnterAnimation } from '../../animations/bottomToUpEnter.animation';
import { MatSidenav } from '@angular/material/sidenav';
import { LanguagePipe } from '../../modules/shared/pipes/language.pipe';
import { InitialDatas } from '../../initial-datas';

@Component({
    selector: 'app-mapvoting',
    animations: [bottomToTopEnterAnimation],
    templateUrl: './mapvoting.component.html',
    styleUrls: ['./mapvoting.component.scss'],
})
export class MapVotingComponent implements OnInit, OnDestroy {
    private static readonly Numpad1KeyCode = 97;
    private static readonly Numpad9KeyCode = 105;

    active = InitialDatas.isMapVotingActive;
    data: MapDataDto[] = InitialDatas.getMapsForVoting();
    selectedNav: string;
    selectedMap: MapDataDto;
    mapSearchFilter = '';
    title: string;

    @ViewChild('snav') snav: MatSidenav;

    constructor(
        public settings: SettingsService,
        public voting: MapVotingService,
        private rageConnector: RageConnectorService,
        public changeDetector: ChangeDetectorRef
    ) {
        this.rageConnector.listen(FromClientEvent.OpenMapMenu, this.activate.bind(this));
        this.rageConnector.listen(FromClientEvent.CloseMapMenu, () => this.deactivate(false));
        this.voting.mapsInVotingChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnInit(): void {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.InTeamOrderModusChanged.on(null, this.detectChanges.bind(this));
        this.settings.FavoriteMapsChanged.on(null, this.detectChanges.bind(this));
        this.settings.InFightLobbyChanged.on(null, this.detectChanges.bind(this));
        this.settings.MapBuyStatsChanged.on(null, this.detectChanges.bind(this));
        this.settings.MoneyChanged.on(null, this.detectChanges.bind(this));
        this.settings.IsLobbyOwnerChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));

        this.mapSearchFilter = '';
        this.refreshTitle();
    }

    ngOnDestroy(): void {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.InTeamOrderModusChanged.off(null, this.detectChanges.bind(this));
        this.settings.FavoriteMapsChanged.off(null, this.detectChanges.bind(this));
        this.settings.InFightLobbyChanged.off(null, this.detectChanges.bind(this));
        this.settings.MapBuyStatsChanged.off(null, this.detectChanges.bind(this));
        this.settings.MoneyChanged.off(null, this.detectChanges.bind(this));
        this.settings.IsLobbyOwnerChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }

    private activate(mapsJson: string) {
        this.data = JSON.parse(mapsJson);
        this.selectedNav = 'All';
        this.selectedMap = undefined;
        this.active = true;
        this.refreshTitle();
        this.changeDetector.detectChanges();
    }

    deactivate(sendToClient: boolean) {
        this.active = false;
        if (sendToClient) {
            this.rageConnector.call(ToClientEvent.CloseMapVotingMenu);
        }
        this.changeDetector.detectChanges();
    }

    changeSelectedMap(map: MapDataDto) {
        this.selectedMap = map;
        this.refreshTitle();
        this.changeDetector.detectChanges();
    }

    changeToNav(nav: string) {
        this.selectedNav = nav;
        this.selectedMap = undefined;
        this.refreshTitle();
        this.changeDetector.detectChanges();
    }

    getNavs(): Array<string> {
        const keys = Object.keys(MapNav);
        return keys.slice(keys.length / 2);
    }

    isSelectedMapInVoting() {
        return this.voting.mapsInVoting.some((m) => m[0] == this.selectedMap[0]);
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

    applyFilter(filter: string) {
        this.mapSearchFilter = filter;
        this.changeDetector.detectChanges();
    }

    buyMap(mapId: number) {
        this.rageConnector.callServer(ToServerEvent.BuyMap, mapId);
    }

    canBuyMap(): boolean {
        if (this.settings.IsLobbyOwner) {
            return true;
        }
        if (this.getMapBuyPrice() > this.settings.Money) {
            return false;
        }
        return true;
    }

    private getMapBuyPrice() {
        return Math.ceil(this.settings.Constants[4] + this.settings.Constants[5] * this.settings.MapsBoughtCounter);
    }

    private refreshTitle() {
        this.title = new LanguagePipe().transform('MapVoting', this.settings.Lang);
        if (this.selectedMap) {
            this.title += ' - ' + this.selectedMap[1];
        }
    }

    @HostListener('document:keyup', ['$event'])
    handleKeyboardEvent(event: KeyboardEvent) {
        if (!this.voting.mapsInVoting.length || this.settings.InTeamOrderModus) return;
        // tslint:disable-next-line: deprecation
        if (event.keyCode < MapVotingComponent.Numpad1KeyCode || event.keyCode > MapVotingComponent.Numpad9KeyCode) return;
        // tslint:disable-next-line: deprecation
        const voteIndex = event.keyCode - MapVotingComponent.Numpad1KeyCode;
        if (this.voting.mapsInVoting.length <= voteIndex) return;
        this.voting.voteForMapId(this.voting.mapsInVoting[voteIndex][0]);
    }
}
