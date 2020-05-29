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
import { DToServerEvent } from '../../enums/dtoserverevent.enum';

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
    mapSearchFilter = "";

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
        this.settings.MapBuyStatsChanged.on(null, this.detectChanges.bind(this));
        this.settings.MoneyChanged.on(null, this.detectChanges.bind(this));
        this.settings.IsLobbyOwnerChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingsLoaded.on(null, this.detectChanges.bind(this));

        this.mapSearchFilter = "";
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
        this.settings.ThemeSettingsLoaded.off(null, this.detectChanges.bind(this));
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
        return this.voting.mapsInVoting.some(m => m[0] == this.selectedMap[0]);
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
        this.rageConnector.callServer(DToServerEvent.BuyMap, mapId);
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
        this.voting.voteForMapId(this.voting.mapsInVoting[voteIndex][0]);
    }
}
