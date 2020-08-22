/*import EventsService from "../events/events.service";
import { inject } from "inversify";
import LobbySettings from "../../data/interfaces/lobbies/lobby-settings.interface";
import alt from "alt-client"*/

import { injectable } from "inversify";

//Todo: Implement discord rich presence later
@injectable()
export default class DiscordService {

    //private lastLobbyName = "Login/Register";
    //private lastTeamName = "Spectator";

    constructor(
        //@inject(EventsService) eventsService: EventsService
    ) {
        //eventsService.onLobbyJoined.on(this.lobbyJoined.bind(this));
        //eventsService.onTeamChanged.on(this.teamChanged.bind(this));
    }

    /*private lobbyJoined(lobbySettings: LobbySettings) {
        this.lastLobbyName = lobbySettings.name;
        this.update(this.lastTeamName, this.lastTeamName);
    }

    private teamChanged(args: { newTeamName: string }) {
        this.lastTeamName = args.newTeamName;
        this.update(this.lastTeamName, this.lastTeamName);

    }

    private update(lobbyName: string, teamName: string) {
        alt.Discord.Update($"TDS-V - {lobbyName}", teamName);
    }*/
}
