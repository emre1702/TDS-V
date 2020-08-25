import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import BrowsersService from "../browsers/browsers.service";
import LobbySettings from "../../datas/interfaces/lobbies/lobby-settings.interface";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";
import LobbyType from "../../datas/enums/lobbies/lobby-type.enum";

@injectable()
export default class FreeroamService {

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.BrowsersService) private browserService: BrowsersService
    ) {
        eventsService.onLobbyJoined.on(this.lobbyJoined.bind(this));
        eventsService.onLobbyLeft.on(this.lobbyLeft.bind(this));
    }

    private lobbyJoined(settings: LobbySettings) {
        switch (settings.Type) {
            case LobbyType.MapCreateLobby:
            case LobbyType.GangLobby:
                this.browserService.angular.execute(ToBrowserEvent.ToggleFreeroam, true);
                break;
        }
    }

    private lobbyLeft(settings: LobbySettings) {
        switch (settings.Type) {
            case LobbyType.MapCreateLobby:
            case LobbyType.GangLobby:
                this.browserService.angular.execute(ToBrowserEvent.ToggleFreeroam, false);
                break;
        }
    }
}
