var MapVotingComponent_1;
import { __decorate } from "tslib";
import { Component, ViewChild, HostListener, ChangeDetectionStrategy } from '@angular/core';
import { MapNav } from './enums/mapnav.enum';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { transition, animate, style, trigger } from '@angular/animations';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
let MapVotingComponent = MapVotingComponent_1 = class MapVotingComponent {
    constructor(settings, voting, rageConnector, changeDetector) {
        this.settings = settings;
        this.voting = voting;
        this.rageConnector = rageConnector;
        this.changeDetector = changeDetector;
        this.active = false;
        this.data = [];
        this.mapSearchFilter = "";
        this.rageConnector.listen(DFromClientEvent.OpenMapMenu, this.activate.bind(this));
        this.rageConnector.listen(DFromClientEvent.CloseMapMenu, () => this.deactivate(false));
        this.voting.mapsInVotingChanged.on(null, this.detectChanges.bind(this));
    }
    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.InTeamOrderModusChanged.on(null, this.detectChanges.bind(this));
        this.settings.FavoriteMapsChanged.on(null, this.detectChanges.bind(this));
        this.settings.InFightLobbyChanged.on(null, this.detectChanges.bind(this));
        this.mapSearchFilter = "";
    }
    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.InTeamOrderModusChanged.off(null, this.detectChanges.bind(this));
        this.settings.FavoriteMapsChanged.off(null, this.detectChanges.bind(this));
        this.settings.InFightLobbyChanged.off(null, this.detectChanges.bind(this));
    }
    detectChanges() {
        this.changeDetector.detectChanges();
    }
    activate(mapsJson) {
        this.data = JSON.parse(mapsJson);
        this.selectedNav = "All";
        this.selectedMap = undefined;
        this.active = true;
        this.changeDetector.detectChanges();
    }
    deactivate(sendToClient) {
        this.active = false;
        if (sendToClient) {
            this.rageConnector.call(DToClientEvent.CloseMapVotingMenu);
        }
        this.changeDetector.detectChanges();
    }
    changeSelectedMap(map) {
        this.selectedMap = map;
        this.changeDetector.detectChanges();
    }
    changeToNav(nav) {
        this.selectedNav = nav;
        this.selectedMap = undefined;
        this.changeDetector.detectChanges();
    }
    getNavs() {
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
    toggleMapIdToFavorite(mapId) {
        this.settings.toggleMapIdToFavorite(mapId);
        this.changeDetector.detectChanges();
    }
    voteForMapId(mapId) {
        this.voting.voteForMapId(mapId);
        this.changeDetector.detectChanges();
    }
    applyFilter(filter) {
        this.mapSearchFilter = filter;
        this.changeDetector.detectChanges();
    }
    handleKeyboardEvent(event) {
        if (!this.voting.mapsInVoting.length || this.settings.InTeamOrderModus)
            return;
        // tslint:disable-next-line: deprecation
        if (event.keyCode < MapVotingComponent_1.Numpad1KeyCode || event.keyCode > MapVotingComponent_1.Numpad9KeyCode)
            return;
        // tslint:disable-next-line: deprecation
        const voteIndex = event.keyCode - MapVotingComponent_1.Numpad1KeyCode;
        if (this.voting.mapsInVoting.length <= voteIndex)
            return;
        this.voting.voteForMapId(this.voting.mapsInVoting[voteIndex].Id);
    }
};
MapVotingComponent.Numpad1KeyCode = 97;
MapVotingComponent.Numpad9KeyCode = 105;
__decorate([
    ViewChild('snav', { static: false })
], MapVotingComponent.prototype, "snav", void 0);
__decorate([
    HostListener("document:keyup", ["$event"])
], MapVotingComponent.prototype, "handleKeyboardEvent", null);
MapVotingComponent = MapVotingComponent_1 = __decorate([
    Component({
        selector: 'app-mapvoting',
        animations: [
            trigger('hideShowAnimation', [
                transition(':enter', [
                    style({ transform: 'translateY(100%)', opacity: 0 }),
                    animate('500ms', style({ transform: 'translateY(0)', opacity: 0.9 }))
                ]),
                transition(':leave', [
                    animate('500ms', style({ transform: 'translateY(100%)', opacity: 0 })),
                ])
            ])
        ],
        templateUrl: './mapvoting.component.html',
        styleUrls: ['./mapvoting.component.scss'],
        changeDetection: ChangeDetectionStrategy.OnPush
    })
], MapVotingComponent);
export { MapVotingComponent };
//# sourceMappingURL=mapvoting.component.js.map