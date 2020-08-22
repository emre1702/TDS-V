import EventsService from "../events/events.service";
import BrowserServiceBase from "./browser-service-base";
import { angularMainBrowserPath } from "../../datas/constants";
import { inject } from "inversify";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";
import FromBrowserEvent from "../../datas/enums/events/from-browser-event.enum";
import Utils from "../../datas/utils";
import PlayerDataKey from "../../datas/enums/data/player-data-key.enum";
import { Player, onServer } from "alt-client";
import PlayerSettings from "../../datas/interfaces/players/player-settings.interface";
import PlayerAngularChatSettings from "../../datas/interfaces/players/player-angular-chat-settings.interface";
import MapCreatorObject from "../../datas/interfaces/lobbies/map-creator/map-creator-object.interface";
import MapCreatorPosData from "../../datas/interfaces/lobbies/map-creator/map-creator-object-data.interface";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import MapCreatorObjectData from "../../datas/interfaces/lobbies/map-creator/map-creator-object-data.interface";


export default class AngularBrowserService extends BrowserServiceBase {

    constructor(
        @inject(DIIdentifier.EventsService) private eventsService: EventsService
    ) {
        super(angularMainBrowserPath);

        eventsService.onInFightStatusChanged.on((inFight) => this.execute(ToBrowserEvent.ToggleRoundStats, inFight));
        eventsService.onCountdownStarted.on(this.onCountdownStart.bind(this));
        eventsService.onLobbyLeft.on(this.onLobbyLeft.bind(this));
        eventsService.onRoundEnded.on((_) => this.execute(ToBrowserEvent.ResetMapVoting));
        eventsService.onAngularCooldown.on(() => this.execute(ToBrowserEvent.ShowCooldown));
        eventsService.onChatInputToggled.on((activated) => this.execute(ToBrowserEvent.ToggleChatOpened, activated));
        eventsService.onLanguageChanged.on(data => this.execute(ToBrowserEvent.LoadLanguage, data.language));

        onServer(ToClientEvent.ToBrowserEvent, this.execute.bind(this));
        onServer(ToClientEvent.FromBrowserEventReturn, this.execute.bind(this));
        onServer(ToClientEvent.JoinSameLobby, (player: Player) => this.execute(ToBrowserEvent.AddNameForChat, player.getName()));
        onServer(ToClientEvent.SyncSettings, this.loadChatSettings.bind(this));

        this.browser.on(FromBrowserEvent.GetHashedPassword, this.onGetHashedPassword.bind(this));
    }

    closeMapMenu() {
        this.execute(ToBrowserEvent.CloseMapMenu);
    }

    private onCountdownStart() {
        this.hideRankings();
    }

    private onLobbyLeft() {
        this.execute(ToBrowserEvent.ResetMapVoting);
        this.hideRankings();
    }

    private onGetHashedPassword(password: string) {
        const hashedPassword = Utils.hashPasswordClient(password);
        this.execute(FromBrowserEvent.GetHashedPassword, hashedPassword);
    }

    private hideRankings() {
        this.execute(ToBrowserEvent.HideRankings);
    }

    private loadChatSettings(settings: PlayerSettings) {
        this.execute(ToBrowserEvent.LoadChatSettings, settings.Chat)
    }

    private addMapCreatorObject(objectData: MapCreatorObjectData) {
        this.execute(ToBrowserEvent.AddMapCreatorObject, objectData);
    }
}
